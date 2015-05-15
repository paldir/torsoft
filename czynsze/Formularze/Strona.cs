using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Formularze
{
    public class Strona : System.Web.UI.Page
    {
        public T PobierzWartośćParametru<T>(string klucz)
        {
            string wartość = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith(klucz))];

            if (wartość == null)
                return default(T);

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), wartość);
            else
                return (T)Convert.ChangeType(wartość, typeof(T));
        }

        public static void DodajNowąLinię(System.Web.UI.Control pojemnik)
        {
            pojemnik.Controls.Add(new System.Web.UI.LiteralControl("<br />"));
        }

        public static void DodajWybórLokali(System.Web.UI.Control pojemnik, out int minimalnyBudynek, out int minimalnyLokal, out int maksymalnyBudynek, out int maksymalnyLokal)
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
                pojemnik.Controls.Add(new Kontrolki.DropDownList("field", "odLokalu", lokaleDoListy, String.Format("{0}-{1}", minimalnyBudynek, minimalnyLokal), true, false));
                DodajNowąLinię(pojemnik);
                pojemnik.Controls.Add(new Kontrolki.Label("label", "doLokalu", "Ostatni lokal: ", String.Empty));
                pojemnik.Controls.Add(new Kontrolki.DropDownList("field", "doLokalu", lokaleDoListy, String.Format("{0}-{1}", maksymalnyBudynek, maksymalnyLokal), true, false));
            }
        }
    }
}