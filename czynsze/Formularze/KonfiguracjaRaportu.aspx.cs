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
        Enumeratory.Raport _raport;
        bool _analiza;

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
            _raport = (Enumeratory.Raport)Enum.Parse(typeof(Enumeratory.Raport), klucz.Replace("raport", String.Empty).Substring(klucz.LastIndexOf('$') + 1));
            klucz = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"));
            int indeks = Request.UrlReferrer.Query.IndexOf("id=");

            if (!String.IsNullOrEmpty(klucz))
                identyfikatory[0] = PobierzWartośćParametru<int>(klucz);

            if (indeks != -1)
                identyfikatory[1] = Int32.Parse(Request.UrlReferrer.Query.Substring(indeks + 3));

            placeOfConfigurationFields.Controls.Add(new Kontrolki.HtmlInputHidden(_raport + "raport", "#"));

            switch (_raport)
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

                case Enumeratory.Raport.MiesieczneSumySkladnikow:
                    nagłówek += "(Sumy miesięczne składnika)";

                    break;

                case Enumeratory.Raport.NaleznosciIObrotyNajemcy:
                    nagłówek += "(Należności i obroty najemcy)";

                    break;

                case Enumeratory.Raport.MiesiecznaAnalizaNaleznosciIObrotow:
                    nagłówek += "(Analiza miesięczna)";

                    break;

                case Enumeratory.Raport.SzczegolowaAnalizaNaleznosciIObrotow:
                    nagłówek += "(Analiza szczegółowa)";

                    break;

                case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                case Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla:
                case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                case Enumeratory.Raport.WykazWgSkladnika:
                case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                case Enumeratory.Raport.ObrotyZaDanyMiesiacLokale:
                case Enumeratory.Raport.ObrotyZaDanyMiesiacBudynki:
                case Enumeratory.Raport.ObrotyZaDanyMiesiacWspolnoty:
                case Enumeratory.Raport.ObrotySzczegolowoMiesiacLokale:
                case Enumeratory.Raport.ObrotySzczegolowoMiesiacBudynki:
                case Enumeratory.Raport.ObrotySzczegolowoMiesiacWspolnoty:
                case Enumeratory.Raport.ObrotyWgEwidencjiLokale:
                case Enumeratory.Raport.ObrotyWgEwidencjiBudynki:
                case Enumeratory.Raport.ObrotyWgEwidencjiWspolnoty:
                case Enumeratory.Raport.ObrotyWgGrupSkladnikiLokale:
                case Enumeratory.Raport.ObrotyWgGrupSkladnikiBudynki:
                case Enumeratory.Raport.ObrotyWgGrupSkladnikiWspolnoty:
                case Enumeratory.Raport.ObrotyWgGrupSumyLokale:
                case Enumeratory.Raport.ObrotyWgGrupSumyBudynki:
                case Enumeratory.Raport.ObrotyWgGrupSumyWspolnoty:
                    identyfikatory[0] = PobierzWartośćParametru<int>("odBudynku");
                    identyfikatory[1] = PobierzWartośćParametru<int>("odLokalu");
                    identyfikatory[2] = PobierzWartośćParametru<int>("doBudynku");
                    identyfikatory[3] = PobierzWartośćParametru<int>("doLokalu");

                    switch (_raport)
                    {
                        case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                        case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                        case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                        case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                        case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                        case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                        case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                        case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                        case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                        case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                        case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                        case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                        case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                        case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                        case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                        case Enumeratory.Raport.ObrotyZaDanyMiesiacLokale:
                        case Enumeratory.Raport.ObrotyZaDanyMiesiacBudynki:
                        case Enumeratory.Raport.ObrotyZaDanyMiesiacWspolnoty:
                        case Enumeratory.Raport.ObrotySzczegolowoMiesiacLokale:
                        case Enumeratory.Raport.ObrotySzczegolowoMiesiacBudynki:
                        case Enumeratory.Raport.ObrotySzczegolowoMiesiacWspolnoty:
                        case Enumeratory.Raport.ObrotyWgEwidencjiLokale:
                        case Enumeratory.Raport.ObrotyWgEwidencjiBudynki:
                        case Enumeratory.Raport.ObrotyWgEwidencjiWspolnoty:
                        case Enumeratory.Raport.ObrotyWgGrupSkladnikiLokale:
                        case Enumeratory.Raport.ObrotyWgGrupSkladnikiBudynki:
                        case Enumeratory.Raport.ObrotyWgGrupSkladnikiWspolnoty:
                        case Enumeratory.Raport.ObrotyWgGrupSumyLokale:
                        case Enumeratory.Raport.ObrotyWgGrupSumyBudynki:
                        case Enumeratory.Raport.ObrotyWgGrupSumyWspolnoty:
                            identyfikatory[4] = PobierzWartośćParametru<int>("odWspólnoty");
                            identyfikatory[5] = PobierzWartośćParametru<int>("doWspólnoty");
                            _analiza = true;

                            switch ((Enumeratory.Analiza)Session["trybAnalizy"])
                            {
                                case Enumeratory.Analiza.NaleznosciBiezace:
                                    nagłówek += "(Bieżące należności)";

                                    break;

                                case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                    nagłówek += "(Należności za dany miesiąc)";

                                    break;

                                case Enumeratory.Analiza.NaleznosciSzczegolowoMiesiac:
                                    nagłówek += "(Należności za dany miesiąc szczegółowo)";

                                    break;

                                case Enumeratory.Analiza.NaleznosciWgEwidencji:
                                    nagłówek += "(Należności wg ewidencji)";

                                    break;

                                case Enumeratory.Analiza.NaleznosciWgGrupSkladniki:
                                    nagłówek += "(Należności wg grup - składniki)";

                                    break;

                                case Enumeratory.Analiza.NaleznosciWgGrupSumy:
                                    nagłówek += "(Należności wg grup - sumy)";

                                    break;

                                case Enumeratory.Analiza.ObrotyBiezace:
                                    nagłówek += "(Bieżące obroty)";

                                    break;

                                case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                    nagłówek += "(Obroty za dany miesiąc)";

                                    break;

                                case Enumeratory.Analiza.ObrotySzczegolowoMiesiac:
                                    nagłówek += "(Obroty za dany miesiąc szczegółowo)";

                                    break;

                                case Enumeratory.Analiza.ObrotyWgEwidencji:
                                    nagłówek += "(Obroty wg ewidencji)";

                                    break;

                                case Enumeratory.Analiza.ObrotyWgGrupSkladniki:
                                    nagłówek += "(Obroty wg grup - składniki)";

                                    break;

                                case Enumeratory.Analiza.ObrotyWgGrupSumy:
                                    nagłówek += "(Obroty wg grup - sumy)";

                                    break;
                            }

                            break;

                        case Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla:
                            nagłówek += "(Składniki czynszu - stawka zwykła)";

                            break;
                        case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                            nagłówek += "(Składniki czynszu - stawka informacyjna)";

                            break;
                        case Enumeratory.Raport.WykazWgSkladnika:
                            identyfikatory[4] = PobierzWartośćParametru<int>("nrSkladnika");

                            switch ((Enumeratory.WykazWedługSkładnika)Convert.ChangeType(Session["trybWykazuWgSkładnika"], typeof(Enumeratory.WykazWedługSkładnika)))
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
            DateTime data = Start.Data;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                IEnumerable<DostępDoBazy.Należność> należności = null;
                IEnumerable<DostępDoBazy.Obrót> obroty = null;
                IEnumerable<DostępDoBazy.IPozycjaDoAnalizy> pozycje = null;
                IEnumerable<DostępDoBazy.IInformacjeOPozycji> informacjeOPozycjach = null;
                string napisRaportu = _raport.ToString();

                if (napisRaportu.StartsWith("Naleznosci"))
                    informacjeOPozycjach = db.SkładnikiCzynszu;

                switch (Start.AktywnyZbiór)
                {
                    case Enumeratory.Zbiór.Czynsze:
                        if (_analiza)
                        {
                            if (napisRaportu.StartsWith("Naleznosci"))
                                pozycje = db.NależnościZPierwszegoZbioru;
                        }
                        else
                        {
                            należności = db.NależnościZPierwszegoZbioru;
                            obroty = db.ObrotyZPierwszegoZbioru;
                        }

                        break;

                    case Enumeratory.Zbiór.Drugi:
                        if (_analiza)
                        {
                            if (napisRaportu.StartsWith("Naleznosci"))
                                pozycje = db.NależnościZDrugiegoZbioru;
                        }
                        else
                        {
                            należności = db.NależnościZDrugiegoZbioru;
                            obroty = db.ObrotyZDrugiegoZbioru;
                        }

                        break;

                    case Enumeratory.Zbiór.Trzeci:
                        if (_analiza)
                        {
                            if (napisRaportu.StartsWith("Naleznosci"))
                                pozycje = db.NależnościZTrzeciegoZbioru;
                        }
                        else
                        {
                            należności = db.NależnościZTrzeciegoZbioru;
                            obroty = db.ObrotyZTrzeciegoZbioru;
                        }

                        break;
                }

                switch (_raport)
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

                    case Enumeratory.Raport.MiesieczneSumySkladnikow:
                    case Enumeratory.Raport.NaleznosciIObrotyNajemcy:
                    case Enumeratory.Raport.MiesiecznaAnalizaNaleznosciIObrotow:
                    case Enumeratory.Raport.SzczegolowaAnalizaNaleznosciIObrotow:
                        {
                            tabele = new List<List<string[]>> { new List<string[]>() };
                            decimal sumaWn, sumaMa;
                            List<decimal> salda = new List<decimal>();
                            int nr_kontr = identyfikatory[1];

                            //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                            {
                                DostępDoBazy.Najemca najemca = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == nr_kontr);
                                podpisy = new List<string>() { najemca.nazwisko + " " + najemca.imie + "<br />" + najemca.adres_1 + " " + najemca.adres_2 + "<br />" };
                                List<DostępDoBazy.Należność> należnościNajemcy = należności.Where(r => r.nr_kontr == nr_kontr).ToList();
                                List<DostępDoBazy.Obrót> obrotyNajemcy = obroty.Where(r => r.nr_kontr == nr_kontr).ToList();

                                switch (_raport)
                                {
                                    case Enumeratory.Raport.MiesieczneSumySkladnikow:
                                        tytuł = String.Format("ZESTAWIENIE ROZLICZEN MIESIECZNYCH ZA ROK {0}", data.Year);
                                        nagłówki = new List<string>() { "m-c", "Wartość" };

                                        if (identyfikatory[0] < 0)
                                        {

                                            int nr_skl = należnościNajemcy.FirstOrDefault(r => r.__record == -1 * identyfikatory[0]).ZewnętrzneId;
                                            podpisy[0] += db.SkładnikiCzynszu.FirstOrDefault(c => c.Id == nr_skl).Nazwa;

                                            for (int i = 1; i <= 12; i++)
                                                tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N}", należnościNajemcy.Where(r => r.ZewnętrzneId == nr_skl).ToList().Where(r => r.Data.Year == data.Year && r.Data.Month == i).Sum(r => r.Kwota)) });
                                        }
                                        else
                                        {
                                            int kod_wplat = obrotyNajemcy.FirstOrDefault(t => t.__record == identyfikatory[0]).ZewnętrzneId;
                                            podpisy[0] += db.RodzajePłatności.FirstOrDefault(t => t.Id == kod_wplat).Nazwa;

                                            for (int i = 1; i <= 12; i++)
                                                tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N}", obrotyNajemcy.Where(t => t.ZewnętrzneId == kod_wplat).ToList().Where(t => t.Data.Year == data.Year && t.Data.Month == i).Sum(t => t.Kwota)) });
                                        }

                                        tabele[0].Add(new string[] { "Razem", String.Format("{0:N}", tabele[0].Sum(r => Single.Parse(r[1]))) });

                                        break;

                                    case Enumeratory.Raport.NaleznosciIObrotyNajemcy:
                                        tytuł = "ZESTAWIENIE NALEZNOSCI I WPLAT";
                                        nagłówki = new List<string> { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };

                                        foreach (DostępDoBazy.Należność receivable in należnościNajemcy)
                                        {
                                            List<string> pola = receivable.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                            pola.RemoveAt(0);

                                            tabele[0].Add(pola.ToArray());
                                        }

                                        foreach (DostępDoBazy.Obrót turnover in obrotyNajemcy)
                                        {
                                            List<string> pola = turnover.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                            pola.RemoveAt(0);

                                            tabele[0].Add(pola.ToArray());
                                        }

                                        tabele[0] = tabele[0].OrderBy(r => DateTime.Parse(r[2])).ToList();
                                        sumaWn = tabele[0].Sum(r => String.IsNullOrEmpty(r[0]) ? 0 : Decimal.Parse(r[0]));
                                        sumaMa = tabele[0].Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));

                                        tabele[0].Add(new string[] { String.Format("{0:N}", sumaWn), String.Format("{0:N}", sumaMa), String.Empty, String.Empty });
                                        tabele[0].Add(new string[] { "SALDO", String.Format("{0:N}", sumaMa - sumaWn), String.Empty, String.Empty });

                                        break;

                                    case Enumeratory.Raport.MiesiecznaAnalizaNaleznosciIObrotow:
                                        tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                        nagłówki = new List<string>() { "m-c", "suma WN w miesiącu", "suma MA w miesiącu", "saldo w miesiącu", "suma WN narastająco", "suma MA narastająco", "saldo narastająco" };
                                        List<decimal> kwotyWn = new List<decimal>();
                                        List<decimal> kwotyMa = new List<decimal>();

                                        for (int i = 1; i <= 12; i++)
                                        {
                                            List<string[]> miesięczneNależności = należnościNajemcy.Where(r => r.Data.Month == i).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                            List<string[]> miesięczneObroty = obrotyNajemcy.Where(t => t.Data.Month == i).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                            sumaWn = miesięczneNależności.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1])) + miesięczneObroty.Sum(t => String.IsNullOrEmpty(t[1]) ? 0 : Decimal.Parse(t[1]));
                                            sumaMa = miesięczneObroty.Sum(t => String.IsNullOrEmpty(t[2]) ? 0 : Decimal.Parse(t[2]));

                                            kwotyWn.Add(sumaWn);
                                            kwotyMa.Add(sumaMa);
                                            salda.Add(sumaMa - sumaWn);
                                            tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N}", sumaWn), String.Format("{0:N}", sumaMa), String.Format("{0:N}", salda.Last()), String.Format("{0:N}", kwotyWn.Sum()), String.Format("{0:N}", kwotyMa.Sum()), String.Format("{0:N}", salda.Sum()) });
                                        }

                                        break;

                                    case Enumeratory.Raport.SzczegolowaAnalizaNaleznosciIObrotow:
                                        tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                        nagłówki = new List<string>() { "m-c", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Saldo miesiąca", "Saldo narastająco" };
                                        string[] nowyWiersz;

                                        for (int i = 1; i <= 12; i++)
                                        {
                                            decimal[] suma = new decimal[] { 0, 0, 0, 0 };
                                            sumaWn = sumaMa = 0;

                                            foreach (DostępDoBazy.Należność należność in należnościNajemcy.Where(r => r.Data.Month == i))
                                            {
                                                string[] wiersz = należność.WażnePolaDoNależnościIObrotówNajemcy();
                                                int indeks = db.SkładnikiCzynszu.FirstOrDefault(c => c.Id == należność.ZewnętrzneId).rodz_e - 1;

                                                if (!String.IsNullOrEmpty(wiersz[1]))
                                                {
                                                    decimal kwota = Decimal.Parse(wiersz[1]);
                                                    suma[indeks] += kwota;
                                                    sumaWn += kwota;
                                                }
                                            }

                                            foreach (DostępDoBazy.Obrót obrót in obrotyNajemcy.Where(t => t.Data.Month == i))
                                            {
                                                string[] wiersz = obrót.WażnePolaDoNależnościIObrotówNajemcy();
                                                int indeks = db.RodzajePłatności.FirstOrDefault(t => t.Id == obrót.ZewnętrzneId).rodz_e - 1;

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
                                                nowyWiersz[j] = String.Format("{0:N}", suma[j - 1]);

                                            nowyWiersz[5] = String.Format("{0:N}", salda.Last());
                                            nowyWiersz[6] = String.Format("{0:N}", salda.Sum());

                                            tabele[0].Add(nowyWiersz);
                                        }

                                        nowyWiersz = new string[7];
                                        nowyWiersz[0] = "Razem";

                                        for (int i = 1; i < nowyWiersz.Length - 1; i++)
                                            nowyWiersz[i] = String.Format("{0:N}", tabele[0].Sum(r => Single.Parse(r[i])));

                                        nowyWiersz[6] = nowyWiersz[5];

                                        tabele[0].Add(nowyWiersz);

                                        break;
                                }
                            }
                        }

                        break;

                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                        {
                            Enumeratory.Analiza rodzajAnalizy = ((Enumeratory.Analiza)Convert.ChangeType(Session["trybAnalizy"], typeof(Enumeratory.Analiza)));
                            DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                            DateTime koniecMiesiąca = początekMiesiąca.AddDays(DateTime.DaysInMonth(początekMiesiąca.Year, początekMiesiąca.Month)).AddSeconds(-1);
                            IEnumerable<DostępDoBazy.Należność> należnościZaDanyMiesiąc = null;

                            switch (rodzajAnalizy)
                            {
                                case Enumeratory.Analiza.NaleznosciBiezace:
                                    tytuł = "BIEZACE NALEZNOSCI";

                                    break;

                                case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                    tytuł = String.Format("NALEZNOSCI ZA {0} {1}", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[data.Month - 1].ToString().ToUpper(), data.Year);
                                    należnościZaDanyMiesiąc = należności.Where(n => n.Data >= początekMiesiąca && n.Data <= koniecMiesiąca);

                                    break;
                            }

                            switch (_raport)
                            {
                                case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
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

                                                switch (rodzajAnalizy)
                                                {
                                                    case Enumeratory.Analiza.NaleznosciBiezace:
                                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == lokal.kod_lok && c.nr_lok == lokal.nr_lok))
                                                        {
                                                            decimal ilosc;
                                                            decimal stawka;

                                                            składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                            suma += Decimal.Round(ilosc * stawka, 2);
                                                        }

                                                        break;

                                                    case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                        foreach (DostępDoBazy.NależnośćZPierwszegoZbioru należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == lokal.kod_lok && n.nr_lok == lokal.nr_lok))
                                                            suma += należność.Kwota;

                                                        break;
                                                }

                                                tabela.Add(new string[] { indeks.ToString(), lokal.kod_lok.ToString(), lokal.nr_lok.ToString(), lokal.Rozpoznaj_kod_typ(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Format("{0:N}", suma) });

                                                indeks++;
                                                sumaBudynku += suma;
                                            }

                                            tabela.Add(new string[] { String.Empty, i.ToString(), String.Empty, String.Empty, "<b>RAZEM</b>", "<b>BUDYNEK</b>", String.Format("{0} {1}", budynki.adres, budynki.adres_2), String.Format("{0:N}", sumaBudynku) });
                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);

                                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                                            ogólnaSuma += sumaBudynku;
                                        }

                                        DostępDoBazy.Lokal.TypesOfPlace = null;
                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                                        if (tabele.Any())
                                            tabele.Last().Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", "<b>WSZYSTKIE</b>", "<b>BUDYNKI</b>", String.Format("{0:N}", ogólnaSuma) });
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
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
                                            DostępDoBazy.Budynek budynek = db.Budynki.SingleOrDefault(b => b.kod_1 == i);

                                            if (budynek != null)
                                            {
                                                decimal suma = 0;
                                                List<DostępDoBazy.AktywnyLokal> aktywneLokale = db.AktywneLokale.Where(p => p.kod_lok == i).ToList();
                                                DostępDoBazy.SkładnikCzynszuLokalu.Lokale = aktywneLokale;

                                                foreach (DostępDoBazy.AktywnyLokal aktywnyLokal in aktywneLokale)
                                                    switch (rodzajAnalizy)
                                                    {
                                                        case Enumeratory.Analiza.NaleznosciBiezace:
                                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == i && c.nr_lok == aktywnyLokal.nr_lok))
                                                            {
                                                                decimal ilosc;
                                                                decimal stawka;

                                                                składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                                suma += Decimal.Round(ilosc * stawka, 2);
                                                            }

                                                            break;

                                                        case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                            foreach (DostępDoBazy.NależnośćZPierwszegoZbioru należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == i && n.nr_lok == aktywnyLokal.nr_lok))
                                                                suma += należność.Kwota;

                                                            break;
                                                    }

                                                sumaGłówna += suma;
                                                DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;

                                                tabela.Add(new string[] { String.Format("{0}", i - kod1 + 1), budynek.kod_1.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N}", suma) });
                                            }
                                        }

                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                                        tabela.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N}", sumaGłówna) });
                                        podpisy.Add(String.Empty);
                                        tabele.Add(tabela);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                                    nagłówki = new List<string>() { "Lp.", "Kod budynku", "Adres", "Kwota czynszu" };

                                    //using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                                    {
                                        int kod1 = identyfikatory[4];
                                        int kod2 = identyfikatory[5];
                                        decimal sumaOgólna = 0;
                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();

                                        for (int i = kod1; i <= kod2; i++)
                                        {
                                            DostępDoBazy.Wspólnota wspólnota = db.Wspólnoty.SingleOrDefault(w => w.kod == i);

                                            if (wspólnota != null)
                                            {
                                                List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == i).ToList();
                                                decimal sumaWspólnoty = 0;
                                                List<string[]> tabela = new List<string[]>();

                                                foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                {
                                                    DostępDoBazy.Budynek budynek = db.Budynki.FirstOrDefault(b => b.kod_1 == budynekWspólnoty.kod_1);
                                                    List<DostępDoBazy.AktywnyLokal> aktywneLokale = db.AktywneLokale.Where(p => p.kod_lok == budynek.kod_1).ToList();
                                                    DostępDoBazy.SkładnikCzynszuLokalu.Lokale = aktywneLokale;
                                                    decimal suma = 0;

                                                    foreach (DostępDoBazy.AktywnyLokal aktywnyLokal in aktywneLokale)
                                                        switch (rodzajAnalizy)
                                                        {
                                                            case Enumeratory.Analiza.NaleznosciBiezace:
                                                                foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == i && c.nr_lok == aktywnyLokal.nr_lok))
                                                                {
                                                                    decimal ilosc;
                                                                    decimal stawka;

                                                                    składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                                    suma += Decimal.Round(ilosc * stawka, 2);
                                                                }

                                                                break;

                                                            case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                                foreach (DostępDoBazy.NależnośćZPierwszegoZbioru należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == i && n.nr_lok == aktywnyLokal.nr_lok))
                                                                    suma += należność.Kwota;

                                                                break;
                                                        }

                                                    sumaWspólnoty += suma;
                                                    DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;

                                                    tabela.Add(new string[] { String.Format("{0}", i - kod1 + 1), budynek.kod_1.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N}", suma) });
                                                }

                                                sumaOgólna += sumaWspólnoty;

                                                tabela.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N}", sumaWspólnoty) });
                                                tabele.Add(tabela);
                                                podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                            }
                                        }

                                        DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;
                                    }

                                    break;
                            }
                        }

                        break;

                    case Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla:
                    case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
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
                                    DostępDoBazy.SkładnikCzynszu składnikCzynszu = DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu.FirstOrDefault(s => s.Id == składnikCzynszuLokalu.nr_skl);

                                    składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilość, out stawka);

                                    switch (_raport)
                                    {
                                        case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                                            stawka = składnikCzynszu.stawka_inf;

                                            break;
                                    }

                                    decimal wartość = Decimal.Round(ilość * stawka, 2);
                                    suma += wartość;

                                    WypełnijTagXml(nowySkładnikOpłat, "nazwa", składnikCzynszu.Nazwa);
                                    WypełnijTagXml(nowySkładnikOpłat, "stawka", stawka.ToString("N"));
                                    WypełnijTagXml(nowySkładnikOpłat, "ilość", ilość.ToString("N"));
                                    WypełnijTagXml(nowySkładnikOpłat, "wartość", wartość.ToString("N"));
                                    składnikOpłat.ParentNode.InsertBefore(nowySkładnikOpłat, składnikOpłat);
                                }

                                WypełnijTagXml(nowyDruk, "razem", suma.ToString("N"));
                                składnikOpłat.ParentNode.RemoveChild(składnikOpłat);
                                gotowaDefinicjaHtml.Add(nowyDruk.OuterXml);
                            }

                            DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;
                            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                        }

                        break;

                    case Enumeratory.Raport.WykazWgSkladnika:
                        {
                            List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                            int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                            int indeksOstatniego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                            wszystkieLokale = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                            int nrSkładnika = identyfikatory[4];
                            DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.First(s => s.Id == nrSkładnika);
                            DateTime początekMiesiąca = DateTime.MaxValue;
                            DateTime koniecMiesiąca = new DateTime(data.Year, data.Month, 1).AddMonths(1).AddSeconds(-1);
                            IEnumerable<DostępDoBazy.Należność> należnościDotycząceDanegoSkładnika = należności.Where(n => n.ZewnętrzneId == nrSkładnika && n.Data <= koniecMiesiąca);
                            IEnumerable<DostępDoBazy.Należność> należnościDoAnalizy = null;
                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Nazwisko", "Imię", "Adres" };
                            tytuł = "Wykaz wedlug skladnika ";
                            Enumeratory.WykazWedługSkładnika tryb = (Enumeratory.WykazWedługSkładnika)Convert.ChangeType(Session["trybWykazuWgSkładnika"], typeof(Enumeratory.WykazWedługSkładnika));

                            switch (tryb)
                            {
                                case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                    string rodzajHistorii = null;
                                    początekMiesiąca = Convert.ToDateTime(Session["dataWykazuWgSkładnika"]);
                                    należnościDoAnalizy = należnościDotycząceDanegoSkładnika.Where(n => n.Data >= początekMiesiąca);

                                    switch (tryb)
                                    {
                                        case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                            rodzajHistorii = "ogolem";

                                            break;

                                        case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                            rodzajHistorii = "specyfikacja";

                                            break;
                                    }

                                    tytuł += String.Format("({0} od {1:yyyy - MM} do {2:yyyy - MM})", rodzajHistorii, początekMiesiąca, koniecMiesiąca);

                                    break;

                                case Enumeratory.WykazWedługSkładnika.Obecny:
                                    tytuł += "(Biezacy miesiac)";
                                    początekMiesiąca = data.AddDays(-data.Day + 1);
                                    należnościDoAnalizy = należnościDotycząceDanegoSkładnika.Where(n => n.Data >= początekMiesiąca);

                                    break;
                            }

                            switch (tryb)
                            {
                                case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                case Enumeratory.WykazWedługSkładnika.Obecny:
                                    nagłówki.AddRange(new string[] { "Stawka", "Ilość" });

                                    break;
                            }

                            nagłówki.Add("Wartość");

                            switch (tryb)
                            {
                                case Enumeratory.WykazWedługSkładnika.Obecny:
                                case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                    {
                                        List<string[]> tabela = new List<string[]>();

                                        for (int i = 1; i <= wszystkieLokale.Count; i++)
                                        {
                                            DostępDoBazy.Lokal lokal = wszystkieLokale[i - 1];
                                            IEnumerable<DostępDoBazy.Należność> należnościLokalu = należnościDoAnalizy.Where(n => n.kod_lok == lokal.kod_lok && n.nr_lok == lokal.nr_lok);
                                            List<string> opcjonalnyKawałekTablicy = null;
                                            decimal wartość = 0;

                                            switch (tryb)
                                            {
                                                case Enumeratory.WykazWedługSkładnika.Obecny:
                                                    decimal stawka;
                                                    decimal ilość;

                                                    if (należnościLokalu.Any())
                                                    {
                                                        DostępDoBazy.Należność należność = należnościLokalu.Single();
                                                        stawka = należność.Stawka;
                                                        ilość = należność.Ilość;
                                                        wartość = należność.Kwota;
                                                    }
                                                    else
                                                    {
                                                        stawka = 0;
                                                        ilość = 0;
                                                    }

                                                    opcjonalnyKawałekTablicy = new List<string>() { stawka.ToString("N"), ilość.ToString("N") };

                                                    break;

                                                case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                                    opcjonalnyKawałekTablicy = new List<string>();

                                                    if (należnościLokalu.Any())
                                                        wartość = należnościLokalu.Sum(n => n.Kwota);

                                                    break;
                                            }

                                            tabela.Add(new List<string>() { i.ToString(), lokal.kod_lok.ToString(), lokal.nr_lok.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2) }.Concat(opcjonalnyKawałekTablicy).Concat(new string[] { wartość.ToString("N") }).ToArray());
                                        }

                                        tabele.Add(tabela);
                                        podpisy.Add(składnikCzynszu.Nazwa);
                                    }

                                    break;

                                case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                    {
                                        List<DateTime> początkiMiesięcy = new List<DateTime>();
                                        List<DateTime> końceMiesięcy = new List<DateTime>();
                                        nagłówki = new List<string>() { "L.p.", "Nazwisko", "Imię", "Adres", "Miesiąc", "Stawka", "Ilość", "Wartość" };
                                        List<string[]> tabela = new List<string[]>();

                                        for (DateTime i = początekMiesiąca; i <= koniecMiesiąca; i = i.AddMonths(1))
                                        {
                                            początkiMiesięcy.Add(i);
                                            końceMiesięcy.Add(i.AddMonths(1).AddSeconds(-1));
                                        }

                                        for (int i = 1; i <= wszystkieLokale.Count; i++)
                                        {
                                            DostępDoBazy.AktywnyLokal lokal = wszystkieLokale[i - 1];
                                            int kodLokalu = lokal.kod_lok;
                                            int nrLokalu = lokal.nr_lok;
                                            IEnumerable<DostępDoBazy.Należność> należnościLokalu = należnościDoAnalizy.Where(n => n.kod_lok == kodLokalu && n.nr_lok == nrLokalu);

                                            tabela.Add(new string[] { i.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, String.Empty, String.Empty });

                                            for (int j = 0; j < początkiMiesięcy.Count; j++)
                                            {
                                                decimal stawka = 0;
                                                decimal ilość = 0;
                                                decimal wartość = 0;
                                                DateTime początek = początkiMiesięcy[j];

                                                if (należnościLokalu.Any())
                                                {
                                                    DostępDoBazy.Należność należność = należnościLokalu.SingleOrDefault(n => n.Data >= początek && n.Data <= końceMiesięcy[j]);

                                                    if (należność != null)
                                                    {
                                                        stawka = należność.Stawka;
                                                        ilość = należność.Ilość;
                                                        wartość = należność.Kwota;
                                                    }
                                                }

                                                tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Format("{0:yyyy - MM}", początek), stawka.ToString("N"), ilość.ToString("N"), wartość.ToString("N") });
                                            }
                                        }

                                        tabele.Add(tabela);
                                        podpisy.Add(składnikCzynszu.Nazwa);
                                    }

                                    break;
                            }
                        }

                        break;

                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                        {
                            int miesiąc = data.Month;
                            int rok = data.Year;
                            tytuł = String.Format("SZCZEGOLOWA ANALIZA NALEZNOSCI ZA M-C {0:D2} - {1}", miesiąc, rok);
                            DateTime początekMiesiąca = new DateTime(rok, miesiąc, 1);
                            DateTime koniecMiesiąca = początekMiesiąca.AddMonths(1).AddSeconds(-1);
                            List<DostępDoBazy.Należność> należnościZaDanyMiesiąc = należności.Where(n => n.Data >= początekMiesiąca && n.Data <= koniecMiesiąca).ToList();

                            switch (_raport)
                            {
                                case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                                    {
                                        List<DostępDoBazy.AktywnyLokal> lokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                        int indeksPierwszego = lokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                        int indeksOstatniego = lokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                        List<DostępDoBazy.AktywnyLokal> lokaleDoAnalizy = lokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                        List<string[]> tabela = new List<string[]>();
                                        nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Nazwisko<br /><br />Typ Lokalu", "Imię", "Adres<br /><br />Nazwa składnika", "Stawka", "Ilość", "Kwota czynszu" };
                                        decimal sumaOgólna = 0;
                                        DostępDoBazy.AktywnyLokal pierwszyLokal = lokaleDoAnalizy.First();
                                        DostępDoBazy.AktywnyLokal ostatniLokal = lokaleDoAnalizy.Last();
                                        int liczbaPorządkowa = 1;
                                        int liczbaKolumn = nagłówki.Count;

                                        for (int i = pierwszyLokal.kod_lok; i <= ostatniLokal.kod_lok; i++)
                                        {
                                            IEnumerable<DostępDoBazy.AktywnyLokal> lokaleBudynku = lokaleDoAnalizy.Where(l => l.kod_lok == i);
                                            decimal sumaBudynku = 0;
                                            DostępDoBazy.Budynek budynek = db.Budynki.First(b => b.kod_1 == i);

                                            foreach (DostępDoBazy.AktywnyLokal lokal in lokaleBudynku)
                                            {
                                                int kodLok = lokal.kod_lok;
                                                int nrLok = lokal.nr_lok;
                                                IEnumerable<DostępDoBazy.Należność> należnościLokalu = należnościZaDanyMiesiąc.Where(l => l.kod_lok == kodLok && l.nr_lok == nrLok);
                                                decimal suma = 0;
                                                DostępDoBazy.TypLokalu typLokalu = db.TypyLokali.First(t => t.kod_typ == lokal.kod_typ);

                                                tabela.Add(new string[] { i.ToString(), kodLok.ToString(), nrLok.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, String.Empty });
                                                tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, typLokalu.typ_lok, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty });

                                                foreach (DostępDoBazy.Należność należność in należnościLokalu)
                                                {
                                                    DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.First(s => s.Id == należność.ZewnętrzneId);
                                                    decimal kwota = należność.Kwota;
                                                    suma += kwota;

                                                    tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, składnikCzynszu.Nazwa, należność.Stawka.ToString("N"), należność.Ilość.ToString("N"), kwota.ToString("N") });
                                                }

                                                sumaBudynku += suma;

                                                tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", String.Empty, String.Empty, suma.ToString("N") });
                                                tabela.Add(PustaLinia(liczbaKolumn));

                                                liczbaPorządkowa++;
                                            }

                                            tabela.Add(new string[] { String.Empty, i.ToString(), String.Empty, "<b>RAZEM</b>", "<b>BUDYNEK</b>", String.Format("<b>{0} {1}</b>", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });

                                            sumaOgólna += sumaBudynku;
                                        }

                                        tabela.Add(PustaLinia(liczbaKolumn));
                                        tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", "<b>zakres lokali</b>", String.Format("<b>{0} - {1}</b>", pierwszyLokal.kod_lok, pierwszyLokal.nr_lok), String.Format("<b>{0} - {1}</b>", ostatniLokal.kod_lok, ostatniLokal.nr_lok), sumaOgólna.ToString("N") });
                                        podpisy.Add(String.Empty);
                                        tabele.Add(tabela);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                                    {
                                        List<DostępDoBazy.Budynek> budynki = db.Budynki.OrderBy(b => b.kod_1).ToList();
                                        int indeksPierwszego = budynki.FindIndex(b => b.kod_1 == identyfikatory[0]);
                                        int indeksOstatniego = budynki.FindIndex(b => b.kod_1 == identyfikatory[2]);
                                        List<DostępDoBazy.Budynek> budynkiDoAnalizy = budynki.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                        List<string[]> tabela = new List<string[]>();
                                        nagłówki = new List<string>() { "L.p.", "Kod budynku", "Adres", "Nazwa składnika", "Kwota" };

                                        for (int i = 1; i <= budynkiDoAnalizy.Count; i++)
                                        {
                                            DostępDoBazy.Budynek budynek = budynkiDoAnalizy[i - 1];
                                            Dictionary<int, decimal> nrSkładnikaNaSumę = new Dictionary<int, decimal>();
                                            int kodBudynku = budynek.kod_1;
                                            decimal sumaBudynku = 0;

                                            tabela.Add(new string[] { i.ToString(), kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty });

                                            foreach (DostępDoBazy.Należność należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == kodBudynku))
                                            {
                                                int nrSkładnika = należność.ZewnętrzneId;
                                                decimal kwota = należność.Kwota;

                                                if (nrSkładnikaNaSumę.ContainsKey(nrSkładnika))
                                                    nrSkładnikaNaSumę[nrSkładnika] += kwota;
                                                else
                                                    nrSkładnikaNaSumę.Add(nrSkładnika, kwota);
                                            }

                                            foreach (KeyValuePair<int, decimal> składnik in nrSkładnikaNaSumę.OrderBy(p => p.Key))
                                            {
                                                DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.First(s => s.Id == składnik.Key);
                                                decimal kwota = składnik.Value;
                                                sumaBudynku += kwota;

                                                tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, składnikCzynszu.Nazwa, kwota.ToString("N") });
                                            }

                                            tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", sumaBudynku.ToString("N") });
                                            tabela.Add(PustaLinia(nagłówki.Count));
                                        }

                                        tabele.Add(tabela);
                                        podpisy.Add(String.Empty);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                                    {
                                        List<DostępDoBazy.Wspólnota> wspólnoty = db.Wspólnoty.OrderBy(w => w.kod).ToList();
                                        int indeksPierwszej = wspólnoty.FindIndex(w => w.kod == identyfikatory[4]);
                                        int indeksOstatniej = wspólnoty.FindIndex(w => w.kod == identyfikatory[5]);
                                        List<DostępDoBazy.Wspólnota> wspólnotyDoAnalizy = wspólnoty.GetRange(indeksPierwszej, indeksOstatniej - indeksPierwszej + 1);
                                        nagłówki = new List<string>() { "Kod budynku", "Adres budynku", "Nazwa składnika", "Kwota" };

                                        for (int i = 1; i <= wspólnotyDoAnalizy.Count; i++)
                                        {
                                            DostępDoBazy.Wspólnota wspólnota = wspólnotyDoAnalizy[i - 1];
                                            List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == wspólnota.kod).OrderBy(b => b.kod_1).ToList();
                                            List<string[]> tabela = new List<string[]>();
                                            decimal sumaWspólnoty = 0;

                                            foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                            {
                                                DostępDoBazy.Budynek budynek = db.Budynki.First(b => b.kod_1 == budynekWspólnoty.kod_1);
                                                Dictionary<int, decimal> nrSkładnikaNaSumę = new Dictionary<int, decimal>();
                                                int kodBudynku = budynek.kod_1;
                                                decimal sumaBudynku = 0;

                                                tabela.Add(new string[] { kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty });

                                                foreach (DostępDoBazy.Należność należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == kodBudynku))
                                                {
                                                    int nrSkładnika = należność.ZewnętrzneId;
                                                    decimal kwota = należność.Kwota;

                                                    if (nrSkładnikaNaSumę.ContainsKey(nrSkładnika))
                                                        nrSkładnikaNaSumę[nrSkładnika] += kwota;
                                                    else
                                                        nrSkładnikaNaSumę.Add(nrSkładnika, kwota);
                                                }

                                                foreach (KeyValuePair<int, decimal> składnik in nrSkładnikaNaSumę.OrderBy(p => p.Key))
                                                {
                                                    DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.First(s => s.Id == składnik.Key);
                                                    decimal kwota = składnik.Value;
                                                    sumaBudynku += kwota;

                                                    tabela.Add(new string[] { String.Empty, String.Empty, składnikCzynszu.Nazwa, kwota.ToString("N") });
                                                }

                                                sumaWspólnoty += sumaBudynku;

                                                tabela.Add(new string[] { String.Empty, String.Empty, "RAZEM", sumaBudynku.ToString("N") });
                                                tabela.Add(PustaLinia(nagłówki.Count));
                                            }

                                            tabela.Add(new string[] { String.Empty, String.Empty, "RAZEM WSPÓLNOTA", sumaWspólnoty.ToString("N") });
                                            tabele.Add(tabela);
                                            podpisy.Add(wspólnota.nazwa_pel);
                                        }
                                    }

                                    break;
                            }
                        }

                        break;

                    case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                    case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                    case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                        {
                            DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                            DateTime koniecMiesiąca = początekMiesiąca.AddMonths(1).AddSeconds(-1);
                            List<DostępDoBazy.Należność> należnościZaDanyMiesiąc = należności.Where(n => n.Data >= początekMiesiąca && n.Data <= koniecMiesiąca).ToList();
                            tytuł = String.Format("KWOTA NALEZNOSCI WG RODZAJU EWIDENCJI ZA M-C {0:D2} - {1}", data.Month, data.Year);
                            Dictionary<int, decimal> słownikWzorcowy = new Dictionary<int, decimal>() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 } };

                            switch (_raport)
                            {
                                case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                                    {
                                        List<DostępDoBazy.AktywnyLokal> lokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                        int indeksPierwszego = lokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                        int indeksOstatniego = lokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                        List<DostępDoBazy.AktywnyLokal> lokaleDoAnalizy = lokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                        DostępDoBazy.AktywnyLokal pierwszyLokal = lokaleDoAnalizy.First();
                                        DostępDoBazy.AktywnyLokal ostatniLokal = lokaleDoAnalizy.Last();
                                        nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr budynku", "Nazwisko", "Imię", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Ogólna kwota czynszu" };
                                        int liczbaPorządkowa = 1;
                                        List<string[]> tabela = new List<string[]>();
                                        Dictionary<int, decimal> rodzajEwidencjiNaSumęOgólną = new Dictionary<int, decimal>(słownikWzorcowy);

                                        for (int i = pierwszyLokal.kod_lok; i <= ostatniLokal.kod_lok; i++)
                                        {
                                            DostępDoBazy.Budynek budynek = db.Budynki.First(b => b.kod_1 == i);
                                            IEnumerable<DostępDoBazy.AktywnyLokal> lokaleBudynku = lokaleDoAnalizy.Where(l => l.kod_lok == i);
                                            int kodLokalu = budynek.kod_1;
                                            Dictionary<int, decimal> rodzajEwidencjiNaSumęBudynku = new Dictionary<int, decimal>(słownikWzorcowy);

                                            foreach (DostępDoBazy.AktywnyLokal lokal in lokaleBudynku)
                                            {
                                                Dictionary<int, decimal> rodzajEwidencjiNaSumę = new Dictionary<int, decimal>(słownikWzorcowy);
                                                int nrLokalu = lokal.nr_lok;

                                                foreach (DostępDoBazy.Należność należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == kodLokalu && n.nr_lok == nrLokalu))
                                                {
                                                    decimal kwota = należność.Kwota;
                                                    DostępDoBazy.SkładnikCzynszu składnik = db.SkładnikiCzynszu.Single(s => s.Id == należność.ZewnętrzneId);
                                                    rodzajEwidencjiNaSumę[składnik.rodz_e] += kwota;
                                                }

                                                foreach (int klucz in rodzajEwidencjiNaSumę.Keys)
                                                    rodzajEwidencjiNaSumęBudynku[klucz] += rodzajEwidencjiNaSumę[klucz];

                                                tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodLokalu.ToString(), nrLokalu.ToString(), lokal.nazwisko, lokal.imie, rodzajEwidencjiNaSumę[1].ToString("N"), rodzajEwidencjiNaSumę[2].ToString("N"), rodzajEwidencjiNaSumę[3].ToString("N"), rodzajEwidencjiNaSumę[4].ToString("N"), rodzajEwidencjiNaSumę.Values.Sum().ToString("N") });

                                                liczbaPorządkowa++;
                                            }

                                            tabela.Add(new string[] { String.Empty, kodLokalu.ToString(), String.Empty, "<b>RAZEM</b>", "BUDYNEK", rodzajEwidencjiNaSumęBudynku[1].ToString("N"), rodzajEwidencjiNaSumęBudynku[2].ToString("N"), rodzajEwidencjiNaSumęBudynku[3].ToString("N"), rodzajEwidencjiNaSumęBudynku[4].ToString("N"), rodzajEwidencjiNaSumęBudynku.Values.Sum().ToString("N") });
                                            tabela.Add(PustaLinia(nagłówki.Count));

                                            foreach (int klucz in rodzajEwidencjiNaSumęBudynku.Keys)
                                                rodzajEwidencjiNaSumęOgólną[klucz] += rodzajEwidencjiNaSumęBudynku[klucz];
                                        }

                                        tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "<b>RAZEM ZAKRES LOKALI</b>", String.Format("{0}-{1} - {2}-{3}", pierwszyLokal.kod_lok, pierwszyLokal.nr_lok, ostatniLokal.kod_lok, ostatniLokal.nr_lok), rodzajEwidencjiNaSumęOgólną[1].ToString("N"), rodzajEwidencjiNaSumęOgólną[2].ToString("N"), rodzajEwidencjiNaSumęOgólną[3].ToString("N"), rodzajEwidencjiNaSumęOgólną[4].ToString("N"), rodzajEwidencjiNaSumęOgólną.Values.Sum().ToString("N") });
                                        tabele.Add(tabela);
                                        podpisy.Add(String.Empty);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                                    {
                                        int kodPierwszego = identyfikatory[0];
                                        int kodOstatniego = identyfikatory[2];
                                        int liczbaPorządkowa = 1;
                                        List<string[]> tabela = new List<string[]>();
                                        nagłówki = new List<string>() { "L.p.", "Kod budynku", "Adres", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Ogólna kwota czynszu" };
                                        Dictionary<int, decimal> rodzajEwidencjiNaSumęOgólną = new Dictionary<int, decimal>(słownikWzorcowy);

                                        for (int i = kodPierwszego; i <= kodOstatniego; i++)
                                        {
                                            DostępDoBazy.Budynek budynek = db.Budynki.SingleOrDefault(b => b.kod_1 == i);

                                            if (budynek != null)
                                            {
                                                Dictionary<int, decimal> rodzajEwidencjiNaSumęBudynku = new Dictionary<int, decimal>(słownikWzorcowy);

                                                foreach (DostępDoBazy.Należność należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == i))
                                                {
                                                    decimal kwota = należność.Kwota;
                                                    DostępDoBazy.SkładnikCzynszu składnik = db.SkładnikiCzynszu.Single(s => s.Id == należność.ZewnętrzneId);
                                                    rodzajEwidencjiNaSumęBudynku[składnik.rodz_e] += kwota;
                                                }

                                                tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), rodzajEwidencjiNaSumęBudynku[1].ToString("N"), rodzajEwidencjiNaSumęBudynku[2].ToString("N"), rodzajEwidencjiNaSumęBudynku[3].ToString("N"), rodzajEwidencjiNaSumęBudynku[4].ToString("N"), rodzajEwidencjiNaSumęBudynku.Values.Sum().ToString("N") });

                                                foreach (int klucz in rodzajEwidencjiNaSumęBudynku.Keys)
                                                    rodzajEwidencjiNaSumęOgólną[klucz] += rodzajEwidencjiNaSumęBudynku[klucz];

                                                liczbaPorządkowa++;
                                            }
                                        }

                                        tabela.Add(PustaLinia(nagłówki.Count));
                                        tabela.Add(new string[] { String.Empty, String.Empty, String.Format("RAZEM BUDYNKI {0} - {1}", kodPierwszego, kodOstatniego), rodzajEwidencjiNaSumęOgólną[1].ToString("N"), rodzajEwidencjiNaSumęOgólną[2].ToString("N"), rodzajEwidencjiNaSumęOgólną[3].ToString("N"), rodzajEwidencjiNaSumęOgólną[4].ToString("N"), rodzajEwidencjiNaSumęOgólną.Values.Sum().ToString("N") });
                                        tabele.Add(tabela);
                                        podpisy.Add(String.Empty);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                                    {
                                        int numerPierwszej = identyfikatory[4];
                                        int numerOstatniej = identyfikatory[5];
                                        nagłówki = new List<string>() { "L.p.", "Kod budynku", "Adres", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Ogólna kwota czynszu" };
                                        Dictionary<int, decimal> rodzajEwidencjiNaSumęOgólną = new Dictionary<int, decimal>(słownikWzorcowy);

                                        for (int i = numerPierwszej; i <= numerOstatniej; i++)
                                        {
                                            DostępDoBazy.Wspólnota wspólnota = db.Wspólnoty.SingleOrDefault(w => w.kod == i);

                                            if (wspólnota != null)
                                            {
                                                List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == i).OrderBy(b => b.kod_1).ToList();
                                                List<string[]> tabela = new List<string[]>();
                                                int liczbaPorządkowa = 1;
                                                Dictionary<int, decimal> rodzajEwidencjiNaSumęWspólnoty = new Dictionary<int, decimal>(słownikWzorcowy);

                                                foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                {
                                                    int kodBudynku = budynekWspólnoty.kod_1;
                                                    DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == kodBudynku);
                                                    Dictionary<int, decimal> rodzajEwidencjiNaSumęBudynku = new Dictionary<int, decimal>(słownikWzorcowy);

                                                    foreach (DostępDoBazy.Należność należność in należnościZaDanyMiesiąc.Where(n => n.kod_lok == i))
                                                    {
                                                        decimal kwota = należność.Kwota;
                                                        DostępDoBazy.SkładnikCzynszu składnik = db.SkładnikiCzynszu.Single(s => s.Id == należność.ZewnętrzneId);
                                                        rodzajEwidencjiNaSumęBudynku[składnik.rodz_e] += kwota;
                                                    }

                                                    foreach (int klucz in rodzajEwidencjiNaSumęBudynku.Keys)
                                                        rodzajEwidencjiNaSumęWspólnoty[klucz] += rodzajEwidencjiNaSumęBudynku[klucz];

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), rodzajEwidencjiNaSumęBudynku[1].ToString("N"), rodzajEwidencjiNaSumęBudynku[2].ToString("N"), rodzajEwidencjiNaSumęBudynku[3].ToString("N"), rodzajEwidencjiNaSumęBudynku[4].ToString("N"), rodzajEwidencjiNaSumęBudynku.Values.Sum().ToString("N") });
                                                }

                                                tabela.Add(new string[] { String.Empty, String.Empty, "RAZEM", rodzajEwidencjiNaSumęWspólnoty[1].ToString("N"), rodzajEwidencjiNaSumęWspólnoty[2].ToString("N"), rodzajEwidencjiNaSumęWspólnoty[3].ToString("N"), rodzajEwidencjiNaSumęWspólnoty[4].ToString("N"), rodzajEwidencjiNaSumęWspólnoty.Values.Sum().ToString("N") });
                                                tabele.Add(tabela);
                                                podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                            }
                                        }

                                        tabele.Last().Add(new string[] { String.Empty, String.Empty, String.Format("RAZEM WSPÓLNOTY {0} - {1}", numerPierwszej, numerOstatniej), rodzajEwidencjiNaSumęOgólną[1].ToString("N"), rodzajEwidencjiNaSumęOgólną[2].ToString("N"), rodzajEwidencjiNaSumęOgólną[3].ToString("N"), rodzajEwidencjiNaSumęOgólną[4].ToString("N"), rodzajEwidencjiNaSumęOgólną.Values.Sum().ToString("N") });
                                    }

                                    break;
                            }
                        }

                        break;

                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                    case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                    case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                    case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                        {
                            DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                            DateTime koniecMiesiąca = początekMiesiąca.AddMonths(1).AddSeconds(-1);
                            tytuł = String.Format("ANALIZA NALEZNOSCI WG GRUP CZYNSZOWYCH ZA M-C {0:D2} - {1}", data.Month, data.Year);
                            List<int> grupySkładnikówCzynszu = (List<int>)Session["grupySkładnikówCzynszu"];
                            List<DostępDoBazy.Należność> należnościZaDanyMiesiąc = należności.Where(n => n.Data >= początekMiesiąca && n.Data <= koniecMiesiąca).ToList();
                            List<DostępDoBazy.SkładnikCzynszu> składnikiCzynszuZWybranychGrup = db.SkładnikiCzynszu.Where(s => grupySkładnikówCzynszu.Contains(s.kod)).ToList();
                            IEnumerable<int> numerySkładnikówCzynszuZWybranychGrup = składnikiCzynszuZWybranychGrup.Select(s => s.Id);
                            IEnumerable<DostępDoBazy.Należność> należnościDoAnalizy = należnościZaDanyMiesiąc.OrderBy(n => n.ZewnętrzneId);
                            List<DostępDoBazy.GrupaSkładnikówCzynszu> wybraneGrupySkładnikówCzynszu = grupySkładnikówCzynszu.Select(g => db.GrupySkładnikówCzynszu.Single(gs => gs.kod == g)).ToList();
                            Dictionary<int, List<DostępDoBazy.Należność>> numerGrupyNaNależności = new Dictionary<int, List<DostępDoBazy.Należność>>();

                            foreach (int grupa in grupySkładnikówCzynszu)
                                numerGrupyNaNależności.Add(grupa, new List<DostępDoBazy.Należność>());

                            foreach (DostępDoBazy.Należność należność in należnościDoAnalizy)
                            {
                                int numerSkładnika = należność.ZewnętrzneId;

                                if (numerySkładnikówCzynszuZWybranychGrup.Contains(numerSkładnika))
                                {
                                    DostępDoBazy.SkładnikCzynszu składnik = składnikiCzynszuZWybranychGrup.Single(s => s.Id == numerSkładnika);

                                    numerGrupyNaNależności[składnik.kod].Add(należność);
                                }
                            }

                            switch (_raport)
                            {
                                case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                                    {
                                        List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                        int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                        int indeksOstatniego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                        List<DostępDoBazy.AktywnyLokal> lokaleDoAnalizy = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                        DostępDoBazy.AktywnyLokal pierwszy = lokaleDoAnalizy.First();
                                        DostępDoBazy.AktywnyLokal ostatni = lokaleDoAnalizy.Last();
                                        nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Nazwisko", "Imię<br /><br />Nazwa składnika", "Adres<br /><br />Grupa składnika", "Ilość", "Stawka", "Kwota" };
                                        int liczbaKolumn = nagłówki.Count;
                                        int liczbaPorządkowa = 1;
                                        decimal sumaOgólna = 0;

                                        for (int i = pierwszy.kod_lok; i <= ostatni.kod_lok; i++)
                                        {
                                            IEnumerable<DostępDoBazy.AktywnyLokal> lokaleBudynku = lokaleDoAnalizy.Where(l => l.kod_lok == i);

                                            if (lokaleBudynku.Any())
                                            {
                                                DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == i);
                                                List<string[]> tabela = new List<string[]>();
                                                decimal sumaBudynku = 0;

                                                foreach (DostępDoBazy.AktywnyLokal lokal in lokaleBudynku)
                                                {
                                                    int numerLokalu = lokal.nr_lok;
                                                    decimal sumaLokalu = 0;
                                                    List<string[]> podTabela = new List<string[]>();

                                                    for (int j = 0; j < numerGrupyNaNależności.Count; j++)
                                                    {
                                                        KeyValuePair<int, List<DostępDoBazy.Należność>> para = numerGrupyNaNależności.ElementAt(j);
                                                        IEnumerable<DostępDoBazy.Należność> należnościLokalu = para.Value.Where(n => n.kod_lok == i && n.nr_lok == numerLokalu);
                                                        DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupySkładnikówCzynszu[j];
                                                        decimal sumaGrupy = 0;
                                                        decimal sumaStawek = 0;

                                                        foreach (DostępDoBazy.Należność należność in należnościLokalu)
                                                        {
                                                            DostępDoBazy.SkładnikCzynszu składnik = składnikiCzynszuZWybranychGrup.Single(s => s.Id == należność.ZewnętrzneId);
                                                            decimal kwota = należność.Kwota;
                                                            decimal stawka = należność.Stawka;
                                                            sumaGrupy += kwota;
                                                            sumaStawek += stawka;

                                                            podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, składnik.Nazwa, grupa.nazwa, należność.Ilość.ToString("N"), stawka.ToString("N"), kwota.ToString("N") });
                                                        }

                                                        sumaLokalu += sumaGrupy;

                                                        podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "RAZEM", grupa.nazwa, String.Empty, sumaStawek.ToString("N"), sumaGrupy.ToString("N") });
                                                        podTabela.Add(PustaLinia(liczbaKolumn));
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), numerLokalu.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, sumaLokalu.ToString("N") });
                                                    tabela.Add(PustaLinia(liczbaKolumn));
                                                    tabela.AddRange(podTabela);

                                                    liczbaPorządkowa++;
                                                    sumaBudynku += sumaLokalu;
                                                }

                                                tabela.Add(new string[] { String.Empty, i.ToString(), String.Empty, String.Empty, String.Empty, "RAZEM BUDYNEK", String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                tabele.Add(tabela);
                                                podpisy.Add(String.Format("Budynek nr {0}, {1} {2}", i, budynek.adres, budynek.adres_2));

                                                sumaOgólna += sumaBudynku;
                                            }
                                        }

                                        List<string[]> ostatniaTabela = tabele.Last();

                                        ostatniaTabela.Add(PustaLinia(liczbaKolumn));
                                        ostatniaTabela.Add(PustaLinia(liczbaKolumn));
                                        ostatniaTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("ZAKRES LOKALI {0}-{1} - {2}-{3}", pierwszy.kod_lok, pierwszy.nr_lok, ostatni.kod_lok, ostatni.nr_lok), String.Empty, String.Empty, sumaOgólna.ToString("N") });
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                                    {
                                        int numerPierwszego = identyfikatory[0];
                                        int numerOstatniego = identyfikatory[2];
                                        nagłówki = new List<string>() { "L.p", "Kod budynku", "Adres", "Składnik", "Grupa", "Kwota" };
                                        List<string[]> tabela = new List<string[]>();
                                        int liczbaPorządkowa = 1;
                                        int liczbaKolumn = nagłówki.Count;

                                        for (int i = numerPierwszego; i <= numerOstatniego; i++)
                                        {
                                            DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == i);
                                            decimal sumaBudynku = 0;
                                            List<string[]> podTabela = new List<string[]>();

                                            for (int j = 0; j < numerGrupyNaNależności.Count; j++)
                                            {
                                                KeyValuePair<int, List<DostępDoBazy.Należność>> paraGrupaNależności = numerGrupyNaNależności.ElementAt(j);
                                                string nazwaGrupy = wybraneGrupySkładnikówCzynszu[j].nazwa;
                                                IEnumerable<DostępDoBazy.Należność> należnościBudynku = paraGrupaNależności.Value.Where(n => n.kod_lok == i);
                                                Dictionary<int, decimal> numerSkładnikaNaSumę = new Dictionary<int, decimal>();

                                                foreach (DostępDoBazy.Należność należność in należnościBudynku)
                                                {
                                                    int numerSkładnika = należność.ZewnętrzneId;
                                                    decimal kwota = należność.Kwota;

                                                    if (numerSkładnikaNaSumę.ContainsKey(numerSkładnika))
                                                        numerSkładnikaNaSumę[numerSkładnika] += kwota;
                                                    else
                                                        numerSkładnikaNaSumę.Add(numerSkładnika, kwota);
                                                }

                                                foreach (KeyValuePair<int, decimal> paraSkładnikSuma in numerSkładnikaNaSumę)
                                                {
                                                    DostępDoBazy.SkładnikCzynszu składnik = składnikiCzynszuZWybranychGrup.Single(s => s.Id == paraSkładnikSuma.Key);

                                                    podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, składnik.Nazwa, nazwaGrupy, paraSkładnikSuma.Value.ToString("N") });
                                                }

                                                decimal sumaGrupy = numerSkładnikaNaSumę.Values.Sum();
                                                sumaBudynku += sumaGrupy;

                                                podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM", nazwaGrupy, sumaGrupy.ToString("N") });
                                                podTabela.Add(PustaLinia(liczbaKolumn));
                                            }

                                            tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                            tabela.Add(PustaLinia(liczbaKolumn));
                                            tabela.AddRange(podTabela);

                                            liczbaPorządkowa++;
                                        }

                                        tabele.Add(tabela);
                                        podpisy.Add(String.Empty);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                                    {
                                        int indeksPierwszej = identyfikatory[4];
                                        int indeksOstatniej = identyfikatory[5];
                                        int liczbaPorządkowa = 1;
                                        nagłówki = new List<string>() { "L.p", "Kod budynku", "Adres", "Składnik", "Grupa", "Kwota" };
                                        int liczbaKolumn = nagłówki.Count;

                                        for (int i = indeksPierwszej; i <= indeksOstatniej; i++)
                                        {
                                            List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == i).ToList();

                                            if (budynkiWspólnoty.Any())
                                            {
                                                List<string[]> tabela = new List<string[]>();
                                                DostępDoBazy.Wspólnota wspólnota = db.Wspólnoty.Single(w => w.kod == i);

                                                foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                {
                                                    DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == budynekWspólnoty.kod_1);
                                                    decimal sumaBudynku = 0;
                                                    List<string[]> podTabela = new List<string[]>();

                                                    for (int j = 0; j < numerGrupyNaNależności.Count; j++)
                                                    {
                                                        KeyValuePair<int, List<DostępDoBazy.Należność>> paraGrupaNależności = numerGrupyNaNależności.ElementAt(j);
                                                        string nazwaGrupy = wybraneGrupySkładnikówCzynszu[j].nazwa;
                                                        IEnumerable<DostępDoBazy.Należność> należnościBudynku = paraGrupaNależności.Value.Where(n => n.kod_lok == i);
                                                        Dictionary<int, decimal> numerSkładnikaNaSumę = new Dictionary<int, decimal>();

                                                        foreach (DostępDoBazy.Należność należność in należnościBudynku)
                                                        {
                                                            int numerSkładnika = należność.ZewnętrzneId;
                                                            decimal kwota = należność.Kwota;

                                                            if (numerSkładnikaNaSumę.ContainsKey(numerSkładnika))
                                                                numerSkładnikaNaSumę[numerSkładnika] += kwota;
                                                            else
                                                                numerSkładnikaNaSumę.Add(numerSkładnika, kwota);
                                                        }

                                                        foreach (KeyValuePair<int, decimal> paraSkładnikSuma in numerSkładnikaNaSumę)
                                                        {
                                                            DostępDoBazy.SkładnikCzynszu składnik = składnikiCzynszuZWybranychGrup.Single(s => s.Id == paraSkładnikSuma.Key);

                                                            podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, składnik.Nazwa, nazwaGrupy, paraSkładnikSuma.Value.ToString("N") });
                                                        }

                                                        decimal sumaGrupy = numerSkładnikaNaSumę.Values.Sum();
                                                        sumaBudynku += sumaGrupy;

                                                        podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM", nazwaGrupy, sumaGrupy.ToString("N") });
                                                        podTabela.Add(PustaLinia(liczbaKolumn));
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                    tabela.Add(PustaLinia(liczbaKolumn));
                                                    tabela.AddRange(podTabela);
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));

                                                    liczbaPorządkowa++;
                                                }
                                            }
                                        }
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                                    {
                                        List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                        int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                        int indeksOstatniego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                        List<DostępDoBazy.AktywnyLokal> lokaleDoAnalizy = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                        DostępDoBazy.AktywnyLokal pierwszyLokal = lokaleDoAnalizy.First();
                                        DostępDoBazy.AktywnyLokal ostatniLokal = lokaleDoAnalizy.Last();
                                        int liczbaPorządkowa = 1;
                                        nagłówki = new List<string>() { "Lp.", "Kod budynku", "Nr lokalu", "Nazwisko", "Imię", "Adres", "Grupa", "Ilość", "Kwota" };
                                        decimal sumaOgólna = 0;
                                        int liczbaKolumn = nagłówki.Count;

                                        for (int i = pierwszyLokal.kod_lok; i <= ostatniLokal.kod_lok; i++)
                                        {
                                            IEnumerable<DostępDoBazy.AktywnyLokal> lokaleBudynku = lokaleDoAnalizy.Where(l => l.kod_lok == i);

                                            if (lokaleBudynku.Any())
                                            {
                                                List<string[]> tabela = new List<string[]>();
                                                decimal sumaBudynku = 0;
                                                DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == i);

                                                foreach (DostępDoBazy.AktywnyLokal lokal in lokaleBudynku)
                                                {
                                                    int numerLokalu = lokal.nr_lok;
                                                    decimal sumaLokalu = 0;
                                                    List<string[]> podTabela = new List<string[]>();

                                                    for (int j = 0; j < numerGrupyNaNależności.Count; j++)
                                                    {
                                                        KeyValuePair<int, List<DostępDoBazy.Należność>> para = numerGrupyNaNależności.ElementAt(j);
                                                        IEnumerable<DostępDoBazy.Należność> należnościLokalu = para.Value.Where(n => n.kod_lok == i && n.nr_lok == numerLokalu);
                                                        DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupySkładnikówCzynszu[j];
                                                        decimal sumaKwot = 0;
                                                        decimal sumaIlości = 0;

                                                        foreach (DostępDoBazy.Należność należność in należnościLokalu)
                                                        {
                                                            sumaIlości += należność.Ilość;
                                                            sumaKwot += należność.Kwota;
                                                        }

                                                        sumaLokalu += sumaKwot;

                                                        podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, grupa.nazwa, sumaIlości.ToString("N"), sumaKwot.ToString("N") });
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), numerLokalu.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, sumaLokalu.ToString("N") });
                                                    tabela.AddRange(podTabela);
                                                    tabela.Add(PustaLinia(liczbaKolumn));

                                                    liczbaPorządkowa++;
                                                    sumaBudynku += sumaLokalu;
                                                }

                                                sumaOgólna += sumaBudynku;

                                                tabela.Add(new string[] { String.Empty, i.ToString(), String.Empty, "RAZEM", "BUDYNEK", String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                tabele.Add(tabela);
                                                podpisy.Add(String.Empty);
                                            }

                                            List<string[]> ostatniaTabela = tabele.Last();

                                            ostatniaTabela.Add(PustaLinia(liczbaKolumn));
                                            ostatniaTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM", "ZAKRES", String.Format("LOKALI {0}-{1} - {2}-{3}", pierwszyLokal.kod_lok, pierwszyLokal.nr_lok, ostatniLokal.kod_lok, ostatniLokal.nr_lok), String.Empty, String.Empty, sumaOgólna.ToString("N") });
                                        }
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                                    {
                                        nagłówki = new List<string>() { "Liczba porządkowa", "Kod budynku", "Adres", "Grupa", "Ilość", "Kwota" };
                                        int kodPierwszego = identyfikatory[0];
                                        int kodOstatniego = identyfikatory[2];
                                        List<string[]> tabela = new List<string[]>();
                                        decimal liczbaPorządkowa = 1;

                                        for (int i = kodPierwszego; i <= kodOstatniego; i++)
                                            if (db.AktywneLokale.Any(l => l.kod_lok == i))
                                            {
                                                DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == i);
                                                List<string[]> podTabela = new List<string[]>();
                                                decimal sumaBudynku = 0;

                                                for (int j = 0; j < numerGrupyNaNależności.Count; j++)
                                                {
                                                    KeyValuePair<int, List<DostępDoBazy.Należność>> para = numerGrupyNaNależności.ElementAt(j);
                                                    IEnumerable<DostępDoBazy.Należność> należnościBudynku = para.Value.Where(n => n.kod_lok == i);
                                                    DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupySkładnikówCzynszu[j];
                                                    decimal sumaKwot = 0;
                                                    decimal sumaIlości = 0;

                                                    foreach (DostępDoBazy.Należność należność in należnościBudynku)
                                                    {
                                                        sumaIlości += należność.Ilość;
                                                        sumaKwot += należność.Kwota;
                                                    }

                                                    sumaBudynku += sumaKwot;

                                                    podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, grupa.nazwa, sumaIlości.ToString("N"), sumaKwot.ToString("N") });
                                                }

                                                tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                tabela.AddRange(podTabela);
                                                tabela.Add(PustaLinia(nagłówki.Count));
                                            }

                                        tabele.Add(tabela);
                                        podpisy.Add(String.Empty);
                                    }

                                    break;

                                case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                                    {
                                        nagłówki = new List<string>() { "Liczba porządkowa", "Kod budynku", "Adres", "Grupa", "Ilość", "Kwota" };
                                        int kodPierwszej = identyfikatory[4];
                                        int kodOstatniej = identyfikatory[5];
                                        decimal liczbaPorządkowa = 1;

                                        for (int i = kodPierwszej; i <= kodOstatniej; i++)
                                        {
                                            List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == i).ToList();

                                            if (budynkiWspólnoty.Any())
                                            {
                                                List<string[]> tabela = new List<string[]>();
                                                DostępDoBazy.Wspólnota wspólnota = db.Wspólnoty.Single(w => w.kod == i);
                                                decimal sumaWspólnoty = 0;

                                                foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                {
                                                    DostępDoBazy.Budynek budynek = db.Budynki.Single(b => b.kod_1 == i);
                                                    List<string[]> podTabela = new List<string[]>();
                                                    decimal sumaBudynku = 0;

                                                    for (int j = 0; j < numerGrupyNaNależności.Count; j++)
                                                    {
                                                        KeyValuePair<int, List<DostępDoBazy.Należność>> para = numerGrupyNaNależności.ElementAt(j);
                                                        IEnumerable<DostępDoBazy.Należność> należnościBudynku = para.Value.Where(n => n.kod_lok == i);
                                                        DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupySkładnikówCzynszu[j];
                                                        decimal sumaKwot = 0;
                                                        decimal sumaIlości = 0;

                                                        foreach (DostępDoBazy.Należność należność in należnościBudynku)
                                                        {
                                                            sumaIlości += należność.Ilość;
                                                            sumaKwot += należność.Kwota;
                                                        }

                                                        sumaBudynku += sumaKwot;

                                                        podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, grupa.nazwa, sumaIlości.ToString("N"), sumaKwot.ToString("N") });
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), i.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                    tabela.AddRange(podTabela);
                                                    tabela.Add(PustaLinia(nagłówki.Count));

                                                    sumaWspólnoty += sumaBudynku;
                                                    liczbaPorządkowa++;
                                                }

                                                tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM WSPÓLNOTA", String.Empty, sumaWspólnoty.ToString("N") });
                                                tabele.Add(tabela);
                                                podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                            }
                                        }
                                    }

                                    break;
                            }
                        }

                        break;
                }
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

        static string[] PustaLinia(int ilośćKolumn)
        {
            string[] linia = new string[ilośćKolumn];

            linia[0] = ".";

            for (int i = 1; i < ilośćKolumn; i++)
                linia[i] = String.Empty;

            return linia;
        }
    }
}