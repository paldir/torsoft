using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

using Pechkin;

namespace czynsze
{
    public partial class Report : System.Web.UI.Page
    {
        string html;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> headers = (List<string>)Session["headers"];
            List<List<string[]>> tables = (List<List<string[]>>)Session["tables"];

            StringWriter stringWriter = new StringWriter();

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {

                foreach (List<string[]> table in tables)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "reportTable");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                    foreach (string header in headers)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write(header);
                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();

                    foreach (string[] row in table)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                        foreach (string cell in row)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.Write(cell);
                            writer.RenderEndTag();
                        }

                        writer.RenderEndTag();
                    }

                    writer.RenderEndTag();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "newPage");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderEndTag();
                }
            }

            html = stringWriter.ToString();

            placeOfReport.Controls.Add(new LiteralControl(html));

            downloadButton.Click += downloadButton_Click;
        }

        void downloadButton_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StyleSheet.css"));
            string css = reader.ReadToEnd();

            reader.Close();

            html = html.Insert(0, "<!DOCTYPE html><html><head><title></title><style type='text/css'>" + css + "</style></head><body>");
            html = String.Concat(html, "</body></html>");

            GlobalConfig globalConfig = new GlobalConfig();

            globalConfig.SetPaperSize(System.Drawing.Printing.PaperKind.A4);

            IPechkin pechkin = new SimplePechkin(globalConfig);
            ObjectConfig config = new ObjectConfig();

            config.SetPrintBackground(true);
            config.SetAllowLocalContent(true);
            config.Header.SetTexts("System CZYNSZE\n" + Session["naz_wiz"].ToString(), "LOKALE W BUDYNKACH", "Data: " + DateTime.Today.ToShortDateString() + "\nCzas: " + DateTime.Now.ToShortTimeString());
            config.Header.SetFontName("Arial");
            config.Header.SetFontSize(8);
            config.Footer.SetTexts("Torsoft Toruń", String.Empty, String.Empty);
            config.Footer.SetFontName("Arial");
            config.Footer.SetFontSize(8);

            byte[] bytes = pechkin.Convert(config, html);
            HttpContext.Current.Response.Buffer = true;

            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.ContentType = "application/pdf";

            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Report." + "pdf");
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}