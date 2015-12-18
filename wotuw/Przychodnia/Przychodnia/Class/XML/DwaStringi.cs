using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przychodnia.Class.XML
{
    class DwaStringi
    {
        public string Nazwa { get; private set; }
        public string Wartosc { get; private set; }

        public DwaStringi(string nazwa, string wartosc)
        {
            Nazwa = nazwa;
            Wartosc = wartosc;
        }

        public static string ZnajdzNazwePolaListy(string wartosc, List<DwaStringi> lista)
        {
            string wynik = null;
            foreach (var dwaStringi in lista)
                if (dwaStringi.Wartosc == wartosc) wynik = dwaStringi.Nazwa;
            return wynik;
        }

        public static string ZnajdzWartoscPolaListy(string nazwa, List<DwaStringi> lista)
        {
            string wynik = null;
            foreach (var dwaStringi in lista)
                if (dwaStringi.Nazwa == nazwa) wynik = dwaStringi.Wartosc;
            return wynik;
        }
    }
}
