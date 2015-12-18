using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public class ElementŚcieżkiStrony
    {
        public string Etykieta { get; set; }
        public string Link { get; set; }

        public ElementŚcieżkiStrony(string etykieta, string link = null)
        {
            Etykieta = etykieta;
            Link = link;
        }
    }
}