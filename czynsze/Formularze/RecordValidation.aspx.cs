using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;
using System.Reflection;

namespace czynsze.Formularze
{
    public partial class RecordValidation : Strona
    {
        Enumeratory.Tabela table;
        Enumeratory.Akcja action;
        int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                //string[] recordFields = null;
                //string validationResult = null;
                string dbWriteResult = null;
                table = PobierzWartośćParametru<Enumeratory.Tabela>("table");
                action = PobierzWartośćParametru<Enumeratory.Akcja>("action");
                string backUrl = "javascript: Load('Lista.aspx?table=" + table + "')";
                string nominativeCase = String.Empty;
                string genitiveCase = String.Empty;
                DostępDoBazy.Rekord record = null;
                Type typRekordu = null;
                Type attributeType = null;
                Type inactiveType = null;
                Type typPlików = null;
                List<string> nazwyPólZProblemami = null;
                string validationResult = null;

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
                    id = PobierzWartośćParametru<int>("id");

                    form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));
                }

                form.Controls.Add(new Kontrolki.HtmlInputHidden("table", table.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("action", action.ToString()));

                try
                {
                    {
                        switch (table)
                        {
                            case Enumeratory.Tabela.Budynki:
                                typRekordu = typeof(DostępDoBazy.Budynek);
                                attributeType = typeof(DostępDoBazy.AtrybutBudynku);
                                typPlików = typeof(DostępDoBazy.PlikBudynku);
                                nominativeCase = "budynek";
                                genitiveCase = "budynku";

                                break;

                            case Enumeratory.Tabela.AktywneLokale:
                                typRekordu = typeof(DostępDoBazy.AktywnyLokal);
                                attributeType = typeof(DostępDoBazy.AtrybutLokalu);
                                typPlików = typeof(DostępDoBazy.PlikLokalu);
                                inactiveType = typeof(DostępDoBazy.NieaktywnyLokal);
                                nominativeCase = "lokal";
                                genitiveCase = "lokalu";

                                break;

                            case Enumeratory.Tabela.NieaktywneLokale:
                                typRekordu = typeof(DostępDoBazy.NieaktywnyLokal);
                                inactiveType = typeof(DostępDoBazy.AktywnyLokal);
                                typPlików = typeof(DostępDoBazy.PlikLokalu);
                                nominativeCase = "lokal (nieaktywny)";
                                genitiveCase = "lokalu (nieaktywnego)";

                                break;

                            case Enumeratory.Tabela.AktywniNajemcy:
                                typRekordu = typeof(DostępDoBazy.AktywnyNajemca);
                                attributeType = typeof(DostępDoBazy.AtrybutNajemcy);
                                inactiveType = typeof(DostępDoBazy.NieaktywnyNajemca);
                                nominativeCase = "najemca";
                                genitiveCase = "najemcy";

                                break;

                            case Enumeratory.Tabela.NieaktywniNajemcy:
                                typRekordu = typeof(DostępDoBazy.NieaktywnyNajemca);
                                inactiveType = typeof(DostępDoBazy.AktywnyNajemca);
                                nominativeCase = "najemca (nieaktywny)";
                                genitiveCase = "najemcy (nieaktywnego)";

                                break;

                            case Enumeratory.Tabela.Wspolnoty:
                                typRekordu = typeof(DostępDoBazy.Wspólnota);
                                attributeType = typeof(DostępDoBazy.AtrybutWspólnoty);
                                nominativeCase = "wspólnota";
                                genitiveCase = "wspólnoty";

                                break;

                            case Enumeratory.Tabela.SkladnikiCzynszu:
                                typRekordu = typeof(DostępDoBazy.SkładnikCzynszu);
                                nominativeCase = "składnik opłat";
                                genitiveCase = "składnika opłat";

                                break;

                            case Enumeratory.Tabela.TypyLokali:
                                typRekordu = typeof(DostępDoBazy.TypLokalu);
                                nominativeCase = "typ lokali";
                                genitiveCase = "typu lokali";

                                break;

                            case Enumeratory.Tabela.TypyKuchni:
                                typRekordu = typeof(DostępDoBazy.TypKuchni);
                                nominativeCase = "typ kuchni";
                                genitiveCase = "typu kuchni";

                                break;

                            case Enumeratory.Tabela.RodzajeNajemcy:
                                typRekordu = typeof(DostępDoBazy.TypNajemcy);
                                nominativeCase = "rodzaj najemcy";
                                genitiveCase = "rodzaju najemcy";

                                break;

                            case Enumeratory.Tabela.TytulyPrawne:
                                typRekordu = typeof(DostępDoBazy.TytułPrawny);
                                nominativeCase = "tytuł prawny do lokali";
                                genitiveCase = "tytułu prawnego do lokali";

                                break;

                            case Enumeratory.Tabela.TypyWplat:
                                typRekordu = typeof(DostępDoBazy.RodzajPłatności);
                                nominativeCase = "rodzaj wpłaty lub wypłaty";
                                genitiveCase = "rodzaju wpłaty lub wypłaty";

                                break;

                            case Enumeratory.Tabela.GrupySkładnikowCzynszu:
                                typRekordu = typeof(DostępDoBazy.GrupaSkładnikówCzynszu);
                                nominativeCase = "grupa składników czynszu";
                                genitiveCase = "grupy składników czynszu";

                                break;

                            case Enumeratory.Tabela.GrupyFinansowe:
                                typRekordu = typeof(DostępDoBazy.GrupaFinansowa);
                                nominativeCase = "grupa finansowa";
                                genitiveCase = "grupy finansowej";

                                break;

                            case Enumeratory.Tabela.StawkiVat:
                                typRekordu = typeof(DostępDoBazy.StawkaVat);
                                nominativeCase = "stawka VAT";
                                genitiveCase = "stawki VAt";

                                break;

                            case Enumeratory.Tabela.Atrybuty:
                                typRekordu = typeof(DostępDoBazy.Atrybut);
                                nominativeCase = "cecha obiektów";
                                genitiveCase = "cechy obiektów";

                                break;

                            case Enumeratory.Tabela.Uzytkownicy:
                                typRekordu = typeof(DostępDoBazy.Użytkownik);
                                nominativeCase = "użytkownik";
                                genitiveCase = "użytkownika";

                                break;

                            case Enumeratory.Tabela.ObrotyNajemcy:
                                nominativeCase = "obrót najemcy";
                                genitiveCase = "obrotu najemcy";

                                switch (Start.AktywnyZbiór)
                                {
                                    case Enumeratory.Zbiór.Czynsze:
                                        typRekordu = typeof(DostępDoBazy.Obrót1);

                                        break;

                                    case Enumeratory.Zbiór.Drugi:
                                        typRekordu = typeof(DostępDoBazy.Obrót2);

                                        break;

                                    case Enumeratory.Zbiór.Trzeci:
                                        typRekordu = typeof(DostępDoBazy.Obrót3);

                                        break;
                                }

                                throw new NotImplementedException("Brak adresu zwrotnego.");

                            //backUrl = backUrl.Insert(backUrl.LastIndexOf('\''), "&id=" + recordFields[8]);
                        }

                        DbSet dbSet = db.Set(typRekordu);
                        DbSet dbSetOfAttributes = null;
                        DbSet zbiórPlików = null;

                        if (attributeType != null)
                            dbSetOfAttributes = db.Set(attributeType);

                        if (typPlików != null)
                            zbiórPlików = db.Set(typPlików);

                        switch (action)
                        {
                            case Enumeratory.Akcja.Dodaj:
                                record = Activator.CreateInstance(typRekordu) as DostępDoBazy.Rekord;

                                break;

                            case Enumeratory.Akcja.Edytuj:
                            case Enumeratory.Akcja.Usuń:
                            case Enumeratory.Akcja.Przenieś:
                                record = dbSet.Find(id) as DostępDoBazy.Rekord;

                                break;
                        }

                        switch (action)
                        {
                            case Enumeratory.Akcja.Dodaj:
                            case Enumeratory.Akcja.Edytuj:
                                nazwyPólZProblemami = UstawParametry(record);

                                break;
                        }

                        if (nazwyPólZProblemami.Any())
                            validationResult = String.Join(" ", nazwyPólZProblemami);
                        else
                        {
                            switch (action)
                            {
                                case Enumeratory.Akcja.Dodaj:
                                    dbSet.Add(record);
                                    db.SaveChanges();

                                    if (dbSetOfAttributes != null)
                                        foreach (DostępDoBazy.AtrybutObiektu attribute in WartościSesji.AtrybutyObiektu)
                                        {
                                            attribute.kod_powiaz = record.__record.ToString();

                                            dbSetOfAttributes.Add(attribute);
                                        }

                                    if (zbiórPlików != null)
                                        foreach (DostępDoBazy.Plik plik in WartościSesji.Pliki)
                                        {
                                            plik.id_obiektu = record.__record;

                                            zbiórPlików.Add(plik);
                                        }

                                    switch (table)
                                    {
                                        case Enumeratory.Tabela.AktywneLokale:
                                            DostępDoBazy.AktywnyLokal lokal = record as DostępDoBazy.AktywnyLokal;

                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in WartościSesji.SkładnikiCzynszuLokalu)
                                            {
                                                rentComponentOfPlace.kod_lok = lokal.kod_lok;
                                                rentComponentOfPlace.nr_lok = lokal.nr_lok;

                                                db.SkładnikiCzynszuLokalu.Add(rentComponentOfPlace);
                                            }

                                            break;

                                        case Enumeratory.Tabela.Wspolnoty:
                                            DostępDoBazy.Wspólnota wspólnota = record as DostępDoBazy.Wspólnota;

                                            foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in WartościSesji.BudynkiWspólnoty)
                                            {
                                                communityBuilding.kod = wspólnota.kod;

                                                db.BudynkiWspólnot.Add(communityBuilding);
                                            }

                                            break;
                                    }

                                    break;

                                case Enumeratory.Akcja.Edytuj:
                                    if (dbSetOfAttributes != null)
                                    {
                                        DostępDoBazy.AtrybutLokalu.Lokale = db.AktywneLokale.ToList();
                                        DostępDoBazy.AtrybutBudynku.Budynki = db.Budynki.ToList();
                                        DostępDoBazy.AtrybutNajemcy.Najemcy = db.AktywniNajemcy.ToList();
                                        DostępDoBazy.AtrybutWspólnoty.Wspólnoty = db.Wspólnoty.ToList();

                                        foreach (DostępDoBazy.AtrybutObiektu attributeOfObject in dbSetOfAttributes.ToListAsync().Result.Cast<DostępDoBazy.AtrybutObiektu>().Where(a => a.kod_powiaz.Trim() == id.ToString()))
                                            dbSetOfAttributes.Remove(attributeOfObject);

                                        foreach (DostępDoBazy.AtrybutObiektu attributeOfObject in WartościSesji.AtrybutyObiektu)
                                            dbSetOfAttributes.Add(attributeOfObject);

                                        foreach (DostępDoBazy.Plik plik in zbiórPlików.ToListAsync().Result.Cast<DostępDoBazy.Plik>().Where(p => p.id_obiektu == record.__record))
                                            zbiórPlików.Remove(plik);

                                        foreach (DostępDoBazy.Plik plik in WartościSesji.Pliki)
                                            zbiórPlików.Add(plik);
                                    }

                                    switch (table)
                                    {
                                        case Enumeratory.Tabela.AktywneLokale:
                                            DostępDoBazy.Lokal place = (DostępDoBazy.Lokal)record;

                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                                db.SkładnikiCzynszuLokalu.Remove(rentComponentOfPlace);

                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace in WartościSesji.SkładnikiCzynszuLokalu)
                                                db.SkładnikiCzynszuLokalu.Add(rentComponentOfPlace);

                                            break;

                                        case Enumeratory.Tabela.Wspolnoty:
                                            DostępDoBazy.Wspólnota community = (DostępDoBazy.Wspólnota)record;

                                            foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in db.BudynkiWspólnot.Where(c => c.kod == community.kod))
                                                db.BudynkiWspólnot.Remove(communityBuilding);

                                            foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in WartościSesji.BudynkiWspólnoty)
                                                db.BudynkiWspólnot.Add(communityBuilding);

                                            break;
                                    }

                                    break;

                                case Enumeratory.Akcja.Usuń:
                                    dbSet.Remove(record);

                                    /*if (dbSetOfAttributes != null)
                                        foreach (DostępDoBazy.AtrybutObiektu attributeOfObject in dbSetOfAttributes.ToListAsync().Result.Cast<DostępDoBazy.AtrybutObiektu>().Where(a => a.kod_powiaz.Trim() == id.ToString()))
                                            dbSetOfAttributes.Remove(attributeOfObject);

                                    switch (table)
                                    {
                                        case Enumeratory.Tabela.AktywneLokale:
                                            DostępDoBazy.Lokal place = (DostępDoBazy.Lokal)record;

                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu component in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                                db.SkładnikiCzynszuLokalu.Remove(component);

                                            break;

                                        case Enumeratory.Tabela.Wspolnoty:
                                            DostępDoBazy.Wspólnota community = (DostępDoBazy.Wspólnota)record;

                                            foreach (DostępDoBazy.BudynekWspólnoty communityBuilding in db.BudynkiWspólnot.Where(c => c.kod == community.kod))
                                                db.BudynkiWspólnot.Remove(communityBuilding);

                                            break;
                                    }*/

                                    break;

                                case Enumeratory.Akcja.Przenieś:
                                    DostępDoBazy.Rekord inactive = (DostępDoBazy.Rekord)Activator.CreateInstance(inactiveType);

                                    dbSet.Remove(record);
                                    //inactive.Ustaw(record.WszystkiePola());
                                    db.Set(inactiveType).Add(inactive);

                                    break;

                            }

                            if (db.ChangeTracker.HasChanges())
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
                    placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Repair", "Popraw", "Rekord.aspx"));
                    placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Cancel", "Anuluj", backUrl));

                    //Session["values"] = recordFields;
                }
                else
                    placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Back", "Powrót", backUrl));

                DostępDoBazy.AtrybutLokalu.Lokale = null;
                DostępDoBazy.AtrybutBudynku.Budynki = null;
                DostępDoBazy.AtrybutNajemcy.Najemcy = null;
                DostępDoBazy.AtrybutWspólnoty.Wspólnoty = null;
            }
        }

        List<string> UstawParametry(DostępDoBazy.Rekord rekord)
        {
            IEnumerable<PropertyInfo> właściwości = rekord.GetType().GetProperties().Where(p => p.GetSetMethod() != null);
            List<string> nazwyPólZProblemami = new List<string>();

            foreach (PropertyInfo właściwość in właściwości)
                try
                {
                    właściwość.SetValue(rekord, PobierzWartośćParametru(właściwość.Name, właściwość.PropertyType));
                }
                catch (InvalidCastException)
                {
                    nazwyPólZProblemami.Add(właściwość.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name);
                }

            return nazwyPólZProblemami;
        }
    }
}