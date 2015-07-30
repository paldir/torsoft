using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("vat", Schema = "public")]
    public class StawkaVat : IRekord
    {
        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("nazwa")]
        public string nazwa { get; set; }

        [Column("symb_fisk")]
        public string symb_fisk { get; set; }

        public int id { get { return __record; } }

        public string[] PolaDoTabeli()
        {
            return new string[]
            {
                __record.ToString(),
                nazwa,
                symb_fisk
            };
        }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[]
            {
                nazwa,
                nazwa
            };
        }

        public string[] WszystkiePola()
        {
            return new string[]
            {
                __record.ToString(),
                nazwa.Trim(),
                symb_fisk.Trim()
            };
        }

        public void Ustaw(string[] rekord)
        {
            nazwa = rekord[1];
            symb_fisk = rekord[2];
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            string nazwa = rekord[1];

            if (akcja == Enumeratory.Akcja.Usuń)
                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.RodzajePłatności.Any(t => t.vat == nazwa))
                        wynik += "Nie można usunąć stawki VAT, ponieważ jest ona wykorzystywana w innych tabelach! <br />";

            return wynik;
        }
    }
}