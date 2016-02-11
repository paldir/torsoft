using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Odpady.DostępDoDanych;
using Odpady.Wydruki;

namespace Testy
{
    internal class Program
    {
        private static void Main()
        {
            using (Połączenie p = new Połączenie())
            {
                foreach (Kontrahent kontrahent in p.PobierzWszystkie<Kontrahent>())
                {
                    var tmp1 = kontrahent.ODDZIAL.PELNA_NAZWA;
                    var tmp2 = kontrahent.ODDZIAL.SKROCONA_NAZWA;


                }
            }
        }
    }
}