using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public class ŚcieżkaStrony
    {
        IEnumerable<string> _etykiety { get { return Elementy.Select(e => e.Etykieta); } }

        public List<ElementŚcieżkiStrony> Elementy { get; private set; }

        public ŚcieżkaStrony()
        {
            Elementy = new List<ElementŚcieżkiStrony>();
        }

        public ŚcieżkaStrony(params string[] etykiety)
        {
            Elementy = etykiety.Select(e => new ElementŚcieżkiStrony(e)).ToList();
        }

        public ŚcieżkaStrony(params ElementŚcieżkiStrony[] elementy)
        {
            Elementy = elementy.ToList();
        }

        public void Dodaj(string etykieta, string link = null)
        {
            if (!_etykiety.Contains(etykieta))
                Elementy.Add(new ElementŚcieżkiStrony(etykieta, link));
        }

        public void Wyczyść()
        {
            Elementy.Clear();
        }
    }
}