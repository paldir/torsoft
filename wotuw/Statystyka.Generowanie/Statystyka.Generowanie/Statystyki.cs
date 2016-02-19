using System;
using System.Collections.Generic;
using System.Linq;

namespace Statystyka.Generowanie
{
    public class Statystyki
    {
        private readonly Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>> _słownik;

        public int Ogółem { get; private set; }
        public int OgółemMężczyźni { get; private set; }
        public Dictionary<PrzedziałWiekowy, int> PrzedziałWiekowyNaLiczbęMężczyzn { get; private set; }
        public Dictionary<PrzedziałWiekowy, int> PrzedziałWiekowyNaLiczbęOgółem { get; private set; }
        public Dictionary<string, int> ZabiegNaLiczbęMężczyzn { get; private set; }
        public Dictionary<string, int> ZabiegNaLiczbęOgółem { get; private set; }

        public Statystyki(Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>> słownik)
        {
            throw new Exception("To działa źle.");
            
            _słownik = słownik;
            PrzedziałWiekowyNaLiczbęMężczyzn = new Dictionary<PrzedziałWiekowy, int>();
            PrzedziałWiekowyNaLiczbęOgółem = new Dictionary<PrzedziałWiekowy, int>();
            ZabiegNaLiczbęMężczyzn = new Dictionary<string, int>();
            ZabiegNaLiczbęOgółem = new Dictionary<string, int>();
        }

        public void ObliczStatystykę()
        {
            foreach (ZabiegPacjenta zabiegPacjenta in _słownik.SelectMany(przedziałWiekowyNaZabiegiPacjenta => przedziałWiekowyNaZabiegiPacjenta.Value.SelectMany(zabiegiPacjenta => zabiegiPacjenta.Value)))
            {
                PrzedziałWiekowy przedziałWiekowy = zabiegPacjenta.PrzedziałWiekowy;
                string zabieg = zabiegPacjenta.Zabieg;
                Ogółem++;

                if (!PrzedziałWiekowyNaLiczbęOgółem.ContainsKey(przedziałWiekowy))
                    PrzedziałWiekowyNaLiczbęOgółem.Add(przedziałWiekowy, 0);

                if (!ZabiegNaLiczbęOgółem.ContainsKey(zabieg))
                    ZabiegNaLiczbęOgółem.Add(zabieg, 0);

                PrzedziałWiekowyNaLiczbęOgółem[przedziałWiekowy]++;
                ZabiegNaLiczbęOgółem[zabieg]++;

                if (zabiegPacjenta.Płeć == Płeć.M)
                {
                    OgółemMężczyźni++;

                    if (!PrzedziałWiekowyNaLiczbęMężczyzn.ContainsKey(przedziałWiekowy))
                        PrzedziałWiekowyNaLiczbęMężczyzn.Add(przedziałWiekowy, 0);

                    if (!ZabiegNaLiczbęMężczyzn.ContainsKey(zabieg))
                        ZabiegNaLiczbęMężczyzn.Add(zabieg, 0);

                    PrzedziałWiekowyNaLiczbęMężczyzn[przedziałWiekowy]++;
                    ZabiegNaLiczbęMężczyzn[zabieg]++;
                }
            }
        }
    }
}