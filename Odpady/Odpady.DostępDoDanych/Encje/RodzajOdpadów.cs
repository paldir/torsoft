namespace Odpady.DostępDoDanych
{
    public class RodzajOdpadów : Rekord
    {
        public string KOD { get; set; }
        public string OPIS { get; set; }
        public long? FK_JEDNOSTKA_MIARY { get; set; }

        private JednostkaMiary _jednostka_miary;

        public JednostkaMiary JEDNOSTKA_MIARY
        {
            get
            {
                UstawObcyObiekt(ref _jednostka_miary, FK_JEDNOSTKA_MIARY.Value);

                return _jednostka_miary;
            }
        }
    }
}