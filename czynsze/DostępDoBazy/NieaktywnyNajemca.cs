using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("najemc_a", Schema = "public")]
    public class NieaktywnyNajemca : Najemca
    {
        [Key, Column("nr_kontr"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public override int nr_kontr { get; set; }

        [Column("nazwisko")]
        public override string nazwisko { get; set; }

        [Column("imie")]
        public override string imie { get; set; }

        [Column("adres_1")]
        public override string adres_1 { get; set; }

        [Column("adres_2")]
        public override string adres_2 { get; set; }

        [Column("kod_najem")]
        public override int kod_najem { get; set; }

        [Column("nr_dow")]
        public override string nr_dow { get; set; }

        [Column("pesel")]
        public override string pesel { get; set; }

        [Column("nazwa_z")]
        public override string nazwa_z { get; set; }

        [Column("e_mail")]
        public override string e_mail { get; set; }

        [Column("l__has")]
        public override string l__has { get; set; }

        [Column("uwagi_1")]
        public override string uwagi_1 { get; set; }

        [Column("uwagi_2")]
        public override string uwagi_2 { get; set; }
    }
}