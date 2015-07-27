using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class GeneracjaNaleznosci : Strona
    {
        readonly Func<DostępDoBazy.Należność, bool> należnościZBieżącegoMiesiącaFunc = c => c.data_nal >= new DateTime(Start.Data.Year, Start.Data.Month, 1) && c.data_nal <= new DateTime(Start.Data.Year, Start.Data.Month, DateTime.DaysInMonth(Start.Data.Year, Start.Data.Month));

        int rok
        {
            get { return (int)ViewState["rok"]; }
            set { ViewState["rok"] = value; }
        }

        int miesiąc
        {
            get { return (int)ViewState["miesiąc"]; }
            set { ViewState["miesiąc"] = value; }
        }

        int dzień
        {
            get { return (int)ViewState["dzień"]; }
            set { ViewState["dzień"] = value; }
        }

        int odBudynku
        {
            get { return (int)ViewState["odBudynku"]; }
            set { ViewState["odBudynku"] = value; }
        }

        int odLokalu
        {
            get { return (int)ViewState["odLokalu"]; }
            set { ViewState["odLokalu"] = value; }
        }

        int doBudynku
        {
            get { return (int)ViewState["doBudynku"]; }
            set { ViewState["doBudynku"] = value; }
        }

        int doLokalu
        {
            get { return (int)ViewState["doLokalu"]; }
            set { ViewState["doLokalu"] = value; }
        }

        int minimumBudynków
        {
            get { return Convert.ToInt32(ViewState["minimumBudynków"]); }
            set { ViewState["minimumBudynków"] = value; }
        }

        int minimumLokali
        {
            get { return Convert.ToInt32(ViewState["minimumLokali"]); }
            set { ViewState["minimumLokali"] = value; }
        }

        int maksimumBudynków
        {
            get { return Convert.ToInt32(ViewState["maksimumBudynków"]); }
            set { ViewState["maksimumBudynków"] = value; }
        }

        int maksimumLokali
        {
            get { return Convert.ToInt32(ViewState["maksimumLokali"]); }
            set { ViewState["maksimumLokali"] = value; }
        }

        public static int PostępPrzetwarzaniaNależności { get; private set; }
        public static string BłądPrzetwarzaniaNależności { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                int ilośćDniWMiesiącu = DateTime.DaysInMonth(Start.Data.Year, Start.Data.Month);
                string trybGeneracji = PobierzWartośćParametru<string>("Generacja");
                string powtarzanieGeneracji = PobierzWartośćParametru<string>("Powtarzanie");
                //Start.ŚcieżkaStrony = new List<string>() { "Rozliczenia finansowe", "Generacja należności" };
                Start.ŚcieżkaStrony = new czynsze.ŚcieżkaStrony("Rozliczenia finansowe", "Generacja należności");

                {
                    Func<DostępDoBazy.Należność, bool> należnościZZakresuLokali = r => r.kod_lok >= odBudynku && r.kod_lok <= doBudynku && r.nr_lok >= odLokalu && r.nr_lok <= doLokalu;

                    if (String.IsNullOrEmpty(trybGeneracji))
                        if (db.Zamknięte.ToList().Exists(c => c.rok == Start.Data.Year && c.miesiac == Start.Data.Month && c.z_rok && c.z_mies))
                            form.Controls.Add(new LiteralControl("Miesiąc został już zamknięty."));
                        else
                        {
                            dzień = db.Konfiguracje.FirstOrDefault().p_46;
                            int kod_1_1;
                            int nr1;
                            int kod_1_2;
                            int nr2;

                            if (dzień >= 1)
                            {
                                if (dzień == 31)
                                    dzień = ilośćDniWMiesiącu;
                            }
                            else
                                dzień = 15;

                            placeOfDate.Controls.Add(new LiteralControl("Podaj termin płatności: "));
                            DodajNowąLinię(placeOfDate);
                            placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "rok", Start.Data.Year.ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, true));
                            placeOfDate.Controls.Add(new LiteralControl("-"));
                            placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "miesiąc", Start.Data.Month.ToString("D2"), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, true));
                            placeOfDate.Controls.Add(new LiteralControl("-"));
                            placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "dzień", dzień.ToString("D2"), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, true));
                            DodajNowąLinię(placeOfGeneration);
                            placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "wszystkieGeneracja", "Generacja całego zestawienia", String.Empty));
                            DodajNowąLinię(placeOfGeneration);
                            placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "odDoGeneracja", "Generacja od-do żądanego lokalu", String.Empty));
                            DodajNowąLinię(placeOfGeneration);
                            DodajWybórLokali(placeOfGeneration, out kod_1_1, out nr1, out kod_1_2, out nr2);

                            minimumBudynków = kod_1_1;
                            minimumLokali = nr1;
                            maksimumBudynków = kod_1_2;
                            maksimumLokali = nr2;
                        }
                    else
                    {
                        rok = PobierzWartośćParametru<int>("rok");
                        miesiąc = PobierzWartośćParametru<int>("miesiąc");
                        dzień = PobierzWartośćParametru<int>("dzień");
                        Lista<DostępDoBazy.Należność1> należności1;
                        //IEnumerable<DostępDoBazy.NależnośćZDrugiegoZbioru> należności2;
                        //IEnumerable<DostępDoBazy.NależnośćZTrzeciegoZbioru> należności3;

                        if (trybGeneracji.Contains("od-do"))
                        {
                            string[] od_ = PobierzWartośćParametru<string>("odLokalu").Split('-');
                            string[] do_ = PobierzWartośćParametru<string>("doLokalu").Split('-');
                            odBudynku = Convert.ToInt32(od_[0]);
                            odLokalu = Convert.ToInt32(od_[1]);
                            doBudynku = Convert.ToInt32(do_[0]);
                            doLokalu = Convert.ToInt32(do_[1]);

                            if (odBudynku > doBudynku)
                            {
                                odBudynku = minimumBudynków;
                                doBudynku = maksimumBudynków;
                            }

                            if (odLokalu > doLokalu)
                            {
                                odLokalu = minimumLokali;
                                doLokalu = maksimumLokali;
                            }

                            należności1 = db.Należności1.OrderBy(n => n.kod_lok).ThenBy(n => n.nr_lok).ToList();
                            int indeksPierwszej = należności1.FindIndex(n => n.kod_lok == odBudynku && n.nr_lok == odLokalu);
                            int indeksOstatniej = należności1.FindLastIndex(n => n.kod_lok == doBudynku && n.nr_lok == doLokalu);
                            należności1 = należności1.GetRange(indeksPierwszej, indeksOstatniej - indeksPierwszej + 1);
                            //należności2 = db.NależnościZDrugiegoZbioru.Where(należnościZZakresuLokali).Cast<DostępDoBazy.NależnośćZDrugiegoZbioru>();
                            //należności3 = db.NależnościZTrzeciegoZbioru.Where(należnościZZakresuLokali).Cast<DostępDoBazy.NależnośćZTrzeciegoZbioru>();
                        }
                        else
                        {
                            odBudynku = minimumBudynków;
                            doBudynku = maksimumBudynków;
                            odLokalu = minimumLokali;
                            doLokalu = maksimumLokali;
                            należności1 = db.Należności1.ToList();
                            //należności2 = db.NależnościZDrugiegoZbioru;
                            //należności3 = db.NależnościZTrzeciegoZbioru;
                        }

                        if (należności1.Any(należnościZBieżącegoMiesiącaFunc))// || należności2.Any(należnościZBieżącegoMiesiąca) || należności3.Any(należnościZBieżącegoMiesiąca))
                        {
                            placeOfGeneration.Controls.Add(new LiteralControl("Generacja była już wykonana. Czy chcesz powtórzyć?<br />"));
                            placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "takPowtarzanie", "Tak", String.Empty));
                            placeOfGeneration.Controls.Add(new Kontrolki.Button("button", "nie", "Nie", String.Empty));
                        }
                        else
                            powtarzanieGeneracji = "Tak";
                    }

                    if (!String.IsNullOrEmpty(powtarzanieGeneracji))
                    {
                        System.Threading.Thread thread = new System.Threading.Thread(() => Generate(odBudynku, doBudynku, odLokalu, doLokalu));

                        thread.Start();
                        Response.Redirect("/Formularze/PostepGeneracjiNaleznosci.aspx");
                    }
                }
            }
        }

        void Generate(int odBudynku, int doBudynku, int odLokalu, int doLokalu)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                int ilośćDniWMiesiącu = DateTime.DaysInMonth(Start.Data.Year, Start.Data.Month);

                try
                {
                    {
                        Lista<DostępDoBazy.AktywnyLokal> aktywneLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                        int indeksPierwszego = aktywneLokale.FindIndex(l => l.kod_lok == odBudynku && l.nr_lok == odLokalu);
                        int indeksOstatniego = aktywneLokale.FindLastIndex(l => l.kod_lok == doBudynku && l.nr_lok == doLokalu);
                        aktywneLokale = aktywneLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                        int liczbaAktywnychLokali = aktywneLokale.Count;
                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                        DostępDoBazy.SkładnikCzynszuLokalu.Lokale = aktywneLokale;
                        IEnumerable<DostępDoBazy.Należność1> należnościZBieżącegoMiesiąca = db.Należności1.Where(należnościZBieżącegoMiesiącaFunc).Cast<DostępDoBazy.Należność1>();

                        for (int i = 0; i < liczbaAktywnychLokali; i++)
                        {
                            DostępDoBazy.AktywnyLokal lokal = aktywneLokale[i];
                            PostępPrzetwarzaniaNależności = i * 100 / liczbaAktywnychLokali;

                            db.Należności1.RemoveRange(należnościZBieżącegoMiesiąca.Where(r => r.kod_lok == lokal.kod_lok && r.nr_lok == lokal.nr_lok));

                            foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == lokal.kod_lok && c.nr_lok == lokal.nr_lok).ToList())
                            {
                                DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == składnikCzynszuLokalu.nr_skl);
                                decimal ilosc = 0;
                                decimal stawka = składnikCzynszu.stawka;
                                DostępDoBazy.Należność1 należność1 = new DostępDoBazy.Należność1();

                                try { new DateTime(rok, miesiąc, 1); }
                                catch (ArgumentOutOfRangeException) { miesiąc = Start.Data.Month; }

                                try { new DateTime(rok, miesiąc, dzień); }
                                catch (ArgumentOutOfRangeException) { dzień = this.dzień; }

                                składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                IEnumerable<DateTime> poprawnePoczątkiZakresuDat = new List<DateTime?>() { lokal.dat_od, składnikCzynszuLokalu.dat_od, składnikCzynszu.data_1 }.Where(d => d.HasValue).Cast<DateTime>();
                                IEnumerable<DateTime> poprawneKońceZakresuDat = new List<DateTime?>() { lokal.dat_do, składnikCzynszuLokalu.dat_do, składnikCzynszu.data_2 }.Where(d => d.HasValue).Cast<DateTime>();
                                //IEnumerable<DateTime> properStartsOfDates = new List<DateTime>() { new DateTime(2015, 1, 1), new DateTime(2015, 2, 5), new DateTime(2015, 2, 18) };
                                //IEnumerable<DateTime> properEndsOfDates = new List<DateTime>() { new DateTime(2015, 12, 3), new DateTime(2015, 9, 12), new DateTime(2015, 4, 20) };
                                DateTime początekMiesiąca = new DateTime(Start.Data.Year, Start.Data.Month, 1);
                                DateTime koniecMiesiąca = new DateTime(Start.Data.Year, Start.Data.Month, ilośćDniWMiesiącu);
                                int mnożnikDniWMiesiącu = ilośćDniWMiesiącu;
                                DateTime? początekZakresuDat = poprawnePoczątkiZakresuDat.Any() ? (DateTime?)poprawnePoczątkiZakresuDat.Max() : null;
                                DateTime? koniecZakresuDat = poprawneKońceZakresuDat.Any() ? (DateTime?)poprawneKońceZakresuDat.Min() : null;

                                if (początekZakresuDat.HasValue && koniecZakresuDat.HasValue && początekZakresuDat > koniecZakresuDat)
                                    mnożnikDniWMiesiącu = 0;
                                else
                                {
                                    if (początekZakresuDat.HasValue)
                                        if (początekZakresuDat > koniecMiesiąca)
                                            mnożnikDniWMiesiącu = 0;
                                        else
                                            if (początekZakresuDat >= początekMiesiąca)
                                                mnożnikDniWMiesiącu -= ((DateTime)początekZakresuDat - początekMiesiąca).Days;

                                    if (mnożnikDniWMiesiącu != 0 && koniecZakresuDat.HasValue)
                                        if (koniecZakresuDat < początekMiesiąca)
                                            mnożnikDniWMiesiącu = 0;
                                        else
                                            if (koniecZakresuDat <= koniecMiesiąca)
                                                mnożnikDniWMiesiącu -= (koniecMiesiąca - (DateTime)koniecZakresuDat).Days;
                                }

                                if (mnożnikDniWMiesiącu != 0)
                                {
                                    należność1.Ustaw(Decimal.Round(ilosc * stawka, 2, MidpointRounding.AwayFromZero) * mnożnikDniWMiesiącu / ilośćDniWMiesiącu, new DateTime(rok, miesiąc, dzień), String.Format("{0} za m-c {1:00}", składnikCzynszu.nazwa.Trim(), Start.Data.Month), (int)lokal.nr_kontr, składnikCzynszu.nr_skl, lokal.kod_lok, lokal.nr_lok, stawka, ilosc);
                                    db.Należności1.Add(należność1);
                                }
                            }
                        }

                        db.SaveChanges();

                        PostępPrzetwarzaniaNależności = 100;
                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;
                        DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                    }
                }
                catch (Exception wyjątek)
                {
                    BłądPrzetwarzaniaNależności = Start.ExceptionMessage(wyjątek);
                }
            }
        }
    }
}