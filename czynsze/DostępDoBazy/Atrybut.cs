using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("cechy", Schema = "public")]
    public class Atrybut : IRekord
    {
        [Key, Column("kod"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod { get; set; }

        [Column("nazwa")]
        public string nazwa { get; set; }

        [Column("nr_str")]
        public string nr_str { get; set; }

        [Column("zb_l")]
        public string zb_l { get; set; }

        [Column("zb_n")]
        public string zb_n { get; set; }

        [Column("zb_b")]
        public string zb_b { get; set; }

        [Column("zb_s")]
        public string zb_s { get; set; }

        [Column("jedn")]
        public string jedn { get; set; }

        [Column("wartosc_n")]
        public decimal wartosc_n { get; set; }

        [Column("wartosc_s")]
        public string wartosc_s { get; set; }

        [Column("uwagi")]
        public string uwagi { get; set; }

        public string[] WażnePola()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                nazwa,
                nr_str,
                zb_l,
                zb_n,
                zb_b,
                zb_s
            };
        }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[]
            {
                kod.ToString(),
                nazwa
            };
        }

        public string[] WszystkiePola()
        {
            string wartosc = String.Empty;

            switch (nr_str)
            {
                case "N":
                    wartosc = wartosc_n.ToString("F2");

                    break;

                case "C":
                    wartosc = wartosc_s.Trim();

                    break;
            }

            return new string[]
            {
                kod.ToString(),
                nazwa.Trim(),
                nr_str,
                jedn.Trim(),
                wartosc,
                uwagi.Trim(),
                zb_l,
                zb_n,
                zb_b,
                zb_s
            };
        }

        public void Ustaw(string[] record)
        {
            kod = Int32.Parse(record[0]);
            nazwa = record[1];
            nr_str = record[2];
            jedn = record[3];

            switch (nr_str)
            {
                case "N":
                    wartosc_n = Decimal.Parse(record[4]);

                    break;

                case "C":
                    wartosc_s = record[4];

                    break;
            }

            uwagi = record[5];
            zb_l = record[6];
            zb_n = record[7];
            zb_b = record[8];
            zb_s = record[9];
        }

        public string Waliduj(Enums.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod;

            if (akcja == Enums.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.Atrybuty.Any(a => a.kod == kod))
                                wynik += "Istnieje już cecha obiektów o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod cechy obiektów musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod cechy obiektów! <br />";
            }

            if (akcja != Enums.Akcja.Usuń)
            {
                if (rekord[2] == "N")
                    wynik += CzynszeKontekst.WalidujFloat("Wartość domyślna", ref rekord[4]);
                else
                    rekord[3] = String.Empty;

                for (int i = 0; i < 4; i++)
                    if (rekord[i + 6] != null)
                        rekord[i + 6] = "X";
            }
            else
            {
                kod = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                {
                    //
                    // TODO
                    //
                }
            }

            return wynik;
        }
    }
}