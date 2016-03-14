using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace Statystyka.Generowanie
{
    public class Analiza
    {
        public static readonly Dictionary<Grupa, IEnumerable<string>> GrupaNaZabiegi = new Dictionary<Grupa, IEnumerable<string>>()
        {
            {Grupa.G1, new[] {"F43", "F43.2", "F60.0", "F63", "F63.0", "F63.8"}},
            {Grupa.G2, new[] {"F11", "F11.2", "F12", "F12.1", "F12.2", "F13", "F13.1", "F13.2", "F14", "F14.2", "F15", "F15.1", "F15.2", "F18", "F18.2", "F19", "F19.1", "F19.2"}},
            {Grupa.G3, new[] {"F10", "F10.0", "F10.1", "F10.2"}}
        };

        public IEnumerable<ZabiegPacjenta> ZabiegiPacjentów;
        public Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>> ZabiegNaPrzedziałWiekowyNaZabiegiPacjentów { get; private set; }
        public Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>> GrupaNaPrzedziałWiekowyNaLiczbęMężczyzn { get; private set; }
        public Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>> GrupaNaPrzedziałWiekowyNaLiczbęOgółem { get; private set; }
        public Dictionary<string, int> ZabiegNaLiczbęMężczyzn { get; private set; }
        public Dictionary<string, int> ZabiegNaLiczbęOgółem { get; private set; }
        public Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>> GrupaNaPrzedziałWiekowyNaLiczbęMężczyznPierwszyRaz { get; private set; }
        public Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>> GrupaNaPrzedziałWiekowyNaLiczbęOgółemPierwszyRaz { get; private set; }
        public Dictionary<string, int> ZabiegNaLiczbęMężczyznPierwszyRaz { get; private set; }
        public Dictionary<string, int> ZabiegNaLiczbęOgółemPierwszyRaz { get; private set; }

        public Analiza(string ścieżkaPliku, Poradnia poradnia)
        {
            PobierzDaneZPliku(ścieżkaPliku, poradnia);
            Zestawienie();
            ObliczStatystykę();
        }

        public IEnumerable<WierszZestawienia> OstateczneZestawienie(Grupa grupa)
        {
            List<WierszZestawienia> wiersze = new List<WierszZestawienia>();
            Type typWierszaZestawienia = typeof(WierszZestawienia);
            Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęOgółem = GrupaNaPrzedziałWiekowyNaLiczbęOgółem[grupa];
            Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęMężczyzn = GrupaNaPrzedziałWiekowyNaLiczbęMężczyzn[grupa];
            Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęOgółemPierwszyRaz = GrupaNaPrzedziałWiekowyNaLiczbęOgółemPierwszyRaz[grupa];
            Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęMężczyznPierwszyRaz = GrupaNaPrzedziałWiekowyNaLiczbęMężczyznPierwszyRaz[grupa];
            WierszZestawienia ogółem = new WierszZestawienia
            {
                Zabieg = "Ogółem",
                Ogółem = przedziałWiekowyNaLiczbęOgółem.Sum(p => p.Value),
                WTymMężczyźni = przedziałWiekowyNaLiczbęMężczyzn.Sum(p => p.Value),
                W18 = przedziałWiekowyNaLiczbęOgółem[PrzedziałWiekowy.W18],
                W29 = przedziałWiekowyNaLiczbęOgółem[PrzedziałWiekowy.W29],
                W64 = przedziałWiekowyNaLiczbęOgółem[PrzedziałWiekowy.W64],
                W200 = przedziałWiekowyNaLiczbęOgółem[PrzedziałWiekowy.W200],
                OgółemPierwszyRaz = przedziałWiekowyNaLiczbęOgółemPierwszyRaz.Sum(p => p.Value),
                WTymMężczyźniPierwszyRaz = przedziałWiekowyNaLiczbęMężczyznPierwszyRaz.Sum(p => p.Value),
                W18PierwszyRaz = przedziałWiekowyNaLiczbęOgółemPierwszyRaz[PrzedziałWiekowy.W18],
                W29PierwszyRaz = przedziałWiekowyNaLiczbęOgółemPierwszyRaz[PrzedziałWiekowy.W29],
                W64PierwszyRaz = przedziałWiekowyNaLiczbęOgółemPierwszyRaz[PrzedziałWiekowy.W64],
                W200PierwszyRaz = przedziałWiekowyNaLiczbęOgółemPierwszyRaz[PrzedziałWiekowy.W200]
            };

            WierszZestawienia mężczyźni = new WierszZestawienia()
            {
                Zabieg = "mężczyźni",
                W18 = przedziałWiekowyNaLiczbęMężczyzn[PrzedziałWiekowy.W18],
                W29 = przedziałWiekowyNaLiczbęMężczyzn[PrzedziałWiekowy.W29],
                W64 = przedziałWiekowyNaLiczbęMężczyzn[PrzedziałWiekowy.W64],
                W200 = przedziałWiekowyNaLiczbęMężczyzn[PrzedziałWiekowy.W200],
                W18PierwszyRaz = przedziałWiekowyNaLiczbęMężczyznPierwszyRaz[PrzedziałWiekowy.W18],
                W29PierwszyRaz = przedziałWiekowyNaLiczbęMężczyznPierwszyRaz[PrzedziałWiekowy.W29],
                W64PierwszyRaz = przedziałWiekowyNaLiczbęMężczyznPierwszyRaz[PrzedziałWiekowy.W64],
                W200PierwszyRaz = przedziałWiekowyNaLiczbęMężczyznPierwszyRaz[PrzedziałWiekowy.W200]
            };

            wiersze.Add(ogółem);
            wiersze.Add(mężczyźni);

            foreach (string zabieg in GrupaNaZabiegi[grupa])
            {
                WierszZestawienia wiersz = new WierszZestawienia
                {
                    Zabieg = zabieg,
                    Ogółem = ZabiegNaLiczbęOgółem[zabieg],
                    WTymMężczyźni = ZabiegNaLiczbęMężczyzn[zabieg],
                    OgółemPierwszyRaz = ZabiegNaLiczbęOgółemPierwszyRaz[zabieg],
                    WTymMężczyźniPierwszyRaz = ZabiegNaLiczbęMężczyznPierwszyRaz[zabieg]
                };

                foreach (PrzedziałWiekowy przedziałWiekowy in Enum.GetValues(typeof(PrzedziałWiekowy)))
                {
                    List<ZabiegPacjenta> zabiegiPacjentów = ZabiegNaPrzedziałWiekowyNaZabiegiPacjentów[zabieg][przedziałWiekowy];

                    typWierszaZestawienia.GetProperty(przedziałWiekowy.ToString()).SetValue(wiersz, zabiegiPacjentów.Count, null);
                    typWierszaZestawienia.GetProperty(string.Concat(przedziałWiekowy, "PierwszyRaz")).SetValue(wiersz, zabiegiPacjentów.Count(z => !z.PierwszaWizyta.HasValue), null);
                }

                wiersze.Add(wiersz);
            }

            return wiersze;
        }

        private void PobierzDaneZPliku(string ścieżka, Poradnia poradnia)
        {
            const string parametry = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=wotuiw-server)  (PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=KS)));User Id=PPS;Password=KSPPS;";
            string nazwaPliku = ścieżka;
            DataTable tabela = new DataTable();

            using (OracleConnection połączenie = new OracleConnection(parametry))
            {
                połączenie.Open();

                string kodPoradni = poradnia == Poradnia.Alkohol ? "200016825" : "200016822";
                int rok = ZabiegPacjenta.Rok;
                string treść = @"select KNK.NJ10,EPD.NPAC,PAC.PESL,NLKR,PAC.PLEC,PAC.DATU,PAC.DTWO from KNK JOIN EPD ON EPD.NINS=KNK.NINS_EPD
                         and EPD.NEPD=KNK.NEPD
                         and EPD.AKTW = 'T'
                         and EPD.WERS > 0
                         JOIN PAC ON EPD.NINS_PAC = PAC.NINS
                         and EPD.NPAC = PAC.NPAC
                         and KNK.NMWU = '" + kodPoradni + @"'
                         where KNK.DTOD >='" + rok + @"-01-01' and KNK.DTOD<='" + rok + @"-12-31'
                        order by PAC.DATU ASC";

                using (OracleDataAdapter adapter = new OracleDataAdapter(treść, połączenie))
                    adapter.Fill(tabela);
            }

            using (StreamWriter pisarz = new StreamWriter(nazwaPliku))
                foreach (DataRow wiersz in tabela.Rows)
                {
                    foreach (object pole in wiersz.ItemArray)
                        pisarz.Write("{0}\t", pole);

                    pisarz.WriteLine();
                }

            List<ZabiegPacjenta> zabiegiPacjentów = new List<ZabiegPacjenta>();

            using (StreamReader strumień = new StreamReader(ścieżka))
                while (!strumień.EndOfStream)
                {
                    string linia = strumień.ReadLine();

                    if (!string.IsNullOrEmpty(linia))
                    {
                        string[] dane = linia.Split('\t');
                        ZabiegPacjenta zabiegPacjenta = new ZabiegPacjenta(DateTime.ParseExact(dane[5], "yyyy-MM-dd HH:mm:ss", null))
                        {
                            Zabieg = dane[0],
                            Nr = int.Parse(dane[1]),
                            Płeć = (Płeć)Enum.Parse(typeof(Płeć), dane[4])
                        };

                        string pierwszaWizyta = dane[6];

                        if (!string.IsNullOrEmpty(pierwszaWizyta))
                            zabiegPacjenta.PierwszaWizyta = DateTime.ParseExact(pierwszaWizyta, "yyyy-MM-dd HH:mm:ss", null);

                        if (!zabiegiPacjentów.Exists(z => (z.Nr == zabiegPacjenta.Nr) && (z.Zabieg == zabiegPacjenta.Zabieg)))
                            zabiegiPacjentów.Add(zabiegPacjenta);
                    }
                }

            ZabiegiPacjentów = zabiegiPacjentów;
        }

        private void Zestawienie()
        {
            ZabiegNaPrzedziałWiekowyNaZabiegiPacjentów = new Dictionary<string, Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>>>();

            foreach (string zabieg in GrupaNaZabiegi[Grupa.G1].Concat(GrupaNaZabiegi[Grupa.G2]).Concat(GrupaNaZabiegi[Grupa.G3]))
            {
                Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>> przedziałWiekowyNaZabiegiPacjentów = Enum.GetValues(typeof(PrzedziałWiekowy)).Cast<PrzedziałWiekowy>().ToDictionary(przedziałWiekowy => przedziałWiekowy, przedziałWiekowy => new List<ZabiegPacjenta>());

                ZabiegNaPrzedziałWiekowyNaZabiegiPacjentów.Add(zabieg, przedziałWiekowyNaZabiegiPacjentów);
            }

            foreach (ZabiegPacjenta zabiegPacjenta in ZabiegiPacjentów)
            {
                string zabieg = zabiegPacjenta.Zabieg;
                Dictionary<PrzedziałWiekowy, List<ZabiegPacjenta>> przedziałWiekowyNaZabiegiPacjentów = ZabiegNaPrzedziałWiekowyNaZabiegiPacjentów[zabieg];
                PrzedziałWiekowy przedziałWiekowy = zabiegPacjenta.PrzedziałWiekowy;

                przedziałWiekowyNaZabiegiPacjentów[przedziałWiekowy].Add(zabiegPacjenta);
            }
        }

        private void ObliczStatystykę()
        {
            GrupaNaPrzedziałWiekowyNaLiczbęMężczyzn = new Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>>();
            GrupaNaPrzedziałWiekowyNaLiczbęOgółem = new Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>>();
            ZabiegNaLiczbęMężczyzn = new Dictionary<string, int>();
            ZabiegNaLiczbęOgółem = new Dictionary<string, int>();
            GrupaNaPrzedziałWiekowyNaLiczbęMężczyznPierwszyRaz = new Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>>();
            GrupaNaPrzedziałWiekowyNaLiczbęOgółemPierwszyRaz = new Dictionary<Grupa, Dictionary<PrzedziałWiekowy, int>>();
            ZabiegNaLiczbęMężczyznPierwszyRaz = new Dictionary<string, int>();
            ZabiegNaLiczbęOgółemPierwszyRaz = new Dictionary<string, int>();

            foreach (Grupa grupa in Enum.GetValues(typeof(Grupa)))
            {
                Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęMężczyzn = new Dictionary<PrzedziałWiekowy, int>();
                Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęOgółem = new Dictionary<PrzedziałWiekowy, int>();
                Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęMężczyznPierwszyRaz = new Dictionary<PrzedziałWiekowy, int>();
                Dictionary<PrzedziałWiekowy, int> przedziałWiekowyNaLiczbęOgółemPierwszyRaz = new Dictionary<PrzedziałWiekowy, int>();

                foreach (PrzedziałWiekowy przedziałWiekowy in Enum.GetValues(typeof(PrzedziałWiekowy)))
                {
                    przedziałWiekowyNaLiczbęMężczyzn.Add(przedziałWiekowy, 0);
                    przedziałWiekowyNaLiczbęOgółem.Add(przedziałWiekowy, 0);
                    przedziałWiekowyNaLiczbęMężczyznPierwszyRaz.Add(przedziałWiekowy, 0);
                    przedziałWiekowyNaLiczbęOgółemPierwszyRaz.Add(przedziałWiekowy, 0);
                }

                GrupaNaPrzedziałWiekowyNaLiczbęMężczyzn.Add(grupa, przedziałWiekowyNaLiczbęMężczyzn);
                GrupaNaPrzedziałWiekowyNaLiczbęOgółem.Add(grupa, przedziałWiekowyNaLiczbęOgółem);
                GrupaNaPrzedziałWiekowyNaLiczbęMężczyznPierwszyRaz.Add(grupa, przedziałWiekowyNaLiczbęMężczyznPierwszyRaz);
                GrupaNaPrzedziałWiekowyNaLiczbęOgółemPierwszyRaz.Add(grupa, przedziałWiekowyNaLiczbęOgółemPierwszyRaz);

                foreach (string zabieg in GrupaNaZabiegi[grupa])
                {
                    ZabiegNaLiczbęMężczyzn.Add(zabieg, 0);
                    ZabiegNaLiczbęOgółem.Add(zabieg, 0);
                    ZabiegNaLiczbęMężczyznPierwszyRaz.Add(zabieg, 0);
                    ZabiegNaLiczbęOgółemPierwszyRaz.Add(zabieg, 0);
                }
            }

            foreach (ZabiegPacjenta zabiegPacjenta in ZabiegNaPrzedziałWiekowyNaZabiegiPacjentów.SelectMany(przedziałWiekowyNaZabiegiPacjenta => przedziałWiekowyNaZabiegiPacjenta.Value.SelectMany(zabiegiPacjenta => zabiegiPacjenta.Value)))
            {
                PrzedziałWiekowy przedziałWiekowy = zabiegPacjenta.PrzedziałWiekowy;
                string zabieg = zabiegPacjenta.Zabieg;
                Dictionary<Grupa, IEnumerable<string>> grupaNaZabiegi = GrupaNaZabiegi;
                Grupa grupa = grupaNaZabiegi.Keys.Single(k => grupaNaZabiegi[k].Contains(zabieg));
                DateTime? pierwszaWizyta = zabiegPacjenta.PierwszaWizyta;

                GrupaNaPrzedziałWiekowyNaLiczbęOgółem[grupa][przedziałWiekowy]++;
                ZabiegNaLiczbęOgółem[zabieg]++;

                if (!pierwszaWizyta.HasValue)
                {
                    GrupaNaPrzedziałWiekowyNaLiczbęOgółemPierwszyRaz[grupa][przedziałWiekowy]++;
                    ZabiegNaLiczbęOgółemPierwszyRaz[zabieg]++;
                }

                if (zabiegPacjenta.Płeć == Płeć.M)
                {
                    GrupaNaPrzedziałWiekowyNaLiczbęMężczyzn[grupa][przedziałWiekowy]++;
                    ZabiegNaLiczbęMężczyzn[zabieg]++;

                    if (!pierwszaWizyta.HasValue)
                    {
                        GrupaNaPrzedziałWiekowyNaLiczbęMężczyznPierwszyRaz[grupa][przedziałWiekowy]++;
                        ZabiegNaLiczbęMężczyznPierwszyRaz[zabieg]++;
                    }
                }
            }
        }
    }
}