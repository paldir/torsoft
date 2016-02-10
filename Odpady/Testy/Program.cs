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
            foreach (var proces in Process.GetProcessesByName("AcroRd32"))
                proces.Kill();

            using (Połączenie p = new Połączenie())
            {
                IEnumerable<SzczegółDostawy> sd = p.PobierzWszystkie<SzczegółDostawy>().Where(s => s.RODZAJ_ODPADOW.KOD == "17 01 01");

                Wydruk.ZapiszBajtyJakoPdfIOtwórz(Wydruk.ZestawienieOdpadu(DateTime.Now, DateTime.Now, sd), "test.pdf");
            }
        }
    }
}