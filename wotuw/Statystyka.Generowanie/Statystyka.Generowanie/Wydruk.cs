using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using Pechkin;
using Pechkin.Synchronized;

namespace Statystyka.Generowanie
{
    public static class Wydruk
    {
        public static byte[] Zestawienie(IEnumerable<DaneDoWydruku> daneDoWydruku)
        {
            const string klasaLiczby = "liczba";
            const string początek = @"    
    <table>
        <caption>{tytuł}</caption>
        <thead>
            <tr>
                <th></th>
                <th>ogółem</th>
                <th>w tym mężczyźni</th>
                <th>0-18</th>
                <th>19-29</th>
                <th>30-64</th>
                <th>65 i więcej</th>
                <th>ogółem</th>
                <th>w tym mężczyźni</th>
                <th>0-18</th>
                <th>19-29</th>
                <th>30-64</th>
                <th>65 i więcej</th>
            </tr>
        </thead>
        <tbody>";

            const string koniec = @"
        </tbody>
    </table>";

            if (daneDoWydruku != null)
            {
                DaneDoWydruku[] tablicaDanych = daneDoWydruku.ToArray();
                int liczbaTabel = tablicaDanych.Length;
                StringBuilder budowniczy = new StringBuilder();

                for (int i = 0; i < liczbaTabel; i++)
                {
                    DaneDoWydruku dane = tablicaDanych[i];
                    string tytuł = dane.Tytuł;
                    IEnumerable<WierszZestawienia> wiersze = dane.WierszeZestawienia;

                    using (StringWriter pisarzNapisów = new StringWriter())
                    using (HtmlTextWriter pisarzHtml = new HtmlTextWriter(pisarzNapisów))
                    {
                        foreach (WierszZestawienia wierszZestawienia in wiersze)
                        {
                            pisarzHtml.RenderBeginTag(HtmlTextWriterTag.Tr);

                            DodajKomórkę("pierwszaKolumna", wierszZestawienia.Zabieg, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.Ogółem, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.WTymMężczyźni, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W18, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W29, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W64, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W200, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.OgółemPierwszyRaz, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.WTymMężczyźniPierwszyRaz, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W18PierwszyRaz, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W29PierwszyRaz, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W64PierwszyRaz, pisarzHtml);
                            DodajKomórkę(klasaLiczby, wierszZestawienia.W200PierwszyRaz, pisarzHtml);

                            pisarzHtml.RenderEndTag();
                        }

                        string htmlTabeli = string.Concat(początek, pisarzNapisów, koniec);
                        htmlTabeli = htmlTabeli.Replace("{tytuł}", tytuł);

                        if (i != liczbaTabel - 1)
                            htmlTabeli = string.Concat(htmlTabeli, "<div style=\"page-break-after: always\"></div>");

                        budowniczy.Append(htmlTabeli);
                    }
                }

                string wzór = File.ReadAllText("Wzór.html");
                wzór = wzór.Replace("{body}", budowniczy.ToString());
                byte[] bajty = StwórzPdfZHtml(wzór);

                return bajty;
            }

            return null;
        }

        public static void ZapiszBajtyJakoPdfIOtwórz(byte[] bajty, string ścieżkaDoPliku)
        {
            File.WriteAllBytes(ścieżkaDoPliku, bajty);
            Process.Start(ścieżkaDoPliku);
        }

        private static void DodajKomórkę(string klasa, object zawartość, HtmlTextWriter pisarzHtml)
        {
            pisarzHtml.AddAttribute(HtmlTextWriterAttribute.Class, klasa);
            pisarzHtml.RenderBeginTag(HtmlTextWriterTag.Td);
            pisarzHtml.Write(zawartość);
            pisarzHtml.RenderEndTag();
        }

        private static byte[] StwórzPdfZHtml(string html)
        {
            GlobalConfig konfiguracja = new GlobalConfig();

            konfiguracja.SetPaperSize(PaperKind.A4Rotated);

            IPechkin pechkin = new SynchronizedPechkin(konfiguracja);
            byte[] bajty = pechkin.Convert(html);

            return bajty;
        }
    }
}
