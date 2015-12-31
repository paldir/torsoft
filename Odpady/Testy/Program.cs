using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Odpady.Wydruki;

namespace Testy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<InformacjeOOdpadzie> odpady = new List<InformacjeOOdpadzie>();
            string daneDoFaktury = @"Janina Nowak
                87-100 Toruń
                ul. Wały gen. Sikorskiego 1
                NIP 123456789";

            odpady.Add(new InformacjeOOdpadzie("opony", "4 szt."));
            odpady.Add(new InformacjeOOdpadzie("złom", "2000 kg"));
            odpady.Add(new InformacjeOOdpadzie("olej silnikowy", "13 l"));

            byte[] bajty = Wydruki.PrzyjęcieOdpadówOdOsobyFizycznej("Jan Kowalski", "00010112345", "Toruń", "Lubicka 23/1", odpady, daneDoFaktury);

            Wydruki.ZapiszBajtyJakoPdf(bajty, "test.pdf");
        }
    }
}