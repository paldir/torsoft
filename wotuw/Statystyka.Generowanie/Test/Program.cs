using System;
using System.Collections.Generic;
using Statystyka.Generowanie;

namespace Test
{
    public class Program
    {
        private static void Main()
        {
            ZabiegPacjenta.Rok = 2015;
            Analiza analiza = new Analiza("wynik.txt");
            IEnumerable<WierszZestawienia> z1 = analiza.OstateczneZestawienie(Grupa.G1);
            IEnumerable<WierszZestawienia> z2 = analiza.OstateczneZestawienie(Grupa.G2);
            IEnumerable<WierszZestawienia> z3 = analiza.OstateczneZestawienie(Grupa.G3);

            foreach (WierszZestawienia wierszZestawienia in z1)
                Console.WriteLine(string.Join("\t", wierszZestawienia.Zabieg, wierszZestawienia.Ogółem, wierszZestawienia.WTymMężczyźni, wierszZestawienia.W18, wierszZestawienia.W29, wierszZestawienia.W64, wierszZestawienia.W200));

            Console.ReadKey();

            foreach (WierszZestawienia wierszZestawienia in z2)
                Console.WriteLine(string.Join("\t", wierszZestawienia.Zabieg, wierszZestawienia.Ogółem, wierszZestawienia.WTymMężczyźni, wierszZestawienia.W18, wierszZestawienia.W29, wierszZestawienia.W64, wierszZestawienia.W200));

            Console.ReadKey();

            foreach (WierszZestawienia wierszZestawienia in z3)
                Console.WriteLine(string.Join("\t", wierszZestawienia.Zabieg, wierszZestawienia.Ogółem, wierszZestawienia.WTymMężczyźni, wierszZestawienia.W18, wierszZestawienia.W29, wierszZestawienia.W64, wierszZestawienia.W200));

            Console.ReadKey();

        }
    }
}