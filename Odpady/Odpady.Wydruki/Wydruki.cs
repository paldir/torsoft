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
    public static class Wydruki
    {
        public static byte[] PrzyjęcieOdpadówOdOsobyFizycznej(string imięINazwiskoDostarczającego, string pesel, string miasto, string ulica, IEnumerable<InformacjeOOdpadzie> odpady, string daneDoFaktury)
        {
            GlobalConfig konfiguracjaGlobalna = new GlobalConfig();

            konfiguracjaGlobalna.SetPaperSize(PaperKind.A4);

            string format = "<b>{0}</b>";
            IPechkin pechkin = new SynchronizedPechkin(konfiguracjaGlobalna);
            string dokument = File.ReadAllText(Path.Combine("Html", "Fizyczne.html"));
            dokument = dokument.Replace("{data}", DateTime.Now.ToString("dd.MM.yyyy"));
            dokument = dokument.Replace("{imięNazwisko}", String.Format(format, imięINazwiskoDostarczającego));
            dokument = dokument.Replace("{pesel}", String.Format(format, pesel));
            dokument = dokument.Replace("{miasto}", String.Format(format, miasto));
            dokument = dokument.Replace("{ulica}", String.Format(format, ulica));
            dokument = dokument.Replace("{daneDoFaktury}", daneDoFaktury.Replace(Environment.NewLine, "<br />"));
            StringBuilder budowniczyTabeli = new StringBuilder();

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