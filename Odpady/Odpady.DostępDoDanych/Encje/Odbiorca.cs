using System.Collections.Generic;

namespace Odpady.DostępDoDanych
{
    public class Odbiorca : Rekord
    {
        public string NAZWA { get; set; }
        public string NAZWA_SKROCONA { get; set; }
        public string KOD_POCZTOWY { get; set; }
        public string MIASTO { get; set; }
        public string ULICA { get; set; }
        public string NR_DOMU { get; set; }
        public string NR_LOKALU { get; set; }
        public string NAZWA_1 { get; set; }
        public string KOD_POCZTOWY_1 { get; set; }
        public string MIASTO_1 { get; set; }
        public string ULICA_1 { get; set; }
        public string NR_DOMU_1 { get; set; }
        public string NR_LOKALU_1 { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string NR_REJESTROWY { get; set; }
        public string TELEFON { get; set; }
        public string EMAIL { get; set; }

        public List<string> ToList()
        {
            return new List<string>
            {
                NAZWA, NAZWA_SKROCONA, KOD_POCZTOWY, MIASTO, ULICA, NR_DOMU, NR_LOKALU, NAZWA_1,
                KOD_POCZTOWY_1, MIASTO_1, ULICA_1, NR_DOMU_1, NR_LOKALU_1, NIP, REGON, NR_REJESTROWY, TELEFON, EMAIL
            };
        }
    }
}
