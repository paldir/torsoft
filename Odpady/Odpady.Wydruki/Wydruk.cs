using System;
using System.Collections.Generic;
using System.Text;
using Pechkin;
using Pechkin.Synchronized;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;
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
                budowniczyTabeli.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", odpad.Nazwa, odpad.Opis, odpad.Ilość, odpad.PozLimit, odpad.JednMiary);

            string zamiennikPonadLimit = ponadLimit ? "ponad limit" : string.Empty;
            dokument = dokument.Replace("{tabela}", budowniczyTabeli.ToString());
            dokument = dokument.Replace("{ponadLimit}", zamiennikPonadLimit);
            byte[] bajty = pechkin.Convert(dokument);

            return bajty;
        }

        public static byte[] Kpo()
        {
            PdfReader.unethicalreading = true;
            byte[] bajty;

            using (PdfReader czytaczPdf = new PdfReader(Path.Combine(KatalogZWzorami, "Kpo.pdf")))
            {
                Rectangle rozmiar = czytaczPdf.GetPageSizeWithRotation(1);
                Document dokument = new Document(rozmiar);
                PdfWriter pisarzPdf;

                dokument.SetPageSize(PageSize.A4.Rotate());

                using (MemoryStream strumień = new MemoryStream())
                {
                    pisarzPdf = PdfWriter.GetInstance(dokument, strumień);

                    dokument.Open();

                    PdfContentByte zawartośćPdf = pisarzPdf.DirectContent;
                    Font czcionka = FontFactory.GetFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, false, 11, Font.BOLD);

                    //Nr karty
                    WypełnijPole(zawartośćPdf, czcionka, 545, 530, 75, "11", Wyrównanie.DoŚrodka);
                    //Rok kalendarzowy
                    WypełnijPole(zawartośćPdf, czcionka, 750, 530, 45, DateTime.Now.Year, Wyrównanie.DoŚrodka);
                    //Miejsce prowadzenia działalności
                    WypełnijPole(zawartośćPdf, czcionka, 20, 437, 250, "GMINA ŁYSOMICE", Wyrównanie.DoLewej);
                    //Nazwa i adres posiadacza odpadów transportującego odpad.
                    WypełnijPole(zawartośćPdf, czcionka, 285, 500, 250, "Zakład Gospodarki Komunalnej Sp. z o.o. 87-140 Chełmża, ul. Toruńska 1", Wyrównanie.DoLewej);
                    //Nazwa i adres posiadacza odpadów, który przejmuje odpad.
                    WypełnijPole(zawartośćPdf, czcionka, 545, 500, 250, "Miejskie Przedsiębiorstwo Oczyszczania Sp. z o.o. ul. Grudziądzka 159, 87-100 Toruń", Wyrównanie.DoLewej);
                    //Miejsce prowadzenia działalności
                    WypełnijPole(zawartośćPdf, czcionka, 545, 450, 250, "ul. Kociewska 37-53, 87-100 Toruń", Wyrównanie.DoLewej);
                    //Nr rejestrowy
                    WypełnijPole(zawartośćPdf, czcionka, 345, 415, 190, "E0008276Z", Wyrównanie.DoLewej);
                    //Nr rejestrowy
                    WypełnijPole(zawartośćPdf, czcionka, 605, 415, 190, "E0008276Z", Wyrównanie.DoLewej);
                    //NIP
                    WypełnijPole(zawartośćPdf, czcionka, 315, 380, 75, "879-20-61-345", Wyrównanie.DoLewej);
                    //REGON
                    WypełnijPole(zawartośćPdf, czcionka, 460, 380, 75, 871097485, Wyrównanie.DoLewej);
                    //NIP
                    WypełnijPole(zawartośćPdf, czcionka, 575, 380, 75, "879-016-92-80", Wyrównanie.DoLewej);
                    //REGON
                    WypełnijPole(zawartośćPdf, czcionka, 720, 380, 75, 871097485, Wyrównanie.DoLewej);
                    //Posiadacz odpadu, któremu należy przekazać odpad
                    WypełnijPole(zawartośćPdf, czcionka, 285, 340, 510, "ZUOK Toruń ul. Kociewska 37-53", Wyrównanie.DoLewej);
                    //Kod odpadu
                    WypełnijPole(zawartośćPdf, czcionka, 110, 240, 165, "20 03 07", Wyrównanie.DoŚrodka);
                    //Rodzaj odpadu
                    WypełnijPole(zawartośćPdf, czcionka, 345, 240, 450, "ODPADY WIELKOGABARYTOWE", Wyrównanie.DoLewej);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 20, 193, 250, "13.01.2016", Wyrównanie.DoŚrodka);
                    //Masa przekazanych odpadów [Mg]
                    WypełnijPole(zawartośćPdf, czcionka, 285, 193, 250, 2000, Wyrównanie.DoŚrodka);
                    //Numer rejestracyjny pojazdu, przyczepy lub naczepy                    
                    WypełnijPole(zawartośćPdf, czcionka, 545, 193, 135, "CTR 70NY", Wyrównanie.DoŚrodka);
                    //Odpad pochodzi z
                    WypełnijPole(zawartośćPdf, czcionka, 95, 180, 180, "GMINA ŁYSOMICE", Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 140, 73, 130, "13.01.2016", Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 400, 73, 130, "13.01.2016", Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 660, 73, 130, "13.01.2016", Wyrównanie.DoŚrodka);

                    PdfImportedPage strona = pisarzPdf.GetImportedPage(czytaczPdf, 1);

                    zawartośćPdf.AddTemplate(strona, 0, 0);
                    dokument.Close();

                    bajty = strumień.GetBuffer();
                }

                pisarzPdf.Close();
            }

            return bajty;
        }

        private static void WypełnijPole(PdfContentByte zawartość, Font czcionka, float x, float y, float szerokość, object tekst, Wyrównanie wyrównanie)
        {
            ColumnText kolumna = new ColumnText(zawartość);
            int numerWyrównania;
            const int wysokośćLinii = 12;

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

            kolumna.SetSimpleColumn(new Phrase(new Chunk(tekst.ToString(), czcionka)), x, y + wysokośćLinii, x + szerokość, 0, wysokośćLinii, numerWyrównania);
            kolumna.Go();
        }

        public static void ZapiszBajtyJakoPdfIOtwórz(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
            Process.Start(ścieżkaDoPliku);
        }
    }
}