using System;
using System.Collections.Generic;
using System.IO;
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
            DaneDoWydruku[] dane =
            {
                new DaneDoWydruku {Tytuł = "t", WierszeZestawienia = z1},
                new DaneDoWydruku {Tytuł = "t2", WierszeZestawienia = z1}
            };

            byte[] bajty = Wydruk.Zestawienie(dane);

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(bajty, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf"));

        }
    }
}