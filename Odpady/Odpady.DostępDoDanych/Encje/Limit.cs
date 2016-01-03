using System;

namespace Odpady.DostępDoDanych
{
    public class Limit : Rekord
    {
        public Nullable<long> FK_RODZAJ_ODPADOW { get; set; }
        public Nullable<long> FK_ODDZIAL { get; set; }
        public Nullable<short> OSOBA_FIZYCZNA { get; set; }
        public Nullable<float> LIMIT { get; set; }
        public Nullable<float> KARA { get; set; }

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<RodzajOdpadów>(FK_RODZAJ_ODPADOW.Value); }
        }

        public Oddział ODDZIAL
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Oddział>(FK_ODDZIAL.Value); }
        }
    }
}