using System;
using System.Collections.Generic;

using System.IO;
using iTextSharp.text.pdf;
using System.Xml;

namespace PdfBrowser
{
    internal class PdfFile
    {
        #region pola prywatne

        private static string[] _ścieżkiWszystkichPlikówTxt;
        private static string[] _ścieżkiWszystkichPlikówXades;
        private static int _licznik;
        #endregion

        #region właściwości

        private static int _rok;
        public static int Rok
        {
            get { return _rok; }

            set
            {
                int obecnyRok = DateTime.Now.Year;

                if ((value < 2014) || (value > obecnyRok))
                    _rok = obecnyRok;
                else
                    _rok = value;

                Katalog = Path.Combine("PITY_PDF", _rok.ToString());

                if (!Directory.Exists(Katalog))
                    Directory.CreateDirectory(Katalog);
            }
        }

        public static string Katalog { get; private set; }

        private string _ścieżkaPdf;
        public string ŚcieżkaPdf
        {
            get { return _ścieżkaPdf; }

            private set
            {
                if (File.Exists(value))
                    _ścieżkaPdf = value;
                else
                    throw new FileNotFoundException();
            }
        }

        private List<string> _ścieżkiTxt = new List<string>();
        public List<string> ŚcieżkiTxt
        {
            get
            {
                if (TxtIstnieje)
                    return _ścieżkiTxt;
                else
                    return new List<string>();
            }

            private set { _ścieżkiTxt = value; }
        }

        private string _ścieżkaUpo;
        public string ŚcieżkaUpo
        {
            get
            {
                if (UpoIstnieje)
                    return _ścieżkaUpo;
                else
                    return string.Empty;
            }

            private set { _ścieżkaUpo = value; }
        }

        private string _numerReferencyjny;
        public string NumerReferencyjny
        {
            get
            {
                if (TxtIstnieje)
                    return _numerReferencyjny;
                else
                    return string.Empty;
            }

            private set { _numerReferencyjny = value; }
        }

        public int Id { get; private set; }
        public string NazwaPliku { get { return Path.GetFileName(ŚcieżkaPdf); } }
        public bool SigIstnieje { get { return File.Exists(Path.ChangeExtension(ŚcieżkaPdf, "sig")); } }
        public bool TxtIstnieje { get; private set; }
        public bool UpoIstnieje { get; private set; }

        public string ŚcieżkaSig
        {
            get
            {
                if (SigIstnieje)
                    return Path.ChangeExtension(ŚcieżkaPdf, "sig");
                else
                    return string.Empty;
            }
        }
        #endregion

        #region konstruktory
        static PdfFile()
        {
            Rok = DateTime.Now.Year;

            Inicjalizuj();
        }

        public PdfFile(string ścieżka)
        {
            Id = ++_licznik;
            ŚcieżkaPdf = ścieżka;
            TxtIstnieje = SprawdźCzyIstniejeTxt();
            NumerReferencyjny = PobierzNumerReferencyjny();
            UpoIstnieje = SprawdźCzyIstniejeUpo();
        }
        #endregion

        #region metody prywatne

        /*private static void SetPrefixDeep(XmlNode xmlNode, string prefix)
        {
            xmlNode.Prefix = prefix;

            foreach (XmlNode child in xmlNode.ChildNodes)
                SetPrefixDeep(child, prefix);
        }*/

        private bool SprawdźCzyIstniejeTxt()
        {
            if (Katalog == string.Empty) return false;

            List<DateTime> datyPlikówTxt = new List<DateTime>();
            List<string> ścieżkiPlikówTxt = new List<string>();

            foreach (string ścieżka in _ścieżkiWszystkichPlikówTxt)
            {
                string nazwaPliku = Path.GetFileName(ścieżka);

                if (string.IsNullOrEmpty(nazwaPliku) || string.IsNullOrEmpty(NazwaPliku) || !nazwaPliku.StartsWith(Path.GetFileNameWithoutExtension(NazwaPliku))) continue;

                int indeksDaty = nazwaPliku.LastIndexOf("_", StringComparison.Ordinal) + 1;
                int długośćDaty = 0;

                for (int i = indeksDaty; i < nazwaPliku.Length; i++)
                {
                    char znak = nazwaPliku[i];

                    if (char.IsLetter(znak) || (znak == '.'))
                        break;
                    else
                        długośćDaty++;
                }

                string napisDaty = nazwaPliku.Substring(indeksDaty, długośćDaty).Trim();
                DateTime data;

                try
                {
                    data = Convert.ToDateTime(napisDaty);
                }
                catch (FormatException)
                {
                    data = DateTime.ParseExact(napisDaty, "yyyyMMdd", null);
                }

                datyPlikówTxt.Add(data);
                ścieżkiPlikówTxt.Add(ścieżka);
            }

            if (ścieżkiPlikówTxt.Count == 0)
                return false;
            else
            {
                int n = datyPlikówTxt.Count;

                do
                {
                    for (int i = 0; i < n - 1; i++)
                        if (datyPlikówTxt[i] < datyPlikówTxt[i + 1])
                        {
                            datyPlikówTxt.Reverse(i, 2);
                            ścieżkiPlikówTxt.Reverse(i, 2);
                        }

                    n--;
                }
                while (n > 1);

                /*for (int i = 1; i < datesOfTxtFiles.Count; i++)
                        TxtPaths.Add(pathsOfTxtFiles[i]);*/

                ŚcieżkiTxt = ścieżkiPlikówTxt;

                return true;
            }
        }

        private bool SprawdźCzyIstniejeUpo()
        {
            if (string.IsNullOrEmpty(NumerReferencyjny)) return false;

            foreach (string ścieżkaPlikuXades in _ścieżkiWszystkichPlikówXades)
                if (ścieżkaPlikuXades.IndexOf(NumerReferencyjny, StringComparison.Ordinal) != -1)
                {
                    ŚcieżkaUpo = ścieżkaPlikuXades;

                    return true;
                }

            return false;
        }

        private string PobierzNumerReferencyjny()
        {
            if (!TxtIstnieje) return string.Empty;

            string[] txt = File.ReadAllLines(ŚcieżkiTxt[0]);

            if (txt.Length <= 0) return string.Empty;

            string pierwszaLinia = txt[0];

            return pierwszaLinia.Substring(pierwszaLinia.IndexOf(':') + 2);
        }

        /*private static void UsuńPusteWęzły(XmlNode węzełXml)
        {
            foreach (XmlNode węzełPotomny in węzełXml.ChildNodes)
                if ((węzełPotomny.ChildNodes.Count == 0) && (węzełPotomny.InnerText == string.Empty))
                    węzełXml.RemoveChild(węzełPotomny);
                else
                    UsuńPusteWęzły(węzełPotomny);

            //return xmlElement;
        }*/
        #endregion

        #region metody publiczne
        public static void Inicjalizuj()
        {
            _ścieżkiWszystkichPlikówTxt = Directory.GetFiles(Katalog, "*.txt");
            _ścieżkiWszystkichPlikówXades = Directory.GetFiles(Katalog, "*.xades");
            _licznik = 0;
        }

        public string EksportujXml()
        {
            try
            {
                using (FileStream strumień = new FileStream(ŚcieżkaPdf, FileMode.Open))
                using (PdfReader czytelnik = new PdfReader(strumień))
                {
                    XmlDocument dokumentXml = new XmlDocument();
                    string xml = czytelnik.AcroFields.Xfa.DatasetsNode.OuterXml;
                    xml = xml.Substring(xml.IndexOf("<xfa:data>", StringComparison.Ordinal) + "<xfa:data>".Length);

                    dokumentXml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + xml.Substring(0, xml.IndexOf("</Deklaracja>", StringComparison.Ordinal) + "</Deklaracja>".Length));

                    XmlNodeList pusteWęzły = dokumentXml.SelectNodes("//*[count(@*) = 0 and count(child::*) = 0 and not(text())]");

                    if (pusteWęzły == null) return dokumentXml.OuterXml;

                    foreach (XmlNode pustyWęzeł in pusteWęzły)
                    {
                        XmlNode parent = pustyWęzeł.ParentNode;

                        if (parent != null)
                            parent.RemoveChild(pustyWęzeł);
                    }

                    return dokumentXml.OuterXml;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
    }
}