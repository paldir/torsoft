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

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<RodzajOdpadów>(FK_RODZAJ_ODPADOW.Value); }
        }

        public Oddział ODDZIAL
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Oddział>(FK_ODDZIAL.Value); }
        }

        public Limit GRUPA
        {
            get { return FK_GRUPA!=0 ? PołączenieDlaObcychObiektów.Pobierz<Limit>(FK_GRUPA.Value) : null; }
        }
    }
}