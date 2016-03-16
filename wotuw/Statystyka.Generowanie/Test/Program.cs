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
            Analiza analiza = new Analiza(Poradnia.Narkotyki);
            IEnumerable<WierszZestawienia> z1 = analiza.OstateczneZestawienie(Grupa.G1);
            IEnumerable<WierszZestawienia> z2 = analiza.OstateczneZestawienie(Grupa.G2);
            IEnumerable<WierszZestawienia> z3 = analiza.OstateczneZestawienie(Grupa.G3);
            DaneDoWydruku[] dane =
            {
                new DaneDoWydruku {Tytuł = "t1", WierszeZestawienia = z1},
                new DaneDoWydruku {Tytuł = "t2", WierszeZestawienia = z2},
                new DaneDoWydruku {Tytuł = "t3", WierszeZestawienia = z3}
            };

            byte[] bajty = Wydruk.Zestawienie(dane);

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(bajty, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf"));
        }
    }
}