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

        public Kontrahent KONTRAHENT
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Kontrahent>(FK_KONTRAHENT.Value); }
        }

        public Oddział ODDZIAL
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Oddział>(FK_ODDZIAL.Value); }
        }
    }
}