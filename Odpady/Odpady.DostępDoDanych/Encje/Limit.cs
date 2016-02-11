namespace Odpady.DostępDoDanych
{
    public class Limit : Rekord
    {
        public long? FK_RODZAJ_ODPADOW { get; set; }
        public long? FK_ODDZIAL { get; set; }
        public long? FK_GRUPA { get; set; }
        public short? OSOBA_FIZYCZNA { get; set; }
        public decimal? LIMIT { get; set; }
        public decimal? KARA { get; set; }

        private RodzajOdpadów _rodzaj_odpadow;

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get
            {
                UstawObcyObiekt(ref _rodzaj_odpadow, FK_RODZAJ_ODPADOW.Value);

                return _rodzaj_odpadow;
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

        public Limit GRUPA
        {
            get { return FK_GRUPA != 0 ? PołączenieDlaObcychObiektów.Pobierz<Limit>(FK_GRUPA.Value) : null; }
        }
    }
}