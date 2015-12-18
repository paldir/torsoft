using System;
using System.Collections.Generic;

using System.IO;
using iTextSharp.text.pdf;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace PdfBrowser
{
    class PdfFile
    {
        #region pola prywatne
        static string[] _ścieżkiWszystkichPlikówTxt;
        static string[] _ścieżkiWszystkichPlikówXades;
        static int _licznik;
        #endregion

        #region właściwości
        static int _rok;
        public static int Rok
        {
            get { return _rok; }

            set
            {
                int obecnyRok = DateTime.Now.Year;

                if (value < 2014 || value > obecnyRok)
                    _rok = obecnyRok;
                else
                    _rok = value;

                Katalog = Path.Combine("PITY_PDF", _rok.ToString());

                if (!Directory.Exists(Katalog))
                    Directory.CreateDirectory(Katalog);
            }
        }

        public static string Katalog { get; private set; }

        string _ścieżkaPdf;
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

        List<string> _ścieżkiTxt = new List<string>();
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

        string _ścieżkaUpo;
        public string ŚcieżkaUpo
        {
            get
            {
                if (UpoIstnieje)
                    return _ścieżkaUpo;
                else
                    return String.Empty;
            }

            private set { _ścieżkaUpo = value; }
        }

        string _numerReferencyjny;
        public string NumerReferencyjny
        {
            get
            {
                if (TxtIstnieje)
                    return _numerReferencyjny;
                else
                    return String.Empty;
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
                    return String.Empty;
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
        static void SetPrefixDeep(XmlNode xmlNode, string prefix)
        {
            xmlNode.Prefix = prefix;

            foreach (XmlNode child in xmlNode.ChildNodes)
                SetPrefixDeep(child, prefix);
        }

        bool SprawdźCzyIstniejeTxt()
        {
            if (Katalog != String.Empty)
            {
                List<DateTime> datyPlikówTxt = new List<DateTime>();
                List<string> ścieżkiPlikówTxt = new List<string>();

                foreach (string ścieżka in _ścieżkiWszystkichPlikówTxt)
                {
                    string nazwaPliku = Path.GetFileName(ścieżka);

                    if (nazwaPliku.StartsWith(Path.GetFileNameWithoutExtension(NazwaPliku)))
                    {
                        int indeksDaty = nazwaPliku.LastIndexOf("_") + 1;
                        int długośćDaty = 0;

                        for (int i = indeksDaty; i < nazwaPliku.Length; i++)
                            if (Char.IsLetter(nazwaPliku[i]))
                                break;
                            else
                                długośćDaty++;

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

            return false;
        }

        bool SprawdźCzyIstniejeUpo()
        {
            if (NumerReferencyjny != String.Empty)
                foreach (string ścieżkaPlikuXades in _ścieżkiWszystkichPlikówXades)
                    if (ścieżkaPlikuXades.IndexOf(NumerReferencyjny) != -1)
                    {
                        ŚcieżkaUpo = ścieżkaPlikuXades;

                        return true;
                    }

            return false;
        }

        string PobierzNumerReferencyjny()
        {
            if (TxtIstnieje)
            {
                string[] txt = File.ReadAllLines(ŚcieżkiTxt[0]);

                if (txt.Length > 0)
                {
                    string pierwszaLinia = txt[0];

                    return pierwszaLinia.Substring(pierwszaLinia.IndexOf(':') + 2);
                }
            }

            return String.Empty;
        }

        static void UsuńPusteWęzły(XmlNode węzełXml)
        {
            foreach (XmlNode węzełPotomny in węzełXml.ChildNodes)
                if (węzełPotomny.ChildNodes.Count == 0 && węzełPotomny.InnerText == String.Empty)
                    węzełXml.RemoveChild(węzełPotomny);
                else
                    UsuńPusteWęzły(węzełPotomny);

            //return xmlElement;
        }
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
                    xml = xml.Substring(xml.IndexOf("<xfa:data>") + "<xfa:data>".Length);

                    dokumentXml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + xml.Substring(0, xml.IndexOf("</Deklaracja>") + "</Deklaracja>".Length));

                    XmlNodeList pusteWęzły = dokumentXml.SelectNodes("//*[count(@*) = 0 and count(child::*) = 0 and not(text())]");

                    foreach (XmlNode pustyWęzeł in pusteWęzły)
                        pustyWęzeł.ParentNode.RemoveChild(pustyWęzeł);

                    return dokumentXml.OuterXml;
                }
            }
            catch { return String.Empty; }
        }
        #endregion
    }
}