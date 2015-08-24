using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("tyt_praw", Schema = "public")]
    public class TytułPrawny : Rekord
    {
        [Display(Name = "kod")]
        public int kod_praw { get; set; }

        [Display(Name = "tytuł prawny")]
        public string tyt_prawny { get; set; }

        [Display(Name = "kod")]
        public override int id
        {
            get { return kod_praw; }
            set { kod_praw = value; }
        }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                tyt_prawny 
            };
        }

        public override string[] PolaDoTabeli()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                kod_praw.ToString(), 
                tyt_prawny 
            };
        }

        public override string[] WszystkiePola()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                tyt_prawny.Trim()
            };
        }

        public override void Ustaw(string[] rekord)
        {
            kod_praw = Int32.Parse(rekord[0]);
            tyt_prawny = rekord[1];
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod_praw;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod_praw = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.TytułyPrawne.Any(t => t.kod_praw == kod_praw))
                                wynik += "Istnieje już tytuł prawny do lokali o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod tytułu prawnego do lokali musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod tytułu prawnego do lokali! <br />";
            }

            if (akcja == Enumeratory.Akcja.Usuń)
            {
                kod_praw = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.AktywneLokale.Any(t => t.kod_praw == kod_praw))
                        wynik += "Nie można usunąć tytułu prawnego do lokali, jeśli jest on używany! <br />";
            }

            return wynik;
        }
    }
}