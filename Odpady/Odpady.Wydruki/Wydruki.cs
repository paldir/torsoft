using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pechkin;
using Pechkin.Synchronized;
using System.IO;
using System.Drawing.Printing;

namespace Odpady.Wydruki
{
    public enum DostawcaOdpadów { OsobaFizyczna, Firma }

    public static class Wydruki
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

        public static void ZapiszBajtyJakoPdf(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
        }
    }
}