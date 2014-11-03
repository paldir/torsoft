using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;

namespace czynsze.Forms
{
    public partial class ReportConfiguration : System.Web.UI.Page
    {
        EnumP.Report report;

        int id
        {
            get
            {
                if (ViewState["id"] == null)
                    return 0;

                return Convert.ToInt16(ViewState["id"]);
            }
            set { ViewState["id"] = value; }
        }

        int additionalId
        {
            get
            {
                if (ViewState["additionalId"] == null)
                    return 0;

                return Convert.ToInt16(ViewState["additionalId"]);
            }
            set { ViewState["additionalId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //report = (EnumP.Report)Enum.Parse(typeof(EnumP.Report), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("report"))]);
            List<Control> controls = new List<Control>();
            List<string> labels = new List<string>();
            string heading = "Konfiguracja wydruku ";
            string key = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("report"));
            report = (EnumP.Report)Enum.Parse(typeof(EnumP.Report), key.Replace("report", String.Empty).Substring(key.LastIndexOf('$') + 1));
            key = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"));

            if (key != null)
            {
                id = Convert.ToInt16(Request.Params[key]);
                additionalId = Convert.ToInt16(Request.UrlReferrer.Query.Substring(Request.UrlReferrer.Query.IndexOf("id") + 3));
            }

            placeOfConfigurationFields.Controls.Add(new ControlsP.HtmlInputHiddenP(report + "report", "#"));

            switch (report)
            {
                case EnumP.Report.PlacesInEachBuilding:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        heading += "(Lokale w budynkach)";
                        int firstBuildingNumber = db.buildings.Select(b => b.kod_1).Min();
                        int lastBuildingNumber = db.buildings.Select(b => b.kod_1).Max();
                        ControlsP.HtmlGenericControlP firstBuilding = new ControlsP.HtmlGenericControlP("div", "control");
                        ControlsP.HtmlGenericControlP secondBuilding = new ControlsP.HtmlGenericControlP("div", "control");
                        List<string[]> buildings = db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList();

                        firstBuilding.Controls.Add(new ControlsP.TextBoxP("field", "kod_1_start", firstBuildingNumber.ToString(), ControlsP.TextBoxP.TextBoxModeP.Number, 5, 1, true));
                        firstBuilding.Controls.Add(new ControlsP.DropDownListP("field", "kod_1_start_dropdown", buildings, firstBuildingNumber.ToString(), true, false));
                        secondBuilding.Controls.Add(new ControlsP.TextBoxP("field", "kod_1_end", lastBuildingNumber.ToString(), ControlsP.TextBoxP.TextBoxModeP.Number, 5, 1, true));
                        secondBuilding.Controls.Add(new ControlsP.DropDownListP("field", "kod_1_end_dropdown", buildings, lastBuildingNumber.ToString(), true, false));

                        controls = new List<Control>()
                            {
                                firstBuilding,
                                secondBuilding,
                                new ControlsP.CheckBoxListP("field", "kod_typ", db.typesOfPlace.Select(t=>t.typ_lok).ToList(), db.typesOfPlace.Select(t=>t.kod_typ.ToString()).ToList(), db.typesOfPlace.Select(t=>t.kod_typ.ToString()).ToList(), true)
                            };
                    }

                    labels = new List<string>()
                    {
                        "Numer pierwszego budynku: ",
                        "Numer ostatniego budynku:",
                        "Typy lokali: "
                    };
                    break;
                case EnumP.Report.MonthlySumOfComponent:
                    heading += "(Sumy miesięczne składnika)";
                    break;
                case EnumP.Report.SumOfTurnoversOn:
                    heading += "(Suma obrotów w dniu)";
                    break;
            }

            placeOfConfigurationFields.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            controls.Add(new ControlsP.RadioButtonListP("list", "format", new List<string>() { "PDF", "CSV" }, new List<string>() { EnumP.ReportFormat.Pdf.ToString(), EnumP.ReportFormat.Csv.ToString() }, EnumP.ReportFormat.Pdf.ToString(), true, false));
            labels.Add("Format: ");

            for (int i = 0; i < controls.Count; i++)
            {
                placeOfConfigurationFields.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                placeOfConfigurationFields.Controls.Add(new ControlsP.LabelP("label", controls[i].ID, labels[i], String.Empty));
                placeOfConfigurationFields.Controls.Add(new LiteralControl("<br />"));
                placeOfConfigurationFields.Controls.Add(controls[i]);
                placeOfConfigurationFields.Controls.Add(new LiteralControl("</div>"));
            }

            generationButton.Click += generationButton_Click;
            Title = heading;

            if (Forms.Hello.siteMapPath.Count > 0)
                if (Forms.Hello.siteMapPath.IndexOf(heading) == -1)
                {
                    Forms.Hello.siteMapPath[Forms.Hello.siteMapPath.Count - 1] = String.Concat("<a href=\"javascript: Load('" + Request.UrlReferrer.PathAndQuery + "')\">", Forms.Hello.siteMapPath[Forms.Hello.siteMapPath.Count - 1], "</a>");

                    Forms.Hello.siteMapPath.Add(heading);
                }
        }

        /*float[] CalculateColumnsWidths(List<string> headers, List<string[]> rows, Font font)
        {
            float[] result = new float[rows.First().Length];
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);

            for (int i = 0; i < headers.Count; i++)
                result[i] = graphics.MeasureString(headers.ElementAt(i), font).Width;

            foreach (string[] row in rows)
                for (int i = 0; i < row.Length; i++)
                    if (graphics.MeasureString(row[i], font).Width > result[i])
                        result[i] = graphics.MeasureString(row[i], font).Width;

            return result;
        }*/

        void generationButton_Click(object sender, EventArgs e)
        {
            List<List<string[]>> tables = new List<List<string[]>>();
            List<string> headers = null;
            List<string> captions = new List<string>();
            string title = null;
            //Font font = new Font("Arial", 8);

            switch (report)
            {
                case EnumP.Report.PlacesInEachBuilding:
                    int kod_1_start;
                    int kod_1_end;
                    List<int> selectedTypesOfPlace = new List<int>();
                    title = "LOKALE W BUDYNKACH";
                    headers = new List<string>()
                    {
                        "Numer lokalu",
                        "Typ lokalu",
                        "Powierzchnia użytkowa",
                        "Nazwisko",
                        "Imię"
                    };

                    try { kod_1_start = Convert.ToInt16(((TextBox)placeOfConfigurationFields.FindControl("kod_1_start")).Text); }
                    catch { kod_1_start = 0; }

                    try { kod_1_end = Convert.ToInt16(((TextBox)placeOfConfigurationFields.FindControl("kod_1_end")).Text); }
                    catch { kod_1_end = 0; }

                    try
                    {
                        foreach (ListItem item in ((CheckBoxList)placeOfConfigurationFields.FindControl("kod_typ")).Items)
                            if (item.Selected)
                                selectedTypesOfPlace.Add(Convert.ToInt16(item.Value));
                    }
                    catch { }

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        for (int i = kod_1_start; i <= kod_1_end; i++)
                        {
                            DataAccess.Building building = db.buildings.Where(b => b.kod_1 == i).FirstOrDefault();

                            if (building != null)
                            {
                                captions.Add("Budynek nr " + building.kod_1.ToString() + ", " + building.adres + ", " + building.adres_2);
                                tables.Add(db.places.Where(p => p.kod_lok == i && selectedTypesOfPlace.Contains(p.kod_typ)).OrderBy(p => p.nr_lok).ToList().Select(p => p.ImportantFields().ToList().GetRange(2, p.ImportantFields().Length - 2).ToArray()).ToList());
                            }
                        }

                    break;
                case EnumP.Report.MonthlySumOfComponent:
                    title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH ZA ROK " + Hello.date.Year.ToString();

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        DataAccess.Tenant tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == additionalId);
                        headers = new List<string>() { "m-c", "Wartość" };
                        captions = new List<string>() { tenant.nazwisko + " " + tenant.imie + "<br />" + tenant.adres_1 + " " + tenant.adres_2 + "<br />" };
                        tables = new List<List<string[]>> { new List<string[]>() };

                        if (id < 0)
                        {
                            int nr_skl = db.receivablesFor14.FirstOrDefault(r => r.__record == -1 * id).nr_skl;
                            captions[0] += db.rentComponents.FirstOrDefault(c => c.nr_skl == nr_skl).nazwa;

                            for (int i = 1; i <= 12; i++)
                                tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", db.receivablesFor14.Where(r => r.nr_kontr == additionalId && r.nr_skl == nr_skl).ToList().Where(r => Convert.ToDateTime(r.data_nal).Year == Hello.date.Year && Convert.ToDateTime(r.data_nal).Month == i).Sum(r => r.kwota_nal)) });
                        }
                        else
                        {
                            int kod_wplat = db.turnoversFor14.FirstOrDefault(t => t.__record == id).kod_wplat;
                            captions[0] += db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;

                            for (int i = 1; i <= 12; i++)
                                tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", db.turnoversFor14.Where(t => t.nr_kontr == additionalId && t.kod_wplat == kod_wplat).ToList().Where(t => Convert.ToDateTime(t.data_obr).Year == Hello.date.Year && Convert.ToDateTime(t.data_obr).Month == i).Sum(t => t.suma)) });
                        }
                    }
                    break;
            }

            Session["headers"] = headers;
            Session["tables"] = tables;
            Session["captions"] = captions;
            Session["format"] = ((RadioButtonList)placeOfConfigurationFields.FindControl("format")).SelectedValue;
            Session["title"] = title;

            Response.Redirect("Report.aspx");
        }
    }
}