using Pechkin;
using Pechkin.Synchronized;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace Odpady.Wydruki
{
    public enum DostawcaOdpadów { OsobaFizyczna, Firma }

    /*
            List<InformacjeOOdpadzie> odpady = new List<InformacjeOOdpadzie>();
            string daneDoFaktury = @"Janina Nowak
                87-100 Toruń
                ul. Wały gen. Sikorskiego 1
                NIP 123456789";

            odpady.Add(new InformacjeOOdpadzie("opony", "4 szt."));
            odpady.Add(new InformacjeOOdpadzie("złom", "2000 kg"));
            odpady.Add(new InformacjeOOdpadzie("olej silnikowy", "13 l"));

            byte[] bajty = Wydruk.PrzyjęcieOdpadów(DostawcaOdpadów.OsobaFizyczna, "Jan Kowalski", "00010112345", "Toruń", "Lubicka 23/1", odpady, daneDoFaktury);

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(bajty, "test.pdf");
     */

    public static class Wydruk
    {
        public static byte[] PrzyjęcieOdpadów(DostawcaOdpadów dostawca, string nazwaDostarczającego, string numerIdentyfikujący, string miasto, string ulica, IEnumerable<InformacjeOOdpadzie> odpady, string daneDoFaktury)
        {
            GlobalConfig konfiguracjaGlobalna = new GlobalConfig();

            konfiguracjaGlobalna.SetPaperSize(PaperKind.A4);

            string format = "<b>{0}</b>";
            IPechkin pechkin = new SynchronizedPechkin(konfiguracjaGlobalna);
            string dokument = File.ReadAllText(Path.Combine("Wzór.html"));
            dokument = dokument.Replace("{data}", DateTime.Now.ToString("dd.MM.yyyy"));
            dokument = dokument.Replace("{nazwa}", String.Format(format, nazwaDostarczającego));
            dokument = dokument.Replace("{identyfikator}", String.Format(format, numerIdentyfikujący));
            dokument = dokument.Replace("{miasto}", String.Format(format, miasto));
            dokument = dokument.Replace("{ulica}", String.Format(format, ulica));
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
            }

            foreach (InformacjeOOdpadzie odpad in odpady)
                budowniczyTabeli.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", odpad.Nazwa, odpad.Ilość);

            dokument = dokument.Replace("{tabela}", budowniczyTabeli.ToString());

            byte[] bajty = pechkin.Convert(dokument);

            return bajty;
        }

        public static void ZapiszBajtyJakoPdfIOtwórz(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
            Process.Start(ścieżkaDoPliku);
        }
    }
}