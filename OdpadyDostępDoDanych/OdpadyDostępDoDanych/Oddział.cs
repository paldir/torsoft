using System;
using System.Collections.Generic;

namespace OdpadyDostępDoDanych
{
    public class Oddział : Rekord
    {
        public string PELNA_NAZWA { get; set; }
        public string SKROCONA_NAZWA { get; set; }
        public string KOD_POCZTOWY { get; set; }
        public string MIASTO { get; set; }
        public string ULICA { get; set; }
        public Nullable<int> NR_DOMU { get; set; }
        public Nullable<int> NR_LOKALU { get; set; }
        public string EMAIL { get; set; }
        public string TELEFON { get; set; }

        public List<string> ToList()
        {
            return new List<string>
            {
                PELNA_NAZWA,
                SKROCONA_NAZWA,
                KOD_POCZTOWY,
                MIASTO,
                ULICA,
                NR_DOMU.ToString(),
                NR_LOKALU.ToString(),
                TELEFON,
                EMAIL,
            };
        }
    }
}