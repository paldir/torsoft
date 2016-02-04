using System;

namespace Odpady.DostępDoDanych
{
    public class Kpo : Rekord
    {
        public string NR_KARTY { get; set; }
        public string ROK_KALENDARZOWY { get; set; }
        public long? FK_ODPAD { get; set; }
        public decimal? MASA { get; set; }
        public DateTime? DATA { get; set; }
        public string NUMER_REJESTRACYJNY { get; set; }
        public string TRANSPORTUJACY_2 { get; set; }
        public string PRZEJMUJACY_3 { get; set; }
        public string MIEJSCE_DZIALALNOSCI_1 { get; set; }
        public string MIEJSCE_DZIALALNOSCI_3 { get; set; }
        public string NR_REJESTROWY_3 { get; set; }
        public string NR_REJESTROWY_2 { get; set; }
        public string NIP_3 { get; set; }
        public string NIP_2 { get; set; }
        public string ODBIORCA { get; set; }
        public string ODPAD_POCHODZI_Z { get; set; }
        public string REGON_2 { get; set; }
        public string REGON_3 { get; set; }
        public long? FK_ODDZIAL { get; set; }
        public string DATA_MIESIAC { get; set; }

        public RodzajOdpadów ODPAD
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<RodzajOdpadów>(FK_ODPAD.Value); }
        }

        public Oddział ODDZIAL
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Oddział>(FK_ODDZIAL.Value); }
        }
    }
}