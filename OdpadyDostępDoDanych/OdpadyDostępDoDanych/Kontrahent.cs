using System;
using System.Collections.Generic;

namespace OdpadyDostępDoDanych
{
    public class Kontrahent : Rekord
    {
        public string NAZWA_NAZWISKO { get; set; }
        public string NAZWA_SKROCONA_IMIE { get; set; }
        public string KOD_POCZTOWY { get; set; }
        public string MIASTO { get; set; }
        public string ULICA { get; set; }
        public Nullable<int> NR_DOMU { get; set; }
        public Nullable<int> NR_LOKALU { get; set; }
        public string WOJEWODZTWO { get; set; }
        public string POWIAT { get; set; }
        public string GMINA { get; set; }
        public string NIP_PESEL { get; set; }
        public string REGON_NR_DOKUMENTU { get; set; }
        public string TELEFON { get; set; }
        public string EMAIL { get; set; }
        public Nullable<short> OSOBA_FIZYCZNA { get; set; }
        public Nullable<long> FK_ODDZIAL { get; set; }

        public Oddział ODDZIAL
        {
            get
            {
                using (Połączenie połączenie = new Połączenie())
                    return połączenie.Pobierz<Oddział>(FK_ODDZIAL.Value);
            }
        }

        public List<string> ToList()
        {
            return new List<string>
            {
                NAZWA_NAZWISKO, NAZWA_SKROCONA_IMIE, KOD_POCZTOWY, MIASTO, ULICA, NR_DOMU.ToString(), NR_LOKALU.ToString(),
                WOJEWODZTWO, POWIAT, GMINA, NIP_PESEL, REGON_NR_DOKUMENTU, TELEFON, EMAIL
            };
        }
    }
}