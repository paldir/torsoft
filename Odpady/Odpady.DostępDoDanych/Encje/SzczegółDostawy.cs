namespace Odpady.DostępDoDanych
{
    public class SzczegółDostawy : Rekord
    {
        public float? ILOSC { get; set; }
        public long? FK_RODZAJ_ODPADOW { get; set; }
        public long? FK_DOSTAWA { get; set; }

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<RodzajOdpadów>(FK_RODZAJ_ODPADOW.Value); }
        }

        public Dostawa DOSTAWA
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Dostawa>(FK_DOSTAWA.Value); }
        }
    }
}