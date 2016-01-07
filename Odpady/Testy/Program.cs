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
                List<WarunekZapytania> warunki = new List<WarunekZapytania>
                {
                    new WarunekZapytania("opis", ZnakPorównania.Zawiera, "rud"),
                    new WarunekZapytania("id", ZnakPorównania.RównaSię, 1)
                };

                var tmp = połączenie.Pobierz<RodzajOdpadów>(1);
            }
        }
    }
}