using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace bazaUpsize
{
    class Program
    {
        const string nazwaPlikuBazy = "baza.xml";
        const string ścieżkaBaz = "C:/bazy";

        static void Main(string[] args)
        {
            XmlDocument plikBazy = new XmlDocument();

            plikBazy.Load(nazwaPlikuBazy);

            List<string> ścieżkiDbf = Directory.GetFiles(ścieżkaBaz, "*.dbf", SearchOption.AllDirectories).Select(s => s.ToLower()).ToList();

            foreach (string ścieżkaDbf in ścieżkiDbf)
            {
                string ścieżkaNtx = Path.ChangeExtension(ścieżkaDbf, "ntx");
                string nazwaTabeli = Path.GetFileNameWithoutExtension(ścieżkaDbf).ToLower();
                XmlNode węzełTabeli = plikBazy.SelectSingleNode(String.Format("/tabele/tabela[@nazwa='{0}']", nazwaTabeli));

                if (File.Exists(ścieżkaNtx) && węzełTabeli == null)
                {
                    XmlElement nowyWęzełTabela = plikBazy.CreateElement("tabela");
                    XmlAttribute atrybutNazwa = plikBazy.CreateAttribute("nazwa");
                    atrybutNazwa.Value = nazwaTabeli;
                    XmlElement nowyWęzełIndeks = plikBazy.CreateElement("indeks");
                    nowyWęzełIndeks.InnerText = Path.GetFileNameWithoutExtension(ścieżkaNtx);

                    nowyWęzełTabela.Attributes.Append(atrybutNazwa);
                    nowyWęzełTabela.AppendChild(nowyWęzełIndeks);
                    plikBazy.SelectSingleNode("/tabele").AppendChild(nowyWęzełTabela);
                }
            }

            plikBazy.Save(nazwaPlikuBazy);

            XElement root = XElement.Load("baza.xml");
            IEnumerable<XElement> tagiTableDoPosortowania = root.Elements("tabela");
            XElement[] posortowaneTagiTable = tagiTableDoPosortowania.OrderBy(t => t.Attribute("nazwa").ToString()).ToArray();

            foreach (XElement tagTable in posortowaneTagiTable)
            {
                IEnumerable<XElement> tagiOrder = tagTable.Elements("indeks");

                if (tagiOrder.Any())
                {
                    XElement[] posortowaneTagiOrder = tagiOrder.OrderBy(t => t.Value).ToArray();

                    tagTable.RemoveNodes();
                    tagTable.Add(posortowaneTagiOrder);
                }
            }

            tagiTableDoPosortowania.Remove();
            root.Add(posortowaneTagiTable);

            string xmlBezWielkichLiter = root.ToString().ToLower();

            using (StreamWriter strumień = new StreamWriter("baza.xml"))
                strumień.Write(xmlBezWielkichLiter);
        }
    }
}