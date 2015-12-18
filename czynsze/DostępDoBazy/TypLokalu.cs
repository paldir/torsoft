using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("typ_lok", Schema = "public")]
    public class TypLokalu : Rekord
    {
        [Display(Name = "kod")]
        public int kod_typ { get; set; }

        [Display(Name = "typ lokalu")]
        public string typ_lok { get; set; }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[] 
            { 
                kod_typ.ToString(), 
                typ_lok 
            };
        }

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[] 
            { 
                kod_typ.ToString(), 
                typ_lok 
            });
        }

        /*public override void Ustaw(string[] rekord)
        {
            kod_typ = Int32.Parse(rekord[0]);
            typ_lok = rekord[1];
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod_typ;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod_typ = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.TypyLokali.Any(t => t.kod_typ == kod_typ))
                                wynik += "Istnieje już typ lokali o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod typu lokali musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod typu lokali! <br />";
            }

            if (akcja == Enumeratory.Akcja.Usuń)
            {
                kod_typ = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.AktywneLokale.Any(p => p.kod_typ == kod_typ))
                        wynik += "Nie można usunąć typu lokali, jeśli jest on używany! <br />";
            }

            return wynik;
        }*/
    }
}