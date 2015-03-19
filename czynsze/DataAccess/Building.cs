using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("budynki", Schema = "public")]
    public class Building : IRecord
    {
        [Key, Column("kod_1"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_1 { get; set; }

        [Column("adres")]
        public string adres { get; set; }

        [Column("adres_2")]
        public string adres_2 { get; set; }

        [Column("il_miesz")]
        public int il_miesz { get; set; }

        [Column("sp_rozl")]
        public int sp_rozl { get; set; }

        [Column("udzial_w_k")]
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

        public void Set(string[] record)
        {
            kod_1 = Convert.ToInt16(record[0]);
            il_miesz = Convert.ToInt16(record[1]);
            sp_rozl = Convert.ToInt16(record[2]);
            adres = record[3];
            adres_2 = record[4];
            udzial_w_k = Convert.ToSingle(record[5]);

            record[6] = record[6].PadRight(420);

            uwagi_1 = record[6].Substring(0, 70).Trim();
            uwagi_2 = record[6].Substring(70, 70).Trim();
            uwagi_3 = record[6].Substring(140, 70).Trim();
            uwagi_4 = record[6].Substring(210, 70).Trim();
            uwagi_5 = record[6].Substring(280, 70).Trim();
            uwagi_6 = record[6].Substring(350).Trim();
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = "";
            int id;

            if (action == Enums.Action.Dodaj)
                if (record[0].Length > 0)
                {
                    try
                    {
                        id = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.buildings.Any(b => b.kod_1 == id))
                                result += "Kod budynku jest już używany! <br />";
                    }
                    catch { result += "Kod budynku musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod budynku! <br />";

            if (action != Enums.Action.Usuń)
            {
                if (record[1].Length > 0)
                    try { Convert.ToInt16(record[1]); }
                    catch { result += "Ilość lokali musi być liczbą całkowitą! <br />"; }
                else
                    record[1] = "0";

                if (record[5].Length > 0)
                    try { Convert.ToSingle(record[5]); }
                    catch { result += "Udział w kosztach musi być liczbą! <br />"; }
                else
                    record[5] = "0";
            }
            else
            {
                id = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.places.Any(p => p.kod_lok == id))
                        result += "Nie można usunąć budynku, w którym znajdują się lokale! <br />";
            }

            return result;
        }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_1.ToString(), 
                kod_1.ToString(), 
                adres, 
                adres_2 
            };
        }

        public string[] AllFields()
        {
            return new string[] 
            { 
                kod_1.ToString(), 
                il_miesz.ToString(), 
                sp_rozl.ToString(), 
                adres.Trim(), 
                adres_2.Trim(), 
                udzial_w_k.ToString(), 
                String.Concat(uwagi_1.Trim(), uwagi_2.Trim(), uwagi_3.Trim(), uwagi_4.Trim(), uwagi_5.Trim(), uwagi_6.Trim()) 
            };
        }
    }
}