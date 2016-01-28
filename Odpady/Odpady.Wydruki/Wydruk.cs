using System;
using System.Collections.Generic;
using System.Text;
using Pechkin;
using Pechkin.Synchronized;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
        private const string KatalogZWzorami = "Wzory";

        private enum Wyrównanie
        {
            DoLewej,
            DoŚrodka
        }

        public static byte[] PrzyjęcieOdpadów
            (
            DostawcaOdpadów dostawca,
            string nazwaDostarczającego,
            string numerIdentyfikujący,
            string miasto,
            string ulica,
            IEnumerable<InformacjeOOdpadzie> odpady,
            string daneDoFaktury,
            bool ponadLimit,
            DateTime data
            )
        {
            GlobalConfig konfiguracjaGlobalna = new GlobalConfig();

            konfiguracjaGlobalna.SetPaperSize(PaperKind.A4);

            const string format = "<b>{0}</b>";
            IPechkin pechkin = new SynchronizedPechkin(konfiguracjaGlobalna);
            string dokument = File.ReadAllText(Path.Combine(KatalogZWzorami, "PrzyjęcieOdpadów.html"));
            dokument = dokument.Replace("{data}", data.ToString("dd.MM.yyyy"));
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
                budowniczyTabeli.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", odpad.Nazwa, odpad.Opis, odpad.Ilość, odpad.JednMiary);

            string zamiennikPonadLimit = ponadLimit ? "ponad limit" : string.Empty;
            dokument = dokument.Replace("{tabela}", budowniczyTabeli.ToString());
            dokument = dokument.Replace("{ponadLimit}", zamiennikPonadLimit);
            byte[] bajty = pechkin.Convert(dokument);

            return bajty;
        }

        public static void Kpo()
        {
            PdfReader.unethicalreading = true;
            string test = string.Concat(Enumerable.Repeat(".", 100));

            using (PdfReader czytaczPdf = new PdfReader(Path.Combine(KatalogZWzorami, "Kpo.pdf")))
            {
                Rectangle rozmiar = czytaczPdf.GetPageSizeWithRotation(1);
                Document dokument = new Document(rozmiar);
                PdfWriter pisarzPdf;

                dokument.SetPageSize(PageSize.A4.Rotate());

                using (FileStream strumień = new FileStream("test.pdf", FileMode.Create, FileAccess.Write))
                {
                    pisarzPdf = PdfWriter.GetInstance(dokument, strumień);

                    dokument.Open();

                    PdfContentByte zawartośćPdf = pisarzPdf.DirectContent;
                    Font czcionka = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, false, 11, Font.BOLD);

                    //nr karty
                    WypełnijPole(zawartośćPdf, czcionka, 545, 530, 75, "11", Wyrównanie.DoŚrodka);
                    //rok kalendarzowy
                    WypełnijPole(zawartośćPdf, czcionka, 750, 530, 45, DateTime.Now.Year, Wyrównanie.DoŚrodka);
                    //miejsce prowadzenia działalności
                    WypełnijPole(zawartośćPdf, czcionka, 20, 437, 255, "Gmina Łysomice", Wyrównanie.DoLewej);

                    PdfImportedPage strona = pisarzPdf.GetImportedPage(czytaczPdf, 1);

                    zawartośćPdf.AddTemplate(strona, 0, 0); //, 0, -1f, 1f, 0, 0, rozmiar.Height);
                    dokument.Close();
                }

                pisarzPdf.Close();
            }

            Process.Start("test.pdf");
        }

        private static void WypełnijPole(PdfContentByte zawartość, Font czcionka, float x, float y, float szerokość, object tekst, Wyrównanie wyrównanie)
        {
            ColumnText kolumna = new ColumnText(zawartość);
            Phrase fraza = new Phrase(tekst.ToString(), czcionka);
            int numerWyrównania;

            switch (wyrównanie)
            {
                case Wyrównanie.DoLewej:
                    numerWyrównania = Element.ALIGN_LEFT;

                    break;

                case Wyrównanie.DoŚrodka:
                    numerWyrównania = Element.ALIGN_CENTER;

                    break;

                default:
                    throw new ArgumentOutOfRangeException("wyrównanie", wyrównanie, null);
            }

            kolumna.SetSimpleColumn(fraza, x, y, x + szerokość, y, 0, numerWyrównania);
            kolumna.Go();
        }

        public static void ZapiszBajtyJakoPdfIOtwórz(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
            Process.Start(ścieżkaDoPliku);
        }
    }
}