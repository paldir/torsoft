using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Statystyka.Generowanie
{
    public static class Analiza
    {
        public static readonly Dictionary<Grupa, IEnumerable<string>> GrupaNaZabiegi = new Dictionary<Grupa, IEnumerable<string>>()
        {
            {Grupa.G1, new[] {"F43", "F43.2", "F60.0", "F63", "F63.0", "F63.8"}},
            {Grupa.G2, new[] {"F11", "F11.2", "F12", "F12.1", "F12.2", "F13", "F13.1", "F13.2", "F14", "F14.2", "F15", "F15.1", "F15.2", "F18", "F18.2", "F19", "F19.1", "F19.2"}},
            {Grupa.G3, new[] {"F10", "F10.0", "F10.1", "F10.2"}}
        };

        public static IEnumerable<ZabiegPacjenta> PobierzDaneZPliku(string ścieżka)
        {
            List<ZabiegPacjenta> zabiegi = new List<ZabiegPacjenta>();

            using (StreamReader strumień = new StreamReader(ścieżka))
                while (!strumień.EndOfStream)
                {
                    string linia = strumień.ReadLine();

                    if (!string.IsNullOrEmpty(linia))
                    {
                        string[] dane = linia.Split('\t');
                        ZabiegPacjenta zabiegPacjenta = new ZabiegPacjenta(DateTime.ParseExact(dane[5], "yyyy-MM-dd", null))
                        {
                            Zabieg = dane[0],
                            Nr = int.Parse(dane[1]),
                            Płeć = (Płeć) Enum.Parse(typeof (Płeć), dane[4])
                        };

                        string pierwszaWizyta = dane[6];

                        if (!string.IsNullOrEmpty(pierwszaWizyta))
                            zabiegPacjenta.PierwszaWizyta = DateTime.ParseExact(pierwszaWizyta, "yyyy-MM-dd HH:mm:ss", null);

                        if (!zabiegi.Exists(z => (z.Nr == zabiegPacjenta.Nr) && (z.Zabieg == zabiegPacjenta.Zabieg)))
                            zabiegi.Add(zabiegPacjenta);
                    }
                }

            return zabiegi;
        }

        public static Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>> Zestawienie(IEnumerable<ZabiegPacjenta> zabiegiPacjentów)
        {
            Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>> zabiegNaPrzedziałWiekowyNaZabiegiPacjentów = new Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>>();

            foreach (string zabieg in GrupaNaZabiegi[Grupa.G1].Concat(GrupaNaZabiegi[Grupa.G2]).Concat(GrupaNaZabiegi[Grupa.G3]))
            {
                Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>> przedziałWiekowyNaZabiegiPacjentów = Enum.GetValues(typeof (PrzedziałWiekowy)).Cast<PrzedziałWiekowy>().ToDictionary(przedziałWiekowy => przedziałWiekowy, przedziałWiekowy => new List<ZabiegPacjenta>());

                zabiegNaPrzedziałWiekowyNaZabiegiPacjentów.Add(zabieg, przedziałWiekowyNaZabiegiPacjentów);
            }

            foreach (ZabiegPacjenta zabiegPacjenta in zabiegiPacjentów)
            {
                string zabieg = zabiegPacjenta.Zabieg;
                Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>> przedziałWiekowyNaZabiegiPacjentów = zabiegNaPrzedziałWiekowyNaZabiegiPacjentów[zabieg];
                PrzedziałWiekowy przedziałWiekowy = zabiegPacjenta.PrzedziałWiekowy;

                przedziałWiekowyNaZabiegiPacjentów[przedziałWiekowy].Add(zabiegPacjenta);
            }

            return zabiegNaPrzedziałWiekowyNaZabiegiPacjentów;
        }
    }
}