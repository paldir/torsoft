﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

namespace czynsze.Formularze
{
    public partial class Lista : Strona
    {
        Enumeratory.Tabela _tabela;

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

        string[] _nagłówki
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

        Enumeratory.PorządekSortowania _porządekSortowania
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

        int _numerKolumnySortownia
        {
            get { return (int)ViewState["numerKolumnySortowania"]; }
            set { ViewState["numerKolumnySortowania"] = value; }
        }

        Kontrolki.InformacjeOSortowalnościTablicy _informacjeOSortowaniu
        {
            get { return ViewState["_informacjeOSortowaniu"] as Kontrolki.InformacjeOSortowalnościTablicy; }
            set { ViewState["_informacjeOSortowaniu"] = value; }
        }

        List<int> _indeksyKolumnNumerycznych
        {
            get
            {
                if (ViewState["indexesOfNumericColumns"] == null)
                    return new List<int>();

                return (List<int>)ViewState["indexesOfNumericColumns"];
            }

            set { ViewState["indexesOfNumericColumns"] = value; }
        }

        List<int> _indeksyKolumnZPodsumowaniem
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
            using (DostępDoBazy.CzynszeKontekst _db = new DostępDoBazy.CzynszeKontekst())
            {
                _tabela = PobierzWartośćParametru<Enumeratory.Tabela>("table");
                string url = "Rekord.aspx";
                string nagłówek;
                string węzełŚcieżkiStrony;
                List<string[]> podMenu = null;
                bool sortowalna = true;
                int indeksKolumnySortującej;
                int __record = PobierzWartośćParametru<int>("id");//-1;
                IEnumerable<DostępDoBazy.Rekord> rekordyTabeli = null;
                string nazwaPrzycisków = "action";
                WartościSesji.Rekord = null;

                switch (_tabela)
                {
                    case Enumeratory.Tabela.NieaktywneLokale:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "browse", "Przeglądaj", url, nazwaPrzycisków));

                        break;

                    case Enumeratory.Tabela.NaleznosciWedlugNajemcow:
                    case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                    case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:

                        break;

                    default:
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "add", "Dodaj", url, nazwaPrzycisków));
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "edit", "Edytuj", url, nazwaPrzycisków));
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "delete", "Usuń", url, nazwaPrzycisków));
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "browse", "Przeglądaj", url, nazwaPrzycisków));

                        break;
                }

                switch (_tabela)
                {
                    case Enumeratory.Tabela.Budynki:
                        nagłówek = węzełŚcieżkiStrony = "Budynki";
                        _nagłówki = new string[] { "Kod", "Adres", "Adres cd." };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.Budynki.OrderBy(b => b.kod_1);

                        break;

                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:
                        nagłówek = "Lokale";
                        _nagłówki = new string[] { "Kod budynku", "Numer lokalu", "Typ lokalu", "Powierzchnia użytkowa", "Nazwisko", "Imię" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1, 2, 4 };
                        IEnumerable<DostępDoBazy.Lokal> lokale = null;
                        indeksKolumnySortującej = 0;

                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "move", "Przenieś", url, nazwaPrzycisków));

                        switch (_tabela)
                        {
                            case Enumeratory.Tabela.AktywneLokale:
                                nagłówek += " (aktywne)";
                                podMenu = new List<string[]>()
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
                                    lokale = _db.AktywneLokale;

                                break;

                            case Enumeratory.Tabela.NieaktywneLokale:
                                nagłówek += " (nieaktywne)";

                                if (!IsPostBack)
                                    lokale = _db.NieaktywneLokale;

                                break;
                        }

                        węzełŚcieżkiStrony = nagłówek;

                        if (lokale != null)
                            rekordyTabeli = lokale.OrderBy(p => p.kod_lok).ThenBy(p => p.nr_lok);

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        nagłówek = "Najemcy";
                        _nagłówki = new string[] { "Numer kontrolny", "Nazwisko", "Imię", "Adres", "Adres cd." };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        IEnumerable<DostępDoBazy.Najemca> najemcy = null;
                        indeksKolumnySortującej = 1;

                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("mainTableButton", "move", "Przenieś", url, nazwaPrzycisków));

                        switch (_tabela)
                        {
                            case Enumeratory.Tabela.AktywniNajemcy:
                                nagłówek += " (aktywni)";

                                if (!IsPostBack)
                                    najemcy = _db.AktywniNajemcy;

                                break;

                            case Enumeratory.Tabela.NieaktywniNajemcy:
                                nagłówek += " (nieaktywni)";

                                if (!IsPostBack)
                                    najemcy = _db.NieaktywniNajemcy;

                                break;
                        }

                        węzełŚcieżkiStrony = nagłówek;

                        if (najemcy != null)
                            rekordyTabeli = najemcy.OrderBy(t => t.nazwisko).ThenBy(t => t.imie);

                        break;

                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        nagłówek = węzełŚcieżkiStrony = "Składniki opłat";
                        _nagłówki = new string[] { "Numer", "Nazwa", "Sposób naliczania", "Typ", "Stawka zł" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1, 5 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.SkładnikiCzynszu.OrderBy(c => c.nr_skl);

                        break;

                    case Enumeratory.Tabela.Wspolnoty:
                        nagłówek = węzełŚcieżkiStrony = "Wspólnoty";
                        _nagłówki = new string[] { "Kod", "Nazwa wspólnoty", "Il. bud.", "Il. miesz." };
                        _indeksyKolumnNumerycznych = new List<int>() { 1, 3, 4 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.Wspólnoty.OrderBy(c => c.kod);

                        break;

                    case Enumeratory.Tabela.TypyLokali:
                        nagłówek = węzełŚcieżkiStrony = "Typy lokali";
                        _nagłówki = new string[] { "Kod", "Typ lokalu" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.TypyLokali.OrderBy(t => t.kod_typ);

                        break;

                    case Enumeratory.Tabela.TypyKuchni:
                        nagłówek = węzełŚcieżkiStrony = "Rodzaje kuchni";
                        _nagłówki = new string[] { "Kod", "Rodzaj kuchni" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.TypyKuchni.OrderBy(t => t.kod_kuch);

                        break;

                    case Enumeratory.Tabela.RodzajeNajemcy:
                        nagłówek = węzełŚcieżkiStrony = "Rodzaje najemców";
                        _nagłówki = new string[] { "Kod", "Rodzaj najemcy" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.TypyNajemców.OrderBy(t => t.kod_najem);

                        break;

                    case Enumeratory.Tabela.TytulyPrawne:
                        nagłówek = węzełŚcieżkiStrony = "Tytuły prawne do lokali";
                        _nagłówki = new string[] { "Kod", "Tytuł prawny" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.TytułyPrawne.OrderBy(t => t.kod_praw);

                        break;

                    case Enumeratory.Tabela.TypyWplat:
                        nagłówek = węzełŚcieżkiStrony = "Rodzaje wpłat i wypłat";
                        _nagłówki = new string[] { "Kod", "Rodzaj wpłaty lub wypłaty", "Sposób rozliczania", "Odsetki", "NO" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.RodzajePłatności.OrderBy(t => t.kod_wplat);

                        break;

                    case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                        nagłówek = węzełŚcieżkiStrony = "Grupy składników czynszu";
                        _nagłówki = new string[] { "Kod", "Nazwa grupy" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.GrupySkładnikówCzynszu.OrderBy(g => g.kod);

                        break;

                    case Enumeratory.Tabela.GrupyFinansowe:
                        nagłówek = węzełŚcieżkiStrony = "Grupy finansowe";
                        _nagłówki = new string[] { "Kod", "Konto", "Nazwa grupy" };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.GrupyFinansowe.OrderBy(g => g.kod);

                        break;

                    case Enumeratory.Tabela.StawkiVat:
                        nagłówek = węzełŚcieżkiStrony = "Stawki VAT";
                        _nagłówki = new string[] { "Oznaczenie stawki", "Symbol fiskalny" };
                        indeksKolumnySortującej = 1;

                        if (!IsPostBack)
                            rekordyTabeli = _db.StawkiVat.OrderBy(r => r.symb_fisk);

                        break;

                    case Enumeratory.Tabela.Atrybuty:
                        nagłówek = węzełŚcieżkiStrony = "Cechy obiektów";
                        _nagłówki = new string[] { "Kod", "Nazwa", "N/C", "L.", "N.", "B.", "Wsp." };
                        _indeksyKolumnNumerycznych = new List<int>() { 1 };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.Atrybuty.OrderBy(a => a.kod);

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        nagłówek = węzełŚcieżkiStrony = "Użytkownicy";
                        _nagłówki = new string[] { "Symbol", "Nazwisko", "Imię" };
                        indeksKolumnySortującej = 0;

                        if (!IsPostBack)
                            rekordyTabeli = _db.Użytkownicy.OrderBy(u => u.symbol);

                        break;

                    case Enumeratory.Tabela.NaleznosciWedlugNajemcow:
                        nagłówek = węzełŚcieżkiStrony = "Należności i obroty według najemców";
                        _nagłówki = new string[] { "Numer kontrolny", "Nazwisko", "Imię", "Adres", "Adres cd." };
                        sortowalna = false;
                        _indeksyKolumnNumerycznych = new List<int>();// { 3, 4 };
                        indeksKolumnySortującej = 1;
                        podMenu = new List<string[]>()
                        {
                            new string[]
                            {
                                "Należności",
                                "<a href=\"javascript: Redirect('Lista.aspx?table=WszystkieNaleznosciNajemcy')\">Wszystkie</a>",
                                "<a href=\"javascript: Redirect('Lista.aspx?table=NieprzeterminowaneNaleznosciNajemcy')\">Nieprzeterminowane</a>"
                            },
                            new string[]
                            {
                                "Rozliczenia",
                                "<a href=\"javascript: Redirect('NaleznosciIObrotyNajemcy.aspx?dummy=dummy')\">Należności i obroty</a>",
                                "<a href='#'>Zaległości płatnicze</a>",
                            }
                        };

                        if (!IsPostBack)
                            rekordyTabeli = _db.AktywniNajemcy.OrderBy(t => t.nazwisko).ThenBy(t => t.imie);

                        Kontrolki.RadioButtonList lista = new Kontrolki.RadioButtonList("list", "by", new List<string> { "wg nazwiska", "wg kodu lokalu" }, new List<string> { "nazwisko", "kod" }, true, true, "nazwisko");
                        lista.SelectedIndexChanged += list_SelectedIndexChanged;

                        placeOfMainTableButtons.Controls.Add(lista);
                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("button", "saldo", "Saldo", "javascript: Redirect('SaldoNajemcy.aspx?dummy=dummy')"));

                        break;

                    case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                    case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                        _nagłówki = new string[] { "Kwota należności", "Termin zapłaty", "Uwagi", "Kod lokalu", "Nr lokalu" };
                        sortowalna = false;
                        indeksKolumnySortującej = 1;
                        _indeksyKolumnNumerycznych = new List<int>() { 1, 4, 5 };
                        _indeksyKolumnZPodsumowaniem = new List<int>() { 1 };
                        {
                            DostępDoBazy.Najemca najemca = _db.AktywniNajemcy.Find(__record);
                            nagłówek = String.Format("Należności najemcy {0} {1}", najemca.nazwisko, najemca.imie);
                            List<DostępDoBazy.Należność1> należności = _db.Należności1.Where(r => r.nr_kontr == najemca.nr_kontr).OrderBy(r => r.data_nal).ToList();

                            switch (_tabela)
                            {
                                case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                                    rekordyTabeli = należności;

                                    break;

                                case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                                    nagłówek += " (nieprzeterminowane)";
                                    rekordyTabeli = należności.Where(r => r.data_nal >= Start.Data);

                                    break;
                            }

                            węzełŚcieżkiStrony = nagłówek;
                        }

                        break;

                    case Enumeratory.Tabela.ObrotyNajemcy:
                        _nagłówki = new string[] { "Kwota", "Data", "Data NO", "Operacja", "Nr dowodu", "Pozycja", "Uwagi" };
                        sortowalna = false;
                        indeksKolumnySortującej = 1;
                        _indeksyKolumnNumerycznych = new List<int>() { 1, 6 };
                        {
                            IEnumerable<DostępDoBazy.Obrót> obroty = null;
                            DostępDoBazy.Najemca najemca = _db.AktywniNajemcy.Find(__record);
                            nagłówek = węzełŚcieżkiStrony = "Obroty najemcy " + najemca.nazwisko + " " + najemca.imie;

                            switch (Start.AktywnyZbiór)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    obroty = _db.Obroty1;

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    obroty = _db.Obroty2;

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    obroty = _db.Obroty3;

                                    break;
                            }

                            rekordyTabeli = obroty.Where(t => t.nr_kontr == najemca.nr_kontr).OrderBy(t => t.data_obr);

                            placeOfMainTableButtons.Controls.Add(new Kontrolki.HtmlInputHidden("additionalId", __record.ToString()));
                        }

                        break;

                    default:
                        nagłówek = null;
                        węzełŚcieżkiStrony = null;
                        indeksKolumnySortującej = -1;

                        break;
                }

                if (!IsPostBack)
                    _wiersze = rekordyTabeli.Select(r => r.PolaDoTabeli().ToArray()).ToList();

                placeOfHeading.Controls.Add(new LiteralControl("<h2>" + nagłówek + "</h2>"));
                placeOfMainTableButtons.Controls.Add(new Kontrolki.HtmlInputHidden("table", _tabela.ToString()));

                if (podMenu != null)
                {
                    Kontrolki.HtmlGenericControl nadUl = new Kontrolki.HtmlGenericControl("ul", "superMenu");

                    foreach (string[] items in podMenu)
                    {
                        Kontrolki.HtmlGenericControl nadLi = new Kontrolki.HtmlGenericControl("li", String.Empty);
                        Kontrolki.HtmlGenericControl podUl = new Kontrolki.HtmlGenericControl("ul", "subMenu");

                        nadLi.Controls.Add(new LiteralControl(items[0]));

                        for (int i = 1; i < items.Length; i++)
                        {
                            Kontrolki.HtmlGenericControl podLi = new Kontrolki.HtmlGenericControl("li", String.Empty);

                            podLi.Controls.Add(new LiteralControl(items[i]));
                            podUl.Controls.Add(podLi);
                        }

                        nadLi.Controls.Add(podUl);
                        nadUl.Controls.Add(nadLi);
                    }

                    placeOfMainTableButtons.Controls.Add(nadUl);
                }

                Title = nagłówek;
                Session["values"] = null;

                if (sortowalna)
                    _informacjeOSortowaniu = new Kontrolki.InformacjeOSortowalnościTablicy(indeksKolumnySortującej);
                else
                    _informacjeOSortowaniu = new Kontrolki.InformacjeOSortowalnościTablicy();

                switch (_tabela)
                {
                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:
                        Start.ŚcieżkaStrony = new ŚcieżkaStrony("Kartoteki", "Lokale");

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        Start.ŚcieżkaStrony = new ŚcieżkaStrony("Kartoteki", "Najemcy");

                        break;

                    case Enumeratory.Tabela.Budynki:
                    case Enumeratory.Tabela.Wspolnoty:
                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        Start.ŚcieżkaStrony = new ŚcieżkaStrony("Kartoteki");

                        break;

                    case Enumeratory.Tabela.NaleznosciWedlugNajemcow:
                        Start.ŚcieżkaStrony = new ŚcieżkaStrony("Rozliczenia finansowe", "Należności i obroty");

                        placeOfMainTableButtons.Controls.Add(new Kontrolki.Button("button", "turnoversEditing", "Dodaj/usuń obroty", "javascript: Redirect('Lista.aspx?table=" + Enumeratory.Tabela.ObrotyNajemcy + "')"));

                        break;

                    case Enumeratory.Tabela.WszystkieNaleznosciNajemcy:
                    case Enumeratory.Tabela.NieprzeterminowaneNaleznosciNajemcy:
                    case Enumeratory.Tabela.ObrotyNajemcy:

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
                        Start.ŚcieżkaStrony = new ŚcieżkaStrony("Słowniki");

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        Start.ŚcieżkaStrony = new ŚcieżkaStrony("Administracja");

                        break;
                }

                Start.ŚcieżkaStrony.Dodaj(węzełŚcieżkiStrony, ŚcieżkaIQuery);
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateMainTable();
        }

        void CreateMainTable()
        {
            Kontrolki.Table głównaTabela = new Kontrolki.Table("mainTable", _wiersze, _nagłówki, _informacjeOSortowaniu, String.Empty, _indeksyKolumnNumerycznych, _indeksyKolumnZPodsumowaniem, "tablica");

            placeOfMainTable.Controls.Clear();
            placeOfMainTable.Controls.Add(głównaTabela);
        }

        void LinkButtonOfColumn_Click(object sender, EventArgs e)
        {
            _numerKolumnySortownia = Int32.Parse(((Kontrolki.LinkButton)sender).ID.Replace("column", String.Empty)) + 1;

            if (_wiersze.Any())
            {
                string pierwszaWartość = _wiersze.First().ElementAt(_numerKolumnySortownia);

                switch (_porządekSortowania)
                {
                    case Enumeratory.PorządekSortowania.Rosnaco:
                        try
                        {
                            if (String.IsNullOrEmpty(pierwszaWartość))
                                throw new FormatException();
                            else
                                Single.Parse(pierwszaWartość);

                            _wiersze.Sort((x, y) => Single.Parse(x[_numerKolumnySortownia]).CompareTo(Single.Parse(y[_numerKolumnySortownia])));
                        }
                        catch (FormatException)
                        {
                            _wiersze.Sort((x, y) => String.Compare(x[_numerKolumnySortownia], y[_numerKolumnySortownia]));
                        }

                        _porządekSortowania = Enumeratory.PorządekSortowania.Malejaco;

                        break;

                    case Enumeratory.PorządekSortowania.Malejaco:
                        try
                        {
                            if (String.IsNullOrEmpty(pierwszaWartość))
                                throw new FormatException();
                            else
                                Single.Parse(pierwszaWartość);

                            _wiersze.Sort((x, y) => -Single.Parse(x[_numerKolumnySortownia]).CompareTo(Single.Parse(y[_numerKolumnySortownia])));
                        }
                        catch (FormatException)
                        {
                            _wiersze.Sort((x, y) => -String.Compare(x[_numerKolumnySortownia], y[_numerKolumnySortownia]));
                        }

                        _porządekSortowania = Enumeratory.PorządekSortowania.Rosnaco;

                        break;
                }
            }

            CreateMainTable();
        }

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList lista = (RadioButtonList)sender;

            switch (lista.SelectedValue)
            {
                /*case "nazwisko":
                    _wiersze = _wiersze.OrderBy(r => r.ElementAt(1)).ThenBy(r => r.ElementAt(2));

                    break;

                case "kod":
                    _wiersze = _wiersze.OrderBy(r => Single.Parse(r.ElementAt(3))).ThenBy(r => Single.Parse(r.ElementAt(4)));

                    break;*/
            }

            CreateMainTable();
        }
    }
}