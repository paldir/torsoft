using System;
using System.Collections.Generic;
using System.Globalization;
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
            using (Połączenie połączenie = new Połączenie())
            {
                DateTime teraz = DateTime.Now;
                const decimal liczba = 1.11m;

                Console.WriteLine(teraz.ToShortDateString());
                Console.WriteLine(teraz.ToShortTimeString());
                Console.WriteLine(liczba.ToString());
                Console.ReadKey();
            }
        }
    }
}