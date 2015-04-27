using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;

namespace czynsze.Forms
{
    public partial class ReportConfiguration : Page
    {
        Enums.Report report;

        /*int id
        {
            get
            {
                if (ViewState["id"] == null)
                    return 0;

                return Int32.Parse(ViewState["id"]);
            }
            set { ViewState["id"] = value; }
        }*/

        /*int additionalId
        {
            get
            {
                if (ViewState["additionalId"] == null)
                    return 0;

                return Int32.Parse(ViewState["additionalId"]);
            }
            set { ViewState["additionalId"] = value; }
        }*/

        int[] ids
        {
            get
            {
                if (ViewState["ids"] == null)
                    ViewState["ids"] = new int[6];

                return (int[])ViewState["ids"];
            }

            set { ViewState["ids"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Control> controls = new List<Control>();
            List<string> labels = new List<string>();
            string heading = "Konfiguracja wydruku ";
            string key = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("report"));
            report = (Enums.Report)Enum.Parse(typeof(Enums.Report), key.Replace("report", String.Empty).Substring(key.LastIndexOf('$') + 1));
            key = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"));
            int index = Request.UrlReferrer.Query.IndexOf("id");

            if (!String.IsNullOrEmpty(key))
                ids[0] = GetParamValue<int>(key);

            if (index != -1)
                ids[1] = Int32.Parse(Request.UrlReferrer.Query.Substring(index + 3));

            placeOfConfigurationFields.Controls.Add(new MyControls.HtmlInputHidden(report + "report", "#"));

            switch (report)
            {
                case Enums.Report.PlacesInEachBuilding:
                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                    {
                        heading += "(Lokale w budynkach)";
                        int firstBuildingNumber, lastBuildingNumber;
                        MyControls.HtmlGenericControl firstBuilding = new MyControls.HtmlGenericControl("div", "control");
                        MyControls.HtmlGenericControl secondBuilding = new MyControls.HtmlGenericControl("div", "control");
                        List<string[]> buildings = db.Budynki.ToList().OrderBy(b => b.kod_1).Select(b => b.WażnePola()).ToList();

                        if (db.Budynki.Any())
                        {
                            firstBuildingNumber = db.Budynki.Min(b => b.kod_1);
                            lastBuildingNumber = db.Budynki.Max(b => b.kod_1);
                        }
                        else
                            firstBuildingNumber = lastBuildingNumber = 0;

                        firstBuilding.Controls.Add(new MyControls.TextBox("field", "kod_1_start", firstBuildingNumber.ToString(), MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                        firstBuilding.Controls.Add(new MyControls.DropDownList("field", "kod_1_start_dropdown", buildings, firstBuildingNumber.ToString(), true, false));
                        secondBuilding.Controls.Add(new MyControls.TextBox("field", "kod_1_end", lastBuildingNumber.ToString(), MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                        secondBuilding.Controls.Add(new MyControls.DropDownList("field", "kod_1_end_dropdown", buildings, lastBuildingNumber.ToString(), true, false));

                        controls = new List<Control>()
                            {
                                firstBuilding,
                                secondBuilding,
                                new MyControls.CheckBoxList("field", "kod_typ", db.TypyLokali.Select(t=>t.typ_lok).ToList(), db.TypyLokali.Select(t=>t.kod_typ.ToString()).ToList(), db.TypyLokali.Select(t=>t.kod_typ.ToString()).ToList(), true)
                            };
                    }

                    labels = new List<string>()
                    {
                        "Numer pierwszego budynku: ",
                        "Numer ostatniego budynku:",
                        "Typy lokali: "
                    };

                    break;

                case Enums.Report.MonthlySumOfComponent:
                    heading += "(Sumy miesięczne składnika)";

                    break;

                case Enums.Report.ReceivablesAndTurnoversOfTenant:
                    heading += "(Należności i obroty najemcy)";

                    break;

                case Enums.Report.MonthlyAnalysisOfReceivablesAndTurnovers:
                    heading += "(Analiza miesięczna)";

                    break;

                case Enums.Report.DetailedAnalysisOfReceivablesAndTurnovers:
                    heading += "(Analiza szczegółowa)";

                    break;

                case Enums.Report.CurrentRentAmountOfPlaces:
                case Enums.Report.CurrentRentAmountOfBuildings:
                case Enums.Report.CurrentRentAmountOfCommunities:
                    heading += "(Bieżąca kwota czynszu)";
                    ids[0] = GetParamValue<int>("fromBuilding");
                    ids[1] = GetParamValue<int>("fromPlace");
                    ids[2] = GetParamValue<int>("toBuilding");
                    ids[3] = GetParamValue<int>("toPlace");
                    ids[4] = GetParamValue<int>("fromCommunity");
                    ids[5] = GetParamValue<int>("toCommunity");

                    break;
            }

            placeOfConfigurationFields.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            controls.Add(new MyControls.RadioButtonList("list", "format", new List<string>() { "PDF", "CSV" }, new List<string>() { Enums.ReportFormat.Pdf.ToString(), Enums.ReportFormat.Csv.ToString() }, Enums.ReportFormat.Pdf.ToString(), true, false));
            labels.Add("Format: ");

            for (int i = 0; i < controls.Count; i++)
            {
                placeOfConfigurationFields.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                placeOfConfigurationFields.Controls.Add(new MyControls.Label("label", controls[i].ID, labels[i], String.Empty));
                AddNewLine(placeOfConfigurationFields);
                placeOfConfigurationFields.Controls.Add(controls[i]);
                placeOfConfigurationFields.Controls.Add(new LiteralControl("</div>"));
            }

            generationButton.Click += generationButton_Click;
            Title = heading;

            if (Hello.SiteMapPath.Any())
                if (!Hello.SiteMapPath.Contains(heading))
                {
                    Hello.SiteMapPath[Hello.SiteMapPath.Count - 1] = String.Concat("<a href=\"javascript: Load('" + Request.UrlReferrer.PathAndQuery + "')\">", Hello.SiteMapPath[Hello.SiteMapPath.Count - 1], "</a>");

                    Hello.SiteMapPath.Add(heading);
                }
        }

        void generationButton_Click(object sender, EventArgs e)
        {
            List<List<string[]>> tables = new List<List<string[]>>();
            List<string> headers = null;
            List<string> captions = new List<string>();
            string title = null;

            switch (report)
            {
                case Enums.Report.PlacesInEachBuilding:
                    {
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

                        try { kod_1_start = Int32.Parse(((TextBox)placeOfConfigurationFields.FindControl("kod_1_start")).Text); }
                        catch { kod_1_start = 0; }

                        try { kod_1_end = Int32.Parse(((TextBox)placeOfConfigurationFields.FindControl("kod_1_end")).Text); }
                        catch { kod_1_end = 0; }

                        try
                        {
                            foreach (ListItem item in ((CheckBoxList)placeOfConfigurationFields.FindControl("kod_typ")).Items)
                                if (item.Selected)
                                    selectedTypesOfPlace.Add(Int32.Parse(item.Value));
                        }
                        catch { }

                        using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        {
                            DostępDoBazy.Lokal.TypesOfPlace = db.TypyLokali.ToList();

                            for (int i = kod_1_start; i <= kod_1_end; i++)
                            {
                                DostępDoBazy.Budynek building = db.Budynki.Where(b => b.kod_1 == i).FirstOrDefault();

                                if (building != null)
                                {
                                    captions.Add("Budynek nr " + building.kod_1.ToString() + ", " + building.adres + ", " + building.adres_2);
                                    tables.Add(db.AktywneLokale.Where(p => p.kod_lok == i && selectedTypesOfPlace.Contains(p.kod_typ)).OrderBy(p => p.nr_lok).ToList().Select(p => p.WażnePola().ToList().GetRange(2, p.WażnePola().Length - 2).ToArray()).ToList());
                                }
                            }

                            DostępDoBazy.Lokal.TypesOfPlace = null;
                        }
                    }

                    break;

                case Enums.Report.MonthlySumOfComponent:
                case Enums.Report.ReceivablesAndTurnoversOfTenant:
                case Enums.Report.MonthlyAnalysisOfReceivablesAndTurnovers:
                case Enums.Report.DetailedAnalysisOfReceivablesAndTurnovers:
                    tables = new List<List<string[]>> { new List<string[]>() };
                    IEnumerable<DostępDoBazy.Należność> receivables = null;
                    IEnumerable<DostępDoBazy.Obrót> turnovers = null;
                    decimal wnSum, maSum;
                    List<decimal> balances = new List<decimal>();
                    int nr_kontr = ids[1];

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                    {
                        DostępDoBazy.Najemca tenant = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == nr_kontr);
                        captions = new List<string>() { tenant.nazwisko + " " + tenant.imie + "<br />" + tenant.adres_1 + " " + tenant.adres_2 + "<br />" };

                        switch (Hello.CurrentSet)
                        {
                            case Enums.SettlementTable.Czynsze:
                                receivables = db.NależnościZPierwszegoZbioru.Where(r => r.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Należność>();
                                turnovers = db.ObrotyZPierwszegoZbioru.Where(t => t.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Obrót>();

                                break;

                            case Enums.SettlementTable.SecondSet:
                                receivables = db.NależnościZDrugiegoZbioru.Where(r => r.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Należność>();
                                turnovers = db.ObrotyZDrugiegoZbioru.Where(t => t.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Obrót>();

                                break;

                            case Enums.SettlementTable.ThirdSet:
                                receivables = db.NależnościZTrzeciegoZbioru.Where(r => r.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Należność>();
                                turnovers = db.ObrotyZTrzeciegoZbioru.Where(t => t.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Obrót>();

                                break;
                        }

                        switch (report)
                        {
                            case Enums.Report.MonthlySumOfComponent:
                                title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH ZA ROK 2014";
                                headers = new List<string>() { "m-c", "Wartość" };

                                if (ids[0] < 0)
                                {

                                    int nr_skl = receivables.FirstOrDefault(r => r.__record == -1 * ids[0]).nr_skl;
                                    captions[0] += db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == nr_skl).nazwa;

                                    for (int i = 1; i <= 12; i++)
                                        tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", receivables.Where(r => r.nr_skl == nr_skl).ToList().Where(r => r.data_nal.Year == Hello.Date.Year && r.data_nal.Month == i).Sum(r => r.kwota_nal)) });
                                }
                                else
                                {
                                    int kod_wplat = turnovers.FirstOrDefault(t => t.__record == ids[0]).kod_wplat;
                                    captions[0] += db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;

                                    for (int i = 1; i <= 12; i++)
                                        tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", turnovers.Where(t => t.kod_wplat == kod_wplat).ToList().Where(t => t.data_obr.Year == Hello.Date.Year && t.data_obr.Month == i).Sum(t => t.suma)) });
                                }

                                tables[0].Add(new string[] { "Razem", String.Format("{0:N2}", tables[0].Sum(r => Single.Parse(r[1]))) });

                                break;

                            case Enums.Report.ReceivablesAndTurnoversOfTenant:
                                title = "ZESTAWIENIE NALEZNOSCI I WPLAT";
                                headers = new List<string> { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };

                                foreach (DostępDoBazy.Należność receivable in receivables)
                                {
                                    List<string> fields = receivable.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                    fields.RemoveAt(0);

                                    tables[0].Add(fields.ToArray());
                                }

                                foreach (DostępDoBazy.Obrót turnover in turnovers)
                                {
                                    List<string> fields = turnover.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                    fields.RemoveAt(0);

                                    tables[0].Add(fields.ToArray());
                                }

                                tables[0] = tables[0].OrderBy(r => DateTime.Parse(r[2])).ToList();
                                wnSum = tables[0].Sum(r => String.IsNullOrEmpty(r[0]) ? 0 : Decimal.Parse(r[0]));
                                maSum = tables[0].Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));

                                tables[0].Add(new string[] { String.Format("{0:N2}", wnSum), String.Format("{0:N2}", maSum), String.Empty, String.Empty });
                                tables[0].Add(new string[] { "SALDO", String.Format("{0:N2}", maSum - wnSum), String.Empty, String.Empty });

                                break;

                            case Enums.Report.MonthlyAnalysisOfReceivablesAndTurnovers:
                                title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                headers = new List<string>() { "m-c", "suma WN w miesiącu", "suma MA w miesiącu", "saldo w miesiącu", "suma WN narastająco", "suma MA narastająco", "saldo narastająco" };
                                List<decimal> wnAmounts = new List<decimal>();
                                List<decimal> maAmounts = new List<decimal>();

                                for (int i = 1; i <= 12; i++)
                                {
                                    List<string[]> monthReceivables = receivables.Where(r => r.data_nal.Month == i).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                    List<string[]> monthTurnovers = turnovers.Where(t => t.data_obr.Month == i).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                    wnSum = monthReceivables.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1])) + monthTurnovers.Sum(t => String.IsNullOrEmpty(t[1]) ? 0 : Decimal.Parse(t[1]));
                                    maSum = monthTurnovers.Sum(t => String.IsNullOrEmpty(t[2]) ? 0 : Decimal.Parse(t[2]));

                                    wnAmounts.Add(wnSum);
                                    maAmounts.Add(maSum);
                                    balances.Add(maSum - wnSum);
                                    tables[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", wnSum), String.Format("{0:N2}", maSum), String.Format("{0:N2}", balances.Last()), String.Format("{0:N2}", wnAmounts.Sum()), String.Format("{0:N2}", maAmounts.Sum()), String.Format("{0:N2}", balances.Sum()) });
                                }

                                break;

                            case Enums.Report.DetailedAnalysisOfReceivablesAndTurnovers:
                                title = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                headers = new List<string>() { "m-c", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Saldo miesiąca", "Saldo narastająco" };
                                string[] newRow;

                                for (int i = 1; i <= 12; i++)
                                {
                                    decimal[] sum = new decimal[] { 0, 0, 0, 0 };
                                    wnSum = maSum = 0;

                                    foreach (DostępDoBazy.Należność receivable in receivables.Where(r => r.data_nal.Month == i))
                                    {
                                        string[] row = receivable.WażnePolaDoNależnościIObrotówNajemcy();
                                        int index = db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == receivable.nr_skl).rodz_e - 1;

                                        if (!String.IsNullOrEmpty(row[1]))
                                        {
                                            decimal single = Decimal.Parse(row[1]);
                                            sum[index] += single;
                                            wnSum += single;
                                        }
                                    }

                                    foreach (DostępDoBazy.Obrót turnover in turnovers.Where(t => t.data_obr.Month == i))
                                    {
                                        string[] row = turnover.WażnePolaDoNależnościIObrotówNajemcy();
                                        int index = db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == turnover.kod_wplat).rodz_e - 1;

                                        if (index >= 0)
                                        {
                                            decimal single;

                                            if (!String.IsNullOrEmpty(row[1]))
                                            {
                                                single = Decimal.Parse(row[1]);
                                                sum[index] += single;
                                                wnSum += single;
                                            }

                                            if (!String.IsNullOrEmpty(row[2]))
                                            {
                                                single = Decimal.Parse(row[2]);
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
                                    newRow[i] = String.Format("{0:N2}", tables[0].Sum(r => Single.Parse(r[i])));

                                newRow[6] = newRow[5];

                                tables[0].Add(newRow);

                                break;
                        }
                    }

                    break;

                case Enums.Report.CurrentRentAmountOfPlaces:
                    title = "BIEZACA KWOTA CZYNSZU";
                    headers = new List<string>() { "Lp.", "Kod budynku", "Nr lokalu", "Typ lokalu", "Nazwisko", "Imię", "Adres", "Kwota czynszu" };

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                    {
                        int kod1 = ids[0];
                        int nr1 = ids[1];
                        int kod2 = ids[2];
                        int nr2 = ids[3];
                        DostępDoBazy.Lokal.TypesOfPlace = db.TypyLokali.ToList();
                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                        int index = 1;
                        decimal overallSum = 0;

                        for (int i = kod1; i <= kod2; i++)
                        {
                            List<DostępDoBazy.AktywnyLokal> activePlaces = db.AktywneLokale.Where(p => p.kod_lok == i && p.nr_lok >= nr1 && p.nr_lok <= nr2).OrderBy(p => p.kod_lok).ThenBy(p => p.nr_lok).ToList();
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = activePlaces;
                            DostępDoBazy.Budynek building = db.Budynki.FirstOrDefault(b => b.kod_1 == i);
                            List<string[]> table = new List<string[]>();
                            decimal buildingSum = 0;

                            foreach (DostępDoBazy.Lokal place in activePlaces)
                            {
                                decimal sum = 0;

                                foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                {
                                    decimal ilosc;
                                    decimal stawka;

                                    rentComponentOfPlace.Rozpoznaj_ilosc_and_stawka(out ilosc, out stawka);

                                    sum += Decimal.Round(ilosc * stawka, 2);
                                }

                                table.Add(new string[] { index.ToString(), place.kod_lok.ToString(), place.nr_lok.ToString(), place.Rozpoznaj_kod_typ(), place.nazwisko, place.imie, String.Format("{0} {1}", place.adres, place.adres_2), String.Format("{0:N2}", sum) });

                                index++;
                                buildingSum += sum;
                            }

                            table.Add(new string[] { String.Empty, i.ToString(), String.Empty, String.Empty, "<b>RAZEM</b>", "<b>BUDYNEK</b>", String.Format("{0} {1}", building.adres, building.adres_2), String.Format("{0:N2}", buildingSum) });
                            tables.Add(table);
                            captions.Add(String.Empty);

                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                            overallSum += buildingSum;
                        }

                        DostępDoBazy.Lokal.TypesOfPlace = null;
                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                        if (tables.Any())
                            tables.Last().Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", "<b>WSZYSTKIE</b>", "<b>BUDYNKI</b>", String.Format("{0:N2}", overallSum) });
                    }

                    break;

                case Enums.Report.CurrentRentAmountOfBuildings:
                    title = "BIEZACA KWOTA CZYNSZU";
                    headers = new List<string>() { "Lp.", "Kod budynku", "Adres", "Kwota czynszu" };

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                    {
                        int kod1 = ids[0];
                        int kod2 = ids[2];
                        decimal overallSum = 0;
                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                        List<string[]> table = new List<string[]>();

                        for (int i = kod1; i <= kod2; i++)
                        {
                            DostępDoBazy.Budynek building = db.Budynki.FirstOrDefault(b => b.kod_1 == i);
                            decimal sum = 0;
                            List<DostępDoBazy.AktywnyLokal> activePlaces = db.AktywneLokale.Where(p => p.kod_lok == i).ToList();
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = activePlaces;

                            foreach (DostępDoBazy.AktywnyLokal activePlace in activePlaces)
                                foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == i && c.nr_lok == activePlace.nr_lok))
                                {
                                    decimal ilosc;
                                    decimal stawka;

                                    rentComponentOfPlace.Rozpoznaj_ilosc_and_stawka(out ilosc, out stawka);

                                    sum += Decimal.Round(ilosc * stawka, 2);
                                }

                            overallSum += sum;
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;

                            table.Add(new string[] { String.Format("{0}", i - kod1 + 1), building.kod_1.ToString(), String.Format("{0} {1}", building.adres, building.adres_2), String.Format("{0:N2}", sum) });
                        }

                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                        table.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N2}", overallSum) });
                        captions.Add(String.Empty);
                        tables.Add(table);
                    }

                    break;

                case Enums.Report.CurrentRentAmountOfCommunities:
                    title = "BIEZACA KWOTA CZYNSZU";

                    using(DostępDoBazy.CzynszeKontekst db=new DostępDoBazy.CzynszeKontekst())
                    {
                        int kod1 = ids[4];
                        int kod2 = ids[5];

                        for(int i=kod1; i<kod2; i++)
                        {
                            //DostępDoBazy.
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