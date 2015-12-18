using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    [Serializable]
    public class InformacjeOSortowalnościTablicy
    {
        public bool Sortowalna { get; private set; }
        public int IndeksKolumnySortującej { get; set; }

        public InformacjeOSortowalnościTablicy()
        {
            Sortowalna = false;
            IndeksKolumnySortującej = -1;
        }

        public InformacjeOSortowalnościTablicy(int indeksKolumny)
        {
            Sortowalna = true;

            if (indeksKolumny >= 0)
                IndeksKolumnySortującej = indeksKolumny;
            else
                throw new Exception("Indeks kolumny nie może być ujemny. - PZ");
        }
    }
}