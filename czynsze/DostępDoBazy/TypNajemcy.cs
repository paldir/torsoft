using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("typ_naje", Schema = "public")]
    public class TypNajemcy : IRekord
    {
        [Key, Column("kod_najem"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_najem { get; set; }

        [Column("r_najemcy")]
        public string r_najemcy { get; set; }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[] 
            { 
                kod_najem.ToString(), 
                r_najemcy 
            };
        }

        public string[] WażnePola()
        {
            return new string[] 
            { 
                kod_najem.ToString(),
                kod_najem.ToString(), 
                r_najemcy 
            };
        }

        public string[] WszystkiePola()
        {
            return new string[] 
            { 
                kod_najem.ToString(), 
                r_najemcy.Trim() 
            };
        }

        public void Ustaw(string[] rekord)
        {
            kod_najem = Int32.Parse(rekord[0]);
            r_najemcy = rekord[1];
        }

        public string Waliduj(Enums.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod_najem;

            if (akcja == Enums.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod_najem = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.TypyNajemców.Any(t => t.kod_najem == kod_najem))
                                wynik += "Istnieje już rodzaj najemców o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod rodzaju najemców musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod rodzaju najemców! <br />";
            }

            if (akcja == Enums.Akcja.Usuń)
            {
                kod_najem = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.AktywniNajemcy.Any(t => t.kod_najem == kod_najem))
                        wynik += "Nie można usunąć rodzaju najemców, jeśli jest on używany! <br />";
            }

            return wynik;
        }
    }
}