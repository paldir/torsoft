using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    public abstract class PozycjaDoAnalizy : Rekord
    {
        public abstract DateTime Data { get; }
        public abstract decimal Kwota { get; }
        public abstract float Ilość { get; }
        public abstract decimal Stawka { get; }
        public abstract int IdInformacji { get; }
        public abstract int KodBudynku { get; }
        public abstract int NrLokalu { get; }
        public IInformacjeOPozycji Informacje { get; set; }

        /*public abstract override void Ustaw(string[] rekord);
        public abstract override string Waliduj(Enumeratory.Akcja akcja, string[] rekord);*/
        public override IEnumerable<string> PolaDoTabeli() { return base.PolaDoTabeli(); }
    }
}