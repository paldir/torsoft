using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

namespace czynsze.Formularze
{
    public partial class List : Strona
    {
        Enumeratory.Tabela table;

        List<string[]> _wiersze
        {
            get
            {
                object wartość = ViewState["wiersze"];

                if (wartość == null)
                    return null;
                else
                    return (List<string[]>)wartość;
            }

            set { ViewState["wiersze"] = value; }
        }

        string[] headers
        {
            get
            {
                if (ViewState["headers"] == null)
                    return new string[0];
                else
                    return (string[])ViewState["headers"];
            }

            set { ViewState["headers"] = value; }
        }

        Enumeratory.PorządekSortowania sortOrder
        {
            get
            {
                if (ViewState["sortOrder"] == null)
                    return Enumeratory.PorządekSortowania.Rosnaco;
                else
                    return (Enumeratory.PorządekSortowania)Enum.Parse(typeof(Enumeratory.PorządekSortowania), ViewState["sortOrder"].ToString());
            }

            set { ViewState["sortOrder"] = value; }
        }

        bool sortable
        {
            get
            {
                if (ViewState["sortable"] == null)
                    return true;

                return (bool)ViewState["sortable"];
            }

            set { ViewState["sortable"] = value; }
        }

        List<int> indexesOfNumericColumns
        {
            get
            {
                if (ViewState["indexesOfNumericColumns"] == null)
                    return new List<int>();

                return (List<int>)ViewState["indexesOfNumericColumns"];
            }

            set { ViewState["indexesOfNumericColumns"] = value; }
        }

        List<int> indexesOfColumnsWithSummary
        {
            get
            {
                if (ViewState["indexesOfColumnsWithSummary"] == null)
                    return new List<int>();

                return (List<int>)ViewState["indexesOfColumnsWithSummary"];
            }

            set { ViewState["indexesOfColumnsWithSummary"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
                table = PobierzWartośćParametru<Enumeratory.Tabela>("table");
                string postBackUrl = "Record.aspx";
                string heading;
                string nodeOfSiteMapPath;
                List<string[]> subMenu = null;
                sortable = true;
                int id = PobierzWartośćParametru<int>("id");//-1;
                IEnumerable<DostępDoBazy.IRekordWyświetlanyWTabeli> rekordyTabeli = null;

                //if (Request.Params["id"] != null)
                //  id = (int)Enum.Parse(typeof(int), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("dfsdf"))]);

                switch (table)
                {
                    case Enumeratory.Tabela.NieaktywneLokale:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));

                        break;

                    case Enumeratory.Tabela.NaleznosciWedlugNajemcow:
                    case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                    case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                    case Enumeratory.Tabela.SaldoNajemcy:

                        break;

                    default:
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "addaction", "Dodaj", postBackUrl));
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "editaction", "Edytuj", postBackUrl));
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "deleteaction", "Usuń", postBackUrl));
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));

                        break;
                }

                switch (table)
                {
                    case Enumeratory.Tabela.Budynki:
                        heading = nodeOfSiteMapPath = "Budynki";
                        headers = new string[] { "Kod", "Adres", "Adres cd." };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.Budynki.OrderBy(b => b.kod_1);

                        break;

                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:
                        heading = nodeOfSiteMapPath = "Lokale";
                        headers = new string[] { "Kod budynku", "Numer lokalu", "Typ lokalu", "Powierzchnia użytkowa", "Nazwisko", "Imię" };
                        indexesOfNumericColumns = new List<int>() { 1, 2, 4 };
                        IEnumerable<DostępDoBazy.Lokal> places = null;

                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "moveaction", "Przenieś", postBackUrl));

                        switch (table)
                        {
                            case Enumeratory.Tabela.AktywneLokale:
                                heading += " (aktywne)";
                                subMenu = new List<string[]>()
                                {
                                    new string[]
                                    {
                                        "Wydruki",
                                        "<a href='KonfiguracjaRaportu.aspx?"+Enumeratory.Raport.LokaleWBudynkach+"raport=#'>Lokale w budynkach</a>",
                                        "<a href='#'>Kolejny wydruk</a>",
                                        "<a href='#'>I jeszcze jeden</a>"
                                    }
                                };

                                if (!IsPostBack)
                                    places = db.AktywneLokale;

                                break;

                            case Enumeratory.Tabela.NieaktywneLokale:
                                heading += " (nieaktywne)";

                                if (!IsPostBack)
                                    places = db.NieaktywneLokale;

                                break;
                        }

                        if (places != null)
                        {
                            DostępDoBazy.Lokal.TypesOfPlace = db.TypyLokali.ToList();
                            rekordyTabeli = places.OrderBy(p => p.kod_lok).ThenBy(p => p.nr_lok);
                        }

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        heading = nodeOfSiteMapPath = "Najemcy";
                        headers = new string[] { "Numer kontrolny", "Nazwisko", "Imię", "Adres", "Adres cd." };
                        indexesOfNumericColumns = new List<int>() { 1 };
                        IEnumerable<DostępDoBazy.Najemca> tenants = null;

                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "moveaction", "Przenieś", postBackUrl));

                        switch (table)
                        {
                            case Enumeratory.Tabela.AktywniNajemcy:
                                heading += " (aktywni)";

                                if (!IsPostBack)
                                    tenants = db.AktywniNajemcy;

                                break;

                            case Enumeratory.Tabela.NieaktywniNajemcy:
                                heading += " (nieaktywni)";

                                if (!IsPostBack)
                                    tenants = db.NieaktywniNajemcy;

                                break;
                        }

                        if (tenants != null)
                            rekordyTabeli = tenants.OrderBy(t => t.nazwisko).ThenBy(t => t.imie);

                        break;

                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        heading = nodeOfSiteMapPath = "Składniki opłat";
                        headers = new string[] { "Numer", "Nazwa", "Sposób naliczania", "Typ", "Stawka zł" };
                        indexesOfNumericColumns = new List<int>() { 1, 5 };

                        if (!IsPostBack)
                            rekordyTabeli = db.SkładnikiCzynszu.OrderBy(c => c.nr_skl);

                        break;

                    case Enumeratory.Tabela.Wspolnoty:
                        heading = nodeOfSiteMapPath = "Wspólnoty";
                        headers = new string[] { "Kod", "Nazwa wspólnoty", "Il. bud.", "Il. miesz." };
                        indexesOfNumericColumns = new List<int>() { 1, 3, 4 };

                        if (!IsPostBack)
                            rekordyTabeli = db.Wspólnoty.OrderBy(c => c.kod);

                        break;

                    case Enumeratory.Tabela.TypyLokali:
                        heading = nodeOfSiteMapPath = "Typy lokali";
                        headers = new string[] { "Kod", "Typ lokalu" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.TypyLokali.OrderBy(t => t.kod_typ);

                        break;

                    case Enumeratory.Tabela.TypyKuchni:
                        heading = nodeOfSiteMapPath = "Rodzaje kuchni";
                        headers = new string[] { "Kod", "Rodzaj kuchni" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.TypyKuchni.OrderBy(t => t.kod_kuch);

                        break;

                    case Enumeratory.Tabela.RodzajeNajemcy:
                        heading = nodeOfSiteMapPath = "Rodzaje najemców";
                        headers = new string[] { "Kod", "Rodzaj najemcy" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.TypyNajemców.OrderBy(t => t.kod_najem);

                        break;

                    case Enumeratory.Tabela.TytulyPrawne:
                        heading = nodeOfSiteMapPath = "Tytuły prawne do lokali";
                        headers = new string[] { "Kod", "Tytuł prawny" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.TytułyPrawne.OrderBy(t => t.kod_praw);

                        break;

                    case Enumeratory.Tabela.TypyWplat:
                        heading = nodeOfSiteMapPath = "Rodzaje wpłat i wypłat";
                        headers = new string[] { "Kod", "Rodzaj wpłaty lub wypłaty", "Sposób rozliczania", "Odsetki", "NO" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.RodzajePłatności.OrderBy(t => t.kod_wplat);

                        break;

                    case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                        heading = nodeOfSiteMapPath = "Grupy składników czynszu";
                        headers = new string[] { "Kod", "Nazwa grupy" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.GrupySkładnikówCzynszu.OrderBy(g => g.kod);

                        break;

                    case Enumeratory.Tabela.GrupyFinansowe:
                        heading = nodeOfSiteMapPath = "Grupy finansowe";
                        headers = new string[] { "Kod", "Konto", "Nazwa grupy" };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.GrupyFinansowe.OrderBy(g => g.kod);

                        break;

                    case Enumeratory.Tabela.StawkiVat:
                        heading = nodeOfSiteMapPath = "Stawki VAT";
                        headers = new string[] { "Oznaczenie stawki", "Symbol fiskalny" };

                        if (!IsPostBack)
                            rekordyTabeli = db.StawkiVat.OrderBy(r => r.symb_fisk);

                        break;

                    case Enumeratory.Tabela.Atrybuty:
                        heading = nodeOfSiteMapPath = "Cechy obiektów";
                        headers = new string[] { "Kod", "Nazwa", "N/C", "L.", "N.", "B.", "Wsp." };
                        indexesOfNumericColumns = new List<int>() { 1 };

                        if (!IsPostBack)
                            rekordyTabeli = db.Atrybuty.OrderBy(a => a.kod);

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        heading = nodeOfSiteMapPath = "Użytkownicy";
                        headers = new string[] { "Symbol", "Nazwisko", "Imię" };

                        if (!IsPostBack)
                            rekordyTabeli = db.Użytkownicy.OrderBy(u => u.symbol);

                        break;

                    case Enumeratory.Tabela.NaleznosciWedlugNajemcow:
                        heading = nodeOfSiteMapPath = "Należności i obroty według najemców";
                        headers = new string[] { "Numer kontrolny", "Nazwisko", "Imię", "Adres", "Adres cd." };
                        sortable = false;
                        indexesOfNumericColumns = new List<int>();// { 3, 4 };
                        subMenu = new List<string[]>()
                        {
                            new string[]
                            {
                                "Należności",
                                "<a href=\"javascript: Redirect('List.aspx?table=WszystkieNaleznosciNajemcy')\">Wszystkie</a>",
                                "<a href=\"javascript: Redirect('List.aspx?table=NieprzeterminowaneNaleznosciNajemcy')\">Nieprzeterminowane</a>"
                            },
                            new string[]
                            {
                                "Rozliczenia",
                                "<a href=\"javascript: Redirect('NaleznosciIObrotyNajemcy.aspx?dummy=dummy')\">Należności i obroty</a>",
                                "<a href='#'>Zaległości płatnicze</a>",
                            }
                        };

                        if (!IsPostBack)
                        {
                            DostępDoBazy.Najemca.AktywneLokale = db.AktywneLokale.ToList();
                            rekordyTabeli = db.AktywniNajemcy.OrderBy(t => t.nazwisko).ThenBy(t => t.imie);
                            DostępDoBazy.Najemca.AktywneLokale = null;
                        }

                        Kontrolki.RadioButtonList list = new Kontrolki.RadioButtonList("list", "by", new List<string> { "wg nazwiska", "wg kodu lokalu" }, new List<string> { "nazwisko", "kod" }, "nazwisko", true, true);
                        list.SelectedIndexChanged += list_SelectedIndexChanged;

                        placeOfMainTableButtons.Controls.Add(list);
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("button", "saldo", "Saldo", "javascript: Redirect('List.aspx?table=SaldoNajemcy')"));

                        break;

                    case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                    case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                        headers = new string[] { "Kwota należności", "Termin zapłaty", "Uwagi", "Kod lokalu", "Nr lokalu" };
                        sortable = false;
                        indexesOfNumericColumns = new List<int>() { 1, 4, 5 };
                        indexesOfColumnsWithSummary = new List<int>() { 1 };
                        {
                            DostępDoBazy.Najemca tenant = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == id);
                            heading = nodeOfSiteMapPath = "Należności najemcy " + tenant.nazwisko + " " + tenant.imie;
                            List<DostępDoBazy.Należność1> receivables = db.Należności1.Where(r => r.nr_kontr == id).OrderBy(r => r.data_nal).ToList();

                            switch (table)
                            {
                                case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                                    rekordyTabeli = receivables;

                                    break;

                                case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                                    heading += " (nieprzeterminowane)";
                                    rekordyTabeli = receivables.Where(r => r.data_nal >= Start.Data);

                                    break;
                            }
                        }

                        break;

                    /*

                    case Enumeratory.Tabela.SaldoNajemcy:
                        headers = new string[] { "Saldo", "Saldo na dzień " + Start.Data.ToShortDateString(), "W tym noty odsetkowe" };
                        sortable = false;
                        indexesOfNumericColumns = new List<int>() { 1, 2, 3 };
                        {
                            IEnumerable<DostępDoBazy.Należność> receivables = null;
                            IEnumerable<DostępDoBazy.Obrót> turnovers = null;
                            DostępDoBazy.Najemca tenant = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == id);
                            heading = tenant.nazwisko + " " + tenant.imie + "<br />" + tenant.adres_1 + " " + tenant.adres_2;
                            nodeOfSiteMapPath = "Saldo najemcy " + tenant.nazwisko + " " + tenant.imie;

                            switch (Start.AktywnyZbiór)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    receivables = db.Należności1.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Należność>();
                                    turnovers = db.Obroty1.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Obrót>();

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    receivables = db.Należności2.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Należność>();
                                    turnovers = db.Obroty2.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Obrót>();

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    receivables = db.Należności3.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Należność>();
                                    turnovers = db.Obroty3.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Obrót>();

                                    break;
                            }

                            List<string[]> rowsOfReceivablesAndTurnovers = receivables.Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).Concat(turnovers.Select(t => t.WażnePolaDoNależnościIObrotówNajemcy())).ToList();
                            decimal wnAmount = rowsOfReceivablesAndTurnovers.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                            decimal maAmount = rowsOfReceivablesAndTurnovers.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                            List<string[]> rowsOfReceivablesAndTurnoversToDate = receivables.Where(r => r.data_nal <= Start.Data).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).Concat(turnovers.Where(t => t.data_obr <= Start.Data).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy())).ToList();
                            decimal wnAmountToDay = rowsOfReceivablesAndTurnoversToDate.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                            decimal maAmountToDay = rowsOfReceivablesAndTurnoversToDate.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                            DostępDoBazy.Konfiguracja configuration = db.Konfiguracje.FirstOrDefault();
                            List<string[]> rowsOfInterestNotes = turnovers.Where(t => t.kod_wplat == configuration.p_20 || t.kod_wplat == configuration.p_37).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                            decimal wnOfInterestNotes = rowsOfInterestNotes.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                            decimal maOfInterestNotes = rowsOfInterestNotes.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                            _rekordyTabeli = new List<string[]>() { new string[] { String.Empty, String.Format("{0:N}", maAmount - wnAmount), String.Format("{0:N}", maAmountToDay - wnAmountToDay), String.Format("{0:N}", maOfInterestNotes - wnOfInterestNotes) } };
                        }

                        break;*/

                    case Enumeratory.Tabela.ObrotyNajemcy:
                        headers = new string[] { "Kwota", "Data", "Data NO", "Operacja", "Nr dowodu", "Pozycja", "Uwagi" };
                        sortable = false;
                        indexesOfNumericColumns = new List<int>() { 1, 6 };
                        {
                            IEnumerable<DostępDoBazy.Obrót> turnovers = null;
                            DostępDoBazy.Najemca tenant = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == id);
                            heading = nodeOfSiteMapPath = "Obroty najemcy " + tenant.nazwisko + " " + tenant.imie;

                            switch (Start.AktywnyZbiór)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    turnovers = db.Obroty1.Where(t => t.nr_kontr == id);

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    turnovers = db.Obroty2.Where(t => t.nr_kontr == id);

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    turnovers = db.Obroty3.Where(t => t.nr_kontr == id);

                                    break;
                            }

                            rekordyTabeli = turnovers.OrderBy(t => t.data_obr);

                            placeOfMainTableButtons.Controls.Add(new Kontrolki.HtmlInputHidden("additionalId", id.ToString()));
                        }

                        break;

                    default:
                        heading = null;
                        nodeOfSiteMapPath = null;

                        break;
                }

                if (!IsPostBack)
                    _wiersze = rekordyTabeli.Select(r => r.PolaDoTabeli()).ToList();

                DostępDoBazy.Lokal.TypesOfPlace = null;

                placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
                placeOfMainTableButtons.Controls.Add(new Kontrolki.HtmlInputHidden("table", table.ToString()));

                if (subMenu != null)
                {
                    Kontrolki.HtmlGenericControl superUl = new Kontrolki.HtmlGenericControl("ul", "superMenu");

                    foreach (string[] items in subMenu)
                    {
                        Kontrolki.HtmlGenericControl superLi = new Kontrolki.HtmlGenericControl("li", String.Empty);
                        Kontrolki.HtmlGenericControl subUl = new Kontrolki.HtmlGenericControl("ul", "subMenu");

                        superLi.Controls.Add(new LiteralControl(items[0]));

                        for (int i = 1; i < items.Length; i++)
                        {
                            Kontrolki.HtmlGenericControl subLi = new Kontrolki.HtmlGenericControl("li", String.Empty);

                            subLi.Controls.Add(new LiteralControl(items[i]));
                            subUl.Controls.Add(subLi);
                        }

                        superLi.Controls.Add(subUl);
                        superUl.Controls.Add(superLi);
                    }

                    placeOfMainTableButtons.Controls.Add(superUl);
                }

                Title = heading;
                Session["values"] = null;

                switch (table)
                {
                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:
                        Start.ŚcieżkaStrony = new List<string>() { "Kartoteki", "Lokale" };

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        Start.ŚcieżkaStrony = new List<string>() { "Kartoteki", "Najemcy" };

                        break;

                    case Enumeratory.Tabela.Budynki:
                    case Enumeratory.Tabela.Wspolnoty:
                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        Start.ŚcieżkaStrony = new List<string>() { "Kartoteki" };

                        break;

                    case Enumeratory.Tabela.NaleznosciWedlugNajemcow:
                        Start.ŚcieżkaStrony = new List<string>() { "Rozliczenia finansowe", "Należności i obroty" };

                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("button", "turnoversEditing", "Dodaj/usuń obroty", "javascript: Redirect('List.aspx?table=" + Enumeratory.Tabela.ObrotyNajemcy + "')"));

                        break;

                    case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                    case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                    case Enumeratory.Tabela.SaldoNajemcy:
                    case Enumeratory.Tabela.ObrotyNajemcy:
                        if (Start.ŚcieżkaStrony.Count > 2)
                        {
                            Start.ŚcieżkaStrony.RemoveRange(3, Start.ŚcieżkaStrony.Count - 3);

                            string node = Start.ŚcieżkaStrony[2].Insert(0, "<a href=\"javascript: Load('List.aspx?table=" + Enumeratory.Tabela.NaleznosciWedlugNajemcow + "')\">") + "</a>";

                            if (!Start.ŚcieżkaStrony.Contains(node))
                                Start.ŚcieżkaStrony[2] = node;
                        }

                        break;

                    case Enumeratory.Tabela.TypyLokali:
                    case Enumeratory.Tabela.TypyKuchni:
                    case Enumeratory.Tabela.RodzajeNajemcy:
                    case Enumeratory.Tabela.TytulyPrawne:
                    case Enumeratory.Tabela.TypyWplat:
                    case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                    case Enumeratory.Tabela.GrupyFinansowe:
                    case Enumeratory.Tabela.StawkiVat:
                    case Enumeratory.Tabela.Atrybuty:
                        Start.ŚcieżkaStrony = new List<string>() { "Słowniki" };

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        Start.ŚcieżkaStrony = new List<string>() { "Administracja" };

                        break;
                }

                if (!Start.ŚcieżkaStrony.Contains(nodeOfSiteMapPath))
                    Start.ŚcieżkaStrony.Add(nodeOfSiteMapPath);

                //
                //CXP PART
                //
                switch (table)
                {
                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:
                        {
                            try
                            {
                                //db.Database.ExecuteSqlCommand("DROP TABLE skl_cz_tmp");
                                db.Database.ExecuteSqlCommand("DROP TABLE pliki_tmp");
                            }
                            catch { }
                        }

                        break;
                }
                //
                //TO DUMP BEHIND THE WALL
                //
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateMainTable();
        }

        void CreateMainTable()
        {
            Kontrolki.Table mainTable = new Kontrolki.Table("mainTable", _wiersze.ToList(), headers, sortable, String.Empty, indexesOfNumericColumns, indexesOfColumnsWithSummary);

            if (sortable)
                foreach (TableCell cell in mainTable.Rows[0].Cells)
                    ((Kontrolki.LinkButton)cell.Controls[0]).Click += LinkButtonOfColumn_Click;

            placeOfMainTable.Controls.Clear();
            placeOfMainTable.Controls.Add(mainTable);
        }

        void LinkButtonOfColumn_Click(object sender, EventArgs e)
        {
            int columnNumber = Int32.Parse(((Kontrolki.LinkButton)sender).ID.Replace("column", String.Empty)) + 1;

            switch (sortOrder)
            {
                case Enumeratory.PorządekSortowania.Rosnaco:
                    try { _wiersze = _wiersze.OrderByDescending(r => Single.Parse(r[columnNumber])).ToList(); }
                    catch { _wiersze = _wiersze.OrderByDescending(r => r[columnNumber]).ToList(); }

                    sortOrder = Enumeratory.PorządekSortowania.Malejaco;

                    break;

                case Enumeratory.PorządekSortowania.Malejaco:
                    try { _wiersze = _wiersze.OrderBy(r => Single.Parse(r[columnNumber])).ToList(); }
                    catch { _wiersze = _wiersze.OrderBy(r => r[columnNumber]).ToList(); }

                    sortOrder = Enumeratory.PorządekSortowania.Rosnaco;

                    break;
            }

            CreateMainTable();
        }

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList list = (RadioButtonList)sender;

            switch (list.SelectedValue)
            {
                case "nazwisko":
                    _wiersze = _wiersze.OrderBy(r => r[1]).ThenBy(r => r[2]).ToList();

                    break;

                case "kod":
                    _wiersze = _wiersze.OrderBy(r => Single.Parse(r[3])).ThenBy(r => Single.Parse(r[4])).ToList();

                    break;
            }

            CreateMainTable();
        }
    }
}