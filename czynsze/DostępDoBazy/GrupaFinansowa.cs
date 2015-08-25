using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("grup_fi", Schema = "public")]
    public class GrupaFinansowa : Rekord
    {
        [Display(Name = "kod")]
        public int kod { get; set; }

        [Display(Name = "nazwa grupy finansowej")]
        public string nazwa { get; set; }

        [Display(Name = "konto FK")]
        public string k_syn { get; set; }

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[]
            {
                kod.ToString(),
                k_syn,
                nazwa
            });
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.GrupyFinansowe.Any(t => t.kod == kod))
                                wynik += "Istnieje już grupa finansowa o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod grupy finansowej musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod grupy finansowej! <br />";
            }

            if (akcja == Enumeratory.Akcja.Usuń)
            {
                //
                //TODO
                //
            }

            return wynik;
        }

        public override void Ustaw(string[] rekord)
        {
            kod = Int32.Parse(rekord[0]);
            k_syn = rekord[1];
            nazwa = rekord[2];
        }
    }
}