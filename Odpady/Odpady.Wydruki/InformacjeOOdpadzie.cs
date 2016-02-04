namespace Odpady.Wydruki
{
    /*
        List<InformacjeOOdpadzie> odpady = new List<InformacjeOOdpadzie>
        {
            new InformacjeOOdpadzie("01 01 01", "opony", "4", "0", "szt."), 
            new InformacjeOOdpadzie("01 01 01", "złom", "2000", "0", "kg"), 
            new InformacjeOOdpadzie("01 01 01", "olej silnikowy", "13", "0", "l")
        };
    */

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