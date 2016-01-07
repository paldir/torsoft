using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                List<WarunekZapytania> warunki = new List<WarunekZapytania>()
                {
                    new WarunekZapytania("opis", ZnakPorównania.Zawiera, String.Empty)
                };

                var tmp = połączenie.PobierzWszystkie<RodzajOdpadów>(warunki);
            }
        }
    }
}