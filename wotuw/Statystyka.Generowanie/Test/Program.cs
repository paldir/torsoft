using System.Collections.Generic;
using Statystyka.Generowanie;

namespace Test
{
    public class Program
    {
        private static void Main()
        {
            IEnumerable<ZabiegPacjenta> zabiegi = Analiza.PobierzDaneZPliku("wynik.txt");
            var zestawienie = Analiza.Zestawienie(zabiegi);
            Statystyki statystyki = new Statystyki(zestawienie);

            statystyki.ObliczStatystykę();
        }
    }
}