using System;

namespace Statystyka.Generowanie
{
    public class ZabiegPacjenta
    {
        public int Nr { get; set; }
        public Płeć Płeć { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string Zabieg { get; set; }
        public DateTime? PierwszaWizyta { get; set; }
        public PrzedziałWiekowy PrzedziałWiekowy { get; private set; }

        public ZabiegPacjenta(DateTime dataUrodzenia)
        {
            DataUrodzenia = dataUrodzenia;
            TimeSpan czasPacjenta = DateTime.Now - DataUrodzenia;
            int wiek = czasPacjenta.Days/365;
            PrzedziałWiekowy = WiekNaPrzedziałWiekowy(wiek);
        }

        private static PrzedziałWiekowy WiekNaPrzedziałWiekowy(int wiek)
        {
            if (wiek <= 18)
                return PrzedziałWiekowy.W18;

            if (wiek <= 29)
                return PrzedziałWiekowy.W29;

            return wiek <= 64 ? PrzedziałWiekowy.W64 : PrzedziałWiekowy.W200;
        }
    }
}