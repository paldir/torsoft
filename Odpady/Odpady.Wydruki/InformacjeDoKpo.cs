using System;

namespace Odpady.Wydruki
{
    public class InformacjeDoKpo
    {
        public string NrKarty { get; set; }
        public string RokKalendarzowy { get; set; }
        public string MiejsceProwadzeniaDziałalności1 { get; set; }
        public string NazwaIAdresPosiadaczaOdpadówTransportującegoOdpad { get; set; }
        public string NazwaIAdresPosiadaczaOdpadówKtóryPrzejmujeOdpad { get; set; }
        public string MiejsceProwadzeniaDziałalności2 { get; set; }
        public string NrRejestrowy1 { get; set; }
        public string NrRejestrowy2 { get; set; }
        public string Nip1 { get; set; }
        public string Regon1 { get; set; }
        public string Nip2 { get; set; }
        public string Regon2 { get; set; }
        public string PosiadaczOdpaduKtóremuNależyPrzekazaćOdpad { get; set; }
        public string KodOdpadu { get; set; }
        public string RodzajOdpadu { get; set; }
        public string Data { get; set; }
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