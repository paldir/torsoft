using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Reflection;

namespace czynsze.Formularze
{
    public partial class Rekord : Strona
    {
        int __record;
        Enumeratory.Akcja _akcja;
        Enumeratory.Tabela _tabela;

        /*List<DostępDoBazy.AtrybutObiektu> _atrybutyObiektu
        {
            get { return Session["attributesOfObject"] as List<DostępDoBazy.AtrybutObiektu>; }
            set { Session["attributesOfObject"] = value; }
        }

        List<DostępDoBazy.SkładnikCzynszuLokalu> _składnikiCzynszuLokalu
        {
            get { return Session["rentComponentsOfPlace"] as List<DostępDoBazy.SkładnikCzynszuLokalu>; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        List<DostępDoBazy.BudynekWspólnoty> _budynkiWspólnoty
        {
            get { return Session["communityBuildings"] as List<DostępDoBazy.BudynekWspólnoty>; }
            set { Session["communityBuildings"] = value; }
        }

        DostępDoBazy.Rekord _rekord
        {
            get { return Session["rekord"] as DostępDoBazy.Rekord; }
            set { Session["rekord"] = value; }
        }*/

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                bool kontrolkiWłączone = false;
                //bool idEnabled = false;
                string[] etykiety = null;
                string nagłówek = null;
                List<Kontrolki.Button> przyciski = new List<Kontrolki.Button>();
                List<Control> kontrolki = new List<Control>();
                List<int> zmianaKolumn = null;
                List<Kontrolki.HtmlIframe> zakładki = null;
                List<Kontrolki.HtmlInputRadioButton> przyciskiZakładek = null;
                List<Kontrolki.Label> etykietyZakładek = null;
                List<Control> podgląd = null;
                __record = PobierzWartośćParametru<int>("id");
                _akcja = PobierzWartośćParametru<Enumeratory.Akcja>("action");
                _tabela = PobierzWartośćParametru<Enumeratory.Tabela>("table");
                string zwrotnyUrl = "javascript: Load('" + Request.UrlReferrer + "')";
                Type typRekordu = DostępDoBazy.CzynszeKontekst.TabelaNaTypRekordu[_tabela];
                System.Data.Entity.DbSet zbiór = db.Set(typRekordu);
                /*Dictionary<bool, string> fromIdEnabledToIdSuffix = new Dictionary<bool, string>()
                {
                    {true, String.Empty},
                    {false, "_disabled"}
                };*/

                if (WartościSesji.Rekord == null)
                    switch (_akcja)
                    {
                        case Enumeratory.Akcja.Dodaj:
                            WartościSesji.Rekord = Activator.CreateInstance(typRekordu) as DostępDoBazy.Rekord;

                            break;

                        default:
                            WartościSesji.Rekord = zbiór.Find(__record) as DostępDoBazy.Rekord;

                            break;
                    }

                if (WartościSesji.Rekord.GetType().GetProperty("__record") == null)
                    throw new Exception("Rekord nie zawiera pola __record.");

                switch (_akcja)
                {
                    case Enumeratory.Akcja.Dodaj:
                    case Enumeratory.Akcja.Edytuj:
                        Kontrolki.Button przycisk = new Kontrolki.Button("buttons", "Save", "Zapisz", "RecordValidation.aspx");

                        przyciski.Add(przycisk);

                        switch (_akcja)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                kontrolkiWłączone = true;
                                nagłówek = "Dodawanie ";

                                przyciski.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", zwrotnyUrl));

                                break;

                            case Enumeratory.Akcja.Edytuj:
                                kontrolkiWłączone = true;
                                nagłówek = "Edycja ";

                                przyciski.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", zwrotnyUrl));

                                break;
                        }

                        break;

                    case Enumeratory.Akcja.Usuń:
                        kontrolkiWłączone = false;
                        nagłówek = "Usuwanie ";

                        przyciski.Add(new Kontrolki.Button("buttons", "Delete", "Usuń", "RecordValidation.aspx"));
                        przyciski.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", zwrotnyUrl));

                        break;

                    case Enumeratory.Akcja.Przeglądaj:
                        kontrolkiWłączone = false;
                        nagłówek = "Przeglądanie ";

                        przyciski.Add(new Kontrolki.Button("buttons", "Back", "Powrót", zwrotnyUrl));

                        break;

                    case Enumeratory.Akcja.Przenieś:
                        kontrolkiWłączone = false;
                        nagłówek = "Przenoszenie ";

                        przyciski.Add(new Kontrolki.Button("buttons", "Move", "Przenieś", "RecordValidation.aspx"));
                        przyciski.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", zwrotnyUrl));

                        break;
                }

                switch (_tabela)
                {
                    case Enumeratory.Tabela.Budynki:
                        Title = "Budynek";
                        nagłówek += "budynku";
                        zmianaKolumn = new List<int>() { 0, 6 };
                        DostępDoBazy.Budynek budynek = WartościSesji.Rekord as DostępDoBazy.Budynek;
                        int kodBudynku = budynek.kod_1;
                        etykiety = new string[] 
                        { 
                            "Kod budynku: ", 
                            "Ilość lokali: ", 
                            "Sposób rozliczania: ", 
                            "Adres: ", 
                            "Adres cd.: ",
                            "Udział w koszt.: ",
                            "Uwagi: " 
                        };

                        przyciskiZakładek = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dokumenty", "tabRadios", "dokumenty", false)
                        };

                        etykietyZakładek = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(1).ID, "Cechy", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(2).ID, "Dokumenty", String.Empty)
                        };

                        zakładki = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AtrybutyObiektu.aspx?attributeOf="+Enumeratory.Atrybut.Budynku+"&parentId="+kodBudynku+"&action="+_akcja.ToString()+"&childAction=Przeglądaj", "hidden"),
                            new Kontrolki.HtmlIframe("tab", "dokumenty_tab", String.Format("Pliki.aspx?id_obiektu={0}&tabela={1}", kodBudynku, _tabela), "hidden")
                        };

                        WartościSesji.AtrybutyObiektu = new List<DostępDoBazy.AtrybutObiektu>();
                        WartościSesji.Pliki = db.PlikiBudynków.Where(p => p.id_obiektu == __record).AsEnumerable<DostępDoBazy.Plik>().ToList();

                        foreach (DostępDoBazy.AtrybutBudynku attributeOfBuilding in db.AtrybutyBudynków.ToList().Where(a => Int32.Parse(a.kod_powiaz) == kodBudynku))
                            WartościSesji.AtrybutyObiektu.Add(attributeOfBuilding);

                        podgląd = new List<Control>()
                        {
                            new LiteralControl("Kod budynku: "),
                            new Kontrolki.Label("previewLabel", String.Empty, kodBudynku.ToString(), "id_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, budynek.adres, "adres_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, budynek.adres_2, "adres_2_preview")
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod_1", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "il_miesz", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.RadioButtonList("field", "sp_rozl", new List<string>() { "budynek", "lokale" }, new List<string>() { "0", "1" }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "udzial_w_k", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 6, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 420, 6, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:

                        Title = "Lokal";
                        nagłówek += "lokalu";

                        if (_tabela == Enumeratory.Tabela.NieaktywneLokale)
                        {
                            Title = "Lokal (nieaktywny)";
                            nagłówek += "(nieaktywnego)";
                        }

                        zmianaKolumn = new List<int> { 0, 5, 10, 17 };
                        etykiety = new string[] 
                        { 
                            //"Nr system: ", 
                            "Budynek: ", 
                            "Nr lokalu: ",
                            "Typ: ", 
                            "Adres: ",
                            "Adres cd.: ",
                            "Powierzchnia użytkowa: ", 
                            "Powierzchnia mieszkalna: ", 
                            "Udział: ", 
                            "Początek zakresu dat: ",
                            "Koniec zakresu dat: ", 
                            "Powierzchnia I pokoju: ",
                            "Powierzchnia II pokoju: ", 
                            "Powierzchnia III pokoju: ", 
                            "Powierzchnia IV pokoju: ", 
                            "Powierzchnia V pokoju: ",
                            "Powierzchnia VI pokoju: ", 
                            "Typ kuchni: ", 
                            "Najemca: ", 
                            "Ilość osób: ", 
                            "Tytuł prawny do lokalu: ", 
                            "Uwagi: " 
                        };

                        DostępDoBazy.Lokal lokal = WartościSesji.Rekord as DostępDoBazy.Lokal;

                        /*if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                            {
                                if (table == Enumeratory.Tabela.AktywneLokale)
                                    rekord = db.AktywneLokale.Single(b => b.nr_system == id);
                                else
                                    rekord = db.NieaktywneLokale.Single(b => b.nr_system == id);
                            }
                            else
                            {
                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywneLokale:
                                        rekord = new DostępDoBazy.AktywnyLokal();

                                        break;

                                    case Enumeratory.Tabela.NieaktywneLokale:
                                        rekord = new DostępDoBazy.NieaktywnyLokal();

                                        break;
                                }

                                IEnumerable<DostępDoBazy.Lokal> places = db.AktywneLokale.AsEnumerable<DostępDoBazy.Lokal>().Concat(db.NieaktywneLokale.AsEnumerable<DostępDoBazy.Lokal>());
                                lokal = rekord as DostępDoBazy.Lokal;

                                if (places.Any())
                                    lokal.nr_system = (places.Max(p => p.nr_system) + 1);
                                else
                                    lokal.nr_system = 1;

                                lokal.kod_lok = lokal.nr_lok = 0;
                            }*/                    
                               
                        int kodLokalu = lokal.kod_lok;
                        int nrLokalu = lokal.nr_lok;
                        int nrSystem = lokal.nr_system;
                        WartościSesji.AtrybutyObiektu = new List<DostępDoBazy.AtrybutObiektu>();
                        WartościSesji.SkładnikiCzynszuLokalu = new List<DostępDoBazy.SkładnikCzynszuLokalu>();
                        WartościSesji.Pliki = db.PlikiLokalów.Where(p => p.id_obiektu == __record).AsEnumerable<DostępDoBazy.Plik>().ToList();
                        lokal = WartościSesji.Rekord as DostępDoBazy.Lokal;
                        DostępDoBazy.AtrybutLokalu.Lokale = db.AktywneLokale.ToList();
                        WartościSesji.AtrybutyObiektu.AddRange(db.AtrybutyLokali.AsEnumerable().Where(a => Int32.Parse(a.kod_powiaz) == __record));
                        DostępDoBazy.AtrybutLokalu.Lokale = null;

                        WartościSesji.SkładnikiCzynszuLokalu.AddRange(db.SkładnikiCzynszuLokalu.AsEnumerable().Where(c => c.kod_lok == lokal.kod_lok && c.nr_lok == lokal.nr_lok));
                        //}

                        przyciskiZakładek = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "skladnikiCzynszu", "tabRadios", "skladnikiCzynszu", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dokumenty", "tabRadios", "dokumenty", false)
                        };

                        etykietyZakładek = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(1).ID, "Składniki czynszu", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(2).ID, "Cechy", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(3).ID, "Dokumenty", String.Empty)
                        };

                        podgląd = new List<Control>()
                        {
                            new LiteralControl("Nr budynku: "),
                            new Kontrolki.Label("previewLabel", String.Empty, kodLokalu.ToString(), "kod_lok_preview"),
                            new LiteralControl("Nr lokalu: "),
                            new Kontrolki.Label("previewLabel", String.Empty, nrLokalu.ToString(), "nr_lok_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, lokal.adres, "adres_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, lokal.adres_2, "adres_2_preview")
                        };

                        zakładki = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "skladnikiCzynszu_tab", String.Format("SkladnikiCzynszuLokalu.aspx?parentAction={0}&kod_lok={1}&nr_lok={2}", _akcja, kodLokalu, nrLokalu), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", String.Format("AtrybutyObiektu.aspx?attributeOf={0}&parentId={1}&action={2}&childAction=Przeglądaj", Enumeratory.Atrybut.Lokalu, __record, _akcja), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "dokumenty_tab", String.Format("Pliki.aspx?id_obiektu={0}&tabela={1}", nrSystem, _tabela), "hidden")
                        };

                        //form.Controls.Add(new Kontrolki.HtmlInputHidden("id", nrSystem));

                        /*if (!globalEnabled)
                        {
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_lok", kodLokalu));
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_lok", nrLokalu));
                        }*/

                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod_lok", db.Budynki.AsEnumerable().OrderBy(b => b.kod_1).Select(b => b.PolaDoTabeli().ToArray()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nr_lok", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod_typ", db.TypyLokali.AsEnumerable<DostępDoBazy.TypLokalu>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "pow_uzyt", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 8, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "pow_miesz", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 8, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "udzial", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "dat_od", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "dat_do", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "p_1", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "p_2", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "p_3", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "p_4", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "p_5", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "p_6", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod_kuch", db.TypyKuchni.AsEnumerable<DostępDoBazy.TypKuchni>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "nr_kontr", db.AktywniNajemcy.AsEnumerable<DostępDoBazy.AktywnyNajemca>().OrderBy(t => t.nazwisko).Select(t => t.PolaDoTabeli().ToList().GetRange(1, 4).ToArray()).ToList(), kontrolkiWłączone, true));
                        kontrolki.Add(new Kontrolki.TextBox("field", "il_osob", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod_praw", db.TytułyPrawne.AsEnumerable<DostępDoBazy.TytułPrawny>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 240, 4, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        Title = "Najemca";
                        nagłówek += "najemcy";
                        zmianaKolumn = new List<int> { 0, 6 };
                        etykiety = new string[] 
                        { 
                            "Nr kontrolny: ", 
                            "Najemca: ", 
                            "Nazwisko: ",
                            "Imię: ", 
                            "Adres: ", 
                            "Adres cd.: ", 
                            "Numer dowodu osobistego: ", 
                            "Pesel: ", 
                            "Zakład pracy: ",
                            "Login/e-mail: ", 
                            "Hasło: ", 
                            "Uwagi: " 
                        };

                        if (_tabela == Enumeratory.Tabela.NieaktywniNajemcy)
                        {
                            Title = "Najemca (nieaktywny)";
                            nagłówek += "(nieaktywnego)";
                        }

                        DostępDoBazy.Najemca najemca = WartościSesji.Rekord as DostępDoBazy.Najemca;

                        /*if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywniNajemcy:
                                        rekord = db.AktywniNajemcy.Single(t => t.nr_kontr == id);

                                        break;

                                    case Enumeratory.Tabela.NieaktywniNajemcy:
                                        rekord = db.NieaktywniNajemcy.Single(t => t.nr_kontr == id);

                                        break;
                                }
                            else
                            {
                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywniNajemcy:
                                        rekord = new DostępDoBazy.AktywnyNajemca();

                                        break;

                                    case Enumeratory.Tabela.NieaktywniNajemcy:
                                        rekord = new DostępDoBazy.NieaktywnyNajemca();

                                        break;
                                }
                            }*/

                        WartościSesji.AtrybutyObiektu = new List<DostępDoBazy.AtrybutObiektu>();

                        foreach (DostępDoBazy.AtrybutNajemcy attributeOfTenant in db.AtrybutyNajemców.ToList().Where(a => Int32.Parse(a.kod_powiaz) == __record))
                            WartościSesji.AtrybutyObiektu.Add(attributeOfTenant);
                        //}

                        podgląd = new List<Control>()
                        {
                            new LiteralControl("Numer kontrolny: "),
                            new Kontrolki.Label("previewLabel", String.Empty, najemca.nr_kontr.ToString(), "id_preview"),
                            new LiteralControl("Nazwisko: "),
                            new Kontrolki.Label("previewLabel", String.Empty, najemca.nazwisko, "nazwisko_preview"),
                            new LiteralControl("Imię: "),
                            new Kontrolki.Label("previewLabel", String.Empty, najemca.imie, "imie_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, najemca.adres_1, "adres_1_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, najemca.adres_2, "adres_2_preview")
                        };

                        przyciskiZakładek = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        etykietyZakładek = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(1).ID, "Cechy", String.Empty),
                        };

                        zakładki = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AtrybutyObiektu.aspx?attributeOf="+Enumeratory.Atrybut.Najemcy+"&parentId="+__record.ToString()+"&action="+_akcja.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "nr_kontr", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 6, 1, false));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod_najem", db.TypyNajemców.AsEnumerable<DostępDoBazy.TypNajemcy>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwisko", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "imie", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres_1", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nr_dow", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 9, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "pesel", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 11, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa_z", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "e_mail", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "l__has", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 120, 2, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        Title = "Składnik opłat";
                        nagłówek += "składnika opłat";
                        zmianaKolumn = new List<int> { 0, 6, 9 };
                        etykiety = new string[]
                        {
                            "Nr składnika: ",
                            "Nazwa: ",
                            "Rodzaj ewidencji: ",
                            "Sposób naliczania: ",
                            "Stawka: ",
                            "Stawka do korespondencji: ",
                            "Typ składnika: ",
                            "Początek okresu naliczania: ",
                            "Koniec okresu naliczania: ",
                            "Grupa składników czynszu: ",
                            "Przedziały za osobę (dotyczy sposoby naliczania &quot;za osobę - przedziały&quot): "
                        };

                        DostępDoBazy.SkładnikCzynszu składnikCzynszu = WartościSesji.Rekord as DostępDoBazy.SkładnikCzynszu;

                        kontrolki.Add(new Kontrolki.TextBox("field", "nr_skl", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "rodz_e", new List<string[]> { new string[] { "1", "dziennik komornego" }, new string[] { "2", "wpłaty" }, new string[] { "3", "zmniejszenia" }, new string[] { "4", "zwiększenia" } }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "s_zaplat", new List<string[]> { new string[] { "1", "za m2 pow. użytkowej" }, new string[] { "2", "za określoną ilość" }, new string[] { "3", "za osobę" }, new string[] { "4", "za lokal" }, new string[] { "5", "za ilość dni w miesiącu" }, new string[] { "6", "za osobę - przedziały" } }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "stawka", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "stawka_inf", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "typ_skl", new List<string[]> { new string[] { "0", "stały" }, new string[] { "1", "zmienny" } }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "data_1", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "data_2", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod", db.GrupySkładnikówCzynszu.AsEnumerable<DostępDoBazy.GrupaSkładnikówCzynszu>().OrderBy(g => g.kod).Select(g => new string[] { g.kod.ToString(), g.kod.ToString(), g.nazwa }).ToList(), kontrolkiWłączone, false));

                        Table interval = new Table();
                        TableHeaderRow headerRow = new TableHeaderRow();
                        TableHeaderCell headerCell = new TableHeaderCell();

                        headerCell.Controls.Add(new LiteralControl("Os."));
                        headerRow.Cells.Add(headerCell);

                        headerCell = new TableHeaderCell();

                        headerCell.Controls.Add(new LiteralControl("Cena"));
                        headerRow.Cells.Add(headerCell);
                        interval.Rows.Add(headerRow);

                        for (int i = 0; i < 10; i++)
                        {
                            TableRow tableRow = new TableRow();
                            TableCell tableCell = new TableCell();

                            tableCell.Controls.Add(new LiteralControl(i.ToString()));
                            tableRow.Controls.Add(tableCell);

                            tableCell = new TableCell();

                            tableCell.Controls.Add(new Kontrolki.TextBox("field", String.Format("stawka_0{0}", i), Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, kontrolkiWłączone));
                            tableRow.Cells.Add(tableCell);
                            interval.Rows.Add(tableRow);
                        }

                        kontrolki.Add(interval);

                        break;

                    case Enumeratory.Tabela.Wspolnoty:
                        Title = "Wspólnota";
                        nagłówek += "wspólnoty";
                        zmianaKolumn = new List<int>() { 0, 7 };
                        DostępDoBazy.Wspólnota wspólnota = WartościSesji.Rekord as DostępDoBazy.Wspólnota;
                        int kodWspólnoty = wspólnota.kod;
                        etykiety = new string[]
                        {
                            "Kod wspólnoty: ",
                            "Ilość budynków: ",
                            "Ilość lokali: ",
                            "Nazwa pełna wspólnoty: ",
                            "Nazwa skrócona: ",
                            "Adres wspólnoty: ",
                            "Adres cd.: ",
                            "Nr konta 1: ",
                            "Nr konta 2: ",
                            "Nr konta 3: ",
                            "Ścieżka do F-K: ",
                            "Uwagi: "
                        };

                        /*if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.Wspólnoty.Single(c => c.kod == __record);
                            else
                                rekord = new DostępDoBazy.Wspólnota();*/

                        WartościSesji.AtrybutyObiektu = new List<DostępDoBazy.AtrybutObiektu>();
                        WartościSesji.BudynkiWspólnoty = new List<DostępDoBazy.BudynekWspólnoty>();

                        WartościSesji.AtrybutyObiektu.AddRange(db.AtrybutyWspólnot.AsEnumerable().Where(a => Int32.Parse(a.kod_powiaz) == kodWspólnoty));
                        WartościSesji.BudynkiWspólnoty.AddRange(db.BudynkiWspólnot.Where(c => c.kod == kodWspólnoty).OrderBy(b => b.kod_1));
                        //}

                        podgląd = new List<Control>()
                        {
                            new LiteralControl("Kod: "),
                            new Kontrolki.Label("previewLabel", String.Empty,kodWspólnoty.ToString(), "id_preview"),
                            new LiteralControl("Nazwa: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.nazwa_skr, "nazwa_skr_preview"),
                            new LiteralControl("Ilość budynków: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.il_bud.ToString(), "il_bud_preview"),
                            new LiteralControl("Ilość lokali: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.il_miesz.ToString(), "il_miesz_preview")
                        };

                        przyciskiZakładek = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "budynki", "tabRadios", "budynki", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        etykietyZakładek = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(1).ID, "Budynki", String.Empty),
                            new Kontrolki.Label("tabLabel", przyciskiZakładek.ElementAt(2).ID, "Cechy", String.Empty),
                        };

                        zakładki = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "budynki_tab", "BudynkiWspolnoty.aspx?kod="+kodWspólnoty.ToString()+"&parentAction="+_akcja.ToString(), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AtrybutyObiektu.aspx?attributeOf="+Enumeratory.Atrybut.Wspólnoty+"&parentId="+kodWspólnoty.ToString()+"&action="+_akcja.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "il_bud", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "il_miesz", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa_pel", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 50, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa_skr", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nr1_konta", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nr2_konta", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nr3_konta", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "sciezka_fk", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 420, 6, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.TypyLokali:
                        Title = "Typ lokali";
                        nagłówek += "typu lokalu";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Typ lokalu: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod_typ", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "typ_lok", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 6, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.TypyKuchni:
                        Title = "Rodzaj kuchni";
                        nagłówek += "rodzaju kuchni";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Rodzaj kuchni: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod_kuch", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "typ_kuch", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.RodzajeNajemcy:
                        Title = "Rodzaj najemców";
                        nagłówek += "rodzaju najemców";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Rodzaj najemcy: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod_najem", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "r_najemcy", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.TytulyPrawne:
                        Title = "Tytuł prawny do lokali";
                        nagłówek += "tytułu prawnego do lokali";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Tytuł prawny: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod_praw", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "tyt_prawny", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.TypyWplat:
                        Title = "Rodzaj wpłaty lub wypłaty";
                        nagłówek += "rodzaju wpłaty lub wypłaty";
                        zmianaKolumn = new List<int>() { 0, 5 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Rodzaj wpłaty lub wypłaty: ",
                            "Rodzaj ewidencji: ",
                            "Sposób rozliczenia: ",
                            "Grupa składników czynszu: ",
                            "Czy naliczać odsetki? ",
                            "Czy liczyć odsetki na nocie? ",
                            "VAT: ",
                            "SWW: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod_wplat", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "typ_wplat", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));

                        kontrolki.Add(new Kontrolki.DropDownList("field", "rodz_e", new List<string[]>()
                        {
                            new string[] {"1", "dziennik komornego"},
                            new string[] {"2", "wpłaty"},
                            new string[] {"3", "zmniejszenia"},
                            new string[] {"4", "zwiększenia"}
                        }, kontrolkiWłączone, false));

                        kontrolki.Add(new Kontrolki.DropDownList("field", "s_rozli", new List<string[]>()
                        {
                            new string[] {"1", "Zmniejszenie"},
                            new string[] {"2", "Zwiększenie"},
                            new string[] {"3", "Zwrot"}
                        }, kontrolkiWłączone, false));

                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod", db.GrupySkładnikówCzynszu.AsEnumerable<DostępDoBazy.GrupaSkładnikówCzynszu>().Select(g => new string[] { g.kod.ToString(), g.nazwa }).ToList(), true, true));
                        kontrolki.Add(new Kontrolki.RadioButtonList("field", "tn_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.RadioButtonList("field", "nota_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.DropDownList("field", "vat", db.StawkiVat.AsEnumerable<DostępDoBazy.StawkaVat>().Select(r => r.WażnePolaDoRozwijanejListy()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "sww", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 10, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                        Title = "Grupa składników czynszu";
                        nagłówek += "grupy składników czynszu";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Nazwa grupy składników czynszu: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.GrupyFinansowe:
                        Title = "Grupa finansowa";
                        nagłówek += "grupy finansowej";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Konto FK: ",
                            "Nazwa grupy finansowej: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "k_syn", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.StawkiVat:
                        Title = "Stawka VAT";
                        nagłówek += "stawki VAT";
                        zmianaKolumn = new List<int>() { 0 };
                        DostępDoBazy.StawkaVat stawkaVat = WartościSesji.Rekord as DostępDoBazy.StawkaVat;
                        etykiety = new string[]
                        {
                            "Oznaczenie stawki: ",
                            "Symbol fiskalny: "
                        };

                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "symb_fisk", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.Atrybuty:
                        Title = "Cecha obiektów";
                        nagłówek += "cechy obiektów";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Kod: ",
                            "Nazwa: ",
                            "Numeryczna/charakter: ",
                            "Jednostka miary: ",
                            "Wartość domyślna: ",
                            "Uwagi: ",
                            "Dotyczy: "
                        };

                        DostępDoBazy.Atrybut atrybut = WartościSesji.Rekord as DostępDoBazy.Atrybut;

                        switch (_akcja)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                atrybut.nr_str = "N";
                                atrybut.zb_b = atrybut.zb_l = atrybut.zb_n = atrybut.zb_s = "X";

                                break;
                        }

                        kontrolki.Add(new Kontrolki.TextBox("field", "kod", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 20, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.RadioButtonList("field", "nr_str", new List<string>() { "numeryczna", "charakter" }, new List<string>() { "N", "C" }, kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "jedn", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 6, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "wartosc", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, kontrolkiWłączone));

                        List<string> selectedValues = new List<string>();

                        if (String.Equals(atrybut.zb_l, "X"))
                            selectedValues.Add("l");

                        if (String.Equals(atrybut.zb_n, "X"))
                            selectedValues.Add("n");

                        if (String.Equals(atrybut.zb_b, "X"))
                            selectedValues.Add("b");

                        if (String.Equals(atrybut.zb_s, "X"))
                            selectedValues.Add("s");

                        kontrolki.Add(new Kontrolki.CheckBoxList("field", "zb", new List<string>() { "lokale", "najemcy", "budynki", "wspólnoty" }, new List<string>() { "l", "n", "b", "s" }, selectedValues, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        Title = "Użytkownik";
                        nagłówek += "użytkownika";
                        zmianaKolumn = new List<int>() { 0 };
                        etykiety = new string[]
                        {
                            "Symbol: ",
                            "Nazwisko: ",
                            "Imię: ",
                            "Użytkownik: ",
                            "Hasło: ",
                            "Potwierdź hasło: "
                        };

                        DostępDoBazy.Użytkownik użytkownik;

                        if (WartościSesji.Rekord == null)
                        {
                            if (_akcja != Enumeratory.Akcja.Dodaj)
                            {
                                WartościSesji.Rekord = db.Użytkownicy.Single(u => u.__record == __record);
                                użytkownik = WartościSesji.Rekord as DostępDoBazy.Użytkownik;
                                użytkownik.haslo = String.Empty;
                            }
                            else
                                WartościSesji.Rekord = new DostępDoBazy.Użytkownik();
                        }

                        użytkownik = WartościSesji.Rekord as DostępDoBazy.Użytkownik;

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", użytkownik.__record));
                        kontrolki.Add(new Kontrolki.TextBox("field", "symbol", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, kontrolkiWłączone));

                        if (!kontrolkiWłączone)
                        {
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nazwisko", użytkownik.nazwisko));
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("imie", użytkownik.imie));
                        }

                        kontrolki.Add(new Kontrolki.TextBox("field", "nazwisko", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "imie", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uzytkownik", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "haslo", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 8, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "haslo2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 8, 1, kontrolkiWłączone));

                        break;

                    case Enumeratory.Tabela.ObrotyNajemcy:
                        Title = "Obrót najemcy";
                        nagłówek += "obrotu najemcy";
                        zmianaKolumn = new List<int>() { 0, 3, 4 };
                        etykiety = new string[]
                        {
                            "Kwota: ",
                            "Data: ",
                            "Data NO: ",
                            "Rodzaj obrotu: ",
                            "Nr dowodu: ",
                            "Pozycja",
                            "Uwagi"
                        };

                        DostępDoBazy.Obrót obrót = WartościSesji.Rekord as DostępDoBazy.Obrót;

                        if (WartościSesji.Rekord == null)
                        {
                            switch (Start.AktywnyZbiór)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    zbiór = db.Obroty1;

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    zbiór = db.Obroty2;

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    zbiór = db.Obroty3;

                                    break;
                            }

                            if (_akcja != Enumeratory.Akcja.Dodaj)
                                WartościSesji.Rekord = zbiór.Find(__record) as DostępDoBazy.Rekord;
                            else
                                (WartościSesji.Rekord as DostępDoBazy.Obrót).nr_kontr = PobierzWartośćParametru<int>("additionalId");
                        }

                        kontrolki.Add(new Kontrolki.TextBox("field", "suma", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 14, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "data_obr", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", String.Empty, Kontrolki.TextBox.TextBoxMode.Data, 10, 1, kontrolkiWłączone));

                        List<DostępDoBazy.RodzajPłatności> typesOfPayment = db.RodzajePłatności.ToList();

                        kontrolki.Add(new Kontrolki.DropDownList("field", "kod_wplat", typesOfPayment.Select(t => t.ImportantFieldsForDropdown()).ToList(), kontrolkiWłączone, false));
                        kontrolki.Add(new Kontrolki.TextBox("field", "nr_dowodu", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 11, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "pozycja_d", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, kontrolkiWłączone));
                        kontrolki.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, kontrolkiWłączone));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_kontr", obrót.nr_kontr));

                        break;
                }

                placeOfHeading.Controls.Add(new LiteralControl("<h2>" + nagłówek + "</h2>"));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("action", _akcja.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("table", _tabela.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("id", __record));

                Control komórka = null;
                int indeksKolumny = -1;
                IEnumerable<PropertyInfo> właściwościDoPobrania = typRekordu.GetProperties();

                for (int i = 0; i < kontrolki.Count; i++)
                {
                    if (zmianaKolumn.Contains(i))
                    {
                        indeksKolumny++;
                        komórka = formRow.FindControl("column" + indeksKolumny.ToString());
                    }

                    Control kontrolka = kontrolki[i];
                    string idKontrolki = kontrolka.ID;
                    string etykieta = String.Empty;

                    if (!String.IsNullOrEmpty(idKontrolki))
                    {
                        PropertyInfo właściwość = właściwościDoPobrania.Single(w => w.Name == idKontrolki);
                        object wartość = właściwość.GetValue(WartościSesji.Rekord);
                        etykieta = właściwość.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(true).Name;
                        etykieta = String.Concat(Char.ToUpper(etykieta[0]), etykieta.Substring(1), ": ");

                        if (wartość != null)
                        {
                            string wartośćDoZapisania;
                            Type typWartości = wartość.GetType();

                            if (typWartości == typeof(decimal))
                                wartośćDoZapisania = Convert.ToDecimal(wartość).ToString("N");
                            else if (typWartości == typeof(DateTime))
                                wartośćDoZapisania = Convert.ToDateTime(wartość).ToShortDateString();
                            else
                                wartośćDoZapisania = wartość.ToString();

                            (kontrolka as Kontrolki.IKontrolkaZWartością).Wartość = wartośćDoZapisania.Trim();
                        }

                        if (!kontrolkiWłączone)
                        {
                            kontrolka.ID = String.Concat(idKontrolki, "_disabled");

                            form.Controls.Add(new Kontrolki.HtmlInputHidden(idKontrolki, wartość));
                        }
                    }

                    komórka.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                    komórka.Controls.Add(new Kontrolki.Label("fieldLabel", kontrolka.ID, etykieta, String.Empty));
                    DodajNowąLinię(komórka);
                    komórka.Controls.Add(kontrolka);
                    komórka.Controls.Add(new LiteralControl("</div>"));
                }

                if (podgląd != null)
                {
                    placeOfPreview.Controls.Add(new LiteralControl("<h3>"));

                    for (int i = 0; i < podgląd.Count; i += 2)
                    {
                        placeOfPreview.Controls.Add(podgląd[i]);
                        placeOfPreview.Controls.Add(podgląd[i + 1]);
                        DodajNowąLinię(placeOfPreview);
                    }

                    placeOfPreview.Controls.Add(new LiteralControl("</h3>"));
                }

                if (przyciskiZakładek != null)
                {
                    for (int i = 0; i < przyciskiZakładek.Count; i++)
                    {
                        placeOfTabButtons.Controls.Add(przyciskiZakładek[i]);
                        placeOfTabButtons.Controls.Add(etykietyZakładek[i]);
                    }

                    foreach (Kontrolki.HtmlIframe tab in zakładki)
                        placeOfTabs.Controls.Add(tab);
                }

                foreach (Kontrolki.Button button in przyciski)
                    placeOfButtons.Controls.Add(button);

                Start.ŚcieżkaStrony.Dodaj(nagłówek);
            }
        }
    }
}