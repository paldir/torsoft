using System;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;

namespace Odpady.DostępDoDanych
{
    public class Szpieg : Rekord
    {
        public string DATA_IN { get; set; }
        public string GODZINA_IN { get; set; }
        public string DATA_OUT { get; set; }
        public string GODZINA_OUT { get; set; }
        public string UZYTKOWNIK { get; set; }
        public string UWAGI { get; set; }
        public string STACJA { get; set; }

        public Szpieg(string użytkownik, string uwagi = null)
        {
            UZYTKOWNIK = użytkownik;
            UWAGI = uwagi;
            STACJA = Environment.MachineName;

            In();
        }

        public void Out()
        {
            DateTime teraz = DateTime.Now;
            DATA_OUT = teraz.ToShortDateString();
            GODZINA_OUT = teraz.ToShortTimeString();

            PołączenieDlaObcychObiektów.Aktualizuj(this);
        }

        void In()
        {
            DateTime teraz = DateTime.Now;
            DATA_IN = teraz.ToShortDateString();
            GODZINA_IN = teraz.ToShortTimeString();

            PołączenieDlaObcychObiektów.Dodaj(this);
        }

        public Użytkownik PobierzUżytkownika()
        {
            return PołączenieDlaObcychObiektów.PobierzWszystkie<Użytkownik>().SingleOrDefault(u => u.UZYTKOWNIK == UZYTKOWNIK);
        }
    }
}