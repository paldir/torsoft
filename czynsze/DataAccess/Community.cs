using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("wspol", Schema = "public")]
    public class Community : IRecord
    {
        [Key, Column("kod"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod { get; set; }

        [Column("nazwa_skr")]
        public string nazwa_skr { get; set; }

        [Column("il_bud")]
        public int il_bud { get; set; }

        [Column("il_miesz")]
        public int il_miesz { get; set; }

        [Column("nazwa_pel")]
        public string nazwa_pel { get; set; }

        [Column("adres")]
        public string adres { get; set; }

        [Column("adres_2")]
        public string adres_2 { get; set; }

        [Column("nr1_konta")]
        public string nr1_konta { get; set; }

        [Column("nr2_konta")]
        public string nr2_konta { get; set; }

        [Column("nr3_konta")]
        public string nr3_konta { get; set; }

        [Column("sciezka_fk")]
        public string sciezka_fk { get; set; }

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

        public string[] ImportantFields()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                nazwa_skr,
                il_bud.ToString(),
                il_miesz.ToString()
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                kod.ToString(),
                il_bud.ToString(),
                il_miesz.ToString(),
                nazwa_pel.Trim(),
                nazwa_skr.Trim(),
                adres.Trim(),
                adres_2.Trim(),
                nr1_konta.Trim(),
                nr2_konta.Trim(),
                nr3_konta.Trim(),
                sciezka_fk.Trim(),
                String.Concat(uwagi_1.Trim(), uwagi_2.Trim(), uwagi_3.Trim(), uwagi_4.Trim(), uwagi_5.Trim(), uwagi_6.Trim())
            };
        }

        public void Set(string[] record)
        {
            kod = Convert.ToInt16(record[0]);
            il_bud = Convert.ToInt16(record[1]);
            il_miesz = Convert.ToInt16(record[2]);
            nazwa_pel = record[3];
            nazwa_skr = record[4];
            adres = record[5];
            adres_2 = record[6];
            nr1_konta = record[7];
            nr2_konta = record[8];
            nr3_konta = record[9];
            sciezka_fk = record[10];

            record[11] = record[11].PadRight(420);

            uwagi_1 = record[11].Substring(0, 70).Trim();
            uwagi_2 = record[11].Substring(70, 70).Trim();
            uwagi_3 = record[11].Substring(140, 70).Trim();
            uwagi_4 = record[11].Substring(210, 70).Trim();
            uwagi_5 = record[11].Substring(280, 70).Trim();
            uwagi_6 = record[11].Substring(350).Trim();
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = "";
            int kod;

            if (action == Enums.Action.Dodaj)
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.communities.Any(c => c.kod == kod))
                                result += "Kod wspólnoty jest już używany! <br />";
                    }
                    catch { result += "Kod wspólnoty musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod wspólnoty! <br />";

            if (action != Enums.Action.Usuń)
            {
                result += Czynsze_Entities.ValidateInt("Ilość budynków", ref record[1]);
                result += Czynsze_Entities.ValidateInt("Ilość lokali", ref record[2]);
            }

            return result;
        }
    }
}