using System.Collections.Generic;
using System.Linq;

namespace Odpady.DostępDoDanych
{
    public class SzczegółDostawy : Rekord
    {
        public decimal? ILOSC { get; set; }
        public decimal? PONAD { get; set; }
        public long? FK_RODZAJ_ODPADOW { get; set; }
        public long? FK_DOSTAWA { get; set; }
        public long? FK_LIMIT { get; set; }

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<RodzajOdpadów>(FK_RODZAJ_ODPADOW.Value); }
        }

        public Dostawa DOSTAWA
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Dostawa>(FK_DOSTAWA.Value); }
        }

        public Limit LIMIT
        {
            get { return PołączenieDlaObcychObiektów.Pobierz<Limit>(FK_LIMIT.Value); }
        }

        public void UstalLimit(short osFiz, long idOddzial)
        {
            var warunkiLimit = new List<WarunekZapytania>()
            {
                new WarunekZapytania("FK_ODDZIAL", ZnakPorównania.RównaSię, idOddzial),
                new WarunekZapytania("FK_RODZAJ_ODPADOW", ZnakPorównania.RównaSię, FK_RODZAJ_ODPADOW),
                new WarunekZapytania("OSOBA_FIZYCZNA", ZnakPorównania.RównaSię, osFiz),
            };

            using (var polaczenie = new Odpady.DostępDoDanych.Połączenie())
            {
                var limit = polaczenie.PobierzWszystkie<Limit>(warunkiLimit).FirstOrDefault();
                if (limit == null)
                {
                    FK_LIMIT = 0;
                    return;
                }

                FK_LIMIT = limit.ID;
            }
        }
    }
}