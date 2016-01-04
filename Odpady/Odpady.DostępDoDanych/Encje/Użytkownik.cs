using System;
using System.Linq;
using System.Collections.Generic;

using System.Text;

namespace Odpady.DostępDoDanych
{
    public class Użytkownik : Rekord
    {
        public string SYMBOL { get; set; }
        public string NAZWISKO { get; set; }
        public string IMIE { get; set; }
        public string HASLO { get; set; }
        public short? KOD { get; set; }
        public string UZYTKOWNIK { get; set; }
        public short? P01 { get; set; }
        public short? P02 { get; set; }
        public short? P03 { get; set; }
        public short? P04 { get; set; }
        public short? P05 { get; set; }
        public short? P06 { get; set; }
        public short? P07 { get; set; }
        public short? P08 { get; set; }
        public short? P09 { get; set; }
        public short? P10 { get; set; }
        public short? P11 { get; set; }
        public short? P12 { get; set; }
        public short? P13 { get; set; }
        public short? P14 { get; set; }
        public short? P15 { get; set; }
        public short? P16 { get; set; }
        public short? P17 { get; set; }
        public short? P18 { get; set; }
        public short? P19 { get; set; }
        public short? P20 { get; set; }
        public short? P21 { get; set; }
        public short? P22 { get; set; }
        public short? P23 { get; set; }
        public short? P24 { get; set; }
        public short? P25 { get; set; }
        public short? P26 { get; set; }

        public static bool DostępPrzyznany(string nazwaUżytkownika, string hasło)
        {
            Użytkownik użytkownik;

            using (Połączenie połączenie = new Połączenie())
            {
                List<Użytkownik> użytkownicy = połączenie.PobierzWszystkie<Użytkownik>();
                użytkownik = użytkownicy.SingleOrDefault(u => String.Equals(u.UZYTKOWNIK, nazwaUżytkownika));
            }

            if (użytkownik == null)
                return false;
            else
            {
                StringBuilder odkodowaneHasło = new StringBuilder();

                foreach (char znak in użytkownik.HASLO)
                    odkodowaneHasło.Append(Convert.ToChar((Convert.ToByte(znak) - 10)));

                if (String.Equals(odkodowaneHasło.ToString(), hasło))
                    return true;
                else
                    return false;
            }
        }
    }
}