using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Pechkin;

namespace czynsze.Formularze
{
    public partial class Raport : System.Web.UI.Page
    {
        string html;
        List<string> nagłówki;
        List<List<string[]>> tabele;
        List<string> podpisy;
        string tytuł;
        HtmlTextWriter pisarz;

        protected void Page_Load(object sender, EventArgs e)
        {
            Enumeratory.FormatRaportu format = (Enumeratory.FormatRaportu)Enum.Parse(typeof(Enumeratory.FormatRaportu), Sesja.Obecna.FormatRaportu);
            nagłówki = Sesja.Obecna.NagłówkiRaportu;
            tabele = Sesja.Obecna.TabeleRaportu;
            podpisy = Sesja.Obecna.PodpisyRaportu;
            tytuł = Sesja.Obecna.TytułRaportu;
            object potencjalnaGotowaDefinicjaHtml = Sesja.Obecna.GotowaDefinicjaHtmlRaportu;

            /*if (potencjalnaGotowaDefinicjaHtml != null)
                html = potencjalnaGotowaDefinicjaHtml.ToString();*/

            switch (format)
            {
                case Enumeratory.FormatRaportu.Pdf:
                    StringWriter pisarzNapisów = new StringWriter();

                    using (pisarz = new HtmlTextWriter(pisarzNapisów))
                        if (potencjalnaGotowaDefinicjaHtml == null)
                        {
                            decimal kwota;
                            int numerWiersza = 1;
                            bool wyjątekNowejStrony;


                            for (int i = 0; i < tabele.Count; i++)
                            {
                                List<string[]> tabela = tabele[i];
                                wyjątekNowejStrony = false;

                                GenerujNagłówekTabeli(i);

                                for (int j = 0; j < tabela.Count; j++)
                                {
                                    string[] wiersz = tabela[j];

                                    pisarz.AddAttribute(HtmlTextWriterAttribute.Id, String.Format("row{0}", numerWiersza));
                                    pisarz.RenderBeginTag(HtmlTextWriterTag.Tr);

                                    foreach (string komórka in wiersz)
                                    {
                                        if (Decimal.TryParse(komórka, out kwota))
                                            pisarz.AddAttribute(HtmlTextWriterAttribute.Class, "numericCell");

                                        pisarz.RenderBeginTag(HtmlTextWriterTag.Td);
                                        pisarz.Write(komórka);
                                        pisarz.RenderEndTag();
                                    }

                                    pisarz.RenderEndTag();

                                    if (numerWiersza >= 60)
                                    {
                                        pisarz.RenderEndTag();
                                        GenerujNowąStronę();

                                        if (j == tabela.Count - 1)
                                            wyjątekNowejStrony = true;
                                        else
                                            GenerujNagłówekTabeli(i);

                                        numerWiersza = 0;
                                    }

                                    numerWiersza++;
                                }

                                if (!wyjątekNowejStrony)
                                    pisarz.RenderEndTag();

                                /*if (i != tables.Count - 1)
                                    RenderNewPage();*/

                                numerWiersza += 5;
                            }
                        }
                        else
                        {
                            html = String.Empty;
                            List<string> sekcjeHtml = (List<string>)potencjalnaGotowaDefinicjaHtml;

                            for (int i = 0; i < sekcjeHtml.Count; i++)
                            {
                                if (i % 2 == 0)
                                {
                                    pisarz.AddAttribute(HtmlTextWriterAttribute.Class, "reportContent");
                                    pisarz.RenderBeginTag(HtmlTextWriterTag.Hr);
                                    pisarz.RenderEndTag();
                                }

                                pisarz.Write(sekcjeHtml[i]);

                                if (i % 2 == 1 && i != sekcjeHtml.Count - 1)
                                    GenerujNowąStronę();
                            }
                        }

                    html = pisarzNapisów.ToString();
                    StreamReader czytacz = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StyleSheet.css"));
                    string css = czytacz.ReadToEnd();

                    czytacz.Close();

                    html = html.Insert(0, "<!DOCTYPE html><html><head><title></title><style type='text/css'>" + css + "</style></head><body>");
                    html = String.Concat(html, "<br /><br /><div class='printingEnd'>KONIEC WYDRUKU</div></body></html>");

                    GlobalConfig globalnaKonfiguracja = new GlobalConfig();

                    globalnaKonfiguracja.SetPaperSize(System.Drawing.Printing.PaperKind.A4);

                    IPechkin pechkin = new Pechkin.Synchronized.SynchronizedPechkin(globalnaKonfiguracja);
                    ObjectConfig konfiguracja = new ObjectConfig();

                    konfiguracja.SetPrintBackground(true);
                    konfiguracja.SetAllowLocalContent(true);
                    konfiguracja.Header.SetTexts("System CZYNSZE " + ((int)Start.AktywnyZbiór == 0 ? String.Empty : "(" + Start.NazwyZbiorów[(int)Start.AktywnyZbiór].Trim() + ")") + "\n\n" + Start.NazwaFirmy, "\n" + tytuł + "\n", "Data: " + DateTime.Today.ToShortDateString() + "\n\nCzas: " + DateTime.Now.ToShortTimeString());
                    konfiguracja.Header.SetFontName("Arial");
                    konfiguracja.Header.SetFontSize(8);
                    konfiguracja.Footer.SetTexts("Torsoft Torun", String.Empty, "Strona [page] z [topage]");
                    konfiguracja.Footer.SetFontName("Arial");
                    konfiguracja.Footer.SetFontSize(8);

                    byte[] bajty = pechkin.Convert(konfiguracja, html);
                    Response.ContentType = "application/pdf";

                    Response.AddHeader("content-disposition", "inline; filename=Wydruk.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bajty);
                    Response.End();

                    break;

                case Enumeratory.FormatRaportu.Csv:
                    string csv = String.Empty;

                    for (int i = 0; i < tabele.Count; i++)
                    {
                        csv += podpisy[i].Replace(",", String.Empty) + ";" + Environment.NewLine;

                        foreach (string nagłówek in nagłówki)
                            csv += nagłówek + ";";

                        csv += Environment.NewLine;

                        foreach (string[] wiersz in tabele[i])
                        {
                            foreach (string komórka in wiersz)
                                csv += komórka.Replace(",", ".") + ";";

                            csv += Environment.NewLine;
                        }

                        csv += Environment.NewLine;
                    }

                    Response.ContentType = "text/csv";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1250");

                    Response.AddHeader("content-disposition", "attachment; filename=raport.csv");
                    Response.Write(csv);
                    Response.End();

                    break;
            }
        }

        void GenerujNagłówekTabeli(int numerTabeli)
        {
            pisarz.AddAttribute(HtmlTextWriterAttribute.Class, "reportTable reportContent");
            pisarz.RenderBeginTag(HtmlTextWriterTag.Table);
            //writer.RenderBeginTag(HtmlTextWriterTag.Caption);
            pisarz.AddAttribute(HtmlTextWriterAttribute.Class, "caption");
            pisarz.RenderBeginTag(HtmlTextWriterTag.Tr);
            pisarz.AddAttribute(HtmlTextWriterAttribute.Colspan, nagłówki.Count.ToString());
            pisarz.RenderBeginTag(HtmlTextWriterTag.Td);
            pisarz.Write(podpisy[numerTabeli]);
            pisarz.RenderEndTag();
            pisarz.RenderEndTag();
            pisarz.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (string nagłówek in nagłówki)
            {
                pisarz.RenderBeginTag(HtmlTextWriterTag.Th);
                pisarz.Write(nagłówek);
                pisarz.RenderEndTag();
            }

            pisarz.RenderEndTag();
        }

        void GenerujNowąStronę()
        {
            pisarz.AddAttribute(HtmlTextWriterAttribute.Class, "newPage");
            pisarz.RenderBeginTag(HtmlTextWriterTag.Div);
            pisarz.RenderEndTag();
        }
    }
}