using System;
using System.Diagnostics;
using System.Linq;
using Odpady.DostępDoDanych;
using Odpady.Wydruki;

namespace Testy
{
    internal class Program
    {
        private static void Main()
        {
            /*foreach (var proces in Process.GetProcessesByName("AcroRd32"))
                proces.Kill();

            InformacjeDoKpo i = new InformacjeDoKpo(DateTime.Now)
            {
                NrKarty = "11",
                RokKalendarzowy = "11",
                NazwaIAdresPosiadaczaOdpadówTransportującegoOdpad = "Zakład Gospodarki Komunalnej Sp. z o.o. 87-140 Chełmża, ul. Toruńska 1",
                NazwaIAdresPosiadaczaOdpadówKtóryPrzejmujeOdpad = "Miejskie Przedsiębiorstwo Oczyszczania Sp. z o.o. ul.Grudziądzka 159, 87-100 Toruń",
                MiejsceProwadzeniaDziałalności1 = "GMINA ŁYSOMICE",
                MiejsceProwadzeniaDziałalności2 = "ul. Kociewska 37-53, 87-100 Toruń",
                NrRejestrowy1 = "E0008276Z",
                NrRejestrowy2 = "E0008276Z",
                Nip1 = "879-20-61-345",
                Regon1 = "871097485",
                Nip2 = "879-016-92-80",
                Regon2 = "871097485",
                PosiadaczOdpaduKtóremuNależyPrzekazaćOdpad = "ZUOK Toruń, ul. Kociewska 37-53",
                KodOdpadu = "20 03 07",
                RodzajOdpadu = "ODPADY WIELKOGABARYTOWE",
                MasaPrzekazanychOdpadów = "2000",
                NumerRejestracyjnyPojazduPrzyczepyLubNaczepy = "CTR 70NY",
                OdpadPochodziZ = "GMINA ŁYSOMICE"
            };

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(Wydruk.Kpo(i), "test.pdf");*/

            using (Połączenie p = new Połączenie())
            {
                p.UtwórzKlasęNaPodstawieTabeli("kpo", "Kpo");
            }
        }
    }
}