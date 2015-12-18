﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("grup_cz", Schema = "public")]
    public class GrupaSkładnikówCzynszu : Rekord
    {
        [Display(Name = "kod")]
        public int kod { get; set; }

        [Display(Name = "nazwa grupy składników czynszu")]
        public string nazwa { get; set; }

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[]
            {
                kod.ToString(),
                nazwa
            });
        }

        /*public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
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
                            if (db.GrupySkładnikówCzynszu.Any(a => a.kod == kod))
                                wynik += "Istnieje już grupa składników czynszu o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod grupy składników czynszu musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod grupy składników czynszu! <br />";
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
            nazwa = rekord[1];
        }*/
    }
}