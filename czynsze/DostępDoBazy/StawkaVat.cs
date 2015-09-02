using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("vat", Schema = "public")]
    public class StawkaVat : Rekord
    {
        [Display(Name = "oznaczenie stawki")]
        public string nazwa { get; set; }

        [Display(Name = "symbol fiskalny")]
        public string symb_fisk { get; set; }

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[]
            {
                nazwa,
                symb_fisk
            });
        }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[]
            {
                nazwa,
                nazwa
            };
        }

        /*public override void Ustaw(string[] rekord)
        {
            nazwa = rekord[1];
            symb_fisk = rekord[2];
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            string nazwa = rekord[1];

            if (akcja == Enumeratory.Akcja.Usuń)
                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.RodzajePłatności.Any(t => t.vat == nazwa))
                        wynik += "Nie można usunąć stawki VAT, ponieważ jest ona wykorzystywana w innych tabelach! <br />";

            return wynik;
        }*/
    }
}