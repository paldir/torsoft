﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("obk_" + Turnover.TurnoverYear + "__", Schema = "public")]
    public class TurnoverFrom2ndSet : Turnover
    {
        [Key, Column("__record")]
        public override int __record { get; set; }

        [Column("suma")]
        public override decimal suma { get; set; }

        [Column("data_obr")]
        public override DateTime data_obr { get; set; }

        [Column("opis")]
        public override string opis { get; set; }

        [Column("nr_kontr")]
        public override int nr_kontr { get; set; }

        [Column("kod_wplat")]
        public override int kod_wplat { get; set; }

        [Column("nr_dowodu")]
        public override string nr_dowodu { get; set; }

        [Column("pozycja_d")]
        public override int pozycja_d { get; set; }

        [Column("uwagi")]
        public override string uwagi { get; set; }
    }
}