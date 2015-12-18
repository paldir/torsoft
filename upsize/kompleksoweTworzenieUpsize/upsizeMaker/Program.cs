using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace upsizeMaker
{
    class Program
    {
        const string Ścieżka = @"C:\bazy";
        const string NazwaNowegoPlikuUpsize = "czynsze.upsize.xml";

        static void Main(string[] args)
        {
            {
                List<string> ścieżkiDbf = Directory.GetFiles(Ścieżka, "*.dbf", SearchOption.AllDirectories).Select(s => s.ToLower()).ToList();
                List<string> ścieżkiNtx = Directory.GetFiles(Ścieżka, "*.ntx", SearchOption.AllDirectories).Select(s => s.ToLower()).ToList();
                XmlDocument nowyPlikUpsize = new XmlDocument();
                XmlDocument baza = new XmlDocument();

                nowyPlikUpsize.Load("czynsze.upsize");
                baza.Load("baza.xml");

                using (StreamWriter strumień = new StreamWriter("tabeleNieZnalezioneWBazie.txt"))
                    for (int i = ścieżkiDbf.Count - 1; i >= 0; i--)
                    {
                        string ścieżkaDbf = ścieżkiDbf[i];
                        string nazwaTabeli = Path.GetFileNameWithoutExtension(ścieżkaDbf).ToLower();
                        XmlNode węzełTabeli = baza.SelectSingleNode(String.Format("/tabele/tabela[@nazwa='{0}']", nazwaTabeli));

                        if (węzełTabeli == null)
                            strumień.WriteLine(ścieżkaDbf);
                        else
                        {
                            XmlElement nowyWęzełTable = nowyPlikUpsize.CreateElement("table");
                            XmlElement nowyWęzełUpsize = nowyPlikUpsize.CreateElement("upsize");

                            nowyWęzełTable.SetAttribute("name", nazwaTabeli);
                            nowyWęzełTable.SetAttribute("dbe", "dbfntx");
                            nowyWęzełTable.SetAttribute("dbf", ścieżkaDbf);
                            nowyWęzełUpsize.SetAttribute("table", nazwaTabeli);
                            nowyWęzełUpsize.SetAttribute("connection", "test");
                            nowyWęzełUpsize.SetAttribute("mode", "isam");

                            foreach (XmlNode węzełIndeksu in węzełTabeli.ChildNodes)
                            {
                                string ścieżkaNtx = Path.Combine(Path.GetDirectoryName(ścieżkaDbf), String.Concat(węzełIndeksu.InnerText, ".ntx"));

                                if (ścieżkiNtx.Remove(ścieżkaNtx))
                                {
                                    XmlElement nowyWęzełOrder = nowyPlikUpsize.CreateElement("order");
                                    nowyWęzełOrder.InnerText = ścieżkaNtx;

                                    nowyWęzełTable.AppendChild(nowyWęzełOrder);
                                }
                                /*else
                                    throw new Exception("Błędny zapis w bazie.");*/
                            }

                            nowyPlikUpsize.DocumentElement.AppendChild(nowyWęzełTable);
                            nowyPlikUpsize.DocumentElement.AppendChild(nowyWęzełUpsize);
                            ścieżkiDbf.Remove(ścieżkaDbf);
                        }
                    }

                using (StreamWriter strumień = new StreamWriter("niewykorzystaneIndeksy.txt"))
                    foreach (string ścieżkaNtx in ścieżkiNtx)
                        strumień.WriteLine(ścieżkaNtx);

                nowyPlikUpsize.Save(NazwaNowegoPlikuUpsize);
            }

            {
                XElement root = XElement.Load(NazwaNowegoPlikuUpsize);
                IEnumerable<XElement> tagiTableDoPosortowania = root.Elements("table");
                IEnumerable<XElement> tagiUpsizeDoPosortowania = root.Elements("upsize");
                XElement[] posortowaneTagiTable = tagiTableDoPosortowania.OrderBy(t => t.Attribute("name").ToString()).ToArray();
                XElement[] posortowaneTagiUpsize = tagiUpsizeDoPosortowania.OrderBy(t => t.Attribute("table").ToString()).ToArray();

                foreach (XElement tagTable in posortowaneTagiTable)
                {
                    IEnumerable<XElement> tagiOrder = tagTable.Elements("order");

                    if (tagiOrder.Any())
                    {
                        XElement[] posortowaneTagiOrder = tagiOrder.OrderBy(t => t.Value).ToArray();

                        tagTable.RemoveNodes();
                        tagTable.Add(posortowaneTagiOrder);
                    }
                }

                tagiTableDoPosortowania.Remove();
                tagiUpsizeDoPosortowania.Remove();
                root.Add(posortowaneTagiTable);
                root.Add(posortowaneTagiUpsize);
                root.Save(NazwaNowegoPlikuUpsize);

                string xmlBezWielkichLiter;

                using (StreamReader czytacz = new StreamReader(NazwaNowegoPlikuUpsize))
                    xmlBezWielkichLiter = czytacz.ReadToEnd().ToLower();

                using (StreamWriter pisarz = new StreamWriter(NazwaNowegoPlikuUpsize))
                    pisarz.Write(xmlBezWielkichLiter);
            }

            {
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
}