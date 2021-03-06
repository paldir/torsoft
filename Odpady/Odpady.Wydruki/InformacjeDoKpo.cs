﻿using System;

namespace Odpady.Wydruki
{
    /*  InformacjeDoKpo i = new InformacjeDoKpo(DateTime.Now)
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
     */
    
    public class InformacjeDoKpo
    {
        public string NrKarty { get; set; }
        public string RokKalendarzowy { get; set; }
        public string MiejsceProwadzeniaDziałalności1 { get; set; }
        public string NazwaIAdresPosiadaczaOdpadówTransportującegoOdpad { get; set; }
        public string NazwaIAdresPosiadaczaOdpadówKtóryPrzejmujeOdpad { get; set; }
        public string MiejsceProwadzeniaDziałalności3 { get; set; }
        public string NrRejestrowy2 { get; set; }
        public string NrRejestrowy3 { get; set; }
        public string Nip2 { get; set; }
        public string Regon2 { get; set; }
        public string Nip3 { get; set; }
        public string Regon3 { get; set; }
        public string PosiadaczOdpaduKtóremuNależyPrzekazaćOdpad { get; set; }
        public string KodOdpadu { get; set; }
        public string RodzajOdpadu { get; set; }
        public string Data { get; set; }
        public string DataMiesiąc { get; set; }
        public string MasaPrzekazanychOdpadów { get; set; }
        public string NumerRejestracyjnyPojazduPrzyczepyLubNaczepy { get; set; }
        public string OdpadPochodziZ { get; set; }

        public InformacjeDoKpo()
        {
        }

        public InformacjeDoKpo(DateTime data)
        {
            Data = data.ToString("dd.MM.yyyy");
        }
    }
}