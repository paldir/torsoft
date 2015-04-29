using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;

namespace czynsze.Formularze
{
    public partial class RecordValidation : Strona
    {
        Enumeratory.Tabela table;
        Enumeratory.Akcja action;
        int id;

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
            string[] recordFields = null;
            string validationResult = null;
            string dbWriteResult = null;
            //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("table"))]);
            table = PobierzWartośćParametru<Enumeratory.Tabela>("table");
            //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("action"))]);
            action = PobierzWartośćParametru<Enumeratory.Akcja>("action");
            string backUrl = "javascript: Load('List.aspx?table=" + table + "')";
            string nominativeCase = String.Empty;
            string genitiveCase = String.Empty;
            DostępDoBazy.IRekord record = null;
            Type attributeType = null;
            Type inactiveType = null;

            Dictionary<Enumeratory.Akcja, string> dictionaryOfActionInfinitives = new Dictionary<Enumeratory.Akcja, string>()
            {
                { Enumeratory.Akcja.Dodaj, "dodać" },
                { Enumeratory.Akcja.Edytuj, "edytować" },
                { Enumeratory.Akcja.Przenieś, "przenieść" },
                { Enumeratory.Akcja.Usuń, "usunąć" }
            };

            Dictionary<Enumeratory.Akcja, string> dictionaryOfActionParticiples = new Dictionary<Enumeratory.Akcja, string>()
            {
                { Enumeratory.Akcja.Dodaj, "dodany" },
                { Enumeratory.Akcja.Edytuj, "wyedytowany" },
                { Enumeratory.Akcja.Przenieś, "przeniesiony" },
                { Enumeratory.Akcja.Usuń, "usunięty" }
            };

            if (action != Enumeratory.Akcja.Dodaj)
            {
                //id = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("id"))]);
                id = PobierzWartośćParametru<int>("id");

                form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));
            }

            form.Controls.Add(new Kontrolki.HtmlInputHidden("table", table.ToString()));
            form.Controls.Add(new Kontrolki.HtmlInputHidden("action", action.ToString()));

            try
            {
                using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                {
                    switch (table)
                    {
                        case Enumeratory.Tabela.Budynki:
                            record = new DostępDoBazy.Budynek();
                            attributeType = typeof(DostępDoBazy.AtrybutBudynku);
                            nominativeCase = "budynek";
                            genitiveCase = "budynku";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("il_miesz"),
                                        PobierzWartośćParametru<string>("sp_rozl"),
                                        PobierzWartośćParametru<string>("adres"),
                                        PobierzWartośćParametru<string>("adres_2"),
                                        PobierzWartośćParametru<string>("udzial_w_k"),
                                        PobierzWartośćParametru<string>("uwagi")
                                    };

                            break;

                        case Enumeratory.Tabela.AktywneLokale:
                            record = new DostępDoBazy.AktywnyLokal();
                            attributeType = typeof(DostępDoBazy.AtrybutLokalu);
                            inactiveType = typeof(DostępDoBazy.NieaktywnyLokal);
                            nominativeCase = "lokal";
                            genitiveCase = "lokalu";

                            recordFields = new string[]
                            {
                                PobierzWartośćParametru<string>("id"),
                                PobierzWartośćParametru<string>("kod_lok"),
                                PobierzWartośćParametru<string>("nr_lok"),
                                PobierzWartośćParametru<string>("kod_typ"),
                                PobierzWartośćParametru<string>("adres"),
                                PobierzWartośćParametru<string>("adres_2"),
                                PobierzWartośćParametru<string>("pow_uzyt"),
                                PobierzWartośćParametru<string>("pow_miesz"),
                                PobierzWartośćParametru<string>("udzial"),
                                PobierzWartośćParametru<string>("dat_od"),
                                PobierzWartośćParametru<string>("dat_do"),
                                PobierzWartośćParametru<string>("p_1"),
                                PobierzWartośćParametru<string>("p_2"),
                                PobierzWartośćParametru<string>("p_3"),
                                PobierzWartośćParametru<string>("p_4"),
                                PobierzWartośćParametru<string>("p_5"),
                                PobierzWartośćParametru<string>("p_6"),
                                PobierzWartośćParametru<string>("kod_kuch"),
                                PobierzWartośćParametru<string>("nr_kontr"),
                                PobierzWartośćParametru<string>("il_osob"),
                                PobierzWartośćParametru<string>("kod_praw"),
                                PobierzWartośćParametru<string>("uwagi")
                            };

                            break;

                        case Enumeratory.Tabela.NieaktywneLokale:
                            record = new DostępDoBazy.NieaktywnyLokal();
                            inactiveType = typeof(DostępDoBazy.AktywnyLokal);
                            nominativeCase = "lokal (nieaktywny)";
                            genitiveCase = "lokalu (nieaktywnego)";

                            break;

                        case Enumeratory.Tabela.AktywniNajemcy:
                            record = new DostępDoBazy.AktywnyNajemca();
                            attributeType = typeof(DostępDoBazy.AtrybutNajemcy);
                            inactiveType = typeof(DostępDoBazy.NieaktywnyNajemca);
                            nominativeCase = "najemca";
                            genitiveCase = "najemcy";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("kod_najem"),
                                        PobierzWartośćParametru<string>("nazwisko"),
                                        PobierzWartośćParametru<string>("imie"),
                                        PobierzWartośćParametru<string>("adres_1"),
                                        PobierzWartośćParametru<string>("adres_2"),
                                        PobierzWartośćParametru<string>("nr_dow"),
                                        PobierzWartośćParametru<string>("pesel"),
                                        PobierzWartośćParametru<string>("nazwa_z"),
                                        PobierzWartośćParametru<string>("e_mail"),
                                        PobierzWartośćParametru<string>("l__has"),
                                        PobierzWartośćParametru<string>("uwagi")
                                    };

                            break;

                        case Enumeratory.Tabela.NieaktywniNajemcy:
                            record = new DostępDoBazy.NieaktywnyNajemca();
                            inactiveType = typeof(DostępDoBazy.AktywnyNajemca);
                            nominativeCase = "najemca (nieaktywny)";
                            genitiveCase = "najemcy (nieaktywnego)";

                            break;

                        case Enumeratory.Tabela.Wspólnoty:
                            record = new DostępDoBazy.Wspólnota();
                            attributeType = typeof(DostępDoBazy.AtrybutWspólnoty);
                            nominativeCase = "wspólnota";
                            genitiveCase = "wspólnoty";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("il_bud"),
                                        PobierzWartośćParametru<string>("il_miesz"),
                                        PobierzWartośćParametru<string>("nazwa_pel"),
                                        PobierzWartośćParametru<string>("nazwa_skr"),
                                        PobierzWartośćParametru<string>("adres"),
                                        PobierzWartośćParametru<string>("adres_2"),
                                        PobierzWartośćParametru<string>("nr1_konta"),
                                        PobierzWartośćParametru<string>("nr2_konta"),
                                        PobierzWartośćParametru<string>("nr3_konta"),
                                        PobierzWartośćParametru<string>("sciezka_fk"),
                                        PobierzWartośćParametru<string>("uwagi")
                                    };

                            break;

                        case Enumeratory.Tabela.SkładnikiCzynszu:
                            record = new DostępDoBazy.SkładnikCzynszu();
                            nominativeCase = "składnik opłat";
                            genitiveCase = "składnika opłat";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("nazwa"),
                                        PobierzWartośćParametru<string>("rodz_e"),
                                        PobierzWartośćParametru<string>("s_zaplat"),
                                        PobierzWartośćParametru<string>("stawka"),
                                        PobierzWartośćParametru<string>("stawka_inf"),
                                        PobierzWartośćParametru<string>("typ_skl"),
                                        PobierzWartośćParametru<string>("data_1"),
                                        PobierzWartośćParametru<string>("data_2")
                                    };

                            if (recordFields[3] == "6")
                                recordFields = recordFields.ToList().Concat(new string[] 
                                        {
                                            PobierzWartośćParametru<string>("stawka_00"),
                                            PobierzWartośćParametru<string>("stawka_01"),
                                            PobierzWartośćParametru<string>("stawka_02"),
                                            PobierzWartośćParametru<string>("stawka_03"),
                                            PobierzWartośćParametru<string>("stawka_04"),
                                            PobierzWartośćParametru<string>("stawka_05"),
                                            PobierzWartośćParametru<string>("stawka_06"),
                                            PobierzWartośćParametru<string>("stawka_07"),
                                            PobierzWartośćParametru<string>("stawka_08"),
                                            PobierzWartośćParametru<string>("stawka_09")
                                        }).ToArray();
                            else
                                recordFields = recordFields.ToList().Concat(new string[] { "", "", "", "", "", "", "", "", "", "" }).ToArray();

                            break;

                        case Enumeratory.Tabela.TypyLokali:
                            record = new DostępDoBazy.TypLokalu();
                            nominativeCase = "typ lokali";
                            genitiveCase = "typu lokali";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("typ_lok")
                                    };

                            break;

                        case Enumeratory.Tabela.TypyKuchni:
                            record = new DostępDoBazy.TypKuchni();
                            nominativeCase = "typ kuchni";
                            genitiveCase = "typu kuchni";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("typ_kuch")
                                    };

                            break;

                        case Enumeratory.Tabela.RodzajeNajemcy:
                            record = new DostępDoBazy.TypNajemcy();
                            nominativeCase = "rodzaj najemcy";
                            genitiveCase = "rodzaju najemcy";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("r_najemcy")
                                    };

                            break;

                        case Enumeratory.Tabela.TytułyPrawne:
                            record = new DostępDoBazy.TytułPrawny();
                            nominativeCase = "tytuł prawny do lokali";
                            genitiveCase = "tytułu prawnego do lokali";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("tyt_prawny")
                                    };

                            break;

                        case Enumeratory.Tabela.TypyWpłat:
                            record = new DostępDoBazy.RodzajPłatności();
                            nominativeCase = "rodzaj wpłaty lub wypłaty";
                            genitiveCase = "rodzaju wpłaty lub wypłaty";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("typ_wplat"),
                                        PobierzWartośćParametru<string>("rodz_e"),
                                        PobierzWartośćParametru<string>("s_rozli"),
                                        PobierzWartośćParametru<string>("tn_odset"),
                                        PobierzWartośćParametru<string>("nota_odset"),
                                        PobierzWartośćParametru<string>("vat"),
                                        PobierzWartośćParametru<string>("sww")
                                    };

                            break;

                        case Enumeratory.Tabela.GrupySkładnikówCzynszu:
                            record = new DostępDoBazy.GrupaSkładnikówCzynszu();
                            nominativeCase = "grupa składników czynszu";
                            genitiveCase = "grupy składników czynszu";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("nazwa")
                                    };

                            break;

                        case Enumeratory.Tabela.GrupyFinansowe:
                            record = new DostępDoBazy.GrupaFinansowa();
                            nominativeCase = "grupa finansowa";
                            genitiveCase = "grupy finansowej";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("k_syn"),
                                        PobierzWartośćParametru<string>("nazwa")
                                    };

                            break;

                        case Enumeratory.Tabela.StawkiVat:
                            record = new DostępDoBazy.StawkaVat();
                            nominativeCase = "stawka VAT";
                            genitiveCase = "stawki VAt";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("nazwa"),
                                        PobierzWartośćParametru<string>("symb_fisk")
                                    };

                            break;

                        case Enumeratory.Tabela.Atrybuty:
                            record = new DostępDoBazy.Atrybut();
                            nominativeCase = "cecha obiektów";
                            genitiveCase = "cechy obiektów";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("nazwa"),
                                        PobierzWartośćParametru<string>("nr_str"),
                                        PobierzWartośćParametru<string>("jedn"),
                                        PobierzWartośćParametru<string>("wartosc"),
                                        PobierzWartośćParametru<string>("uwagi"),
                                        PobierzWartośćParametru<string>("zb_0"),
                                        PobierzWartośćParametru<string>("zb_1"),
                                        PobierzWartośćParametru<string>("zb_2"),
                                        PobierzWartośćParametru<string>("zb_3"),
                                    };

                            break;

                        case Enumeratory.Tabela.Użytkownicy:
                            record = new DostępDoBazy.Użytkownik();
                            nominativeCase = "użytkownik";
                            genitiveCase = "użytkownika";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("symbol"),
                                        PobierzWartośćParametru<string>("nazwisko"),
                                        PobierzWartośćParametru<string>("imie"),
                                        PobierzWartośćParametru<string>("haslo"),
                                        PobierzWartośćParametru<string>("haslo2")
                                    };

                            break;

                        case Enumeratory.Tabela.ObrotyNajemcy:
                            nominativeCase = "obrót najemcy";
                            genitiveCase = "obrotu najemcy";

                            recordFields = new string[]
                                    {
                                        PobierzWartośćParametru<string>("id"),
                                        PobierzWartośćParametru<string>("suma"),
                                        PobierzWartośćParametru<string>("data_obr"),
                                        PobierzWartośćParametru<string>("?"),
                                        PobierzWartośćParametru<string>("kod_wplat"),
                                        PobierzWartośćParametru<string>("nr_dowodu"),
                                        PobierzWartośćParametru<string>("pozycja_d"),
                                        PobierzWartośćParametru<string>("uwagi"),
                                        PobierzWartośćParametru<string>("nr_kontr")
                                    };

                            switch (Hello.CurrentSet)
                            {
                                case Enumeratory.Zbiór.Czynsze:
                                    record = new DostępDoBazy.ObrótZPierwszegoZbioru();

                                    break;

                                case Enumeratory.Zbiór.Drugi:
                                    record = new DostępDoBazy.ObrótZDrugiegoZbioru();

                                    break;

                                case Enumeratory.Zbiór.Trzeci:
                                    record = new DostępDoBazy.ObrótZTrzeciegoZbioru();

                                    break;
                            }

                            backUrl = backUrl.Insert(backUrl.LastIndexOf('\''), "&id=" + recordFields[8]);

                            break;
                    }

                    validationResult = record.Waliduj(action, recordFields);

                    if (String.IsNullOrEmpty(validationResult))
                    {
                        DbSet dbSet = db.Set(record.GetType());
                        DbSet dbSetOfAttributes = null;

                        if (attributeType != null)
                            dbSetOfAttributes = db.Set(attributeType);

                        switch (action)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                record.Ustaw(recordFields);
                                dbSet.Add(record);

                                if (dbSetOfAttributes != null)
                                    foreach (DostępDoBazy.AtrybutObiektu attribute in attributesOfObject)
                                    {
                                        attribute.kod_powiaz = recordFields[0];

                                        dbSetOfAttributes.Add(attribute);
                                    }

                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywneLokale:
                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in rentComponentsOfPlace)
                                        {
                                            rentComponentOfPlace.kod_lok = Int32.Parse(recordFields[1]);
                                            rentComponentOfPlace.nr_lok = Int32.Parse(recordFields[2]);

                                            db.SkładnikiCzynszuLokalu.Add(rentComponentOfPlace);
                                        }

                                        //
                                        // CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                        //
                                        // TO DUMP INTO UNDERDARK
                                        //

                                        break;

                                    case Enumeratory.Tabela.Wspólnoty:
                                        foreach(DostępDoBazy.BudynekWspólnoty communityBuilding in communityBuildings)
                                        {
                                            communityBuilding.kod = Int32.Parse(recordFields[0]);

                                            db.BudynkiWspólnot.Add(communityBuilding);
                                        }

                                        break;
                                }

                                break;

                            case Enumeratory.Akcja.Edytuj:
                                record = (DostępDoBazy.IRekord)dbSet.Find(id);

                                record.Ustaw(recordFields);

                                if (dbSetOfAttributes != null)
                                {
                                    foreach (DostępDoBazy.AtrybutObiektu attributeOfObject in dbSetOfAttributes.ToListAsync().Result.Cast<DostępDoBazy.AtrybutObiektu>().Where(a => a.kod_powiaz.Trim() == id.ToString()))
                                        dbSetOfAttributes.Remove(attributeOfObject);

                                    foreach (DostępDoBazy.AtrybutObiektu attributeOfObject in attributesOfObject)
                                        dbSetOfAttributes.Add(attributeOfObject);
                                }

                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywneLokale:
                                        DostępDoBazy.Lokal place = (DostępDoBazy.Lokal)record;

                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                            db.SkładnikiCzynszuLokalu.Remove(rentComponentOfPlace);

                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in rentComponentsOfPlace)
                                            db.SkładnikiCzynszuLokalu.Add(rentComponentOfPlace);

                                        //
                                        // CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + recordFields[0]);
                                        db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                        //
                                        // TO DUMP INTO UNDERDARK
                                        //

                                        break;

                                    case Enumeratory.Tabela.Wspólnoty:
                                        DostępDoBazy.Wspólnota community = (DostępDoBazy.Wspólnota)record;

                                        foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in db.BudynkiWspólnot.Where(c => c.kod == community.kod))
                                            db.BudynkiWspólnot.Remove(communityBuilding);

                                        foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in communityBuildings)
                                            db.BudynkiWspólnot.Add(communityBuilding);

                                        break;
                                }

                                break;

                            case Enumeratory.Akcja.Usuń:
                                record = (DostępDoBazy.IRekord)dbSet.Find(id);

                                dbSet.Remove(record);

                                if (dbSetOfAttributes != null)
                                    foreach (DostępDoBazy.AtrybutObiektu attributeOfObject in dbSetOfAttributes.ToListAsync().Result.Cast<DostępDoBazy.AtrybutObiektu>().Where(a => a.kod_powiaz.Trim() == id.ToString()))
                                        dbSetOfAttributes.Remove(attributeOfObject);

                                switch (table)
                                {
                                    case Enumeratory.Tabela.AktywneLokale:
                                        DostępDoBazy.Lokal place = (DostępDoBazy.Lokal)record;

                                        foreach (DostępDoBazy.SkładnikCzynszuLokalu component in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                            db.SkładnikiCzynszuLokalu.Remove(component);

                                        //
                                        // CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + recordFields[0]);
                                        //
                                        // TO DUMP INTO UNDERDARK
                                        //

                                        break;

                                    case Enumeratory.Tabela.Wspólnoty:
                                        DostępDoBazy.Wspólnota community = (DostępDoBazy.Wspólnota)record;

                                        foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in db.BudynkiWspólnot.Where(c => c.kod == community.kod))
                                            db.BudynkiWspólnot.Remove(communityBuilding);

                                        break;
                                }

                                break;

                            case Enumeratory.Akcja.Przenieś:
                                DostępDoBazy.IRekord inactive = (DostępDoBazy.IRekord)Activator.CreateInstance(inactiveType);
                                record = (DostępDoBazy.IRekord)dbSet.Find(id);

                                dbSet.Remove(record);
                                inactive.Ustaw(record.WszystkiePola());
                                db.Set(inactiveType).Add(inactive);

                                break;
                        }
                    }

                    if (String.IsNullOrEmpty(validationResult))
                    {

                        db.SaveChanges();

                        if (!String.IsNullOrEmpty(nominativeCase))
                            dbWriteResult = Char.ToUpper(nominativeCase[0]) + nominativeCase.Substring(1) + " " + dictionaryOfActionParticiples[action] + ".";
                    }
                }
            }
            catch (Exception exception) { dbWriteResult = "Nie można " + dictionaryOfActionInfinitives[action] + " " + genitiveCase + "! " + exception.Message; }

            Title = "Edycja " + genitiveCase;

            placeOfMessage.Controls.Add(new LiteralControl(validationResult));

            if (!String.IsNullOrEmpty(dbWriteResult))
                placeOfMessage.Controls.Add(new LiteralControl(dbWriteResult + "<br />"));

            if (!String.IsNullOrEmpty(validationResult) || (!String.IsNullOrEmpty(dbWriteResult) && dbWriteResult.Contains("!")))
            {
                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Repair", "Popraw", "Record.aspx"));
                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Cancel", "Anuluj", backUrl));

                Session["values"] = recordFields;
            }
            else
                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Back", "Powrót", backUrl));
        }
    }
}