using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("budynki", Schema = "public")]
    public class Budynek : IRekord
    {
        [Key, Column("kod_1"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        [PrzyjaznaNazwaPola("kod budynku")]
        public int kod_1 { get; set; }

        [Column("adres")]
        [PrzyjaznaNazwaPola("adres")]
        public string adres { get; set; }

        [Column("adres_2")]
        [PrzyjaznaNazwaPola("adres cd.")]
        public string adres_2 { get; set; }

        [Column("il_miesz")]
        [PrzyjaznaNazwaPola("ilość mieszkań")]
        public int il_miesz { get; set; }

        [Column("sp_rozl")]
        [PrzyjaznaNazwaPola("sposób rozliczania")]
        public int sp_rozl { get; set; }

        [Column("udzial_w_k")]
        [PrzyjaznaNazwaPola("udział w koszt.")]
        public float udzial_w_k { get; set; }

        [Column("uwagi_1")]
        public string uwagi_1 { get; set; }

        [Column("uwagi_2")]
        public string uwagi_2 { get; set; }

        [Column("uwagi_3")]
        public string uwagi_3 { get; set; }

        [Column("uwagi_4")]
        public string uwagi_4 { get; set; }

        [Column("uwagi_5")]
        public string uwagi_5 { get; set; }

        [Column("uwagi_6")]
        public string uwagi_6 { get; set; }

        [PrzyjaznaNazwaPola("uwagi")]
        public string uwagi
        {
            get { return String.Concat(uwagi_1, uwagi_2, uwagi_3, uwagi_4, uwagi_5, uwagi_6).Trim(); }
        }

        [PrzyjaznaNazwaPola("kod budynku")]
        public int id { get { return kod_1; } }

        public void Ustaw(string[] rekord)
        {
            kod_1 = Int32.Parse(rekord[0]);
            il_miesz = Int32.Parse(rekord[1]);
            sp_rozl = Int32.Parse(rekord[2]);
            adres = rekord[3];
            adres_2 = rekord[4];
            udzial_w_k = Single.Parse(rekord[5]);

            RozparcelujUwagi(rekord[6]);
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = "";
            int id;

            if (akcja == Enumeratory.Akcja.Dodaj)
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        id = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.Budynki.Any(b => b.kod_1 == id))
                                wynik += "Kod budynku jest już używany! <br />";
                    }
                    catch { wynik += "Kod budynku musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod budynku! <br />";

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                if (rekord[1].Length > 0)
                    try { Int32.Parse(rekord[1]); }
                    catch { wynik += "Ilość lokali musi być liczbą całkowitą! <br />"; }
                else
                    rekord[1] = "0";

                if (rekord[5].Length > 0)
                    try { Single.Parse(rekord[5]); }
                    catch { wynik += "Udział w kosztach musi być liczbą! <br />"; }
                else
                    rekord[5] = "0";
            }
            else
            {
                id = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.AktywneLokale.Any(p => p.kod_lok == id))
                        wynik += "Nie można usunąć budynku, w którym znajdują się lokale! <br />";
            }

            return wynik;
        }

        public string[] PolaDoTabeli()
        {
            return new string[] 
            { 
                kod_1.ToString(), 
                kod_1.ToString(), 
                adres, 
                adres_2 
            };
        }

        public string[] WszystkiePola()
        {
            return new string[] 
            { 
                kod_1.ToString(), 
                il_miesz.ToString(), 
                sp_rozl.ToString(), 
                adres.Trim(), 
                adres_2.Trim(), 
                udzial_w_k.ToString(), 
                uwagi.Trim()
            };
        }

        void RozparcelujUwagi(string uwagi)
        {
            string wartość = uwagi.PadRight(420);
            uwagi_1 = wartość.Substring(0, 70).Trim();
            uwagi_2 = wartość.Substring(70, 70).Trim();
            uwagi_3 = wartość.Substring(140, 70).Trim();
            uwagi_4 = wartość.Substring(210, 70).Trim();
            uwagi_5 = wartość.Substring(280, 70).Trim();
            uwagi_6 = wartość.Substring(350).Trim();
        }
    }
}