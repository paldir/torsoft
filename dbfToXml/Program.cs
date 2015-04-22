using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Data;
using System.Xml;
using System.IO;

namespace dbfToXml
{
    class Program
    {
        const int NumerPierwszejFaktury = 1;
        const int NumerOstatniejFaktury = 1;

        static XmlDocument dokumentXml;

        static void Main(string[] args)
        {
            var tmp = Environment.CurrentDirectory;

            using (OleDbConnection połączenie = new OleDbConnection(String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;", Path.Combine(Environment.CurrentDirectory, "DBF"))))
            {
                połączenie.Open();

                DataTable fk;
                //DataTable fkList;
                DataTable fkVat;

                {
                    OleDbCommand komendaFk = new OleDbCommand("SELECT * FROM _FK.DBF", połączenie);
                    //OleDbCommand komendaFkList = new OleDbCommand("SELECT * FROM _FK_LIST.DBF", połączenie);
                    OleDbCommand komendaFkVat = new OleDbCommand("SELECT * FROM _FK_VAT.DBF", połączenie);
                    DataSet daneFk = new DataSet();
                    //DataSet daneFkList = new DataSet();
                    DataSet daneFkVat = new DataSet();
                    OleDbDataAdapter adapterFk = new OleDbDataAdapter(komendaFk);
                    //OleDbDataAdapter adapterFkList = new OleDbDataAdapter(komendaFkList);
                    OleDbDataAdapter adapterFkVat = new OleDbDataAdapter(komendaFkVat);

                    adapterFk.Fill(daneFk);
                    //adapterFkList.Fill(daneFkList);
                    adapterFkVat.Fill(daneFkVat);

                    fk = daneFk.Tables[0];
                    //fkList = daneFkList.Tables[0];
                    fkVat = daneFkVat.Tables[0];
                }

                Dictionary<string, int> nazwaKolumnyDoJejNumeruFk = new Dictionary<string, int>();
                //Dictionary<string, int> nazwaKolumnyDoJejNumeruFkList = new Dictionary<string, int>();
                Dictionary<string, int> nazwaKolumnyDoJejNumeruFkVat = new Dictionary<string, int>();

                foreach (DataColumn kolumna in fk.Columns)
                    nazwaKolumnyDoJejNumeruFk.Add(kolumna.ColumnName.ToLower(), kolumna.Ordinal);

                //foreach (DataColumn kolumna in fkList.Columns)
                //    nazwaKolumnyDoJejNumeruFkList.Add(kolumna.ColumnName.ToLower(), kolumna.Ordinal);

                foreach (DataColumn kolumna in fkVat.Columns)
                    nazwaKolumnyDoJejNumeruFkVat.Add(kolumna.ColumnName.ToLower(), kolumna.Ordinal);

                if (!Directory.Exists("XML"))
                    Directory.CreateDirectory("XML");

                foreach (DataRow wiersz in fk.Rows)
                {
                    object[] polaFk = wiersz.ItemArray;
                    string napisNumeruFaktury = polaFk[nazwaKolumnyDoJejNumeruFk["nr_fk"]].ToString();
                    int numerFaktury = Int32.Parse(napisNumeruFaktury.Substring(0, napisNumeruFaktury.IndexOf("/")));

                    if (numerFaktury >= NumerPierwszejFaktury && numerFaktury <= NumerOstatniejFaktury)
                    {
                        dokumentXml = new XmlDocument();
                        string kod, miasto, ulica;
                        string plik = Path.Combine("XML", String.Format("{0}.xml", numerFaktury));
                        double id = (double)polaFk[nazwaKolumnyDoJejNumeruFk["nr_system"]];

                        dokumentXml.Load("wzor_fk.xml");

                        #region stałe
                        ZapiszWXml("wersja", "2.00", false);
                        ZapiszWXml("baza_doc_id", "K1", false);
                        ZapiszWXml("rodzaj", "dostawca", false);
                        ZapiszWXml("kraj_iso", "PL");
                        ZapiszWXml("/root/kontrahenci/kontrahent/eksport", "krajowy");
                        ZapiszWXml("platnik_vat", "Tak");
                        ZapiszWXml("status", "aktualny", false);
                        ZapiszWXml("nip_kraj", "PL");
                        ZapiszWXml("kraj", "Polska");
                        ZapiszWXml("finalny", "Nie", false);
                        ZapiszWXml("modul", "Rejestr Vat", false);
                        ZapiszWXml("typ", "Rejestr sprzedaży", false);
                        ZapiszWXml("rejestr", "SPRZEDAŻ");
                        ZapiszWXml("korekta", "Nie", false);
                        ZapiszWXml("wewnetrzna", "Nie", false);
                        ZapiszWXml("detaliczna", "Nie", false);
                        ZapiszWXml("podatnik_czynny", "Tak", false);
                        ZapiszWXml("typ_podmiotu", "kontrahent");
                        ZapiszWXml("kierunek", "przychód", false);
                        ZapiszWXml("kod_atr", "SALDEO", false);
                        #endregion

                        #region przepisywane
                        ZapiszWXml("akronim", polaFk[nazwaKolumnyDoJejNumeruFk["indeks_kon"]]);
                        ZapiszWXml("podmiot", polaFk[nazwaKolumnyDoJejNumeruFk["indeks_kon"]]);
                        ZapiszWXml("nip", polaFk[nazwaKolumnyDoJejNumeruFk["nr_ident"]]);
                        ZapiszWXml("numer", napisNumeruFaktury);
                        ZapiszWXml("kwota_plat", polaFk[nazwaKolumnyDoJejNumeruFk["wartosc"]], false);
                        ZapiszWXml("kwota_pln_plat", polaFk[nazwaKolumnyDoJejNumeruFk["wartosc"]], false);
                        ZapiszWXml("nazwa1", polaFk[nazwaKolumnyDoJejNumeruFk["platnik"]]);
                        ZapiszWXml("nazwa2", polaFk[nazwaKolumnyDoJejNumeruFk["platnik1"]]);
                        ZapiszWXml("forma_platnosci", polaFk[nazwaKolumnyDoJejNumeruFk["spos_zap"]]);
                        ZapiszWXml("forma_platnosci_plat", polaFk[nazwaKolumnyDoJejNumeruFk["spos_zap"]]);
                        #endregion

                        #region adres
                        AnalizujAdres(String.Format("{0};{1}", polaFk[nazwaKolumnyDoJejNumeruFk["adres1"]], polaFk[nazwaKolumnyDoJejNumeruFk["adres2"]]), out kod, out miasto, out ulica);
                        ZapiszWXml("kod_pocztowy", kod);
                        ZapiszWXml("miasto", miasto);
                        ZapiszWXml("miasto", miasto);
                        #endregion

                        #region boolowskie
                        ZapiszWXml("fiskalna", ZamieńBoolNaPolskąNazwę(polaFk[nazwaKolumnyDoJejNumeruFk["fisk"]]), false);
                        ZapiszWXml("root/rejestry_sprzedazy_vat/rejestr_sprzedazy_vat/eksport", ZamieńBoolNaPolskąNazwę(polaFk[nazwaKolumnyDoJejNumeruFk["exported"]]), false);
                        #endregion

                        #region data
                        DateTime data = (DateTime)polaFk[nazwaKolumnyDoJejNumeruFk["data"]];
                        string napisDaty = data.ToShortDateString();
                        string napisTerminu = data.AddDays((double)polaFk[nazwaKolumnyDoJejNumeruFk["termin"]]).ToShortDateString();
                        ZapiszWXml("data_wystawienia", napisDaty);
                        ZapiszWXml("data_dataobowiazkupodatkowego", napisDaty);
                        ZapiszWXml("data_dataprawaodliczenia", napisDaty);
                        ZapiszWXml("data_kursu", napisDaty);
                        ZapiszWXml("data_kursu_2", napisDaty);
                        ZapiszWXml("termin", napisTerminu);
                        ZapiszWXml("termin_plat", napisTerminu);

                        ZapiszWXml("data_sprzedazy", ((DateTime)polaFk[nazwaKolumnyDoJejNumeruFk["data_sprz"]]).ToShortDateString());
                        #endregion

                        XmlNode pozycje = dokumentXml.GetElementsByTagName("POZYCJE")[0];
                        XmlNode wzórPozycji = dokumentXml.GetElementsByTagName("POZYCJA")[0];

                        foreach (DataRow wierszFkVat in fkVat.Rows)
                        {
                            object[] polaFkVat = wierszFkVat.ItemArray;

                            if ((double)polaFkVat[nazwaKolumnyDoJejNumeruFkVat["nr_system"]] == id)
                            {
                                decimal wartość = Convert.ToDecimal(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["wartosc"]]);

                                if (wartość != 0)
                                    DodajPozycję(pozycje, wzórPozycji, Convert.ToDecimal(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["vat"]]), "opodatkowana", wartość, Convert.ToDecimal(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["pod_wart"]]), "towary", false);
                            }
                        }

                        wzórPozycji.ParentNode.RemoveChild(wzórPozycji);
                        dokumentXml.Save(plik);

                        string xml;

                        using (StreamReader strumień = new StreamReader(plik, Encoding.GetEncoding("Windows-1250")))
                            xml = strumień.ReadToEnd();

                        using (StreamWriter strumień = new StreamWriter(plik))
                            strumień.Write(xml.Replace("&lt;", "<").Replace("&gt;", ">"));
                    }
                }
            }
        }

        static void ZapiszWXml(string tagXmlAlboXPath, object wartość, bool dodaćCData = true)
        {
            tagXmlAlboXPath = tagXmlAlboXPath.ToUpper();
            XmlNodeList węzły;
            string napisWartości = null;

            if (wartość != null)
                napisWartości = wartość.ToString().Trim();

            if (tagXmlAlboXPath.Contains('/'))
                węzły = dokumentXml.SelectNodes(tagXmlAlboXPath);
            else
                węzły = dokumentXml.GetElementsByTagName(tagXmlAlboXPath);

            if (dodaćCData)
                napisWartości = String.Format("<![CDATA[{0}]]>", napisWartości);

            foreach (XmlNode węzeł in węzły)
                węzeł.InnerText = napisWartości;
        }

        static void AnalizujAdres(string adres, out string kod, out string miasto, out string ulica)
        {
            int pierwszaCzęśćKoduPocztowego = 0;
            int drugaCzęśćKoduPocztowego = 0;
            bool kodPocztowyZnaleziony = false;
            int indeksŚrednika = adres.IndexOf(';');
            int indeksMiasta = -1;

            for (int i = 0; i < adres.Length && !kodPocztowyZnaleziony; i++)
                if (Char.IsNumber(adres[i]))
                    try
                    {
                        int indeksMyślnika = adres.IndexOf('-', i + 1);
                        pierwszaCzęśćKoduPocztowego = Int32.Parse(adres.Substring(i, indeksMyślnika - i));

                        for (int j = indeksMyślnika + 1; j < adres.Length - 1; j++)
                            if (!Char.IsNumber(adres[j + 1]))
                            {
                                drugaCzęśćKoduPocztowego = Int32.Parse(adres.Substring(indeksMyślnika + 1, j - indeksMyślnika + 1));
                                indeksMiasta = j + 1;
                                kodPocztowyZnaleziony = true;

                                break;
                            }
                    }
                    catch { }

            if (kodPocztowyZnaleziony)
                kod = String.Format("{0}-{1}", pierwszaCzęśćKoduPocztowego, drugaCzęśćKoduPocztowego);
            else
                kod = null;

            try
            {
                int indeksPrzecinka = adres.IndexOf(adres, indeksMiasta);

                if (indeksPrzecinka == -1)
                    indeksPrzecinka = Int32.MaxValue;

                miasto = adres.Substring(indeksMiasta, new int[] { indeksPrzecinka, indeksŚrednika, adres.Length }.Where(i => i > indeksMiasta).Min() - indeksMiasta).Trim();
            }
            catch
            {
                miasto = null;
            }

            try
            {
                ulica = adres.Replace(kod, String.Empty).Replace(miasto, String.Empty).Replace(",", String.Empty).Replace(";", String.Empty);
                ulica = ulica.Substring(ulica.ToLower().IndexOf("ul.")).Trim();
            }
            catch
            {
                ulica = null;
            }
        }

        static string ZamieńBoolNaPolskąNazwę(object wartość)
        {
            bool wartośćBoolowska = Convert.ToBoolean(wartość);

            if (wartośćBoolowska)
                return "Tak";
            else
                return "Nie";
        }

        static void DodajPozycję(XmlNode rodzic, XmlNode wzór, decimal stawkaVat, string statusVat, decimal netto, decimal vat, string rodzajSprzedaży, bool uwzględnionoWProporcji)
        {
            XmlNode węzełPozycji = wzór.CloneNode(true);

            węzełPozycji.ChildNodes[0].InnerText = stawkaVat.ToString();
            węzełPozycji.ChildNodes[1].InnerText = statusVat.Trim();
            węzełPozycji.ChildNodes[2].InnerText = węzełPozycji.ChildNodes[3].InnerText = węzełPozycji.ChildNodes[4].InnerText = netto.ToString();
            węzełPozycji.ChildNodes[5].InnerText = węzełPozycji.ChildNodes[6].InnerText = węzełPozycji.ChildNodes[7].InnerText = vat.ToString();
            węzełPozycji.ChildNodes[8].InnerText = rodzajSprzedaży.Trim();
            węzełPozycji.ChildNodes[9].InnerText = ZamieńBoolNaPolskąNazwę(uwzględnionoWProporcji);

            rodzic.AppendChild(węzełPozycji);
        }

        static void ZapiszWHtml(DataTable tabela, Dictionary<string, int> nazwaKolumnyDoJejNumeru)
        {
            using (System.IO.StreamWriter strumień = new System.IO.StreamWriter("dbf.html"))
            {
                strumień.Write("<html><head><title></title></head><body><table border='1'><tr>");

                foreach (string kolumna in nazwaKolumnyDoJejNumeru.Keys)
                    strumień.Write(String.Format("<th>{0}</th>", kolumna));

                strumień.Write("</tr>");

                foreach (DataRow dataRow in tabela.Rows)
                {
                    strumień.Write("<tr>");

                    foreach (object field in dataRow.ItemArray)
                        strumień.Write(String.Format("<td>{0}</td>", field));

                    strumień.Write("</tr>");
                }

                strumień.Write("</table></body></html>");
            }
        }
    }
}