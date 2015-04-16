using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Data;
using System.Xml;

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
                    SaveItemToXml("akronim", itemArray[columnNameToOrdinal["indeks_kon"]], true);
                    SaveItemToXml("nip", itemArray[columnNameToOrdinal["nr_ident"]], true);
                    SaveItemToXml("numer", itemArray[columnNameToOrdinal["nr_fk"]], true);
                    SaveItemToXml("kwota_plat", itemArray[columnNameToOrdinal["wartosc"]], false);
                    SaveItemToXml("kwota_pln_plat", itemArray[columnNameToOrdinal["wartosc"]], false);

                    break;
                }

                xmlDocument.Save("1.xml");
            }
        }

        static void SaveItemToXml(string tagName, object value, bool includeCDataHashes)
        {
            tagName = tagName.ToUpper();
            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName(tagName);
            string valueString = value.ToString().Trim();

            if (includeCDataHashes)
                valueString = String.Format("<![CDATA[{0}]]>", valueString);

            foreach (XmlNode xmlNode in xmlNodeList)
                xmlNode.InnerText = valueString;
        }
    }
}