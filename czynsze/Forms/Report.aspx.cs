using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Pechkin;

namespace czynsze.Forms
{
    public partial class Report : System.Web.UI.Page
    {
        string html;

        List<string> headers;
        List<List<string[]>> tables;
        List<string> captions;
        HtmlTextWriter writer;

        protected void Page_Load(object sender, EventArgs e)
        {
            EnumP.ReportFormat format = (EnumP.ReportFormat)Enum.Parse(typeof(EnumP.ReportFormat), Session["format"].ToString());
            headers = (List<string>)Session["headers"];
            tables = (List<List<string[]>>)Session["tables"];
            captions = (List<string>)Session["captions"];

            switch (format)
            {
                case EnumP.ReportFormat.Pdf:
                    StringWriter stringWriter = new StringWriter();

                    using (writer = new HtmlTextWriter(stringWriter))
                    {

                        for (int i = 0; i < tables.Count; i++)
                        {
                            int rowNumber = 0;

                            RenderTableHeader(i);

                            foreach (string[] row in tables[i])
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Id, "row" + rowNumber.ToString());
                                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                                foreach (string cell in row)
                                {
                                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                    writer.Write(cell);
                                    writer.RenderEndTag();
                                }

                                writer.RenderEndTag();

                                if (rowNumber == 60)
                                {
                                    writer.RenderEndTag();
                                    RenderNewPage();
                                    RenderTableHeader(i);

                                    rowNumber = -1;
                                }

                                rowNumber++;
                            }

                            writer.RenderEndTag();

                            if (i != tables.Count - 1)
                                RenderNewPage();
                        }
                    }

                    html = stringWriter.ToString();

                    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StyleSheet.css"));
                    string css = reader.ReadToEnd();

                    reader.Close();

                    html = html.Insert(0, "<!DOCTYPE html><html><head><title></title><style type='text/css'>" + css + "</style></head><body>");
                    html = String.Concat(html, "</body></html>");

                    GlobalConfig globalConfig = new GlobalConfig();

                    globalConfig.SetPaperSize(System.Drawing.Printing.PaperKind.A4);

                    IPechkin pechkin = new Pechkin.Synchronized.SynchronizedPechkin(globalConfig);
                    ObjectConfig config = new ObjectConfig();

                    config.SetPrintBackground(true);
                    config.SetAllowLocalContent(true);
                    config.Header.SetTexts("System CZYNSZE\n\n" + Session["nazwa_1"].ToString(), "LOKALE W BUDYNKACH", "Data: " + DateTime.Today.ToShortDateString() + "\n\nCzas: " + DateTime.Now.ToShortTimeString());
                    config.Header.SetFontName("Arial");
                    config.Header.SetFontSize(8);
                    config.Footer.SetTexts("Torsoft Torun", String.Empty, "Strona [page] z [topage]");
                    config.Footer.SetFontName("Arial");
                    config.Footer.SetFontSize(8);

                    byte[] bytes = pechkin.Convert(config, html);
                    //HttpContext.Current.Response.Buffer = true;

                    //HttpContext.Current.Response.Clear();

                    Response.ContentType = "application/pdf";

                    //HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Wydruk." + "pdf");
                    Response.BinaryWrite(bytes);
                    Response.End();
                    //HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.End();
                    break;
                case EnumP.ReportFormat.Csv:
                    string csv = String.Empty;

                    for (int i = 0; i < tables.Count; i++)
                    {
                        csv += captions[i].Replace(",", String.Empty) + ";" + Environment.NewLine;

                        foreach (string header in headers)
                            csv += header + ";";

                        csv += Environment.NewLine;
                        
                        foreach (string[] row in tables[i])
                        {
                            foreach (string cell in row)
                                csv += cell.Replace(",", ".") + ";";

                            csv += Environment.NewLine;
                        }

                        csv += Environment.NewLine;
                    }

                    Response.ContentType = "text/csv";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1250");

                    Response.AddHeader("content-disposition", "attachment; filename=Report.csv");
                    Response.Write(csv);
                    Response.End();

                    break;
            }
        }

        void RenderTableHeader(int tableNumber)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "reportTable");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Caption);
            writer.Write(captions[tableNumber]);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            foreach (string header in headers)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.Write(header);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }

        void RenderNewPage()
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "newPage");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }
    }
}