using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Odpady.DostępDoDanych;
using Odpady.Wydruki;

namespace Testy
{
    internal class Program
    {
        private static void Main()
        {
            foreach (var proces in Process.GetProcessesByName("AcroRd32"))
                proces.Kill();

            List<InformacjeOOdpadzie> odpady = new List<InformacjeOOdpadzie>
            {
                new InformacjeOOdpadzie("01 01 01", "opony", "4", "0", "szt."),
                new InformacjeOOdpadzie("01 01 01", "złom", "2000", "0", "kg"),
                new InformacjeOOdpadzie("01 01 01", "olej silnikowy", "13", "0", "l")
            };

            Wydruk.ZapiszBajtyJakoPdfIOtwórz(Wydruk.Ewidencja(DateTime.Now.AddDays(-7), DateTime.Now, odpady), "test.pdf");
        }

        private static InformacjeDoKpo KonwertujZEncjiNaInfoDoWydruku(Kpo kpo)
        {
            DateTime? data = kpo.DATA;
            RodzajOdpadów odpad = kpo.ODPAD;
            decimal? masa = kpo.MASA;
            InformacjeDoKpo info = data.HasValue ? new InformacjeDoKpo(data.Value) : new InformacjeDoKpo();
            info.NrKarty = kpo.NR_KARTY;
            info.RokKalendarzowy = kpo.ROK_KALENDARZOWY;
            info.MiejsceProwadzeniaDziałalności1 = kpo.MIEJSCE_DZIALALNOSCI_1;
            info.NazwaIAdresPosiadaczaOdpadówTransportującegoOdpad = kpo.TRANSPORTUJACY_2;
            info.NazwaIAdresPosiadaczaOdpadówKtóryPrzejmujeOdpad = kpo.PRZEJMUJACY_3;
            info.MiejsceProwadzeniaDziałalności3 = kpo.MIEJSCE_DZIALALNOSCI_3;
            info.NrRejestrowy2 = kpo.NR_REJESTROWY_2;
            info.NrRejestrowy3 = kpo.NR_REJESTROWY_3;
            info.Nip2 = kpo.NIP_2;
            info.Regon2 = kpo.REGON_2;
            info.Nip3 = kpo.NIP_3;
            info.Regon3 = kpo.REGON_3;
            info.PosiadaczOdpaduKtóremuNależyPrzekazaćOdpad = kpo.ODBIORCA;
            info.KodOdpadu = odpad.KOD;
            info.RodzajOdpadu = odpad.OPIS;
            info.DataMiesiąc = kpo.DATA_MIESIAC;
            info.NumerRejestracyjnyPojazduPrzyczepyLubNaczepy = kpo.NUMER_REJESTRACYJNY;
            info.OdpadPochodziZ = kpo.ODPAD_POCHODZI_Z;

            if (masa.HasValue)
                info.MasaPrzekazanychOdpadów = masa.Value.ToString(CultureInfo.CurrentCulture);

            return info;
        }
    }
}