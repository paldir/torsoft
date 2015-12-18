﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;

namespace czynsze.Formularze
{
    public abstract class Strona : System.Web.UI.Page
    {
        protected string ŚcieżkaIQuery { get { return Request.Url.PathAndQuery; } }
        public static Sesja WartościSesji { get { return Sesja.Obecna; } }

        protected string PobierzWartośćParametru(string klucz)
        {
            string prawdziwyKlucz = Request.Params.AllKeys.SingleOrDefault(k => k.EndsWith(klucz));

            if (!String.IsNullOrEmpty(prawdziwyKlucz) && prawdziwyKlucz.Contains('$'))
                throw new Exception("Błąd klucza parametru. Zawiera dolary. - PZ");

            return Request.Params[klucz];
        }
        
        protected T PobierzWartośćParametru<T>(string klucz)
        {
            return (T)PobierzWartośćParametru(klucz, typeof(T));
        }

        protected object PobierzWartośćParametru(string klucz, Type zwracanyTyp)
        {
            string wartość = PobierzWartośćParametru(klucz);
            Type typNullable = Nullable.GetUnderlyingType(zwracanyTyp);

            if (String.IsNullOrEmpty(wartość))
            {
                if (zwracanyTyp.IsValueType)
                    return Activator.CreateInstance(zwracanyTyp);
                else
                    return null;
            }

            if (zwracanyTyp.IsEnum)
                return Enum.Parse(zwracanyTyp, wartość.Replace(" ", String.Empty), true);
            else if (typNullable != null)
                return Convert.ChangeType(wartość, typNullable);
            else
                return Convert.ChangeType(wartość, zwracanyTyp);
        }

        public static void DodajNowąLinię(Control pojemnik)
        {
            pojemnik.Controls.Add(new LiteralControl("<br />"));
        }

        public static void DodajWybórLokali(Control pojemnik, out int minimalnyBudynek, out int minimalnyLokal, out int maksymalnyBudynek, out int maksymalnyLokal)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                IEnumerable<DostępDoBazy.AktywnyLokal> lokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok);
                DostępDoBazy.AktywnyLokal pierwszyLokal = lokale.First();
                DostępDoBazy.AktywnyLokal ostatniLokal = lokale.Last();
                minimalnyBudynek = pierwszyLokal.kod_lok;
                minimalnyLokal = pierwszyLokal.nr_lok;
                maksymalnyBudynek = ostatniLokal.kod_lok;
                maksymalnyLokal = ostatniLokal.nr_lok;
                List<string[]> lokaleDoListy = new List<string[]>();

                foreach (DostępDoBazy.AktywnyLokal lokal in lokale)
                {
                    string id = String.Format("{0}-{1}", lokal.kod_lok, lokal.nr_lok);

                    lokaleDoListy.Add(new string[] { id, id, lokal.adres, lokal.adres_2 });
                }

                pojemnik.Controls.Add(new Kontrolki.Label("label", "odLokalu", "Pierwszy lokal: ", String.Empty));
                pojemnik.Controls.Add(new Kontrolki.DropDownList("field", "odLokalu", lokaleDoListy, true, false, String.Format("{0}-{1}", minimalnyBudynek, minimalnyLokal)));
                DodajNowąLinię(pojemnik);
                pojemnik.Controls.Add(new Kontrolki.Label("label", "doLokalu", "Ostatni lokal: ", String.Empty));
                pojemnik.Controls.Add(new Kontrolki.DropDownList("field", "doLokalu", lokaleDoListy, true, false, String.Format("{0}-{1}", maksymalnyBudynek, maksymalnyLokal)));
            }
        }

        public static void DodajWybórLokaliBudynkówIWspólnot(Control pojemnikLokali, Control pojemnikBudynków, Control pojemnikWspólnot, out int minimalnyBudynek, out int minimalnyLokal, out int maksymalnyBudynek, out int maksymalnyLokal, out int minimalnaWspólnota, out int maksymalnaWspólnota)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                List<string[]> budynki = db.Budynki.AsEnumerable<DostępDoBazy.Budynek>().OrderBy(b => b.kod_1).Select(b => new string[] { b.kod_1.ToString(), b.kod_1.ToString(), b.adres, b.adres_2 }).ToList();
                List<string[]> wspólnoty = db.Wspólnoty.AsEnumerable<DostępDoBazy.Wspólnota>().OrderBy(w => w.kod).Select(w => new string[] { w.kod.ToString(), w.kod.ToString(), w.nazwa_skr, w.adres, w.adres_2 }).ToList();
                minimalnaWspólnota = (db.Wspólnoty.Any() ? db.Wspólnoty.Min(c => c.kod) : 0);
                maksymalnaWspólnota = (db.Wspólnoty.Any() ? db.Wspólnoty.Max(c => c.kod) : 0);

                pojemnikLokali.Controls.Add(new Kontrolki.Button("button", "wszystkieLokaleWybór", "Zestawienie wszystkich lokali", String.Empty));
                DodajNowąLinię(pojemnikLokali);
                pojemnikLokali.Controls.Add(new Kontrolki.Button("button", "odDoLokaluWybór", "Od-do żądanego lokalu", String.Empty));
                DodajNowąLinię(pojemnikLokali);
                DodajWybórLokali(pojemnikLokali, out minimalnyBudynek, out minimalnyLokal, out maksymalnyBudynek, out maksymalnyLokal);
                pojemnikLokali.Controls.Add(new Kontrolki.HtmlInputHidden("minimalnyLokal", minimalnyLokal.ToString()));
                pojemnikLokali.Controls.Add(new Kontrolki.HtmlInputHidden("maksymalnyLokal", maksymalnyLokal.ToString()));

                pojemnikBudynków.Controls.Add(new Kontrolki.Button("button", "wszystkieBudynkiWybór", "Zestawienie wszystkich budynków", String.Empty));
                DodajNowąLinię(pojemnikBudynków);
                pojemnikBudynków.Controls.Add(new Kontrolki.Button("button", "odDoBudynkuWybór", "Od-do żądanego budynku", String.Empty));
                DodajNowąLinię(pojemnikBudynków);
                pojemnikBudynków.Controls.Add(new Kontrolki.Label("label", "odBudynku", "Pierwszy budynek: ", String.Empty));
                pojemnikBudynków.Controls.Add(new Kontrolki.DropDownList("field", "odBudynku", budynki, true, false, minimalnyBudynek.ToString()));
                DodajNowąLinię(pojemnikBudynków);
                pojemnikBudynków.Controls.Add(new Kontrolki.Label("label", "doBudynku", "Ostatni budynek: ", String.Empty));
                pojemnikBudynków.Controls.Add(new Kontrolki.DropDownList("field", "doBudynku", budynki, true, false, maksymalnyBudynek.ToString()));
                pojemnikBudynków.Controls.Add(new Kontrolki.HtmlInputHidden("minimalnyBudynek", minimalnyBudynek.ToString()));
                pojemnikBudynków.Controls.Add(new Kontrolki.HtmlInputHidden("maksymalnyBudynek", maksymalnyBudynek.ToString()));

                pojemnikWspólnot.Controls.Add(new Kontrolki.Button("button", "wszystkieWspólnotyWybór", "Zestawienie wszystkich wspólnot", String.Empty));
                DodajNowąLinię(pojemnikWspólnot);
                pojemnikWspólnot.Controls.Add(new Kontrolki.Button("button", "odDoWspólnotyWybór", "Od-do żądanej wspólnoty", String.Empty));
                DodajNowąLinię(pojemnikWspólnot);
                pojemnikWspólnot.Controls.Add(new Kontrolki.Label("label", "odWspólnoty", "Pierwsza wspólnota: ", String.Empty));
                pojemnikWspólnot.Controls.Add(new Kontrolki.DropDownList("field", "odWspólnoty", wspólnoty, true, false, minimalnaWspólnota.ToString()));
                DodajNowąLinię(pojemnikWspólnot);
                pojemnikWspólnot.Controls.Add(new Kontrolki.Label("label", "doWspólnoty", "Ostatnia wspólnota: ", String.Empty));
                pojemnikWspólnot.Controls.Add(new Kontrolki.DropDownList("field", "doWspólnoty", wspólnoty, true, false, maksymalnaWspólnota.ToString()));
                pojemnikWspólnot.Controls.Add(new Kontrolki.HtmlInputHidden("minimalnaWspólnota", minimalnaWspólnota.ToString()));
                pojemnikWspólnot.Controls.Add(new Kontrolki.HtmlInputHidden("maksymalnaWspólnota", maksymalnaWspólnota.ToString()));
            }
        }
    }
}