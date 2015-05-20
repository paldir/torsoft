using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using System.Xml;

namespace czynsze.Formularze
{
    public partial class KonfiguracjaRaportu : Strona
    {
        Enumeratory.Raport raport;

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

        int[] identyfikatory
        {
            get
            {
                if (ViewState["identyfikatory"] == null)
                    ViewState["identyfikatory"] = new int[6];

                return (int[])ViewState["identyfikatory"];
            }

            set { ViewState["identyfikatory"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<Control> kontrolki = new List<Control>();
            List<string> etykiety = new List<string>();
            string nagłówek = "Konfiguracja wydruku ";
            string klucz = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("raport"));
            raport = (Enumeratory.Raport)Enum.Parse(typeof(Enumeratory.Raport), klucz.Replace("raport", String.Empty).Substring(klucz.LastIndexOf('$') + 1));
            klucz = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"));
            int indeks = Request.UrlReferrer.Query.IndexOf("id");

            if (!String.IsNullOrEmpty(klucz))
                identyfikatory[0] = PobierzWartośćParametru<int>(klucz);

            if (indeks != -1)
                identyfikatory[1] = Int32.Parse(Request.UrlReferrer.Query.Substring(indeks + 3));

            placeOfConfigurationFields.Controls.Add(new Kontrolki.HtmlInputHidden(raport + "raport", "#"));

            switch (raport)
            {
                case Enumeratory.Raport.LokaleWBudynkach:
                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                    {
                        nagłówek += "(Lokale w budynkach)";
                        int numerPierwszegoBudynku, numerOstatniegoBudynku;
                        Kontrolki.HtmlGenericControl pierwszyBudynek = new Kontrolki.HtmlGenericControl("div", "control");
                        Kontrolki.HtmlGenericControl drugiBudynek = new Kontrolki.HtmlGenericControl("div", "control");
                        List<string[]> budynki = db.Budynki.ToList().OrderBy(b => b.kod_1).Select(b => b.WażnePola()).ToList();

                        if (db.Budynki.Any())
                        {
                            numerPierwszegoBudynku = db.Budynki.Min(b => b.kod_1);
                            numerOstatniegoBudynku = db.Budynki.Max(b => b.kod_1);
                        }
                        else
                            numerPierwszegoBudynku = numerOstatniegoBudynku = 0;

                        pierwszyBudynek.Controls.Add(new Kontrolki.TextBox("field", "kod_1_start", numerPierwszegoBudynku.ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                        pierwszyBudynek.Controls.Add(new Kontrolki.DropDownList("field", "kod_1_start_dropdown", budynki, numerPierwszegoBudynku.ToString(), true, false));
                        drugiBudynek.Controls.Add(new Kontrolki.TextBox("field", "kod_1_end", numerOstatniegoBudynku.ToString(), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                        drugiBudynek.Controls.Add(new Kontrolki.DropDownList("field", "kod_1_end_dropdown", budynki, numerOstatniegoBudynku.ToString(), true, false));

                        kontrolki = new List<Control>()
                            {
                                pierwszyBudynek,
                                drugiBudynek,
                                new Kontrolki.CheckBoxList("field", "kod_typ", db.TypyLokali.Select(t=>t.typ_lok).ToList(), db.TypyLokali.Select(t=>t.kod_typ.ToString()).ToList(), db.TypyLokali.Select(t=>t.kod_typ.ToString()).ToList(), true)
                            };
                    }

                    etykiety = new List<string>()
                    {
                        "Numer pierwszego budynku: ",
                        "Numer ostatniego budynku:",
                        "Typy lokali: "
                    };

                    break;

                case Enumeratory.Raport.MiesięczneSumySkładników:
                    nagłówek += "(Sumy miesięczne składnika)";

                    break;

                case Enumeratory.Raport.NależnościIObrotyNajemcy:
                    nagłówek += "(Należności i obroty najemcy)";

                    break;

                case Enumeratory.Raport.MiesięcznaAnalizaNależnościIObrotów:
                    nagłówek += "(Analiza miesięczna)";

                    break;

                case Enumeratory.Raport.SzczegółowaAnalizaNależnościIObrotów:
                    nagłówek += "(Analiza szczegółowa)";

                    break;

                case Enumeratory.Raport.KwotaCzynszuLokali:
                case Enumeratory.Raport.KwotaCzynszuBudynków:
                case Enumeratory.Raport.KwotaCzynszuWspólnot:
                case Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła:
                case Enumeratory.Raport.SkładnikiCzynszuStawkaInformacyjna:
                case Enumeratory.Raport.WykazWgSkladnika:
                    identyfikatory[0] = PobierzWartośćParametru<int>("odBudynku");
                    identyfikatory[1] = PobierzWartośćParametru<int>("odLokalu");
                    identyfikatory[2] = PobierzWartośćParametru<int>("doBudynku");
                    identyfikatory[3] = PobierzWartośćParametru<int>("doLokalu");

                    switch (raport)
                    {
                        case Enumeratory.Raport.KwotaCzynszuLokali:
                        case Enumeratory.Raport.KwotaCzynszuBudynków:
                        case Enumeratory.Raport.KwotaCzynszuWspólnot:
                            identyfikatory[4] = PobierzWartośćParametru<int>("odWspólnoty");
                            identyfikatory[5] = PobierzWartośćParametru<int>("doWspólnoty");

                            switch ((Enumeratory.KwotaCzynszu)Session["trybKwotyCzynszu"])
                            {
                                case Enumeratory.KwotaCzynszu.Biezaca:
                                    nagłówek += "(Bieżąca kwota czynszu)";

                                    break;

                                case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                                    nagłówek += "(Kwota czynszu za dany miesiąc)";

                                    break;
                            }

                            break;

                        case Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła:
                            nagłówek += "(Składniki czynszu - stawka zwykła)";

                            break;
                        case Enumeratory.Raport.SkładnikiCzynszuStawkaInformacyjna:
                            nagłówek += "(Składniki czynszu - stawka informacyjna)";

                            break;
                        case Enumeratory.Raport.WykazWgSkladnika:
                            identyfikatory[4] = PobierzWartośćParametru<int>("nrSkladnika");

                            switch((Enumeratory.WykazWedługSkładnika)Convert.ChangeType(Session["trybWykazuWedługSkładnika"], typeof(Enumeratory.WykazWedługSkładnika)))
                            {
                                case Enumeratory.WykazWedługSkładnika.Obecny:
                                    nagłówek += "(Wykaz wg składnika - obecny)";

                                    break;

                                case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                    nagłówek += "(Wykaz wg składnika - historia ogółem)";

                                    break;
                            }

                            break;
                    }

                    break;
            }

            placeOfConfigurationFields.Controls.Add(new LiteralControl("<h2>" + nagłówek + "</h2>"));
            kontrolki.Add(new Kontrolki.RadioButtonList("list", "format", new List<string>() { "PDF", "CSV" }, new List<string>() { Enumeratory.FormatRaportu.Pdf.ToString(), Enumeratory.FormatRaportu.Csv.ToString() }, Enumeratory.FormatRaportu.Pdf.ToString(), true, false));
            etykiety.Add("Format: ");

            for (int i = 0; i < kontrolki.Count; i++)
            {
                placeOfConfigurationFields.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                placeOfConfigurationFields.Controls.Add(new Kontrolki.Label("label", kontrolki[i].ID, etykiety[i], String.Empty));
                DodajNowąLinię(placeOfConfigurationFields);
                placeOfConfigurationFields.Controls.Add(kontrolki[i]);
                placeOfConfigurationFields.Controls.Add(new LiteralControl("</div>"));
            }

            generationButton.Click += generationButton_Click;
            Title = nagłówek;

            if (Start.ŚcieżkaStrony.Any())
                if (!Start.ŚcieżkaStrony.Contains(nagłówek))
                {
                    Start.ŚcieżkaStrony[Start.ŚcieżkaStrony.Count - 1] = String.Concat("<a href=\"javascript: Load('" + Request.UrlReferrer.PathAndQuery + "')\">", Start.ŚcieżkaStrony[Start.ŚcieżkaStrony.Count - 1], "</a>");

                    Start.ŚcieżkaStrony.Add(nagłówek);
                }
        }

        void generationButton_Click(object sender, EventArgs e)
        {
            List<List<string[]>> tabele = new List<List<string[]>>();
            List<string> nagłówki = null;
            List<string> podpisy = new List<string>();
            string tytuł = null;
            List<string> gotowaDefinicjaHtml = null;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                switch (raport)
                {
                    case Enumeratory.Raport.LokaleWBudynkach:
                        {
                            int kod_1_start;
                            int kod_1_koniec;
                            List<int> wybraneTypyLokali = new List<int>();
                            tytuł = "LOKALE W BUDYNKACH";
                            nagłówki = new List<string>()
                            {
                                "Numer lokalu",
                                "Typ lokalu",
                                "Powierzchnia użytkowa",
                                "Nazwisko",
                                "Imię"
                            };

                            try { kod_1_start = Int32.Parse(((TextBox)placeOfConfigurationFields.FindControl("kod_1_start")).Text); }
                            catch { kod_1_start = 0; }

                            try { kod_1_koniec = Int32.Parse(((TextBox)placeOfConfigurationFields.FindControl("kod_1_end")).Text); }
                            catch { kod_1_koniec = 0; }

                            try
                            {
                                foreach (ListItem pozycja in ((CheckBoxList)placeOfConfigurationFields.FindControl("kod_typ")).Items)
                                    if (pozycja.Selected)
                                        wybraneTypyLokali.Add(Int32.Parse(pozycja.Value));
                            }
                            catch { }

                            //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                            {
                                DostępDoBazy.Lokal.TypesOfPlace = db.TypyLokali.ToList();

                                for (int i = kod_1_start; i <= kod_1_koniec; i++)
                                {
                                    DostępDoBazy.Budynek budynek = db.Budynki.Where(b => b.kod_1 == i).FirstOrDefault();

                                    if (budynek != null)
                                    {
                                        podpisy.Add("Budynek nr " + budynek.kod_1.ToString() + ", " + budynek.adres + ", " + budynek.adres_2);
                                        tabele.Add(db.AktywneLokale.Where(p => p.kod_lok == i && wybraneTypyLokali.Contains(p.kod_typ)).OrderBy(p => p.nr_lok).ToList().Select(p => p.WażnePola().ToList().GetRange(2, p.WażnePola().Length - 2).ToArray()).ToList());
                                    }
                                }

                                DostępDoBazy.Lokal.TypesOfPlace = null;
                            }
                        }

                        break;

                    case Enumeratory.Raport.MiesięczneSumySkładników:
                    case Enumeratory.Raport.NależnościIObrotyNajemcy:
                    case Enumeratory.Raport.MiesięcznaAnalizaNależnościIObrotów:
                    case Enumeratory.Raport.SzczegółowaAnalizaNależnościIObrotów:
                        {
                            tabele = new List<List<string[]>> { new List<string[]>() };
                            IEnumerable<DostępDoBazy.Należność> należności = null;
                            IEnumerable<DostępDoBazy.Obrót> obroty = null;
                            decimal sumaWn, sumaMa;
                            List<decimal> salda = new List<decimal>();
                            int nr_kontr = identyfikatory[1];

                            //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                            {
                                DostępDoBazy.Najemca najemca = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == nr_kontr);
                                podpisy = new List<string>() { najemca.nazwisko + " " + najemca.imie + "<br />" + najemca.adres_1 + " " + najemca.adres_2 + "<br />" };

                                switch (Start.AktywnyZbiór)
                                {
                                    case Enumeratory.Zbiór.Czynsze:
                                        należności = db.NależnościZPierwszegoZbioru.Where(r => r.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Należność>();
                                        obroty = db.ObrotyZPierwszegoZbioru.Where(t => t.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Obrót>();

                                        break;

                                    case Enumeratory.Zbiór.Drugi:
                                        należności = db.NależnościZDrugiegoZbioru.Where(r => r.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Należność>();
                                        obroty = db.ObrotyZDrugiegoZbioru.Where(t => t.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Obrót>();

                                        break;

                                    case Enumeratory.Zbiór.Trzeci:
                                        należności = db.NależnościZTrzeciegoZbioru.Where(r => r.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Należność>();
                                        obroty = db.ObrotyZTrzeciegoZbioru.Where(t => t.nr_kontr == nr_kontr).ToList().Cast<DostępDoBazy.Obrót>();

                                        break;
                                }

                                switch (raport)
                                {
                                    case Enumeratory.Raport.MiesięczneSumySkładników:
                                        tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH ZA ROK 2014";
                                        nagłówki = new List<string>() { "m-c", "Wartość" };

                                        if (identyfikatory[0] < 0)
                                        {

                                            int nr_skl = należności.FirstOrDefault(r => r.__record == -1 * identyfikatory[0]).nr_skl;
                                            podpisy[0] += db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == nr_skl).nazwa;

                                            for (int i = 1; i <= 12; i++)
                                                tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", należności.Where(r => r.nr_skl == nr_skl).ToList().Where(r => r.data_nal.Year == Start.Data.Year && r.data_nal.Month == i).Sum(r => r.kwota_nal)) });
                                        }
                                        else
                                        {
                                            int kod_wplat = obroty.FirstOrDefault(t => t.__record == identyfikatory[0]).kod_wplat;
                                            podpisy[0] += db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;

                                            for (int i = 1; i <= 12; i++)
                                                tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", obroty.Where(t => t.kod_wplat == kod_wplat).ToList().Where(t => t.data_obr.Year == Start.Data.Year && t.data_obr.Month == i).Sum(t => t.suma)) });
                                        }

                                        tabele[0].Add(new string[] { "Razem", String.Format("{0:N2}", tabele[0].Sum(r => Single.Parse(r[1]))) });

                                        break;

                                    case Enumeratory.Raport.NależnościIObrotyNajemcy:
                                        tytuł = "ZESTAWIENIE NALEZNOSCI I WPLAT";
                                        nagłówki = new List<string> { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };

                                        foreach (DostępDoBazy.Należność receivable in należności)
                                        {
                                            List<string> pola = receivable.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                            pola.RemoveAt(0);

                                            tabele[0].Add(pola.ToArray());
                                        }

                                        foreach (DostępDoBazy.Obrót turnover in obroty)
                                        {
                                            List<string> pola = turnover.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                            pola.RemoveAt(0);

                                            tabele[0].Add(pola.ToArray());
                                        }

                                        tabele[0] = tabele[0].OrderBy(r => DateTime.Parse(r[2])).ToList();
                                        sumaWn = tabele[0].Sum(r => String.IsNullOrEmpty(r[0]) ? 0 : Decimal.Parse(r[0]));
                                        sumaMa = tabele[0].Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));

                                        tabele[0].Add(new string[] { String.Format("{0:N2}", sumaWn), String.Format("{0:N2}", sumaMa), String.Empty, String.Empty });
                                        tabele[0].Add(new string[] { "SALDO", String.Format("{0:N2}", sumaMa - sumaWn), String.Empty, String.Empty });

                                        break;

                                    case Enumeratory.Raport.MiesięcznaAnalizaNależnościIObrotów:
                                        tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                        nagłówki = new List<string>() { "m-c", "suma WN w miesiącu", "suma MA w miesiącu", "saldo w miesiącu", "suma WN narastająco", "suma MA narastająco", "saldo narastająco" };
                                        List<decimal> kwotyWn = new List<decimal>();
                                        List<decimal> kwotyMa = new List<decimal>();

                                        for (int i = 1; i <= 12; i++)
                                        {
                                            List<string[]> miesięczneNależności = należności.Where(r => r.data_nal.Month == i).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                            List<string[]> miesięczneObroty = obroty.Where(t => t.data_obr.Month == i).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                            sumaWn = miesięczneNależności.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1])) + miesięczneObroty.Sum(t => String.IsNullOrEmpty(t[1]) ? 0 : Decimal.Parse(t[1]));
                                            sumaMa = miesięczneObroty.Sum(t => String.IsNullOrEmpty(t[2]) ? 0 : Decimal.Parse(t[2]));

                                            kwotyWn.Add(sumaWn);
                                            kwotyMa.Add(sumaMa);
                                            salda.Add(sumaMa - sumaWn);
                                            tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N2}", sumaWn), String.Format("{0:N2}", sumaMa), String.Format("{0:N2}", salda.Last()), String.Format("{0:N2}", kwotyWn.Sum()), String.Format("{0:N2}", kwotyMa.Sum()), String.Format("{0:N2}", salda.Sum()) });
                                        }

                                        break;

                                    case Enumeratory.Raport.SzczegółowaAnalizaNależnościIObrotów:
                                        tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                        nagłówki = new List<string>() { "m-c", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Saldo miesiąca", "Saldo narastająco" };
                                        string[] nowyWiersz;

                                        for (int i = 1; i <= 12; i++)
                                        {
                                            decimal[] suma = new decimal[] { 0, 0, 0, 0 };
                                            sumaWn = sumaMa = 0;

                                            foreach (DostępDoBazy.Należność należność in należności.Where(r => r.data_nal.Month == i))
                                            {
                                                string[] wiersz = należność.WażnePolaDoNależnościIObrotówNajemcy();
                                                int indeks = db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == należność.nr_skl).rodz_e - 1;

                                                if (!String.IsNullOrEmpty(wiersz[1]))
                                                {
                                                    decimal kwota = Decimal.Parse(wiersz[1]);
                                                    suma[indeks] += kwota;
                                                    sumaWn += kwota;
                                                }
                                            }

                                            foreach (DostępDoBazy.Obrót obrót in obroty.Where(t => t.data_obr.Month == i))
                                            {
                                                string[] wiersz = obrót.WażnePolaDoNależnościIObrotówNajemcy();
                                                int indeks = db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == obrót.kod_wplat).rodz_e - 1;

                                                if (indeks >= 0)
                                                {
                                                    decimal kwota;

                                                    if (!String.IsNullOrEmpty(wiersz[1]))
                                                    {
                                                        kwota = Decimal.Parse(wiersz[1]);
                                                        suma[indeks] += kwota;
                                                        sumaWn += kwota;
                                                    }

                                                    if (!String.IsNullOrEmpty(wiersz[2]))
                                                    {
                                                        kwota = Decimal.Parse(wiersz[2]);
                                                        suma[indeks] += kwota;
                                                        sumaMa += kwota;
                                                    }
                                                }
                                            }

                                            salda.Add(sumaMa - sumaWn);

                                            nowyWiersz = new string[7];
                                            nowyWiersz[0] = i.ToString();

                                            for (int j = 1; j <= 4; j++)
                                                nowyWiersz[j] = String.Format("{0:N2}", suma[j - 1]);

                                            nowyWiersz[5] = String.Format("{0:N2}", salda.Last());
                                            nowyWiersz[6] = String.Format("{0:N2}", salda.Sum());

                                            tabele[0].Add(nowyWiersz);
                                        }

                                        nowyWiersz = new string[7];
                                        nowyWiersz[0] = "Razem";

                                        for (int i = 1; i < nowyWiersz.Length - 1; i++)
                                            nowyWiersz[i] = String.Format("{0:N2}", tabele[0].Sum(r => Single.Parse(r[i])));

                                        nowyWiersz[6] = nowyWiersz[5];

                                        tabele[0].Add(nowyWiersz);

                                        break;
                                }
                            }
                        }

                        break;

                    case Enumeratory.Raport.KwotaCzynszuLokali:
                    case Enumeratory.Raport.KwotaCzynszuBudynków:
                    case Enumeratory.Raport.KwotaCzynszuWspólnot:
                        {
                            Enumeratory.KwotaCzynszu trybKwotyCzynszu = ((Enumeratory.KwotaCzynszu)Convert.ChangeType(Session["trybKwotyCzynszu"], typeof(Enumeratory.KwotaCzynszu)));
                            DateTime data = Start.Data;
                            DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                            DateTime koniecMiesiąca = początekMiesiąca.AddDays(DateTime.DaysInMonth(początekMiesiąca.Year, początekMiesiąca.Month)).AddSeconds(-1);
                            IEnumerable<DostępDoBazy.NależnośćZPierwszegoZbioru> należnościZaDanyMiesiąc = null;

                            switch (trybKwotyCzynszu)
                            {
                                case Enumeratory.KwotaCzynszu.Biezaca:
                                    tytuł = "BIEZACA KWOTA CZYNSZU";

                                    break;

                                case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                                    tytuł = String.Format("KWOTA CZYNSZU ZA {0} {1}", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[data.Month - 1].ToString().ToUpper(), data.Year);
                                    należnościZaDanyMiesiąc = db.NależnościZPierwszegoZbioru.Where(n => n.data_nal >= początekMiesiąca && n.data_nal <= koniecMiesiąca);

                                    break;
                            }

                            switch (raport)
                            {
                                case Enumeratory.Raport.KwotaCzynszuLokali:
                                    nagłówki = new List<string>() { "Lp.", "Kod budynku", "Nr lokalu", "Typ lokalu", "Nazwisko", "Imię", "Adres", "Kwota czynszu" };

                                    //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                                    {
                                        int kod1 = identyfikatory[0];
                                        int nr1 = identyfikatory[1];
                                        int kod2 = identyfikatory[2];
                                        int nr2 = identyfikatory[3];
                                        DostępDoBazy.Lokal.TypesOfPlace = db.TypyLokali.ToList();
                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                                        int indeks = 1;
                                        decimal ogólnaSuma = 0;
                                        List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                        int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == kod1 && l.nr_lok == nr1);
                                        int indeksOstatniego = wszystkieLokale.FindLastIndex(l => l.kod_lok == kod2 && l.nr_lok == nr2);
                                        List<DostępDoBazy.AktywnyLokal> wybraneLokale = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);

                                        for (int i = kod1; i <= kod2; i++)
                                        {
                                            List<DostępDoBazy.AktywnyLokal> aktywneLokale = wybraneLokale.Where(p => p.kod_lok == i).OrderBy(p => p.nr_lok).ToList();
                                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = aktywneLokale;
                                            DostępDoBazy.Budynek budynki = db.Budynki.FirstOrDefault(b => b.kod_1 == i);
                                            List<string[]> tabela = new List<string[]>();
                                            decimal sumaBudynku = 0;

                                            foreach (DostępDoBazy.Lokal lokal in aktywneLokale)
                                            {
                                                decimal suma = 0;

                                                switch (trybKwotyCzynszu)
                                                {
                                                    case Enumeratory.KwotaCzynszu.Biezaca:
                                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == lokal.kod_lok && c.nr_lok == lokal.nr_lok))
                                                        {
                                                            decimal ilosc;
                                                            decimal stawka;

                                                            składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                            suma += Decimal.Round(ilosc * stawka, 2);
                                                        }

                                                        break;

                                                    case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                                                        foreach (DostępDoBazy.NależnośćZPierwszegoZbioru należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == lokal.kod_lok && n.nr_lok == lokal.nr_lok))
                                                            suma += należność.kwota_nal;

                                                        break;
                                                }

                                                tabela.Add(new string[] { indeks.ToString(), lokal.kod_lok.ToString(), lokal.nr_lok.ToString(), lokal.Rozpoznaj_kod_typ(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Format("{0:N2}", suma) });

                                                indeks++;
                                                sumaBudynku += suma;
                                            }

                                            tabela.Add(new string[] { String.Empty, i.ToString(), String.Empty, String.Empty, "<b>RAZEM</b>", "<b>BUDYNEK</b>", String.Format("{0} {1}", budynki.adres, budynki.adres_2), String.Format("{0:N2}", sumaBudynku) });
                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);

                                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                                            ogólnaSuma += sumaBudynku;
                                        }

                                        DostępDoBazy.Lokal.TypesOfPlace = null;
                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                                        if (tabele.Any())
                                            tabele.Last().Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", "<b>WSZYSTKIE</b>", "<b>BUDYNKI</b>", String.Format("{0:N2}", ogólnaSuma) });
                                    }

                                    break;

                                case Enumeratory.Raport.KwotaCzynszuBudynków:
                                    nagłówki = new List<string>() { "Lp.", "Kod budynku", "Adres", "Kwota czynszu" };

                                    //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                                    {
                                        int kod1 = identyfikatory[0];
                                        int kod2 = identyfikatory[2];
                                        decimal sumaGłówna = 0;
                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                                        List<string[]> tabela = new List<string[]>();

                                        for (int i = kod1; i <= kod2; i++)
                                        {
                                            DostępDoBazy.Budynek budynek = db.Budynki.FirstOrDefault(b => b.kod_1 == i);
                                            decimal suma = 0;
                                            List<DostępDoBazy.AktywnyLokal> aktywneLokale = db.AktywneLokale.Where(p => p.kod_lok == i).ToList();
                                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = aktywneLokale;

                                            foreach (DostępDoBazy.AktywnyLokal aktywnyLokal in aktywneLokale)
                                                switch (trybKwotyCzynszu)
                                                {
                                                    case Enumeratory.KwotaCzynszu.Biezaca:
                                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == i && c.nr_lok == aktywnyLokal.nr_lok))
                                                        {
                                                            decimal ilosc;
                                                            decimal stawka;

                                                            składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                            suma += Decimal.Round(ilosc * stawka, 2);
                                                        }

                                                        break;

                                                    case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                                                        foreach (DostępDoBazy.NależnośćZPierwszegoZbioru należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == i && n.nr_lok == aktywnyLokal.nr_lok))
                                                            suma += należność.kwota_nal;

                                                        break;
                                                }

                                            sumaGłówna += suma;
                                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;

                                            tabela.Add(new string[] { String.Format("{0}", i - kod1 + 1), budynek.kod_1.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N2}", suma) });
                                        }

                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                                        tabela.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N2}", sumaGłówna) });
                                        podpisy.Add(String.Empty);
                                        tabele.Add(tabela);
                                    }

                                    break;

                                case Enumeratory.Raport.KwotaCzynszuWspólnot:
                                    nagłówki = new List<string>() { "Lp.", "Kod budynku", "Adres", "Kwota czynszu" };

                                    //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                                    {
                                        int kod1 = identyfikatory[4];
                                        int kod2 = identyfikatory[5];
                                        decimal sumaOgólna = 0;
                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();

                                        for (int i = kod1; i <= kod2; i++)
                                        {
                                            List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == i).ToList();
                                            DostępDoBazy.Wspólnota wspólnota = db.Wspólnoty.FirstOrDefault(w => w.kod == i);
                                            decimal sumaWspólnoty = 0;
                                            List<string[]> tabela = new List<string[]>();

                                            foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                            {
                                                DostępDoBazy.Budynek budynek = db.Budynki.FirstOrDefault(b => b.kod_1 == budynekWspólnoty.kod_1);
                                                List<DostępDoBazy.AktywnyLokal> aktywneLokale = db.AktywneLokale.Where(p => p.kod_lok == budynek.kod_1).ToList();
                                                DostępDoBazy.SkładnikCzynszuLokalu.Lokale = aktywneLokale;
                                                decimal suma = 0;

                                                foreach (DostępDoBazy.AktywnyLokal aktywnyLokal in aktywneLokale)
                                                    switch (trybKwotyCzynszu)
                                                    {
                                                        case Enumeratory.KwotaCzynszu.Biezaca:
                                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == i && c.nr_lok == aktywnyLokal.nr_lok))
                                                            {
                                                                decimal ilosc;
                                                                decimal stawka;

                                                                składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                                suma += Decimal.Round(ilosc * stawka, 2);
                                                            }

                                                            break;

                                                        case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                                                            foreach (DostępDoBazy.NależnośćZPierwszegoZbioru należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == i && n.nr_lok == aktywnyLokal.nr_lok))
                                                                suma += należność.kwota_nal;

                                                            break;
                                                    }

                                                sumaWspólnoty += suma;
                                                DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;

                                                tabela.Add(new string[] { String.Format("{0}", i - kod1 + 1), budynek.kod_1.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N2}", suma) });
                                            }

                                            sumaOgólna += sumaWspólnoty;

                                            tabela.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N2}", sumaWspólnoty) });
                                            tabele.Add(tabela);
                                            podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                        }

                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;
                                    }

                                    break;
                            }
                        }

                        break;

                    case Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła:
                    case Enumeratory.Raport.SkładnikiCzynszuStawkaInformacyjna:
                        {
                            XmlDocument dokument = new XmlDocument();
                            tytuł = "SKLADNIKI CZYNSZU I OPLAT";

                            dokument.Load(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "Formularze", "Szablon.html"));

                            XmlNode druk = dokument.SelectSingleNode(XPathZnajdźElementPoId("druk"));
                            gotowaDefinicjaHtml = new List<string>();
                            DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                            int kod_1_1 = identyfikatory[0];
                            int nr1 = identyfikatory[1];
                            int kod_1_2 = identyfikatory[2];
                            int nr2 = identyfikatory[3];
                            List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                            int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == kod_1_1 && l.nr_lok == nr1);
                            int indeksDrugiego = wszystkieLokale.FindLastIndex(l => l.kod_lok == kod_1_2 && l.nr_lok == nr2);
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = wszystkieLokale.GetRange(indeksPierwszego, indeksDrugiego - indeksPierwszego + 1);

                            foreach (DostępDoBazy.AktywnyLokal lokal in DostępDoBazy.SkładnikCzynszuLokalu.Lokale)
                            {
                                XmlNode nowyDruk = druk.CloneNode(true);
                                XmlNode razem = nowyDruk.SelectSingleNode(XPathZnajdźElementPoId("razem"));
                                XmlNode składnikOpłat = nowyDruk.SelectSingleNode(XPathZnajdźElementPoId("składnikOpłat"));
                                decimal suma = 0;

                                WypełnijTagXml(nowyDruk, "nazwiskoImię", String.Format("{0} {1}", lokal.nazwisko, lokal.imie));
                                WypełnijTagXml(nowyDruk, "adres1", lokal.adres);
                                WypełnijTagXml(nowyDruk, "adres2", lokal.adres_2);
                                WypełnijTagXml(nowyDruk, "kodLokalu", String.Format("{0} - {1}", lokal.kod_lok, lokal.nr_lok));
                                WypełnijTagXml(nowyDruk, "powierzchnia", lokal.pow_uzyt);
                                WypełnijTagXml(nowyDruk, "ilośćOsób", lokal.il_osob);
                                WypełnijTagXml(nowyDruk, "data", DateTime.Now.ToShortDateString());

                                if (db.Treści.Any())
                                {
                                    DostępDoBazy.Treść treść = db.Treści.FirstOrDefault();

                                    for (int i = 1; i <= 15; i++)
                                    {
                                        string op = typeof(DostępDoBazy.Treść).GetProperty(String.Format("op_{0}", i)).GetValue(treść).ToString();

                                        WypełnijTagXml(nowyDruk, String.Format("op{0}", i), op);
                                    }
                                }

                                foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(s => s.kod_lok == lokal.kod_lok && s.nr_lok == lokal.nr_lok).ToList())
                                {
                                    decimal ilość, stawka;
                                    XmlNode nowySkładnikOpłat = składnikOpłat.CloneNode(true);
                                    DostępDoBazy.SkładnikCzynszu składnikCzynszu = DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu.FirstOrDefault(s => s.nr_skl == składnikCzynszuLokalu.nr_skl);

                                    składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilość, out stawka);

                                    switch (raport)
                                    {
                                        case Enumeratory.Raport.SkładnikiCzynszuStawkaInformacyjna:
                                            stawka = składnikCzynszu.stawka_inf;

                                            break;
                                    }

                                    decimal wartość = Decimal.Round(ilość * stawka, 2);
                                    suma += wartość;

                                    WypełnijTagXml(nowySkładnikOpłat, "nazwa", składnikCzynszu.nazwa);
                                    WypełnijTagXml(nowySkładnikOpłat, "stawka", stawka.ToString("N2"));
                                    WypełnijTagXml(nowySkładnikOpłat, "ilość", ilość.ToString("N2"));
                                    WypełnijTagXml(nowySkładnikOpłat, "wartość", wartość.ToString("N2"));
                                    składnikOpłat.ParentNode.InsertBefore(nowySkładnikOpłat, składnikOpłat);
                                }

                                WypełnijTagXml(nowyDruk, "razem", suma.ToString("N2"));
                                składnikOpłat.ParentNode.RemoveChild(składnikOpłat);
                                gotowaDefinicjaHtml.Add(nowyDruk.OuterXml);
                            }

                            DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                        }

                        break;

                    case Enumeratory.Raport.WykazWgSkladnika:
                        {
                            List<string[]> tabela = new List<string[]>();
                            List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                            int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                            int indeksOstatniego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                            wszystkieLokale = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = wszystkieLokale;
                            int nrSkładnika = identyfikatory[4];
                            DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.First(s => s.nr_skl == nrSkładnika);
                            DateTime data = Start.Data;
                            DateTime początekMiesiąca = data.AddDays(-data.Day + 1);
                            int rok = data.Year;
                            int miesiąc = data.Month;
                            DateTime koniecMiesiąca = new DateTime(rok, miesiąc, DateTime.DaysInMonth(rok, miesiąc));
                            IEnumerable<DostępDoBazy.NależnośćZPierwszegoZbioru> należnościZBieżącegoMiesiącaDotycząceDanegoSkładnika = db.NależnościZPierwszegoZbioru.Where(n => n.nr_skl == nrSkładnika && n.data_nal >= początekMiesiąca && n.data_nal <= koniecMiesiąca);
                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Adres", "Stawka", "Ilość", "Wartość" };
                            tytuł = "Wykaz wedlug skladnika";

                            for (int i = 1; i <= wszystkieLokale.Count; i++)
                            {
                                DostępDoBazy.Lokal lokal = wszystkieLokale[i - 1];
                                DostępDoBazy.NależnośćZPierwszegoZbioru należność = należnościZBieżącegoMiesiącaDotycząceDanegoSkładnika.FirstOrDefault(n => n.kod_lok == lokal.kod_lok && n.nr_lok == lokal.nr_lok);
                                decimal stawka;
                                decimal ilość;

                                if (należność == null)
                                    stawka = ilość = 0;
                                else
                                {
                                    stawka = należność.stawka;
                                    ilość = należność.ilosc;
                                }

                                decimal wartość = Decimal.Round(stawka * ilość, 2);

                                tabela.Add(new string[] { i.ToString(), lokal.kod_lok.ToString(), lokal.nr_lok.ToString(), String.Format("{0} {1}", lokal.adres, lokal.adres_2), stawka.ToString("N2"), ilość.ToString("N2"), wartość.ToString("N2") });
                            }

                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;

                            tabele.Add(tabela);
                            podpisy.Add(składnikCzynszu.nazwa);
                        }

                        break;
                }

            Session["nagłówki"] = nagłówki;
            Session["tabele"] = tabele;
            Session["podpisy"] = podpisy;
            Session["format"] = ((RadioButtonList)placeOfConfigurationFields.FindControl("format")).SelectedValue;
            Session["tytuł"] = tytuł;
            Session["gotowaDefinicjaHtml"] = gotowaDefinicjaHtml;

            Response.Redirect("Raport.aspx");
        }

        static void WypełnijTagXml(XmlNode rodzic, string id, object wartość)
        {
            if (wartość != null)
                rodzic.SelectSingleNode(XPathZnajdźElementPoId(id)).InnerText = wartość.ToString();
        }

        static string XPathZnajdźElementPoId(string id)
        {
            return String.Format("//*[@id='{0}']", id);
        }
    }
}