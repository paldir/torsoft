using System;

namespace Odpady.DostępDoDanych
{
    public class Dostawa : Rekord
    {
        public Nullable<long> FK_RODZAJ_ODPADOW { get; set; }
        public Nullable<long> FK_KONTRAHENT { get; set; }
        public Nullable<float> ILOSC { get; set; }
        public string DATA { get; set; }

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<RodzajOdpadów>(FK_RODZAJ_ODPADOW.Value); }
        }

        public Kontrahent KONTRAHENT
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Kontrahent>(FK_KONTRAHENT.Value); }
        }
    }
}