using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class GenerationOfReceivables : Page
    {
        readonly Func<DostępDoBazy.Należność, bool> receivablesFromCurrentMonth = c => c.data_nal >= new DateTime(Hello.Date.Year, Hello.Date.Month, 1) && c.data_nal <= new DateTime(Hello.Date.Year, Hello.Date.Month, DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month));

        int year
        {
            get { return (int)ViewState["year"]; }
            set { ViewState["year"] = value; }
        }

        int month
        {
            get { return (int)ViewState["month"]; }
            set { ViewState["month"] = value; }
        }

        int day
        {
            get { return (int)ViewState["day"]; }
            set { ViewState["day"] = value; }
        }

        int fromBuilding
        {
            get { return (int)ViewState["fromBuilding"]; }
            set { ViewState["fromBuilding"] = value; }
        }

        int fromPlace
        {
            get { return (int)ViewState["fromPlace"]; }
            set { ViewState["fromPlace"] = value; }
        }

        int toBuilding
        {
            get { return (int)ViewState["toBuilding"]; }
            set { ViewState["toBuilding"] = value; }
        }

        int toPlace
        {
            get { return (int)ViewState["toPlace"]; }
            set { ViewState["toPlace"] = value; }
        }

        public static int ProgressOfProcessingOfReceivables { get; private set; }
        public static string ErrorOfProcessingOfReceivables { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int daysInMonth = DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month);
            string generationMode = GetParamValue<string>("Generation");
            string repeatGeneration = GetParamValue<string>("Repeat");
            Hello.SiteMapPath = new List<string>() { "Rozliczenia finansowe", "Generacja należności" };

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Func<DostępDoBazy.Należność, bool> receivablesFromRangeOfPlaces = r => r.kod_lok >= fromBuilding && r.kod_lok <= toBuilding && r.nr_lok >= fromPlace && r.nr_lok <= toPlace;

                if (String.IsNullOrEmpty(generationMode))
                    if (db.Zamknięte.ToList().Exists(c => c.rok == Hello.Date.Year && c.miesiac == Hello.Date.Month && c.z_rok && c.z_mies))
                        form.Controls.Add(new LiteralControl("Miesiąc został już zamknięty."));
                    else
                    {
                        day = db.Konfiguracje.FirstOrDefault().p_46;

                        if (day >= 1)
                        {
                            if (day == 31)
                                day = daysInMonth;
                        }
                        else
                            day = 15;

                        placeOfDate.Controls.Add(new LiteralControl("Podaj termin płatności: "));
                        AddNewLine(placeOfDate);
                        placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "year", Hello.Date.Year.ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, true));
                        placeOfDate.Controls.Add(new LiteralControl("-"));
                        placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "month", Hello.Date.Month.ToString("D2"), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, true));
                        placeOfDate.Controls.Add(new LiteralControl("-"));
                        placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "day", day.ToString("D2"), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, true));
                        AddNewLine(placeOfGeneration);
                        placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "allGeneration", "Generacja całego zestawienia", String.Empty));
                        AddNewLine(placeOfGeneration);
                        placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "fromToGeneration", "Generacja od-do żądanego lokalu", String.Empty));
                        AddNewLine(placeOfGeneration);
                        placeOfGeneration.Controls.Add(new Kontrolki.Label("label", "fromBuilding", "Numer budynku pierwszego lokalu: ", String.Empty));
                        placeOfGeneration.Controls.Add(new Kontrolki.TextBox("field", "fromBuilding", db.AktywneLokale.Min(p => p.kod_lok).ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                        placeOfGeneration.Controls.Add(new Kontrolki.Label("label", "fromPlace", " Numer pierwszego lokalu: ", String.Empty));
                        placeOfGeneration.Controls.Add(new Kontrolki.TextBox("field", "fromPlace", db.AktywneLokale.Min(p => p.nr_lok).ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));
                        AddNewLine(placeOfGeneration);
                        placeOfGeneration.Controls.Add(new Kontrolki.Label("label", "toBuilding", "Numer budynku ostatniego lokalu: ", String.Empty));
                        placeOfGeneration.Controls.Add(new Kontrolki.TextBox("field", "toBuilding", db.AktywneLokale.Max(p => p.kod_lok).ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                        placeOfGeneration.Controls.Add(new Kontrolki.Label("label", "toPlace", " Numer ostatniego lokalu: ", String.Empty));
                        placeOfGeneration.Controls.Add(new Kontrolki.TextBox("field", "toPlace", db.AktywneLokale.Max(p => p.nr_lok).ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));
                    }
                else
                {
                    year = GetParamValue<int>("year");
                    month = GetParamValue<int>("month");
                    day = GetParamValue<int>("day");
                    IEnumerable<DostępDoBazy.NależnośćZPierwszegoZbioru> receivablesFrom1stSet;
                    IEnumerable<DostępDoBazy.NależnośćZDrugiegoZbioru> receivablesFrom2ndSet;
                    IEnumerable<DostępDoBazy.NależnośćZTrzeciegoZbioru> receivablesFrom3rdSet;

                    if (generationMode.Contains("od-do"))
                    {
                        fromBuilding = GetParamValue<int>("fromBuilding");
                        fromPlace = GetParamValue<int>("fromPlace");
                        toBuilding = GetParamValue<int>("toBuilding");
                        toPlace = GetParamValue<int>("toPlace");

                        if (fromBuilding > toBuilding)
                        {
                            fromBuilding = db.AktywneLokale.Min(p => p.kod_lok);
                            toBuilding = db.AktywneLokale.Max(p => p.kod_lok);
                        }

                        if (fromPlace > toPlace)
                        {
                            fromPlace = db.AktywneLokale.Min(p => p.nr_lok);
                            toPlace = db.AktywneLokale.Max(p => p.nr_lok);
                        }

                        receivablesFrom1stSet = db.NależnościZPierwszegoZbioru.Where(receivablesFromRangeOfPlaces).Cast<DostępDoBazy.NależnośćZPierwszegoZbioru>();
                        receivablesFrom2ndSet = db.NależnościZDrugiegoZbioru.Where(receivablesFromRangeOfPlaces).Cast<DostępDoBazy.NależnośćZDrugiegoZbioru>();
                        receivablesFrom3rdSet = db.NależnościZTrzeciegoZbioru.Where(receivablesFromRangeOfPlaces).Cast<DostępDoBazy.NależnośćZTrzeciegoZbioru>();
                    }
                    else
                    {
                        fromBuilding = db.AktywneLokale.Min(p => p.kod_lok);
                        toBuilding = db.AktywneLokale.Max(p => p.kod_lok);
                        fromPlace = db.AktywneLokale.Min(p => p.nr_lok);
                        toPlace = db.AktywneLokale.Max(p => p.nr_lok);
                        receivablesFrom1stSet = db.NależnościZPierwszegoZbioru;
                        receivablesFrom2ndSet = db.NależnościZDrugiegoZbioru;
                        receivablesFrom3rdSet = db.NależnościZTrzeciegoZbioru;
                    }

                    if (receivablesFrom1stSet.Any(receivablesFromCurrentMonth) || receivablesFrom2ndSet.Any(receivablesFromCurrentMonth) || receivablesFrom3rdSet.Any(receivablesFromCurrentMonth))
                    {
                        placeOfGeneration.Controls.Add(new LiteralControl("Generacja była już wykonana. Czy chcesz powtórzyć?<br />"));
                        placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "yesRepeat", "Tak", String.Empty));
                        placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "no", "Nie", String.Empty));
                    }
                    else
                        repeatGeneration = "Tak";
                }

                if (!String.IsNullOrEmpty(repeatGeneration))
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() => Generate(fromBuilding, toBuilding, fromPlace, toPlace));

                    thread.Start();
                    Response.Redirect("/Forms/ProgressOfGenerationOfReceivables.aspx");
                }
            }
        }

        void Generate(int fromBuilding, int toBuilding, int fromPlace, int toPlace)
        {
            int daysInMonth = DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month);

            try
            {
                using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                {
                    List<DostępDoBazy.AktywnyLokal> activePlaces = db.AktywneLokale.Where(p => p.kod_lok >= fromBuilding && p.kod_lok <= toBuilding && p.nr_lok >= fromPlace && p.nr_lok <= toPlace).ToList();
                    int countOfActivePlaces = activePlaces.Count;

                    db.NależnościZPierwszegoZbioru.RemoveRange(db.NależnościZPierwszegoZbioru.Where(receivablesFromCurrentMonth).Where(r => r.kod_lok >= fromBuilding && r.kod_lok <= toBuilding && r.nr_lok >= fromPlace && r.nr_lok <= toPlace).Cast<DostępDoBazy.NależnośćZPierwszegoZbioru>());

                    for (int i = 0; i < countOfActivePlaces; i++)
                    {
                        DostępDoBazy.AktywnyLokal place = activePlaces[i];
                        ProgressOfProcessingOfReceivables = i * 100 / countOfActivePlaces;

                        foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok).ToList())
                        {
                            DostępDoBazy.SkładnikCzynszu rentComponent = db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == rentComponentOfPlace.nr_skl);
                            decimal ilosc = 0;
                            decimal stawka = rentComponent.stawka;
                            DostępDoBazy.NależnośćZPierwszegoZbioru receivableFrom1stSet = new DostępDoBazy.NależnośćZPierwszegoZbioru();

                            try { new DateTime(year, month, 1); }
                            catch (ArgumentOutOfRangeException) { month = Hello.Date.Month; }

                            try { new DateTime(year, month, day); }
                            catch (ArgumentOutOfRangeException) { day = this.day; }

                            rentComponentOfPlace.Rozpoznaj_ilosc_and_stawka(out ilosc, out stawka);

                            IEnumerable<DateTime> properStartsOfDates = new List<DateTime?>() { place.dat_od, rentComponentOfPlace.dat_od, rentComponent.data_1 }.Where(d => d.HasValue).Cast<DateTime>();
                            IEnumerable<DateTime> properEndsOfDates = new List<DateTime?>() { place.dat_do, rentComponentOfPlace.dat_do, rentComponent.data_2 }.Where(d => d.HasValue).Cast<DateTime>();
                            //IEnumerable<DateTime> properStartsOfDates = new List<DateTime>() { new DateTime(2015, 1, 1), new DateTime(2015, 2, 5), new DateTime(2015, 2, 18) };
                            //IEnumerable<DateTime> properEndsOfDates = new List<DateTime>() { new DateTime(2015, 12, 3), new DateTime(2015, 9, 12), new DateTime(2015, 4, 20) };
                            DateTime monthBeggining = new DateTime(Hello.Date.Year, Hello.Date.Month, 1);
                            DateTime monthEnd = new DateTime(Hello.Date.Year, Hello.Date.Month, daysInMonth);
                            int dayFactor = daysInMonth;
                            DateTime? startDate = properStartsOfDates.Any() ? (DateTime?)properStartsOfDates.Max() : null;
                            DateTime? endDate = properEndsOfDates.Any() ? (DateTime?)properEndsOfDates.Min() : null;

                            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                                dayFactor = 0;
                            else
                            {
                                if (startDate.HasValue)
                                    if (startDate > monthEnd)
                                        dayFactor = 0;
                                    else
                                        if (startDate >= monthBeggining)
                                            dayFactor -= ((DateTime)startDate - monthBeggining).Days;

                                if (dayFactor != 0 && endDate.HasValue)
                                    if (endDate < monthBeggining)
                                        dayFactor = 0;
                                    else
                                        if (endDate <= monthEnd)
                                            dayFactor -= (monthEnd - (DateTime)endDate).Days;
                            }

                            if (dayFactor != 0)
                            {
                                receivableFrom1stSet.Ustaw(Decimal.Round(ilosc * stawka, 2) * dayFactor / daysInMonth, new DateTime(year, month, day), String.Format("{0} za m-c {1:00}", rentComponent.nazwa.Trim(), Hello.Date.Month), (int)place.nr_kontr, rentComponent.nr_skl, place.kod_lok, place.nr_lok, stawka, ilosc);
                                db.NależnościZPierwszegoZbioru.Add(receivableFrom1stSet);
                            }
                        }
                    }

                    db.SaveChanges();

                    ProgressOfProcessingOfReceivables = 100;
                }
            }
            catch (Exception exception)
            {
                ErrorOfProcessingOfReceivables = Hello.ExceptionMessage(exception);
            }
        }
    }
}