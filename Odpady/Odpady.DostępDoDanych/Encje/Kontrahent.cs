using System.Collections.Generic;

namespace Odpady.DostępDoDanych
{
    public class Kontrahent : Rekord
    {
        public string NAZWA_NAZWISKO { get; set; }
        public string NAZWA_SKROCONA_IMIE { get; set; }
        public string KOD_POCZTOWY { get; set; }
        public string MIASTO { get; set; }
        public string ULICA { get; set; }
        public string NR_DOMU { get; set; }
        public string NR_LOKALU { get; set; }
        public string WOJEWODZTWO { get; set; }
        public string POWIAT { get; set; }
        public string GMINA { get; set; }
        public string NIP_PESEL { get; set; }
        public string REGON_NR_DOKUMENTU { get; set; }
        public string TELEFON { get; set; }
        public string EMAIL { get; set; }
        public short? OSOBA_FIZYCZNA { get; set; }
        public long? FK_ODDZIAL { get; set; }

        private Oddział _oddzial;

        public Oddział ODDZIAL
        {
            get
            {
                UstawObcyObiekt(ref _oddzial, FK_ODDZIAL.Value);

                return _oddzial;
            }
        }

        public string FIRMA
        {
            get { return OSOBA_FIZYCZNA==1 ? "" : NAZWA_SKROCONA_IMIE; }
        }

        public string FIRMA_PELNA
        {
            get { return OSOBA_FIZYCZNA == 1 ? "" : NAZWA_NAZWISKO; }
        }

        public List<string> ToList()
        {
            return new List<string>
            {
                NAZWA_NAZWISKO, NAZWA_SKROCONA_IMIE, KOD_POCZTOWY, MIASTO, ULICA, NR_DOMU, NR_LOKALU,
                WOJEWODZTWO, POWIAT, GMINA, NIP_PESEL, REGON_NR_DOKUMENTU, TELEFON, EMAIL
            };
        }
    }
}