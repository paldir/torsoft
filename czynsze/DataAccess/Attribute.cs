using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("cechy", Schema = "public")]
    public class Attribute
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
        public float wartosc_n { get; set; }

        [Column("wartosc_s")]
        public string wartosc_s { get; set; }

        [Column("uwagi")]
        public string uwagi { get; set; }

        public string[] ImportantFields()
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

        public string[] ImportantFieldsForDropDown()
        {
            return new string[]
            {
                kod.ToString(),
                nazwa
            };
        }

        public string[] AllFields()
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

        public void Set(string[] record)
        {
            kod = Convert.ToInt16(record[0]);
            nazwa = record[1];
            nr_str = record[2];
            jedn = record[3];

            switch (nr_str)
            {
                case "N":
                    wartosc_n = Convert.ToSingle(record[4]);
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

        public static string Validate(EnumP.Action action, string[] record)
        {
            string result = String.Empty;
            int kod;

            if (action == EnumP.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.attributes.Count(a => a.kod == kod) != 0)
                                result += "Istnieje już cecha obiektów o podanym kodzie! <br />";
                    }
                    catch { result += "Kod cechy obiektów musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod cechy obiektów! <br />";
            }

            if (action != EnumP.Action.Usuń)
            {
                if (record[2] == "N")
                    result += Czynsze_Entities.ValidateFloat("Wartość domyślna", ref record[4]);
                else
                    record[3] = String.Empty;

                for (int i = 0; i < 4; i++)
                    if (record[i + 6] != null)
                        record[i + 6] = "X";
            }
            else
            {
                kod = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                {
                    //
                    // TODO
                    //
                }
            }

            return result;
        }
    }
}