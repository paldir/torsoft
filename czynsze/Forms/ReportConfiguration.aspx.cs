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
            int index = Request.UrlReferrer.Query.IndexOf("id");

            if (key != null)
                id = Convert.ToInt16(Request.Params[key]);

            if (index != -1)
                additionalId = Convert.ToInt16(Request.UrlReferrer.Query.Substring(index + 3));

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

                case EnumP.Report.ReceivablesAndTurnoversOfTenant:
                    heading += "(Należności i obroty najemcy)";

                    break;

                case EnumP.Report.MonthlyAnalysisOfReceivablesAndTurnovers:
                    heading += "(Analiza miesięczna)";

                    break;

                case EnumP.Report.DetailedAnalysisOfReceivablesAndTurnovers:
                    heading += "(Analiza szczegółowa)";

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

            if (Hello.SiteMapPath.Count > 0)
                if (Hello.SiteMapPath.IndexOf(heading) == -1)
                {
                    Hello.SiteMapPath[Hello.SiteMapPath.Count - 1] = String.Concat("<a href=\"javascript: Load('" + Request.UrlReferrer.PathAndQuery + "')\">", Hello.SiteMapPath[Hello.SiteMapPath.Count - 1], "</a>");

                    Hello.SiteMapPath.Add(heading);
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
                case EnumP.Report.ReceivablesAndTurnoversOfTenant:
                case EnumP.Report.MonthlyAnalysisOfReceivablesAndTurnovers:
                case EnumP.Report.DetailedAnalysisOfReceivablesAndTurnovers:
                    tables = new List<List<string[]>> { new List<string[]>() };
                    List<DataAccess.Receivable> receivables = null;
                    List<DataAccess.Turnover> turnovers = null;
                    float wnSum, maSum;
                    List<float> balances = new List<float>();

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        DataAccess.Tenant tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == additionalId);
                        captions = new List<string>() { tenant.nazwisko + " " + tenant.imie + "<br />" + tenant.adres_1 + " " + tenant.adres_2 + "<br />" };

                        switch (Hello.CurrentSet)
                        {
                            case EnumP.SettlementTable.Czynsze:
                                receivables = db.receivablesFor14.Where(r => r.nr_kontr == additionalId).ToList().Cast<DataAccess.Receivable>().ToList();
                                turnovers = db.turnoversFor14.Where(t => t.nr_kontr == additionalId).ToList().Cast<DataAccess.Turnover>().ToList();

                                break;

                            case EnumP.SettlementTable.SecondSet:
                                receivables = db.receivablesFor14From2ndSet.Where(r => r.nr_kontr == additionalId).ToList().Cast<DataAccess.Receivable>().ToList();
                                turnovers = db.turnoversFor14From2ndSet.Where(t => t.nr_kontr == additionalId).ToList().Cast<DataAccess.Turnover>().ToList();

                                break;

                            case EnumP.SettlementTable.ThirdSet:
                                receivables = db.receivablesFor14From3rdSet.Where(r => r.nr_kontr == additionalId).ToList().Cast<DataAccess.Receivable>().ToList();
                                turnovers = db.turnoversFor14From3rdSet.Where(t => t.nr_kontr == additionalId).ToList().Cast<DataAccess.Turnover>().ToList();

                                break;
                        }

                        switch (report)
                        {
                            case EnumP.Report.MonthlySumOfComponent:
                                title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH ZA ROK 2014";
                                headers = new List<string>() { "m-c", "Wartość" };

                                if (id < 0)
                                {

                                    int nr_skl = receivables.FirstOrDefault(r => r.__record == -1 * id).nr_skl;
                                    captions[0] += db.rentComponents.FirstOrDefault(c => c.nr_skl == nr_skl).nazwa;

                                    for (int i = 1; i <= 12; i++)
                                        tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", receivables.Where(r => r.nr_skl == nr_skl).ToList().Where(r => Convert.ToDateTime(r.data_nal).Year == Hello.Date.Year && Convert.ToDateTime(r.data_nal).Month == i).Sum(r => r.kwota_nal)) });
                                }
                                else
                                {
                                    int kod_wplat = turnovers.FirstOrDefault(t => t.__record == id).kod_wplat;
                                    captions[0] += db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;

                                    for (int i = 1; i <= 12; i++)
                                        tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", turnovers.Where(t => t.kod_wplat == kod_wplat).ToList().Where(t => Convert.ToDateTime(t.data_obr).Year == Hello.Date.Year && Convert.ToDateTime(t.data_obr).Month == i).Sum(t => t.suma)) });
                                }

                                tables[0].Add(new string[] { "Razem", String.Format("{0:N2}", tables[0].Sum(r => Convert.ToSingle(r[1]))) });

                                break;

                            case EnumP.Report.ReceivablesAndTurnoversOfTenant:
                                title = "ZESTAWIENIE NALEZNOSCI I WPLAT";
                                headers = new List<string> { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };

                                foreach (DataAccess.Receivable receivable in receivables)
                                {
                                    List<string> fields = receivable.ImportantFieldsForReceivablesAndTurnoversOfTenant().ToList();

                                    fields.RemoveAt(0);

                                    tables[0].Add(fields.ToArray());
                                }

                                foreach (DataAccess.Turnover turnover in turnovers)
                                {
                                    List<string> fields = turnover.ImportantFieldsForReceivablesAndTurnoversOfTenant().ToList();

                                    fields.RemoveAt(0);

                                    tables[0].Add(fields.ToArray());
                                }

                                tables[0] = tables[0].OrderBy(r => DateTime.Parse(r[2])).ToList();
                                wnSum = tables[0].Sum(r => (r[0] == String.Empty) ? 0 : Convert.ToSingle(r[0]));
                                maSum = tables[0].Sum(r => (r[1] == String.Empty) ? 0 : Convert.ToSingle(r[1]));

                                tables[0].Add(new string[] { String.Format("{0:N2}", wnSum), String.Format("{0:N2}", maSum), String.Empty, String.Empty });
                                tables[0].Add(new string[] { "SALDO", String.Format("{0:N2}", maSum - wnSum), String.Empty, String.Empty });

                                break;

                            case EnumP.Report.MonthlyAnalysisOfReceivablesAndTurnovers:
                                title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                headers = new List<string>() { "m-c", "suma WN w miesiącu", "suma MA w miesiącu", "saldo w miesiącu", "suma WN narastająco", "suma MA narastająco", "saldo narastająco" };
                                List<float> wnAmounts = new List<float>();
                                List<float> maAmounts = new List<float>();

                                for (int i = 1; i <= 12; i++)
                                {
                                    List<string[]> monthReceivables = receivables.Where(r => DateTime.Parse(r.data_nal).Month == i).Select(r => r.ImportantFieldsForReceivablesAndTurnoversOfTenant()).ToList();
                                    List<string[]> monthTurnovers = turnovers.Where(t => DateTime.Parse(t.data_obr).Month == i).Select(t => t.ImportantFieldsForReceivablesAndTurnoversOfTenant()).ToList();
                                    wnSum = monthReceivables.Sum(r => (r[1] == String.Empty) ? 0 : Convert.ToSingle(r[1])) + monthTurnovers.Sum(t => (t[1] == String.Empty) ? 0 : Convert.ToSingle(t[1]));
                                    maSum = monthTurnovers.Sum(t => (t[2] == String.Empty) ? 0 : Convert.ToSingle(t[2]));

                                    wnAmounts.Add(wnSum);
                                    maAmounts.Add(maSum);
                                    balances.Add(maSum - wnSum);
                                    tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", wnSum), String.Format("{0:N2}", maSum), String.Format("{0:N2}", balances.Last()), String.Format("{0:N2}", wnAmounts.Sum()), String.Format("{0:N2}", maAmounts.Sum()), String.Format("{0:N2}", balances.Sum()) });
                                }

                                break;

                            case EnumP.Report.DetailedAnalysisOfReceivablesAndTurnovers:
                                title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                headers = new List<string>() { "m-c", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Saldo miesiąca", "Saldo narastająco" };
                                string[] newRow;

                                for (int i = 1; i <= 12; i++)
                                {
                                    float[] sum = new float[] { 0, 0, 0, 0 };
                                    wnSum = maSum = 0;

                                    foreach (DataAccess.Receivable receivable in receivables.Where(r => DateTime.Parse(r.data_nal).Month == i))
                                    {
                                        string[] row = receivable.ImportantFieldsForReceivablesAndTurnoversOfTenant();
                                        int index = db.rentComponents.FirstOrDefault(c => c.nr_skl == receivable.nr_skl).rodz_e - 1;

                                        if (row[1] != String.Empty)
                                        {
                                            float single = Convert.ToSingle(row[1]);
                                            sum[index] += single;
                                            wnSum += single;
                                        }
                                    }

                                    foreach (DataAccess.Turnover turnover in turnovers.Where(t => DateTime.Parse(t.data_obr).Month == i))
                                    {
                                        string[] row = turnover.ImportantFieldsForReceivablesAndTurnoversOfTenant();
                                        int index = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == turnover.kod_wplat).rodz_e - 1;

                                        if (index >= 0)
                                        {
                                            float single;
                                            
                                            if (row[1] != String.Empty)
                                            {
                                                single = Convert.ToSingle(row[1]);
                                                sum[index] += single;
                                                wnSum += single;
                                            }

                                            if (row[2] != String.Empty)
                                            {
                                                single = Convert.ToSingle(row[2]);
                                                sum[index] += single;
                                                maSum += single;
                                            }
                                        }
                                    }

                                    balances.Add(maSum - wnSum);

                                    newRow = new string[7];
                                    newRow[0] = i.ToString();

                                    for (int j = 1; j <= 4; j++)
                                        newRow[j] = String.Format("{0:N2}", sum[j - 1]);

                                    newRow[5] = String.Format("{0:N2}", balances.Last());
                                    newRow[6] = String.Format("{0:N2}", balances.Sum());

                                    tables[0].Add(newRow);
                                }

                                newRow = new string[7];
                                newRow[0] = "Razem";

                                for (int i = 1; i < newRow.Length - 1; i++)
                                    newRow[i] = String.Format("{0:N2}", tables[0].Sum(r => Convert.ToSingle(r[i])));

                                newRow[6] = newRow[5];

                                tables[0].Add(newRow);

                                break;
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