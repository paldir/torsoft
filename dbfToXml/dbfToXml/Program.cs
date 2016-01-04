using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
using System.Data;
using System.Xml;
using System.IO;
using System.Threading;
using System.Globalization;

namespace dbfToXml
{
    class Program
    {
        static int _numerPierwszejFaktury;
        static int _numerOstatniejFaktury;
        static XmlDocument _dokumentXml;

        /*const string FormatDaty = "{0:yyyy-MM-dd}";
        const string FormatDatyAmerykański = "{0:yyyy-dd-MM}";
        const string FormatDatyOdwrotny = "{0:dd-MM-yyyy}";*/
        const string FormatDatyAmerykańskiOdwrotny = "{0:MM-dd-yyyy}";

        static void Main(string[] args)
        {
            CultureInfo infoOKulturze = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
            string format = FormatDatyAmerykańskiOdwrotny;

            if (infoOKulturze != null)
            {
                infoOKulturze.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = infoOKulturze;
            }

            switch (args.Length)
            {
                case 0:
                    _numerPierwszejFaktury = 1;
                    _numerOstatniejFaktury = Int32.MaxValue;

                    break;

                case 1:
                    _numerPierwszejFaktury = _numerOstatniejFaktury = Int32.Parse(args[0]);

                    break;

                case 2:
                    _numerPierwszejFaktury = Int32.Parse(args[0]);
                    _numerOstatniejFaktury = Int32.Parse(args[1]);

                    break;

                default:
                    _numerPierwszejFaktury = _numerOstatniejFaktury = 0;

                    break;
            }

            using (OleDbConnection połączenie = new OleDbConnection(String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;", Environment.CurrentDirectory)))
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

                if (!Directory.Exists("fk"))
                    Directory.CreateDirectory("fk");

                foreach (DataRow wiersz in fk.Rows)
                {
                    object[] polaFk = wiersz.ItemArray;
                    string napisNumeruFaktury = polaFk[nazwaKolumnyDoJejNumeruFk["nr_fk"]].ToString();
                    int numerFaktury = Int32.Parse(napisNumeruFaktury.Substring(0, napisNumeruFaktury.IndexOf("/", StringComparison.Ordinal)));

                    if (numerFaktury >= _numerPierwszejFaktury && numerFaktury <= _numerOstatniejFaktury)
                    {
                        _dokumentXml = new XmlDocument();
                        string plik = Path.Combine("fk", String.Format("fk{0}.xml", numerFaktury));

                        _dokumentXml.Load("wzor_fk.xml");

                        XmlNode wzórKontrahenta = _dokumentXml.GetElementsByTagName("KONTRAHENT")[0];
                        XmlNode wzórRejestru = _dokumentXml.GetElementsByTagName("REJESTR_SPRZEDAZY_VAT")[0];
                        XmlNode wzórPozycji = _dokumentXml.GetElementsByTagName("POZYCJA")[0];
                        string kod, miasto, ulica;
                        double id = Convert.ToDouble(polaFk[nazwaKolumnyDoJejNumeruFk["nr_system"]]);
                        XmlNode kontrahent = wzórKontrahenta.CloneNode(true);
                        XmlNode rejestr = wzórRejestru.CloneNode(true);
                        DateTime data = Convert.ToDateTime(polaFk[nazwaKolumnyDoJejNumeruFk["data"]]);
                        string napisDaty = String.Format(format, data);
                        string napisTerminu = String.Format(format, data.AddDays(Convert.ToDouble(polaFk[nazwaKolumnyDoJejNumeruFk["termin"]])));
                        string nip = polaFk[nazwaKolumnyDoJejNumeruFk["nr_ident"]].ToString().Replace("-", String.Empty);
                        string sposóbPłatności = polaFk[nazwaKolumnyDoJejNumeruFk["spos_zap"]].ToString().ToLower().Replace("em", String.Empty);
                        string kwota = String.Format("{0:N2}", polaFk[nazwaKolumnyDoJejNumeruFk["wartosc"]]);

                        if (sposóbPłatności.Length == 0)
                            sposóbPłatności = "gotówka";

                        Console.WriteLine("Generowanie XML dla faktury nr {0}.", numerFaktury);
                        AnalizujAdres(String.Format("{0};{1}", polaFk[nazwaKolumnyDoJejNumeruFk["adres1"]], polaFk[nazwaKolumnyDoJejNumeruFk["adres2"]]), out kod, out miasto, out ulica);

                        ZapiszWWęźleXml(ref kontrahent, "akronim", polaFk[nazwaKolumnyDoJejNumeruFk["indeks_kon"]], true);
                        ZapiszWWęźleXml(ref kontrahent, "nazwa1", polaFk[nazwaKolumnyDoJejNumeruFk["platnik"]], true);
                        ZapiszWWęźleXml(ref kontrahent, "nazwa2", polaFk[nazwaKolumnyDoJejNumeruFk["platnik1"]], true);
                        ZapiszWWęźleXml(ref kontrahent, "ulica", ulica, true);
                        ZapiszWWęźleXml(ref kontrahent, "miasto", miasto, true);
                        ZapiszWWęźleXml(ref kontrahent, "kod_pocztowy", kod, true);
                        ZapiszWWęźleXml(ref kontrahent, "nip", nip, true);

                        wzórKontrahenta.ParentNode.AppendChild(kontrahent);

                        ZapiszWWęźleXml(ref rejestr, "data_wystawienia", napisDaty, true);
                        ZapiszWWęźleXml(ref rejestr, "data_sprzedazy", String.Format(format, Convert.ToDateTime(polaFk[nazwaKolumnyDoJejNumeruFk["data_sprz"]])), true);
                        ZapiszWWęźleXml(ref rejestr, "data_dataobowiazkupodatkowego", napisDaty, true);
                        ZapiszWWęźleXml(ref rejestr, "data_dataprawaodliczenia", napisDaty, true);
                        ZapiszWWęźleXml(ref rejestr, "termin", napisTerminu, true);
                        ZapiszWWęźleXml(ref rejestr, "forma_platnosci", sposóbPłatności, true);
                        ZapiszWWęźleXml(ref rejestr, "numer", napisNumeruFaktury, true);
                        ZapiszWWęźleXml(ref rejestr, "fiskalna", ZamieńBoolNaPolskąNazwę(polaFk[nazwaKolumnyDoJejNumeruFk["fisk"]]), false);
                        ZapiszWWęźleXml(ref rejestr, "data_kursu", napisDaty, true);
                        ZapiszWWęźleXml(ref rejestr, "data_kursu_2", napisDaty, true);
                        ZapiszWWęźleXml(ref rejestr, "podmiot", polaFk[nazwaKolumnyDoJejNumeruFk["indeks_kon"]], true);
                        ZapiszWWęźleXml(ref rejestr, "nip", nip, true);
                        ZapiszWWęźleXml(ref rejestr, "ulica", ulica, true);
                        ZapiszWWęźleXml(ref rejestr, "miasto", miasto, true);
                        ZapiszWWęźleXml(ref rejestr, "kod_pocztowy", kod, true);

                        XmlNode pozycje = rejestr.SelectSingleNode("POZYCJE");

                        foreach (DataRow wierszFkVat in fkVat.Rows)
                        {
                            object[] polaFkVat = wierszFkVat.ItemArray;

                            if (Math.Abs(Convert.ToDouble(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["nr_system"]]) - id) < 0.1)
                            {
                                decimal wartość = Convert.ToDecimal(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["wartosc"]]);

                                if (wartość != 0)
                                    DodajPozycję(pozycje, wzórPozycji.CloneNode(true), Convert.ToDecimal(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["vat"]]), "opodatkowana", wartość, Convert.ToDecimal(polaFkVat[nazwaKolumnyDoJejNumeruFkVat["pod_wart"]]), "towary", false);
                            }
                        }

                        ZapiszWWęźleXml(ref rejestr, "termin_plat", napisTerminu, true);
                        ZapiszWWęźleXml(ref rejestr, "forma_platnosci_plat", sposóbPłatności, true);
                        ZapiszWWęźleXml(ref rejestr, "kwota_plat", kwota, false);
                        ZapiszWWęźleXml(ref rejestr, "kwota_pln_plat", kwota, false);

                        wzórRejestru.ParentNode.AppendChild(rejestr);

                        #region stałe
                        ZapiszWXml("wersja", "2.00", false);
                        ZapiszWXml("baza_doc_id", "K1", false);
                        ZapiszWXml("rodzaj", "dostawca", false);
                        ZapiszWXml("kraj_iso", "PL", true);
                        ZapiszWXml("/root/kontrahenci/kontrahent/eksport", "krajowy", true);
                        ZapiszWXml("platnik_vat", "Tak", true);
                        ZapiszWXml("status", "aktualny", false);
                        ZapiszWXml("nip_kraj", "PL", true);
                        ZapiszWXml("kraj", "Polska", true);
                        ZapiszWXml("finalny", "Nie", false);
                        ZapiszWXml("modul", "Rejestr Vat", false);
                        ZapiszWXml("typ", "Rejestr sprzedazy", false);
                        ZapiszWXml("rejestr", "SPRZEDAŻ", true);
                        ZapiszWXml("korekta", "Nie", false);
                        ZapiszWXml("wewnetrzna", "Nie", false);
                        ZapiszWXml("detaliczna", "Nie", false);
                        ZapiszWXml("podatnik_czynny", "Tak", false);
                        ZapiszWXml("typ_podmiotu", "kontrahent", true);
                        ZapiszWXml("kierunek", "przychód", false);
                        //ZapiszWXml("kod_atr", "SALDEO", false);
                        ZapiszWXml("nazwa3", String.Empty, true);
                        ZapiszWXml("telefon1", String.Empty, true);
                        ZapiszWXml("email", String.Empty, true);
                        ZapiszWXml("korekta_numer", String.Empty, true);
                        ZapiszWXml("root/rejestry_sprzedazy_vat/rejestr_sprzedazy_vat/eksport", "nie", false);
                        ZapiszWXml("waluta_dok", String.Empty, true);
                        ZapiszWXml("baza_zrd_id", String.Empty, false);
                        #endregion

                        wzórKontrahenta.ParentNode.RemoveChild(wzórKontrahenta);
                        wzórRejestru.ParentNode.RemoveChild(wzórRejestru);

                        foreach (XmlNode węzełPozycji in _dokumentXml.GetElementsByTagName("POZYCJE"))
                            węzełPozycji.RemoveChild(węzełPozycji.ChildNodes[0]);

                        using (StreamWriter strumień = new StreamWriter(plik, false, Encoding.GetEncoding("Windows-1250")))
                            _dokumentXml.Save(strumień);
                    }
                }

                Console.WriteLine("Zakończono.");
            }
        }

        static void ZapiszWXml(string tagXmlAlboXPath, object wartość, bool dodaćCData)
        {
            tagXmlAlboXPath = tagXmlAlboXPath.ToUpper();
            XmlNodeList węzły;
            string napisWartości = null;
            XmlNode zawartość;

            if (wartość != null)
                napisWartości = wartość.ToString().Trim();

            if (tagXmlAlboXPath.IndexOf('/') != -1)
                węzły = _dokumentXml.SelectNodes(tagXmlAlboXPath);
            else
                węzły = _dokumentXml.GetElementsByTagName(tagXmlAlboXPath);

            if (dodaćCData)
                zawartość = _dokumentXml.CreateCDataSection(napisWartości);
            else
                zawartość = _dokumentXml.CreateTextNode(napisWartości);

            if (węzły != null)
                foreach (XmlNode węzeł in węzły)
                    węzeł.AppendChild(zawartość.CloneNode(true));
        }

        static void ZapiszWWęźleXml(ref XmlNode węzeł, string tagXml, object wartość, bool dodaćCData)
        {
            tagXml = tagXml.ToUpper();
            string napisWartości = null;
            XmlNode zawartość;

            if (wartość != null)
                napisWartości = wartość.ToString().Trim();

            XmlDocument xml = new XmlDocument();

            if (dodaćCData)
                zawartość = xml.CreateCDataSection(napisWartości);
            else
                zawartość = xml.CreateTextNode(napisWartości);

            xml.AppendChild(xml.ImportNode(węzeł, true));

            foreach (XmlNode element in xml.GetElementsByTagName(tagXml))
                element.AppendChild(zawartość.CloneNode(true));

            węzeł = _dokumentXml.ImportNode(xml.FirstChild, true);
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
                int indeksPrzecinka = adres.IndexOf(adres, indeksMiasta, StringComparison.Ordinal);

                if (indeksPrzecinka == -1)
                    indeksPrzecinka = Int32.MaxValue;

                List<int> indeksy = new List<int>() { indeksPrzecinka, indeksŚrednika, adres.Length };

                indeksy.RemoveAll(i => i <= indeksMiasta);

                int minimum = Int32.MaxValue;

                foreach (int indeks in indeksy)
                    if (indeks < minimum)
                        minimum = indeks;

                miasto = adres.Substring(indeksMiasta, minimum - indeksMiasta).Trim();
            }
            catch
            {
                miasto = null;
            }

            try
            {
                ulica = adres.Replace(kod, String.Empty).Replace(miasto, String.Empty).Replace(",", String.Empty).Replace(";", String.Empty);
                ulica = ulica.Substring(ulica.ToLower().IndexOf("ul.", StringComparison.Ordinal)).Trim();
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

        static void DodajPozycję(XmlNode rodzic, XmlNode węzełPozycji, decimal stawkaVat, string statusVat, decimal netto, decimal vat, string rodzajSprzedaży, bool uwzględnionoWProporcji)
        {
            węzełPozycji.ChildNodes[0].InnerText = stawkaVat.ToString();
            węzełPozycji.ChildNodes[1].InnerText = statusVat.Trim();
            węzełPozycji.ChildNodes[2].InnerText = węzełPozycji.ChildNodes[3].InnerText = węzełPozycji.ChildNodes[4].InnerText = String.Format("{0:N2}", netto);
            węzełPozycji.ChildNodes[5].InnerText = węzełPozycji.ChildNodes[6].InnerText = węzełPozycji.ChildNodes[7].InnerText = String.Format("{0:N2}", vat);
            węzełPozycji.ChildNodes[8].InnerText = rodzajSprzedaży.Trim();
            węzełPozycji.ChildNodes[9].InnerText = ZamieńBoolNaPolskąNazwę(uwzględnionoWProporcji);

            rodzic.AppendChild(węzełPozycji);
        }

        static void ZapiszWHtml(DataTable tabela, Dictionary<string, int> nazwaKolumnyDoJejNumeru)
        {
            using (StreamWriter strumień = new StreamWriter("dbf.html"))
            {
                strumień.Write("<html><head><title></title></head><body><table border='1'><tr>");

                foreach (string kolumna in nazwaKolumnyDoJejNumeru.Keys)
                    strumień.Write("<th>{0}</th>", kolumna);

                strumień.Write("</tr>");

                foreach (DataRow dataRow in tabela.Rows)
                {
                    strumień.Write("<tr>");

                    foreach (object field in dataRow.ItemArray)
                        strumień.Write("<td>{0}</td>", field);

                    strumień.Write("</tr>");
                }

                strumień.Write("</table></body></html>");
            }
        }
    }
}