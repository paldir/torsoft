namespace Odpady.Wydruki
{
    public class InformacjeOOdpadzie
    {
        public string Nazwa { get; set; }
        public string Opis { get; set; }
        public string Ilość { get; set; }
        public string PozLimit { get; set; }
        public string JednMiary { get; set; }

        public InformacjeOOdpadzie(string nazwa, string opis, string ilość, string pozLimit, string jednMiary)
        {
            Nazwa = nazwa;
            Opis = opis;
            Ilość = ilość;
            PozLimit = pozLimit;
            JednMiary = jednMiary;
        }
    }
}