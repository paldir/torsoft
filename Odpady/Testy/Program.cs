using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Odpady.DostępDoDanych;
using Odpady.Wydruki;

namespace Testy
{
    internal class Program
    {
        private static void Main()
        {
            List<InformacjeOOdpadzie> odpady = new List<InformacjeOOdpadzie>();
            string daneDoFaktury = @"Janina Nowak
                87-100 Toruń
                ul. Wały gen. Sikorskiego 1
                NIP 123456789";

            odpady.Add(new InformacjeOOdpadzie("opony", "4", "szt."));
            odpady.Add(new InformacjeOOdpadzie("złom", "2000", "kg"));
            odpady.Add(new InformacjeOOdpadzie("olej silnikowy", "13", "l"));

            byte[] bajty = Wydruk.PrzyjęcieOdpadów(DostawcaOdpadów.OsobaFizyczna, "Jan Kowalski", "00010112345", "Toruń", "Lubicka 23/1", odpady, daneDoFaktury, true);

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(bajty, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf"));
        }
    }
}