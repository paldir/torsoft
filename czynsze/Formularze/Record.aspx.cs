using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Reflection;

namespace czynsze.Formularze
{
    public partial class Record : Strona
    {
        int id;
        Enumeratory.Akcja action;
        Enumeratory.Tabela table;

        List<DostępDoBazy.AtrybutObiektu> attributesOfObject
        {
            get { return (List<DostępDoBazy.AtrybutObiektu>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        List<DostępDoBazy.SkładnikCzynszuLokalu> rentComponentsOfPlace
        {
            get { return (List<DostępDoBazy.SkładnikCzynszuLokalu>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        List<DostępDoBazy.BudynekWspólnoty> communityBuildings
        {
            get { return (List<DostępDoBazy.BudynekWspólnoty>)Session["communityBuildings"]; }
            set { Session["communityBuildings"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                bool globalEnabled = false;
                bool idEnabled = false;
                //string[] values = (string[])Session["values"];
                //int numberOfFields = 0;
                string[] labels = null;
                string heading = null;
                List<Kontrolki.Button> buttons = new List<Kontrolki.Button>();
                List<Control> controls = new List<Control>();
                List<int> columnSwitching = null;
                List<Kontrolki.HtmlIframe> tabs = null;
                List<Kontrolki.HtmlInputRadioButton> tabButtons = null;
                List<Kontrolki.Label> labelsOfTabButtons = null;
                List<Control> preview = null;
                //id = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);
                id = PobierzWartośćParametru<int>("id");
                //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
                action = PobierzWartośćParametru<Enumeratory.Akcja>("action");
                //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
                table = PobierzWartośćParametru<Enumeratory.Tabela>("table");
                string backUrl = "javascript: Load('" + Request.UrlReferrer + "')";
                DostępDoBazy.IRekord rekord = Session["rekord"] as DostępDoBazy.IRekord;
                Dictionary<bool, string> fromIdEnabledToIdSuffix = new Dictionary<bool, string>()
            {
                {true, String.Empty},
                {false, "_disabled"}
            };

                switch (action)
                {
                    case Enumeratory.Akcja.Dodaj:
                    case Enumeratory.Akcja.Edytuj:
                        Kontrolki.Button przycisk = new Kontrolki.Button("buttons", "Save", "Zapisz", "RecordValidation.aspx");

                        buttons.Add(przycisk);

                        switch (action)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                globalEnabled = idEnabled = true;
                                heading = "Dodawanie ";

                                buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                                break;

                            case Enumeratory.Akcja.Edytuj:
                                globalEnabled = true;
                                idEnabled = false;
                                heading = "Edycja ";

                                buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                                break;
                        }

                        break;

                    case Enumeratory.Akcja.Usuń:
                        globalEnabled = idEnabled = false;
                        heading = "Usuwanie ";

                        buttons.Add(new Kontrolki.Button("buttons", "Delete", "Usuń", "RecordValidation.aspx"));
                        buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                        break;

                    case Enumeratory.Akcja.Przeglądaj:
                        globalEnabled = idEnabled = false;
                        heading = "Przeglądanie ";

                        buttons.Add(new Kontrolki.Button("buttons", "Back", "Powrót", backUrl));

                        break;

                    case Enumeratory.Akcja.Przenieś:
                        globalEnabled = idEnabled = false;
                        heading = "Przenoszenie ";

                        buttons.Add(new Kontrolki.Button("buttons", "Move", "Przenieś", "RecordValidation.aspx"));
                        buttons.Add(new Kontrolki.Button("buttons", "Cancel", "Anuluj", backUrl));

                        break;
                }

                switch (table)
                {
                    case Enumeratory.Tabela.Budynki:
                        Title = "Budynek";
                        heading += "budynku";
                        columnSwitching = new List<int>() { 0, 6 };
                        labels = new string[] 
                        { 
                            "Kod budynku: ", 
                            "Ilość lokali: ", 
                            "Sposób rozliczania: ", 
                            "Adres: ", 
                            "Adres cd.: ",
                            "Udział w koszt.: ",
                            "Uwagi: " 
                        };

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                        };

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AtrybutyObiektu.aspx?attributeOf="+Enumeratory.Atrybut.Budynku+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.Budynki.Single(b => b.kod_1 == id);
                            else
                                rekord = new DostępDoBazy.Budynek();

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();

                            foreach (DostępDoBazy.AtrybutBudynku attributeOfBuilding in db.AtrybutyBudynków.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfBuilding);
                        }

                        DostępDoBazy.Budynek budynek = rekord as DostępDoBazy.Budynek;

                        preview = new List<Control>()
                        {
                            new LiteralControl("Kod budynku: "),
                            new Kontrolki.Label("previewLabel", String.Empty, id.ToString(), "id_preview"),
                            new LiteralControl("Adres: "),
                            new Kontrolki.Label("previewLabel", String.Empty, budynek.adres, "adres_preview"),
                            new LiteralControl("Adres cd.: "),
                            new Kontrolki.Label("previewLabel", String.Empty, budynek.adres_2, "adres_2_preview")
                        };

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "il_miesz", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.RadioButtonList("field", "sp_rozl", new List<string>() { "budynek", "lokale" }, new List<string>() { "0", "1" }, globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "adres", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "udzial_w_k", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 6, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 420, 6, globalEnabled));

                        break;

                    case Enumeratory.Tabela.AktywneLokale:
                    case Enumeratory.Tabela.NieaktywneLokale:

                        Title = "Lokal";
                        heading += "lokalu";

                        if (table == Enumeratory.Tabela.NieaktywneLokale)
                        {
                            Title = "Lokal (nieaktywny)";
                            heading += "(nieaktywnego)";
                        }

                        columnSwitching = new List<int> { 0, 5, 10, 17 };
                        labels = new string[] 
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

                        DostępDoBazy.Lokal lokal;

                        if (rekord == null)
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
                            }

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();
                            rentComponentsOfPlace = new List<DostępDoBazy.SkładnikCzynszuLokalu>();
                            lokal = rekord as DostępDoBazy.Lokal;

                            attributesOfObject.AddRange(db.AtrybutyLokali.AsEnumerable<DostępDoBazy.AtrybutLokalu>().Where(a => Int32.Parse(a.kod_powiaz) == id));
                            rentComponentsOfPlace.AddRange(db.SkładnikiCzynszuLokalu.AsEnumerable<DostępDoBazy.SkładnikCzynszuLokalu>().Where(c => c.kod_lok == lokal.kod_lok && c.nr_lok == lokal.nr_lok));
                        }

                        lokal = rekord as DostępDoBazy.Lokal;
                        int kodLokalu = lokal.kod_lok;
                        int nrLokalu = lokal.nr_lok;
                        int nrSystem = lokal.nr_system;

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "skladnikiCzynszu", "tabRadios", "skladnikiCzynszu", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dokumenty", "tabRadios", "dokumenty", false)
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Składniki czynszu", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(2).ID, "Cechy", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(3).ID, "Dokumenty", String.Empty)
                        };

                        preview = new List<Control>()
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

                        //
                        //CXP PART
                        //
                        string parentAction;

                        switch (action)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                parentAction = "add";

                                break;

                            case Enumeratory.Akcja.Edytuj:
                                parentAction = "edit";

                                break;

                            case Enumeratory.Akcja.Usuń:
                                parentAction = "delete";

                                break;

                            default:
                                parentAction = "browse";

                                break;
                        }
                        //
                        //TO DUMP BEHIND THE WALL
                        //

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            //new Kontrolki.HtmlIframe("tab", "skladnikiCzynszu_tab", "/czynsze1/SkladnikiCzynszuLokalu.cxp?parentAction="+parentAction+"&kod_lok="+values[1]+"&nr_lok="+values[2], "hidden"),
                            new Kontrolki.HtmlIframe("tab", "skladnikiCzynszu_tab", String.Format("SkladnikiCzynszuLokalu.aspx?parentAction={0}&kod_lok={1}&nr_lok={2}", action, kodLokalu, nrLokalu), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", String.Format("AtrybutyObiektu.aspx?attributeOf={0}&parentId={1}&action={2}&childAction=Przeglądaj", Enumeratory.Atrybut.Lokalu, id, action), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "dokumenty_tab", String.Format("/czynsze1/PlikiNajemcy.cxp?parentAction={0}&nr_system={1}", parentAction, nrSystem), "hidden")
                        };

                        //controls.Add(new Kontrolki.TextBoxP("field", "Nr_system_disabled", values[0], Kontrolki.TextBoxP.TextBoxMode.Number, 14, 1, false));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", nrSystem));

                        if (!idEnabled)
                        {
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_lok", kodLokalu));
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_lok", nrLokalu));
                        }

                        controls.Add(new Kontrolki.DropDownList("field", "kod_lok" + fromIdEnabledToIdSuffix[idEnabled], db.Budynki.ToList().OrderBy(b => b.kod_1).Select(b => b.PolaDoTabeli()).ToList(), idEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "nr_lok" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_typ", db.TypyLokali.AsEnumerable<DostępDoBazy.TypLokalu>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "adres", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pow_uzyt", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 8, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pow_miesz", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 8, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "udzial", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "dat_od", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "dat_do", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_1", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_2", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_3", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_4", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_5", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "p_6", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 5, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_kuch", db.TypyKuchni.AsEnumerable<DostępDoBazy.TypKuchni>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "nr_kontr", db.AktywniNajemcy.AsEnumerable<DostępDoBazy.AktywnyNajemca>().OrderBy(t => t.nazwisko).Select(t => t.PolaDoTabeli().ToList().GetRange(1, 4).ToArray()).ToList(), globalEnabled, true));
                        controls.Add(new Kontrolki.TextBox("field", "il_osob", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_praw", db.TytułyPrawne.AsEnumerable<DostępDoBazy.TytułPrawny>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 240, 4, globalEnabled));

                        //
                        //CXP PART
                        //
                        try
                        {
                            switch (action)
                            {
                                case Enumeratory.Akcja.Dodaj:
                                    //db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE 1=2");
                                    //db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE 1=2");
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp(nr_system numeric(14,0),id numeric(14,0) NOT NULL,plik text,nazwa_pliku character(100),opis character(100), PRIMARY KEY (id))WITH (OIDS=FALSE)");

                                    break;

                                default:
                                    //db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE kod_lok=" + values[1] + " AND nr_lok=" + values[2]);
                                    //db.Database.ExecuteSqlCommand(String.Format("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE nr_system={0}", nrSystem));
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp(nr_system numeric(14,0),id numeric(14,0) NOT NULL,plik text,nazwa_pliku character(100),opis character(100), PRIMARY KEY (id))WITH (OIDS=FALSE)");
                                    db.Database.ExecuteSqlCommand(String.Format("INSERT INTO pliki_tmp SELECT * FROM pliki WHERE nr_system={0}", nrSystem));
                                    
                                    break;
                            }
                        }
                        catch { }
                        //
                        //TO DUMP BEHIND THE WALL
                        //

                        break;

                    case Enumeratory.Tabela.AktywniNajemcy:
                    case Enumeratory.Tabela.NieaktywniNajemcy:
                        Title = "Najemca";
                        heading += "najemcy";
                        columnSwitching = new List<int> { 0, 6 };
                        labels = new string[] 
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

                        if (table == Enumeratory.Tabela.NieaktywniNajemcy)
                        {
                            Title = "Najemca (nieaktywny)";
                            heading += "(nieaktywnego)";
                        }

                        DostępDoBazy.Najemca najemca;

                        if (rekord == null)
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


                                IEnumerable<DostępDoBazy.Najemca> tenants = db.AktywniNajemcy.AsEnumerable<DostępDoBazy.Najemca>().Concat(db.NieaktywniNajemcy.Cast<DostępDoBazy.Najemca>());
                                najemca = rekord as DostępDoBazy.Najemca;

                                if (tenants.Any())
                                    najemca.nr_kontr = (tenants.Max(t => t.nr_kontr) + 1);
                                else
                                    najemca.nr_kontr = 1;
                            }

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();

                            foreach (DostępDoBazy.AtrybutNajemcy attributeOfTenant in db.AtrybutyNajemców.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfTenant);
                        }

                        najemca = rekord as DostępDoBazy.Najemca;

                        preview = new List<Control>()
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

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                        };

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AtrybutyObiektu.aspx?attributeOf="+Enumeratory.Atrybut.Najemcy+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        controls.Add(new Kontrolki.TextBox("field", "nr_kontr_disabled", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 6, 1, false));
                        placeOfButtons.Controls.Add(new Kontrolki.HtmlInputHidden("id", najemca.nr_kontr));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_najem", db.TypyNajemców.AsEnumerable<DostępDoBazy.TypNajemcy>().Select(t => t.WażnePolaDoRozwijanejListy()).ToList(), globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "nazwisko", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "imie", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_1", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr_dow", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 9, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pesel", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 11, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa_z", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "e_mail", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "l__has", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 120, 2, globalEnabled));

                        break;

                    case Enumeratory.Tabela.SkladnikiCzynszu:
                        Title = "Składnik opłat";
                        heading += "składnika opłat";
                        columnSwitching = new List<int> { 0, 6, 9 };
                        labels = new string[]
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

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.SkładnikiCzynszu.Single(c => c.nr_skl == id);
                            else
                                rekord = new DostępDoBazy.SkładnikCzynszu();
                        }

                        DostępDoBazy.SkładnikCzynszu składnikCzynszu = rekord as DostępDoBazy.SkładnikCzynszu;

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", składnikCzynszu.nr_skl));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "rodz_e", new List<string[]> { new string[] { "1", "dziennik komornego" }, new string[] { "2", "wpłaty" }, new string[] { "3", "zmniejszenia" }, new string[] { "4", "zwiększenia" } }, globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "s_zaplat", new List<string[]> { new string[] { "1", "za m2 pow. użytkowej" }, new string[] { "2", "za określoną ilość" }, new string[] { "3", "za osobę" }, new string[] { "4", "za lokal" }, new string[] { "5", "za ilość dni w miesiącu" }, new string[] { "6", "za osobę - przedziały" } }, globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "stawka", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "stawka_inf", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "typ_skl", new List<string[]> { new string[] { "0", "stały" }, new string[] { "1", "zmienny" } }, globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "data_1", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "data_2", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.DropDownList("field", "kod", db.GrupySkładnikówCzynszu.AsEnumerable<DostępDoBazy.GrupaSkładnikówCzynszu>().OrderBy(g => g.kod).Select(g => new string[] { g.kod.ToString(), g.kod.ToString(), g.nazwa }).ToList(), globalEnabled, false));

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

                            tableCell.Controls.Add(new Kontrolki.TextBox("field", String.Format("stawka_0{0}", i), Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 10, 1, globalEnabled));
                            tableRow.Cells.Add(tableCell);
                            interval.Rows.Add(tableRow);
                        }

                        controls.Add(interval);

                        break;

                    case Enumeratory.Tabela.Wspolnoty:
                        Title = "Wspólnota";
                        heading += "wspólnoty";
                        columnSwitching = new List<int>() { 0, 7 };
                        labels = new string[]
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

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.Wspólnoty.Single(c => c.kod == id);
                            else
                                rekord = new DostępDoBazy.Wspólnota();

                            attributesOfObject = new List<DostępDoBazy.AtrybutObiektu>();
                            communityBuildings = new List<DostępDoBazy.BudynekWspólnoty>();

                            attributesOfObject.AddRange(db.AtrybutyWspólnot.ToList().Where(a => Int32.Parse(a.kod_powiaz) == id));
                            communityBuildings.AddRange(db.BudynkiWspólnot.Where(c => c.kod == id).OrderBy(b => b.kod_1));
                        }

                        DostępDoBazy.Wspólnota wspólnota = rekord as DostępDoBazy.Wspólnota;

                        preview = new List<Control>()
                        {
                            new LiteralControl("Kod: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.kod.ToString(), "id_preview"),
                            new LiteralControl("Nazwa: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.nazwa_skr, "nazwa_skr_preview"),
                            new LiteralControl("Ilość budynków: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.il_bud.ToString(), "il_bud_preview"),
                            new LiteralControl("Ilość lokali: "),
                            new Kontrolki.Label("previewLabel", String.Empty, wspólnota.il_miesz.ToString(), "il_miesz_preview")
                        };

                        tabButtons = new List<Kontrolki.HtmlInputRadioButton>()
                        {
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "budynki", "tabRadios", "budynki", false),
                            new Kontrolki.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        };

                        labelsOfTabButtons = new List<Kontrolki.Label>()
                        {
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(1).ID, "Budynki", String.Empty),
                            new Kontrolki.Label("tabLabel", tabButtons.ElementAt(2).ID, "Cechy", String.Empty),
                        };

                        tabs = new List<Kontrolki.HtmlIframe>()
                        {
                            new Kontrolki.HtmlIframe("tab", "budynki_tab", "BudynkiWspolnoty.aspx?kod="+id.ToString()+"&parentAction="+action.ToString(), "hidden"),
                            new Kontrolki.HtmlIframe("tab", "cechy_tab", "AtrybutyObiektu.aspx?attributeOf="+Enumeratory.Atrybut.Wspólnoty+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                        };

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", wspólnota.kod));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "il_bud", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "il_miesz", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa_pel", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 50, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa_skr", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "adres_2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr1_konta", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr2_konta", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nr3_konta", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 32, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "sciezka_fk", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 420, 6, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TypyLokali:
                        Title = "Typ lokali";
                        heading += "typu lokalu";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Typ lokalu: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.TypyLokali.Single(t => t.kod_typ == id);
                            else
                                rekord = new DostępDoBazy.TypLokalu();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.TypLokalu).kod_typ));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "typ_lok", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 6, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TypyKuchni:
                        Title = "Rodzaj kuchni";
                        heading += "rodzaju kuchni";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Rodzaj kuchni: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.TypyKuchni.Single(t => t.kod_kuch == id);
                            else
                                rekord = new DostępDoBazy.TypKuchni();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.TypKuchni).kod_kuch));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "typ_kuch", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.RodzajeNajemcy:
                        Title = "Rodzaj najemców";
                        heading += "rodzaju najemców";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Rodzaj najemcy: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.TypyNajemców.Single(t => t.kod_najem == id);
                            else
                                rekord = new DostępDoBazy.TypNajemcy();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.TypNajemcy).kod_najem));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "r_najemcy", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TytulyPrawne:
                        Title = "Tytuł prawny do lokali";
                        heading += "tytułu prawnego do lokali";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Tytuł prawny: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.TytułyPrawne.Single(t => t.kod_praw == id);
                            else
                                rekord = new DostępDoBazy.TytułPrawny();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.TytułPrawny).kod_praw));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "tyt_prawny", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.TypyWplat:
                        Title = "Rodzaj wpłaty lub wypłaty";
                        heading += "rodzaju wpłaty lub wypłaty";
                        columnSwitching = new List<int>() { 0, 5 };
                        labels = new string[]
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

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.RodzajePłatności.Single(t => t.kod_wplat == id);
                            else
                                rekord = new DostępDoBazy.RodzajPłatności();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.RodzajPłatności).kod_wplat));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "typ_wplat", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        controls.Add(new Kontrolki.DropDownList("field", "rodz_e", new List<string[]>()
                        {
                            new string[] {"1", "dziennik komornego"},
                            new string[] {"2", "wpłaty"},
                            new string[] {"3", "zmniejszenia"},
                            new string[] {"4", "zwiększenia"}
                        }, globalEnabled, false));

                        controls.Add(new Kontrolki.DropDownList("field", "s_rozli", new List<string[]>()
                        {
                            new string[] {"1", "Zmniejszenie"},
                            new string[] {"2", "Zwiększenie"},
                            new string[] {"3", "Zwrot"}
                        }, globalEnabled, false));

                        controls.Add(new Kontrolki.DropDownList("field", "kod", db.GrupySkładnikówCzynszu.AsEnumerable<DostępDoBazy.GrupaSkładnikówCzynszu>().Select(g => new string[] { g.kod.ToString(), g.nazwa }).ToList(), true, true));
                        controls.Add(new Kontrolki.RadioButtonList("field", "tn_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, globalEnabled, false));
                        controls.Add(new Kontrolki.RadioButtonList("field", "nota_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "vat", db.StawkiVat.AsEnumerable<DostępDoBazy.StawkaVat>().Select(r => r.WażnePolaDoRozwijanejListy()).ToList(), globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "sww", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 10, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                        Title = "Grupa składników czynszu";
                        heading += "grupy składników czynszu";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Nazwa grupy składników czynszu: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.GrupySkładnikówCzynszu.Single(g => g.kod == id);
                            else
                                rekord = new DostępDoBazy.GrupaSkładnikówCzynszu();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.GrupaSkładnikówCzynszu).kod));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.GrupyFinansowe:
                        Title = "Grupa finansowa";
                        heading += "grupy finansowej";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Konto FK: ",
                            "Nazwa grupy finansowej: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.GrupyFinansowe.Single(g => g.kod == id);
                            else
                                rekord = new DostępDoBazy.GrupaSkładnikówCzynszu();
                        }

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", (rekord as DostępDoBazy.GrupaFinansowa).kod));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "k_syn", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 3, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.StawkiVat:
                        Title = "Stawka VAT";
                        heading += "stawki VAT";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Oznaczenie stawki: ",
                            "Symbol fiskalny: "
                        };

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.StawkiVat.Single(r => r.__record == id);
                            else
                                rekord = new DostępDoBazy.StawkaVat();
                        }

                        DostępDoBazy.StawkaVat stawkaVat = rekord as DostępDoBazy.StawkaVat;

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", stawkaVat.__record));

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nazwa", stawkaVat.nazwa));

                        controls.Add(new Kontrolki.TextBox("field", "nazwa" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "symb_fisk", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.Atrybuty:
                        Title = "Cecha obiektów";
                        heading += "cechy obiektów";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Kod: ",
                            "Nazwa: ",
                            "Numeryczna/charakter: ",
                            "Jednostka miary: ",
                            "Wartość domyślna: ",
                            "Uwagi: ",
                            "Dotyczy: "
                        };

                        DostępDoBazy.Atrybut atrybut;

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = db.Atrybuty.Single(a => a.kod == id);
                            else
                            {
                                rekord = new DostępDoBazy.Atrybut();
                                atrybut = rekord as DostępDoBazy.Atrybut;
                                atrybut.nr_str = "N";
                                atrybut.zb_b = atrybut.zb_l = atrybut.zb_n = atrybut.zb_s = "X";
                            }
                        }

                        atrybut = rekord as DostępDoBazy.Atrybut;

                        if (!idEnabled)
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("id", atrybut.kod));

                        controls.Add(new Kontrolki.TextBox("field", "id" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 20, 1, globalEnabled));
                        controls.Add(new Kontrolki.RadioButtonList("field", "nr_str", new List<string>() { "numeryczna", "charakter" }, new List<string>() { "N", "C" }, globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "jedn", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 6, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "wartosc", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, globalEnabled));

                        List<string> selectedValues = new List<string>();

                        if (String.Equals(atrybut.zb_l, "X"))
                            selectedValues.Add("l");

                        if (String.Equals(atrybut.zb_n, "X"))
                            selectedValues.Add("n");

                        if (String.Equals(atrybut.zb_b, "X"))
                            selectedValues.Add("b");

                        if (String.Equals(atrybut.zb_s, "X"))
                            selectedValues.Add("s");

                        controls.Add(new Kontrolki.CheckBoxList("field", "zb", new List<string>() { "lokale", "najemcy", "budynki", "wspólnoty" }, new List<string>() { "l", "n", "b", "s" }, selectedValues, globalEnabled));

                        break;

                    case Enumeratory.Tabela.Uzytkownicy:
                        Title = "Użytkownik";
                        heading += "użytkownika";
                        columnSwitching = new List<int>() { 0 };
                        labels = new string[]
                        {
                            "Symbol: ",
                            "Nazwisko: ",
                            "Imię: ",
                            "Użytkownik: ",
                            "Hasło: ",
                            "Potwierdź hasło: "
                        };

                        DostępDoBazy.Użytkownik użytkownik;

                        if (rekord == null)
                        {
                            if (action != Enumeratory.Akcja.Dodaj)
                            {
                                rekord = db.Użytkownicy.Single(u => u.__record == id);
                                użytkownik = rekord as DostępDoBazy.Użytkownik;
                                użytkownik.haslo = String.Empty;
                            }
                            else
                                rekord = new DostępDoBazy.Użytkownik();
                        }

                        użytkownik = rekord as DostępDoBazy.Użytkownik;

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", użytkownik.__record));
                        controls.Add(new Kontrolki.TextBox("field", "symbol", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 2, 1, globalEnabled));

                        if (!idEnabled)
                        {
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("nazwisko", użytkownik.nazwisko));
                            form.Controls.Add(new Kontrolki.HtmlInputHidden("imie", użytkownik.imie));
                        }

                        controls.Add(new Kontrolki.TextBox("field", "nazwisko" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "imie" + fromIdEnabledToIdSuffix[idEnabled], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 15, 1, idEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uzytkownik", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, false));
                        controls.Add(new Kontrolki.TextBox("field", "haslo", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 8, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "haslo2", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 8, 1, globalEnabled));

                        break;

                    case Enumeratory.Tabela.ObrotyNajemcy:
                        Title = "Obrót najemcy";
                        heading += "obrotu najemcy";
                        columnSwitching = new List<int>() { 0, 3, 4 };
                        labels = new string[]
                        {
                            "Kwota: ",
                            "Data: ",
                            "Data NO: ",
                            "Rodzaj obrotu: ",
                            "Nr dowodu: ",
                            "Pozycja",
                            "Uwagi"
                        };

                        DostępDoBazy.Obrót obrót;

                        if (rekord == null)
                        {
                            IEnumerable<DostępDoBazy.Obrót> turnOvers = null;

                            switch (Start.AktywnyZbiór)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    turnOvers = db.Obroty1.AsEnumerable<DostępDoBazy.Obrót>();

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    turnOvers = db.Obroty2.AsEnumerable<DostępDoBazy.Obrót>();

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    turnOvers = db.Obroty3.AsEnumerable<DostępDoBazy.Obrót>();

                                    break;
                            }

                            if (action != Enumeratory.Akcja.Dodaj)
                                rekord = turnOvers.Single(t => t.__record == id);
                            else
                            {
                                rekord = new DostępDoBazy.Obrót();
                                obrót = rekord as DostępDoBazy.Obrót;
                                obrót.nr_kontr = PobierzWartośćParametru<int>("additionalId");

                                if (turnOvers.Any())
                                    obrót.__record = (turnOvers.Max(t => t.__record) + 1);
                                else
                                    obrót.__record = 1;

                            }
                        }

                        obrót = rekord as DostępDoBazy.Obrót;

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", obrót.__record));
                        controls.Add(new Kontrolki.TextBox("field", "suma", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 14, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "data_obr", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", String.Empty, Kontrolki.TextBox.TextBoxMode.Data, 10, 1, globalEnabled));

                        List<DostępDoBazy.RodzajPłatności> typesOfPayment = db.RodzajePłatności.ToList();

                        //controls.Add(new Kontrolki.RadioButtonList("field", "kod_wplat", typesOfPayment.Select(t => t.typ_wplat).ToList(), typesOfPayment.Select(t => t.kod_wplat.ToString()).ToList(), values[4], globalEnabled, false));
                        controls.Add(new Kontrolki.DropDownList("field", "kod_wplat", typesOfPayment.Select(t => t.ImportantFieldsForDropdown()).ToList(), globalEnabled, false));
                        controls.Add(new Kontrolki.TextBox("field", "nr_dowodu", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 11, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "pozycja_d", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, globalEnabled));
                        controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 40, 1, globalEnabled));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_kontr", obrót.nr_kontr));

                        break;
                }

                placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("action", action.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("table", table.ToString()));

                Control cell = null;
                int columnIndex = -1;
                //values = rekord.WszystkiePola();
                Type typRekordu = rekord.GetType();
                IEnumerable<PropertyInfo> właściwościDoUstawienia = typRekordu.GetProperties();

                for (int i = 0; i < controls.Count; i++)
                {
                    if (columnSwitching.Contains(i))
                    {
                        columnIndex++;
                        cell = formRow.FindControl("column" + columnIndex.ToString());
                    }

                    Control kontrolka = controls[i];
                    string idKontrolki = kontrolka.ID;
                    string etykieta = String.Empty;

                    if (!String.IsNullOrEmpty(idKontrolki))
                    {
                        PropertyInfo właściwość = właściwościDoUstawienia.SingleOrDefault(w => w.Name == idKontrolki.Replace("_disabled", String.Empty));

                        if (właściwość != null)
                        {
                            object wartość = właściwość.GetValue(rekord);
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
                        }
                    }

                    cell.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                    cell.Controls.Add(new Kontrolki.Label("fieldLabel", kontrolka.ID, etykieta, String.Empty));
                    DodajNowąLinię(cell);
                    cell.Controls.Add(kontrolka);
                    cell.Controls.Add(new LiteralControl("</div>"));
                }

                if (preview != null)
                {
                    placeOfPreview.Controls.Add(new LiteralControl("<h3>"));

                    for (int i = 0; i < preview.Count; i += 2)
                    {
                        placeOfPreview.Controls.Add(preview[i]);
                        placeOfPreview.Controls.Add(preview[i + 1]);
                        DodajNowąLinię(placeOfPreview);
                    }

                    placeOfPreview.Controls.Add(new LiteralControl("</h3>"));
                }

                if (tabButtons != null)
                {
                    for (int i = 0; i < tabButtons.Count; i++)
                    {
                        placeOfTabButtons.Controls.Add(tabButtons[i]);
                        placeOfTabButtons.Controls.Add(labelsOfTabButtons[i]);
                    }

                    foreach (Kontrolki.HtmlIframe tab in tabs)
                        placeOfTabs.Controls.Add(tab);
                }

                foreach (Kontrolki.Button button in buttons)
                    placeOfButtons.Controls.Add(button);

                Start.ŚcieżkaStrony.Dodaj(heading);
            }
        }
    }
}