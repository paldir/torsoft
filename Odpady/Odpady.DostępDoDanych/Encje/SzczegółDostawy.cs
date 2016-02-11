using System;
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

        private RodzajOdpadów _rodzaj_odpadow;

        public RodzajOdpadów RODZAJ_ODPADOW
        {
            get
            {
                UstawObcyObiekt(ref _rodzaj_odpadow, FK_RODZAJ_ODPADOW.Value);

                return _rodzaj_odpadow;
            }
        }

        private Dostawa _dostawa;

        public Dostawa DOSTAWA
        {
            get
            {
                UstawObcyObiekt(ref _dostawa, FK_DOSTAWA.Value);

                return _dostawa;
            }
        }

        private Limit _limit;

        public Limit LIMIT
        {
            get
            {
                UstawObcyObiekt(ref _limit, FK_LIMIT.Value);

                return _limit;
            }
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

        public decimal ObliczLacznie(long idKontrahent, DateTime data, long fkGrupa)
        {
            IEnumerable<SzczegółDostawy> lacznie;
            using (var polaczenie = new Odpady.DostępDoDanych.Połączenie())
            {
                lacznie =
                    polaczenie.PobierzWszystkie<SzczegółDostawy>().Where(o =>
                        o.DOSTAWA.FK_KONTRAHENT == idKontrahent &&
                        o.DOSTAWA.DATA <= data &&
                        (fkGrupa > 0 ? o.LIMIT.FK_GRUPA == fkGrupa : o.FK_RODZAJ_ODPADOW == FK_RODZAJ_ODPADOW));
            }

            return lacznie.Sum(tmp => (decimal) tmp.ILOSC);
        }

        public decimal LacznyLimit()
        {
            if (FK_LIMIT == 0) return 0;
            var limit = LIMIT;
            return (decimal)(limit.FK_GRUPA > 0 ? limit.GRUPA.LIMIT : limit.LIMIT);
        }
    }
}