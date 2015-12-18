﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("typ_kuch", Schema = "public")]
    public class TypKuchni : Rekord
    {
        [Display(Name = "kod")]
        public int kod_kuch { get; set; }

        [Display(Name = "rodzaj kuchni")]
        public string typ_kuch { get; set; }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[] 
            { 
                kod_kuch.ToString(), 
                typ_kuch 
            };
        }

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[] 
            { 
                kod_kuch.ToString(), 
                typ_kuch 
            });
        }

        /*public override void Ustaw(string[] rekord)
        {
            kod_kuch = Int32.Parse(rekord[0]);
            typ_kuch = rekord[1];
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod_kuch;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod_kuch = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.TypyKuchni.Any(t => t.kod_kuch == kod_kuch))
                                wynik += "Istnieje już rodzaj kuchni o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod rodzaju kuchni musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod rodzaju kuchni! <br />";
            }

            if (akcja == Enumeratory.Akcja.Usuń)
            {
                kod_kuch = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.AktywneLokale.Any(p => p.kod_kuch == kod_kuch))
                        wynik += "Nie można usunąć rodzaju kuchni, jeśli jest on używany! <br />";
            }

            return wynik;
        }*/
    }
}