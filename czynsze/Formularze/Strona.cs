using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;

namespace czynsze.Formularze
{
    public abstract class Strona : System.Web.UI.Page
    {
        protected static ŚcieżkaStrony ŚcieżkaStrony { get; set; }
        protected string ŚcieżkaIQuery { get { return Request.Url.PathAndQuery; } }

        protected T PobierzWartośćParametru<T>(string klucz)
        {
            string wartość = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith(klucz))];
            Type typ = typeof(T);

            if (wartość == null)
                return default(T);

            if (typ.IsEnum)
                return (T)Enum.Parse(typ, wartość);
            else
                return (T)Convert.ChangeType(wartość, typ);
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
                Lista<string[]> lokaleDoListy = new List<string[]>();

                foreach (DostępDoBazy.AktywnyLokal lokal in lokale)
                {
                    string id = String.Format("{0}-{1}", lokal.kod_lok, lokal.nr_lok);

                    lokaleDoListy.Add(new string[] { id, id, lokal.adres, lokal.adres_2 });
                }

                pojemnik.Controls.Add(new Kontrolki.Label("label", "odLokalu", "Pierwszy lokal: ", String.Empty));
                pojemnik.Controls.Add(new Kontrolki.DropDownList("field", "odLokalu", lokaleDoListy, String.Format("{0}-{1}", minimalnyBudynek, minimalnyLokal), true, false));
                DodajNowąLinię(pojemnik);
                pojemnik.Controls.Add(new Kontrolki.Label("label", "doLokalu", "Ostatni lokal: ", String.Empty));
                pojemnik.Controls.Add(new Kontrolki.DropDownList("field", "doLokalu", lokaleDoListy, String.Format("{0}-{1}", maksymalnyBudynek, maksymalnyLokal), true, false));
            }
        }

        public static void DodajWybórLokaliBudynkówIWspólnot(Control pojemnikLokali, Control pojemnikBudynków, Control pojemnikWspólnot, out int minimalnyBudynek, out int minimalnyLokal, out int maksymalnyBudynek, out int maksymalnyLokal, out int minimalnaWspólnota, out int maksymalnaWspólnota)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Lista<string[]> budynki = db.Budynki.AsEnumerable<DostępDoBazy.Budynek>().OrderBy(b => b.kod_1).Select(b => new string[] { b.kod_1.ToString(), b.kod_1.ToString(), b.adres, b.adres_2 }).ToList();
                Lista<string[]> wspólnoty = db.Wspólnoty.AsEnumerable<DostępDoBazy.Wspólnota>().OrderBy(w => w.kod).Select(w => new string[] { w.kod.ToString(), w.kod.ToString(), w.nazwa_skr, w.adres, w.adres_2 }).ToList();
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
                pojemnikBudynków.Controls.Add(new Kontrolki.DropDownList("field", "odBudynku", budynki, minimalnyBudynek.ToString(), true, false));
                DodajNowąLinię(pojemnikBudynków);
                pojemnikBudynków.Controls.Add(new Kontrolki.Label("label", "doBudynku", "Ostatni budynek: ", String.Empty));
                pojemnikBudynków.Controls.Add(new Kontrolki.DropDownList("field", "doBudynku", budynki, maksymalnyBudynek.ToString(), true, false));
                pojemnikBudynków.Controls.Add(new Kontrolki.HtmlInputHidden("minimalnyBudynek", minimalnyBudynek.ToString()));
                pojemnikBudynków.Controls.Add(new Kontrolki.HtmlInputHidden("maksymalnyBudynek", maksymalnyBudynek.ToString()));

                pojemnikWspólnot.Controls.Add(new Kontrolki.Button("button", "wszystkieWspólnotyWybór", "Zestawienie wszystkich wspólnot", String.Empty));
                DodajNowąLinię(pojemnikWspólnot);
                pojemnikWspólnot.Controls.Add(new Kontrolki.Button("button", "odDoWspólnotyWybór", "Od-do żądanej wspólnoty", String.Empty));
                DodajNowąLinię(pojemnikWspólnot);
                pojemnikWspólnot.Controls.Add(new Kontrolki.Label("label", "odWspólnoty", "Pierwsza wspólnota: ", String.Empty));
                pojemnikWspólnot.Controls.Add(new Kontrolki.DropDownList("field", "odWspólnoty", wspólnoty, minimalnaWspólnota.ToString(), true, false));
                DodajNowąLinię(pojemnikWspólnot);
                pojemnikWspólnot.Controls.Add(new Kontrolki.Label("label", "doWspólnoty", "Ostatnia wspólnota: ", String.Empty));
                pojemnikWspólnot.Controls.Add(new Kontrolki.DropDownList("field", "doWspólnoty", wspólnoty, maksymalnaWspólnota.ToString(), true, false));
                pojemnikWspólnot.Controls.Add(new Kontrolki.HtmlInputHidden("minimalnaWspólnota", minimalnaWspólnota.ToString()));
                pojemnikWspólnot.Controls.Add(new Kontrolki.HtmlInputHidden("maksymalnaWspólnota", maksymalnaWspólnota.ToString()));
            }
        }
    }
}