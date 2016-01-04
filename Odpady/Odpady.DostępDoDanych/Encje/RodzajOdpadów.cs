namespace Odpady.DostępDoDanych
{
    public class RodzajOdpadów : Rekord
    {
        public string KOD { get; set; }
        public string OPIS { get; set; }
        public long? FK_JEDNOSTKA_MIARY { get; set; }

        public JednostkaMiary JEDNOSTKA_MIARY
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<JednostkaMiary>(FK_JEDNOSTKA_MIARY.Value); }
        }
    }
}