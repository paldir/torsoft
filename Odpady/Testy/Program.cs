using System;
using System.Collections.Generic;
using System.Linq;
using Odpady.DostępDoDanych;
using Odpady.Wydruki;

namespace Testy
{
    class Program
    {
        static void Main()
        {
            using (Połączenie połączenie = new Połączenie())
            {
            }
        }

        static InformacjeOOdpadzie[] MapujSzczegółyDostawyNaInformacjeOOdpadach(IEnumerable<SzczegółDostawy> szczegółyDostawy)
        {
            return (from szczegółDostawy in szczegółyDostawy let rodzajOdpadów = szczegółDostawy.RODZAJ_ODPADOW select new InformacjeOOdpadzie(rodzajOdpadów.OPIS, String.Format("{0} {1}", szczegółDostawy.ILOSC, rodzajOdpadów.JEDNOSTKA_MIARY.NAZWA))).ToArray();
        }
    }
}