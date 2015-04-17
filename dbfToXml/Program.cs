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
        static XmlDocument xmlDocument;
        //static object[] itemArray;
        //static Dictionary<string, int> columnNameToOrdinal;

        static void Main(string[] args)
        {
            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\paldir\Documents\GitHub\dbfToXmlConverter\dbfToXml\bin\Debug\DBF;Extended Properties=dBASE IV;"))
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand("SELECT * FROM _FK.DBF", connection);
                DataSet dataSet = new DataSet();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(command);

                dataAdapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];
                Dictionary<string, int> columnNameToOrdinal = new Dictionary<string, int>();

                foreach (DataColumn column in table.Columns)
                    columnNameToOrdinal.Add(column.ColumnName.ToLower(), column.Ordinal);

                /*using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("index.html"))
                {
                    streamWriter.Write("<html><head><title></title></head><body><table border='1'><tr>");

                    foreach (string column in columnNameToOrdinal.Values)
                        streamWriter.Write(String.Format("<th>{0}</th>", column));

                    streamWriter.Write("</tr>");

                    foreach (DataRow dataRow in table.Rows)
                    {
                        streamWriter.Write("<tr>");

                        foreach (object field in dataRow.ItemArray)
                            streamWriter.Write(String.Format("<td>{0}</td>", field));

                        streamWriter.Write("</tr>");
                    }

                    streamWriter.Write("</table></body></html>");
                }*/

                xmlDocument = new XmlDocument();

                xmlDocument.Load("wzor_fk.xml");

                foreach (DataRow dataRow in table.Rows)
                {
                    object[] itemArray = dataRow.ItemArray;
                    string kod, miasto, ulica;
                    string fileName = "1.xml";

                    SaveItemToXml("akronim", itemArray[columnNameToOrdinal["indeks_kon"]]);
                    SaveItemToXml("nip", itemArray[columnNameToOrdinal["nr_ident"]]);
                    SaveItemToXml("numer", itemArray[columnNameToOrdinal["nr_fk"]]);
                    SaveItemToXml("kwota_plat", itemArray[columnNameToOrdinal["wartosc"]], false);
                    SaveItemToXml("kwota_pln_plat", itemArray[columnNameToOrdinal["wartosc"]], false);
                    SaveItemToXml("nazwa1", itemArray[columnNameToOrdinal["platnik"]]);
                    SaveItemToXml("nazwa2", itemArray[columnNameToOrdinal["platnik1"]]);

                    ParseAddress(String.Format("{0};{1}", itemArray[columnNameToOrdinal["adres1"]], itemArray[columnNameToOrdinal["adres2"]]), out kod, out miasto, out ulica);
                    SaveItemToXml("kod_pocztowy", kod);
                    SaveItemToXml("miasto", miasto);
                    SaveItemToXml("miasto", miasto);

                    SaveItemToXml("fiskalna", BoolToPolishBool(itemArray[columnNameToOrdinal["fisk"]]), false);
                    SaveItemToXml("root/rejestry_sprzedazy_vat/rejestr_sprzedazy_vat/eksport", BoolToPolishBool(itemArray[columnNameToOrdinal["exported"]]), false);

                    xmlDocument.Save(fileName);

                    string xml;

                    using (StreamReader streamReader = new StreamReader(fileName, Encoding.GetEncoding("Windows-1250")))
                        xml = streamReader.ReadToEnd();

                    using (StreamWriter streamWriter = new StreamWriter(fileName))
                        streamWriter.Write(xml.Replace("&lt;", "<").Replace("&gt;", ">"));

                    break;
                }
            }
        }

        static void SaveItemToXml(string tagNameOrXPath, object value, bool includeCDataHashes = true)
        {
            tagNameOrXPath = tagNameOrXPath.ToUpper();
            XmlNodeList xmlNodeList;
            string valueString = value.ToString().Trim();

            if (tagNameOrXPath.Contains('/'))
                xmlNodeList = xmlDocument.SelectNodes(tagNameOrXPath);
            else
                xmlNodeList = xmlDocument.GetElementsByTagName(tagNameOrXPath);

            if (includeCDataHashes)
                valueString = String.Format("<![CDATA[{0}]]>", valueString);

            foreach (XmlNode xmlNode in xmlNodeList)
                xmlNode.InnerText = valueString;
        }

        static void ParseAddress(string adres, out string kod, out string miasto, out string ulica)
        {
            int firstPartOfPostalCode = 0;
            int secondPartOfPostalCode = 0;
            bool postalCodeFound = false;
            int semicolonIndex = adres.IndexOf(';');
            int cityIndex = -1;

            for (int i = 0; i < adres.Length && !postalCodeFound; i++)
                if (Char.IsNumber(adres[i]))
                    try
                    {
                        int dashIndex = adres.IndexOf('-', i + 1);
                        firstPartOfPostalCode = Int32.Parse(adres.Substring(i, dashIndex - i));

                        for (int j = dashIndex + 1; j < adres.Length - 1; j++)
                            if (!Char.IsNumber(adres[j + 1]))
                            {
                                secondPartOfPostalCode = Int32.Parse(adres.Substring(dashIndex + 1, j - dashIndex + 1));
                                cityIndex = j + 1;
                                postalCodeFound = true;

                                break;
                            }
                    }
                    catch { }

            if (postalCodeFound)
                kod = String.Format("{0}-{1}", firstPartOfPostalCode, secondPartOfPostalCode);
            else
                kod = null;

            try
            {
                int commaIndex = adres.IndexOf(adres, cityIndex);

                if (commaIndex == -1)
                    commaIndex = Int32.MaxValue;

                miasto = adres.Substring(cityIndex, new int[] { commaIndex, semicolonIndex, adres.Length }.Where(i => i > cityIndex).Min() - cityIndex).Trim();
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

        static string BoolToPolishBool(object value)
        {
            bool boolValue = Convert.ToBoolean(value);

            if (boolValue)
                return "Tak";
            else
                return "Nie";
        }
    }
}