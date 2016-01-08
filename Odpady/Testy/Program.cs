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
                RodzajOdpadów r = połączenie.Pobierz<RodzajOdpadów>(1);
            }
        }
    }
}