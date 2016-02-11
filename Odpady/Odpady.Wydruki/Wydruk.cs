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
using Odpady.DostępDoDanych;
using Org.BouncyCastle.Crypto.Paddings;

namespace Odpady.Wydruki
{
    public enum DostawcaOdpadów
    {
        OsobaFizyczna,
        Firma
    }

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
            const string format = "<b>{0}</b>";
            string dokument = File.ReadAllText(Path.Combine(KatalogZWzorami, "PrzyjęcieOdpadów.html"));
            dokument = dokument.Replace("{data}", data.ToString("dd.MM.yyyy"));
            dokument = dokument.Replace("{miasto}", string.Format(format, miasto));
            dokument = dokument.Replace("{ulica}", string.Format(format, ulica));
            dokument = dokument.Replace("{daneDoFaktury}", daneDoFaktury.Replace(Environment.NewLine, "<br />"));
            StringBuilder budowniczyTabeli = new StringBuilder();
            string wydarzenie;

            if (string.IsNullOrEmpty(nazwaDostarczającego) || (nazwaDostarczającego.Length < 3))
                wydarzenie = "dostarczono";
            else
            {
                wydarzenie = "{rodzajDostarczającego} {nazwa} nr {rodzajIdentyfikatora} {identyfikator} dostarczył/dostarczyła";
                wydarzenie = wydarzenie.Replace("{nazwa}", string.Format(format, nazwaDostarczającego));
                wydarzenie = wydarzenie.Replace("{identyfikator}", string.Format(format, numerIdentyfikujący));

                switch (dostawca)
                {
                    case DostawcaOdpadów.OsobaFizyczna:
                        wydarzenie = wydarzenie.Replace("{rodzajDostarczającego}", "Pan / Pani");
                        wydarzenie = wydarzenie.Replace("{rodzajIdentyfikatora}", "PESEL");

                        break;

                    case DostawcaOdpadów.Firma:
                        wydarzenie = wydarzenie.Replace("{rodzajDostarczającego}", "Nazwa Firmy / Przedsiębiorstwa");
                        wydarzenie = wydarzenie.Replace("{rodzajIdentyfikatora}", "NIP");

                        break;

                    default:
                        throw new ArgumentOutOfRangeException("dostawca", dostawca, null);
                }
            }

            dokument = dokument.Replace("{wydarzenie}", wydarzenie);
            

            foreach (InformacjeOOdpadzie odpad in odpady)
                budowniczyTabeli.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", odpad.Nazwa, odpad.Opis, odpad.Ilość, odpad.PozLimit, odpad.JednMiary);

            string zamiennikPonadLimit = ponadLimit ? "ponad limit" : string.Empty;
            dokument = dokument.Replace("{tabela}", budowniczyTabeli.ToString());
            dokument = dokument.Replace("{ponadLimit}", zamiennikPonadLimit);
            byte[] bajty = StwórzPdfZHtml(dokument);

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
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 545, 530, 75, info.NrKarty, Wyrównanie.DoŚrodka);
                    //Rok kalendarzowy
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 750, 530, 45, info.RokKalendarzowy, Wyrównanie.DoŚrodka);
                    //Miejsce prowadzenia działalności
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 20, 437, 250, info.MiejsceProwadzeniaDziałalności1, Wyrównanie.DoLewej);
                    //Nazwa i adres posiadacza odpadów transportującego odpad.
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 285, 500, 250, info.NazwaIAdresPosiadaczaOdpadówTransportującegoOdpad, Wyrównanie.DoLewej);
                    //Nazwa i adres posiadacza odpadów, który przejmuje odpad.
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 545, 500, 250, info.NazwaIAdresPosiadaczaOdpadówKtóryPrzejmujeOdpad, Wyrównanie.DoLewej);
                    //Miejsce prowadzenia działalności
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 545, 450, 250, info.MiejsceProwadzeniaDziałalności3, Wyrównanie.DoLewej);
                    //Nr rejestrowy
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 345, 415, 190, info.NrRejestrowy2, Wyrównanie.DoLewej);
                    //Nr rejestrowy
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 605, 415, 190, info.NrRejestrowy3, Wyrównanie.DoLewej);
                    //NIP
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 315, 380, 75, info.Nip2, Wyrównanie.DoLewej);
                    //REGON
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 460, 380, 75, info.Regon2, Wyrównanie.DoLewej);
                    //NIP
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 575, 380, 75, info.Nip3, Wyrównanie.DoLewej);
                    //REGON
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 720, 380, 75, info.Regon3, Wyrównanie.DoLewej);
                    //Posiadacz odpadu, któremu należy przekazać odpad
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 285, 340, 510, info.PosiadaczOdpaduKtóremuNależyPrzekazaćOdpad, Wyrównanie.DoLewej);
                    //Kod odpadu
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 110, 240, 165, info.KodOdpadu, Wyrównanie.DoŚrodka);
                    //Rodzaj odpadu
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 345, 240, 450, info.RodzajOdpadu, Wyrównanie.DoLewej);
                    //Data/miesiąc
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 20, 193, 250, info.DataMiesiąc, Wyrównanie.DoŚrodka);
                    //Masa przekazanych odpadów [Mg]
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 285, 193, 250, info.MasaPrzekazanychOdpadów, Wyrównanie.DoŚrodka);
                    //Numer rejestracyjny pojazdu, przyczepy lub naczepy                    
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 545, 193, 135, info.NumerRejestracyjnyPojazduPrzyczepyLubNaczepy, Wyrównanie.DoŚrodka);
                    //Odpad pochodzi z
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 95, 180, 180, info.OdpadPochodziZ, Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 140, 73, 130, info.Data, Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 400, 73, 130, info.Data, Wyrównanie.DoŚrodka);
                    //Data
                    WypełnijPoleWPdf(zawartośćPdf, czcionka, 660, 73, 130, info.Data, Wyrównanie.DoŚrodka);

                    PdfImportedPage strona = pisarzPdf.GetImportedPage(czytaczPdf, 1);

                    zawartośćPdf.AddTemplate(strona, 0, 0);
                    dokument.Close();

                    bajty = strumień.GetBuffer();
                }

                pisarzPdf.Close();
            }

            return bajty;
        }

        public static byte[] Ewidencja(DateTime dataOd, DateTime dataDo, IEnumerable<InformacjeOOdpadzie> odpady)
        {
            const string formatDaty = "dd.MM.yyyy";
            InformacjeOOdpadzie[] tablicaOdpadów = odpady.ToArray();
            string dokument = File.ReadAllText(Path.Combine(KatalogZWzorami, "Ewidencja.html"));
            dokument = dokument.Replace("{data1}", dataOd.ToString(formatDaty));
            dokument = dokument.Replace("{data2}", dataDo.ToString(formatDaty));
            dokument = dokument.Replace("{zestawienieDotyczy}", tablicaOdpadów.Length > 1 ? "przyjętych odpadów" : "przyjętego odpadu");
            StringBuilder budowniczyTabeli = new StringBuilder();

            foreach (InformacjeOOdpadzie informacje in tablicaOdpadów)
                budowniczyTabeli.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", informacje.Nazwa, informacje.Opis, informacje.Ilość, informacje.JednMiary);

            dokument = dokument.Replace("{tablica}", budowniczyTabeli.ToString());
            byte[] bajty = StwórzPdfZHtml(dokument);

            return bajty;
        }

        public static byte[] ZestawienieOdpadu(DateTime dataOd, DateTime dataDo, IEnumerable<SzczegółDostawy> szczegółyDostaw)
        {
            const string formatDaty = "dd.MM.yyyy";
            SzczegółDostawy[] tabelaSzczegółów = szczegółyDostaw.ToArray();
            RodzajOdpadów odpad = tabelaSzczegółów.First().RODZAJ_ODPADOW;
            string dokument = File.ReadAllText(Path.Combine(KatalogZWzorami, "ZestawienieOdpadu.html"));
            dokument = dokument.Replace("{data1}", dataOd.ToString(formatDaty));
            dokument = dokument.Replace("{data2}", dataDo.ToString(formatDaty));
            dokument = dokument.Replace("{kod}", odpad.KOD);
            dokument = dokument.Replace("{opis}", odpad.OPIS);
            StringBuilder budowniczyTabeli = new StringBuilder();
            decimal suma = 0;

            foreach (SzczegółDostawy szczegółDostawy in tabelaSzczegółów)
            {
                Dostawa dostawa = szczegółDostawy.DOSTAWA;
                Kontrahent kontrahent = dostawa.KONTRAHENT;
                decimal? potencjalnaIlość = szczegółDostawy.ILOSC;

                if (potencjalnaIlość.HasValue)
                {
                    decimal ilość = potencjalnaIlość.Value;
                    suma += ilość;

                    budowniczyTabeli.AppendFormat("<tr><td>{0:dd.MM.yyyy}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", dostawa.DATA, kontrahent.ULICA, kontrahent.NR_DOMU, kontrahent.NR_LOKALU, ilość, odpad.JEDNOSTKA_MIARY.NAZWA);
                }
            }

            budowniczyTabeli.AppendFormat("<tr><td></td><td></td><td></td><td>Łącznie: </td><td>{0}</td><td>{1}</td></tr>", suma, odpad.JEDNOSTKA_MIARY.NAZWA);

            dokument = dokument.Replace("{tablica}", budowniczyTabeli.ToString());
            byte[] bajty = StwórzPdfZHtml(dokument);

            return bajty;
        }

        public static byte[] ZestawienieKontrahenta(DateTime dataOd, DateTime dataDo, Kontrahent kontrahent, IEnumerable<SzczegółDostawy> szczegółyDostaw)
        {
            const string formatDaty = "dd.MM.yyyy";
            var tabelaSzczegółów = szczegółyDostaw.ToArray();
            var dokument = File.ReadAllText(Path.Combine(KatalogZWzorami, "ZestawienieKontrahent.html"));
            dokument = dokument.Replace("{data1}", dataOd.ToString(formatDaty));
            dokument = dokument.Replace("{data2}", dataDo.ToString(formatDaty));
            dokument = dokument.Replace("{adres}", "ul. " + kontrahent.ULICA + " " + kontrahent.NR_DOMU + (string.IsNullOrEmpty(kontrahent.NR_LOKALU) ? "" : " m." + kontrahent.NR_LOKALU));
            var budowniczyTabeli = new StringBuilder();

            foreach (SzczegółDostawy szczegółDostawy in tabelaSzczegółów)
            {
                var dostawa = szczegółDostawy.DOSTAWA;
                var odpad = szczegółDostawy.RODZAJ_ODPADOW;

                budowniczyTabeli.AppendFormat("<tr><td>{0:dd.MM.yyyy}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", dostawa.DATA, odpad.KOD, odpad.OPIS, szczegółDostawy.ILOSC, odpad.JEDNOSTKA_MIARY.NAZWA);
            }

            dokument = dokument.Replace("{tablica}", budowniczyTabeli.ToString());

            return StwórzPdfZHtml(dokument);
        }

        public static void ZapiszBajtyJakoPdfIOtwórz(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
            Process.Start(ścieżkaDoPliku);
        }

        private static byte[] StwórzPdfZHtml(string html)
        {
            GlobalConfig konfiguracja = new GlobalConfig();

            konfiguracja.SetPaperSize(PaperKind.A4);

            IPechkin pechkin = new SynchronizedPechkin(konfiguracja);
            byte[] bajty = pechkin.Convert(html);

            return bajty;
        }

        private static void WypełnijPoleWPdf(PdfContentByte zawartość, Font czcionka, float x, float y, float szerokość, object tekst, Wyrównanie wyrównanie)
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
    }
}