using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("budynki", Schema = "public")]
    public class Building
    {
        [Key, Column("kod_1")]
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

        public string[] ImportantFields()
        {
            return new string[] { kod_1.ToString(), kod_1.ToString(), adres, adres_2 };
        }

        public string[] AllFields()
        {
            string sp_rozl = null;

            switch (this.sp_rozl)
            {
                case 0:
                    sp_rozl = "budynek";
                    break;
                case 1:
                    sp_rozl = "lokale";
                    break;
            }

            return new string[] { kod_1.ToString(), il_miesz.ToString(), sp_rozl, adres, adres_2, udzial_w_k.ToString() };
        }
    }
}