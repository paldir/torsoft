using System;
using System.Collections.Generic;
using System.Text;

using Pechkin;
using Pechkin.Synchronized;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Odpady.Wydruki
{
    public enum DostawcaOdpadów
    {
        OsobaFizyczna,
        Firma
    }

    /*
            List<InformacjeOOdpadzie> odpady = new List<InformacjeOOdpadzie>();
            string daneDoFaktury = @"Janina Nowak
                87-100 Toruń
                ul. Wały gen. Sikorskiego 1
                NIP 123456789";

            odpady.Add(new InformacjeOOdpadzie("opony", "4", "szt."));
            odpady.Add(new InformacjeOOdpadzie("złom", "2000", "kg"));
            odpady.Add(new InformacjeOOdpadzie("olej silnikowy", "13", "l"));

            byte[] bajty = Wydruk.PrzyjęcieOdpadów(DostawcaOdpadów.OsobaFizyczna, "Jan Kowalski", "00010112345", "Toruń", "Lubicka 23/1", odpady, daneDoFaktury, true);

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(bajty, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf"));
     */

    public static class Wydruk
    {
        public static byte[] PrzyjęcieOdpadów
            (
            DostawcaOdpadów dostawca,
            string nazwaDostarczającego,
            string numerIdentyfikujący,
            string miasto,
            string ulica,
            IEnumerable<InformacjeOOdpadzie> odpady,
            string daneDoFaktury,
            bool ponadLimit
            )
        {
            GlobalConfig konfiguracjaGlobalna = new GlobalConfig();

            konfiguracjaGlobalna.SetPaperSize(PaperKind.A4);

            const string format = "<b>{0}</b>";
            IPechkin pechkin = new SynchronizedPechkin(konfiguracjaGlobalna);
            string dokument = File.ReadAllText(Path.Combine("Wzory", "Wzór.html"));
            dokument = dokument.Replace("{data}", DateTime.Now.ToString("dd.MM.yyyy"));
            dokument = dokument.Replace("{nazwa}", string.Format(format, nazwaDostarczającego));
            dokument = dokument.Replace("{identyfikator}", string.Format(format, numerIdentyfikujący));
            dokument = dokument.Replace("{miasto}", string.Format(format, miasto));
            dokument = dokument.Replace("{ulica}", string.Format(format, ulica));
            dokument = dokument.Replace("{daneDoFaktury}", daneDoFaktury.Replace(Environment.NewLine, "<br />"));
            StringBuilder budowniczyTabeli = new StringBuilder();

            switch (dostawca)
            {
                case DostawcaOdpadów.OsobaFizyczna:
                    dokument = dokument.Replace("{rodzajDostarczającego}", "Pan / Pani");
                    dokument = dokument.Replace("{rodzajIdentyfikatora}", "PESEL");

                    break;

                case DostawcaOdpadów.Firma:
                    dokument = dokument.Replace("{rodzajDostarczającego}", "Nazwa Firmy / Przedsiębiorstwa");
                    dokument = dokument.Replace("{rodzajIdentyfikatora}", "NIP");

                    break;

                default:
                    throw new ArgumentOutOfRangeException("dostawca", dostawca, null);
            }

            foreach (InformacjeOOdpadzie odpad in odpady)
                budowniczyTabeli.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", odpad.Nazwa, odpad.Ilość, odpad.JednMiary);

            string zamiennikPonadLimit = ponadLimit ? "ponad limit" : string.Empty;
            dokument = dokument.Replace("{tabela}", budowniczyTabeli.ToString());
            dokument = dokument.Replace("{ponadLimit}", zamiennikPonadLimit);
            byte[] bajty = pechkin.Convert(dokument);

            return bajty;
        }

        public static byte[] MiesięcznyWykazOdpadów(IEnumerable<InformacjeOOdpadzie> odpady)
        {

        }

        public static void ZapiszBajtyJakoPdfIOtwórz(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
            Process.Start(ścieżkaDoPliku);
        }
    }
}