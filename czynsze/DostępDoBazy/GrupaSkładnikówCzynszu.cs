using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("grup_cz", Schema = "public")]
    public class GrupaSkładnikówCzynszu : IRekord
    {
        [Key, DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        [PrzyjaznaNazwaPola("kod")]
        public int kod { get; set; }

        [PrzyjaznaNazwaPola("nazwa grupy składników czynszu")]
        public string nazwa { get; set; }

        [PrzyjaznaNazwaPola("kod")]
        [NotMapped]
        public int id
        {
            get { return kod; }
            set { kod = value; }
        }

        public string[] PolaDoTabeli()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                nazwa
            };
        }

        public string[] WszystkiePola()
        {
            return new string[]
            {
                kod.ToString(),
                nazwa.Trim()
            };
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
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

        public void Ustaw(string[] rekord)
        {
            kod = Int32.Parse(rekord[0]);
            nazwa = rekord[1];
        }
    }
}