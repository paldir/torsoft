using System;

namespace Odpady.DostępDoDanych
{
    public class Dostawa : Rekord
    {
        public long? FK_KONTRAHENT { get; set; }
        public DateTime? DATA { get; set; }
        public long? FK_ODDZIAL { get; set; }
        public string NAZWA_NAZWISKO { get; set; }
        public string NAZWA_SKROCONA_IMIE { get; set; }
        public string NIP_PESEL { get; set; }

        private Kontrahent _kontrahent;

        public Kontrahent KONTRAHENT
        {
            get
            {
                UstawObcyObiekt(ref _kontrahent, FK_KONTRAHENT.Value);

                return _kontrahent;
            }
        }

        private Oddział _oddzial;

        public Oddział ODDZIAL
        {
            get
            {
                UstawObcyObiekt(ref _oddzial, FK_ODDZIAL.Value);

                return _oddzial;
            }
        }
    }
}