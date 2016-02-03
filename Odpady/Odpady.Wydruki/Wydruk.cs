﻿using System;
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

        public static byte[] Kpo(InformacjeDoKpo info)
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
                    WypełnijPole(zawartośćPdf, czcionka, 545, 530, 75, info.NrKarty, Wyrównanie.DoŚrodka);
                    //Rok kalendarzowy
                    WypełnijPole(zawartośćPdf, czcionka, 750, 530, 45, info.RokKalendarzowy, Wyrównanie.DoŚrodka);
                    //Miejsce prowadzenia działalności
                    WypełnijPole(zawartośćPdf, czcionka, 20, 437, 250, info.MiejsceProwadzeniaDziałalności1, Wyrównanie.DoLewej);
                    //Nazwa i adres posiadacza odpadów transportującego odpad.
                    WypełnijPole(zawartośćPdf, czcionka, 285, 500, 250, info.NazwaIAdresPosiadaczaOdpadówTransportującegoOdpad, Wyrównanie.DoLewej);
                    //Nazwa i adres posiadacza odpadów, który przejmuje odpad.
                    WypełnijPole(zawartośćPdf, czcionka, 545, 500, 250, info.NazwaIAdresPosiadaczaOdpadówKtóryPrzejmujeOdpad, Wyrównanie.DoLewej);
                    //Miejsce prowadzenia działalności
                    WypełnijPole(zawartośćPdf, czcionka, 545, 450, 250, info.MiejsceProwadzeniaDziałalności2, Wyrównanie.DoLewej);
                    //Nr rejestrowy
                    WypełnijPole(zawartośćPdf, czcionka, 345, 415, 190, info.NrRejestrowy1, Wyrównanie.DoLewej);
                    //Nr rejestrowy
                    WypełnijPole(zawartośćPdf, czcionka, 605, 415, 190, info.NrRejestrowy2, Wyrównanie.DoLewej);
                    //NIP
                    WypełnijPole(zawartośćPdf, czcionka, 315, 380, 75, info.Nip1, Wyrównanie.DoLewej);
                    //REGON
                    WypełnijPole(zawartośćPdf, czcionka, 460, 380, 75, info.Regon1, Wyrównanie.DoLewej);
                    //NIP
                    WypełnijPole(zawartośćPdf, czcionka, 575, 380, 75, info.Nip2, Wyrównanie.DoLewej);
                    //REGON
                    WypełnijPole(zawartośćPdf, czcionka, 720, 380, 75, info.Regon2, Wyrównanie.DoLewej);
                    //Posiadacz odpadu, któremu należy przekazać odpad
                    WypełnijPole(zawartośćPdf, czcionka, 285, 340, 510, info.PosiadaczOdpaduKtóremuNależyPrzekazaćOdpad, Wyrównanie.DoLewej);
                    //Kod odpadu
                    WypełnijPole(zawartośćPdf, czcionka, 110, 240, 165, info.KodOdpadu, Wyrównanie.DoŚrodka);
                    //Rodzaj odpadu
                    WypełnijPole(zawartośćPdf, czcionka, 345, 240, 450, info.RodzajOdpadu, Wyrównanie.DoLewej);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 20, 193, 250, info.Data, Wyrównanie.DoŚrodka);
                    //Masa przekazanych odpadów [Mg]
                    WypełnijPole(zawartośćPdf, czcionka, 285, 193, 250, info.MasaPrzekazanychOdpadów, Wyrównanie.DoŚrodka);
                    //Numer rejestracyjny pojazdu, przyczepy lub naczepy                    
                    WypełnijPole(zawartośćPdf, czcionka, 545, 193, 135, info.NumerRejestracyjnyPojazduPrzyczepyLubNaczepy, Wyrównanie.DoŚrodka);
                    //Odpad pochodzi z
                    WypełnijPole(zawartośćPdf, czcionka, 95, 180, 180, info.OdpadPochodziZ, Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 140, 73, 130, info.Data, Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 400, 73, 130, info.Data, Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPole(zawartośćPdf, czcionka, 660, 73, 130, info.Data, Wyrównanie.DoŚrodka);

                    PdfImportedPage strona = pisarzPdf.GetImportedPage(czytaczPdf, 1);

                    zawartośćPdf.AddTemplate(strona, 0, 0);
                    dokument.Close();

                    bajty = strumień.GetBuffer();
                }

                pisarzPdf.Close();
            }

            return bajty;
        }

        public static void Ewidencja(IEnumerable<InformacjeOOdpadzie> informacje)
        {
            //center, bold, duża czcionka: Karty ewidencji odpadów
            //center: ewidencja od DATA do DATA
            //
            //center: tabela jak wyżej czyli: | Kod odpadu | Opis | Stan | Jedn. miary | - przekazana z tabeli
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