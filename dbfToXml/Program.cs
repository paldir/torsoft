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
        static XmlDocument dokumentXml;
        //static object[] itemArray;
        //static Dictionary<string, int> columnNameToOrdinal;

        static void Main(string[] args)
        {
            using (OleDbConnection połączenie = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\paldir\Documents\GitHub\dbfToXmlConverter\dbfToXml\bin\Debug\DBF;Extended Properties=dBASE IV;"))
            {
                połączenie.Open();

                DataTable fk;
                DataTable fkList;

                {
                    OleDbCommand komendaFk = new OleDbCommand("SELECT * FROM _FK.DBF", połączenie);
                    OleDbCommand komendaFkList = new OleDbCommand("SELECT * FROM _FK_LIST.DBF", połączenie);
                    DataSet daneFk = new DataSet();
                    DataSet daneFkList = new DataSet();
                    OleDbDataAdapter adapterFk = new OleDbDataAdapter(komendaFk);
                    OleDbDataAdapter adapterFkList = new OleDbDataAdapter(komendaFkList);

                    adapterFk.Fill(daneFk);
                    adapterFkList.Fill(daneFkList);

                    fk = daneFk.Tables[0];
                    fkList = daneFkList.Tables[0];
                }

                Dictionary<string, int> nazwaKolumnyDoJejNumeruFk = new Dictionary<string, int>();
                Dictionary<string, int> nazwaKolumnyDoJejNumeruFkList = new Dictionary<string, int>();

                foreach (DataColumn kolumna in fk.Columns)
                    nazwaKolumnyDoJejNumeruFk.Add(kolumna.ColumnName.ToLower(), kolumna.Ordinal);

                foreach (DataColumn kolumna in fkList.Columns)
                    nazwaKolumnyDoJejNumeruFkList.Add(kolumna.ColumnName.ToLower(), kolumna.Ordinal);

                /*using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("index.html"))
                {
                    streamWriter.Write("<html><head><title></title></head><body><table border='1'><tr>");

                    foreach (string column in fkListColumnNameToOrdinal.Keys)
                        streamWriter.Write(String.Format("<th>{0}</th>", column));

                    streamWriter.Write("</tr>");

                    foreach (DataRow dataRow in fkListTable.Rows)
                    {
                        streamWriter.Write("<tr>");

                        foreach (object field in dataRow.ItemArray)
                            streamWriter.Write(String.Format("<td>{0}</td>", field));

                        streamWriter.Write("</tr>");
                    }

                    streamWriter.Write("</table></body></html>");
                }*/

                dokumentXml = new XmlDocument();

                dokumentXml.Load("wzor_fk.xml");

                foreach (DataRow wiersz in fk.Rows)
                {
                    object[] polaFk = wiersz.ItemArray;
                    string kod, miasto, ulica;
                    string plik = "1.xml";
                    double id = (double)polaFk[nazwaKolumnyDoJejNumeruFk["nr_system"]];

                    ZapiszWXml("akronim", polaFk[nazwaKolumnyDoJejNumeruFk["indeks_kon"]]);
                    ZapiszWXml("nip", polaFk[nazwaKolumnyDoJejNumeruFk["nr_ident"]]);
                    ZapiszWXml("numer", polaFk[nazwaKolumnyDoJejNumeruFk["nr_fk"]]);
                    ZapiszWXml("kwota_plat", polaFk[nazwaKolumnyDoJejNumeruFk["wartosc"]], false);
                    ZapiszWXml("kwota_pln_plat", polaFk[nazwaKolumnyDoJejNumeruFk["wartosc"]], false);
                    ZapiszWXml("nazwa1", polaFk[nazwaKolumnyDoJejNumeruFk["platnik"]]);
                    ZapiszWXml("nazwa2", polaFk[nazwaKolumnyDoJejNumeruFk["platnik1"]]);

                    AnalizujAdres(String.Format("{0};{1}", polaFk[nazwaKolumnyDoJejNumeruFk["adres1"]], polaFk[nazwaKolumnyDoJejNumeruFk["adres2"]]), out kod, out miasto, out ulica);
                    ZapiszWXml("kod_pocztowy", kod);
                    ZapiszWXml("miasto", miasto);
                    ZapiszWXml("miasto", miasto);

                    ZapiszWXml("fiskalna", ZamieńBoolNaPolskąNazwę(polaFk[nazwaKolumnyDoJejNumeruFk["fisk"]]), false);
                    ZapiszWXml("root/rejestry_sprzedazy_vat/rejestr_sprzedazy_vat/eksport", ZamieńBoolNaPolskąNazwę(polaFk[nazwaKolumnyDoJejNumeruFk["exported"]]), false);

                    DateTime data = (DateTime)polaFk[nazwaKolumnyDoJejNumeruFk["data"]];
                    string napisDaty = data.ToShortDateString();
                    string napisTerminu = data.AddDays((double)polaFk[nazwaKolumnyDoJejNumeruFk["termin"]]).ToShortDateString();
                    ZapiszWXml("data_wystawienia", napisDaty);
                    ZapiszWXml("data_dataobowiazkupodatkowego", napisDaty);
                    ZapiszWXml("data_dataprawaodliczenia", napisDaty);
                    ZapiszWXml("termin", napisTerminu);
                    ZapiszWXml("termin_plat", napisTerminu);

                    ZapiszWXml("data_sprzedazy", ((DateTime)polaFk[nazwaKolumnyDoJejNumeruFk["data_sprz"]]).ToShortDateString());

                    XmlNode pozycje = dokumentXml.GetElementsByTagName("POZYCJE")[0];
                    XmlNode wzórPozycji = dokumentXml.GetElementsByTagName("POZYCJA")[0];

                    foreach (DataRow wierszFkList in fkList.Rows)
                    {
                        object[] polaFkList = wierszFkList.ItemArray;

                        if ((double)polaFkList[nazwaKolumnyDoJejNumeruFkList["nr_system"]] == id)
                            DodajPozycję(pozycje, wzórPozycji, Convert.ToDecimal(polaFkList[nazwaKolumnyDoJejNumeruFkList["vat"]]), String.Empty, 0, 0, String.Empty, false);
                    }

                    wzórPozycji.ParentNode.RemoveChild(wzórPozycji);
                    dokumentXml.Save(plik);

                    string xml;

                    using (StreamReader strumień = new StreamReader(plik, Encoding.GetEncoding("Windows-1250")))
                        xml = strumień.ReadToEnd();

                    using (StreamWriter strumień = new StreamWriter(plik))
                        strumień.Write(xml.Replace("&lt;", "<").Replace("&gt;", ">"));

                    break;
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
            węzełPozycji.ChildNodes[5].InnerText = vat.ToString();
            węzełPozycji.ChildNodes[6].InnerText = rodzajSprzedaży.Trim();
            węzełPozycji.ChildNodes[7].InnerText = ZamieńBoolNaPolskąNazwę(uwzględnionoWProporcji);

            rodzic.AppendChild(węzełPozycji);
        }
    }
}