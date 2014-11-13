using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("lokale", Schema = "public")]
    public class ActivePlace : Place
    {
        [Key, Column("nr_system"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public override int nr_system { get; set; }

        [Column("kod_lok")]
        public override int kod_lok { get; set; }

        [Column("nr_lok")]
        public override int nr_lok { get; set; }

        [Column("kod_typ")]
        public override int kod_typ { get; set; }

        [Column("pow_uzyt")]
        public override float pow_uzyt { get; set; }

        [Column("nazwisko")]
        public override string nazwisko { get; set; }

        [Column("imie")]
        public override string imie { get; set; }

        [Column("adres")]
        public override string adres { get; set; }

        [Column("adres_2")]
        public override string adres_2 { get; set; }

        [Column("pow_miesz")]
        public override float pow_miesz { get; set; }

        [Column("udzial")]
        public override float udzial { get; set; }

        [Column("dat_od")]
        public override string dat_od { get; set; }

        [Column("dat_do")]
        public override string dat_do { get; set; }

        [Column("p_1")]
        public override float p_1 { get; set; }

        [Column("p_2")]
        public override float p_2 { get; set; }

        [Column("p_3")]
        public override float p_3 { get; set; }

        [Column("p_4")]
        public override float p_4 { get; set; }

        [Column("p_5")]
        public override float p_5 { get; set; }

        [Column("p_6")]
        public override float p_6 { get; set; }

        [Column("kod_kuch")]
        public override Nullable<int> kod_kuch { get; set; }

        [Column("nr_kontr")]
        public override Nullable<int> nr_kontr { get; set; }

        [Column("il_osob")]
        public override Nullable<int> il_osob { get; set; }

        [Column("kod_praw")]
        public override Nullable<int> kod_praw { get; set; }

        [Column("uwagi_1")]
        public override string uwagi_1 { get; set; }

        [Column("uwagi_2")]
        public override string uwagi_2 { get; set; }

        [Column("uwagi_3")]
        public override string uwagi_3 { get; set; }

        [Column("uwagi_4")]
        public override string uwagi_4 { get; set; }
    }
}