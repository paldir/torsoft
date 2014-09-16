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

        protected void Page_Load(object sender, EventArgs e)
        {
            report = (EnumP.Report)Enum.Parse(typeof(EnumP.Report), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("report"))]);
            List<Control> controls = null;
            List<string> labels = null;

            switch (report)
            {
                case EnumP.Report.PlacesInEachBuilding:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        controls = new List<Control>()
                            {
                                new ControlsP.TextBoxP("field", "kod_1_start", String.Empty, ControlsP.TextBoxP.TextBoxModeP.Number, 5, 1, true),
                                new ControlsP.TextBoxP("field", "kod_1_end", String.Empty, ControlsP.TextBoxP.TextBoxModeP.Number, 5, 1, true),
                                new ControlsP.CheckBoxListP("field", "kod_typ", db.typesOfPlace.Select(t=>t.typ_lok).ToList(), db.typesOfPlace.Select(t=>t.kod_typ.ToString()).ToList(), db.typesOfPlace.Select(t=>t.kod_typ.ToString()).ToList())
                            };
                    }

                    labels = new List<string>()
                    {
                        "Numer pierwszego budynku: ",
                        "Numer ostatniego budynku:",
                        "Typy lokali: "
                    };
                    break;
            }

            for (int i = 0; i < controls.Count; i++)
            {
                placeOfConfigurationFields.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                placeOfConfigurationFields.Controls.Add(new ControlsP.LabelP("label", controls[i].ID, labels[i], String.Empty));
                placeOfConfigurationFields.Controls.Add(new LiteralControl("<br />"));
                placeOfConfigurationFields.Controls.Add(controls[i]);
                placeOfConfigurationFields.Controls.Add(new LiteralControl("</div>"));
            }

            generationButton.Click += generationButton_Click;

            /*font = new Font("Arial", 10);

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                rows = db.places.Where(p => p.kod_lok != 1).OrderBy(p => p.nr_lok).ToList().Select(p => p.ImportantFields()).ToList();

            namesOfHierarchies = new List<string>();
            namesOfMeasures = new List<string>()
            {
                "ąćęłńóśźż",
                "Kod budynku",
                "Numer lokalu",
                "Typ lokalu",
                "Powierzchnia użytkowa",
                "Nazwisko",
                "Imię"
            };

            RdlGenerator generator = new RdlGenerator("Wydruk", CalculateColumnsWidths(), new SizeF(21, 29.7f), 1, font, "black", new string[] { "lightgray", "lightgray" }, "black", "white");

            //string tmp = generator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);
            
            Session["reportDefinition"] = generator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);

            Response.Redirect("Report.aspx");*/
        }

        float[] CalculateColumnsWidths(List<string> headers, List<string[]> rows, Font font)
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
        }

        void generationButton_Click(object sender, EventArgs e)
        {
            List<List<string[]>> tables = new List<List<string[]>>();
            List<string> headers = null;
            List<string> captions = new List<string>();
            Font font = new Font("Arial", 8);

            switch (report)
            {
                case EnumP.Report.PlacesInEachBuilding:
                    int kod_1_start;
                    int kod_1_end;
                    List<int> selectedTypesOfPlace = new List<int>();
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
            }

            Session["headers"] = headers;
            Session["tables"] = tables;
            Session["captions"] = captions;

            Response.Redirect("Report.aspx");
        }
    }
}