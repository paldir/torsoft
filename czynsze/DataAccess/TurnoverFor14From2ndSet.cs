using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("obk_14__", Schema = "public")]
    public class TurnoverFor14From2ndSet : Turnover
    {
        [Key, Column("__record")]
        public override int __record { get; set; }

        [Column("suma")]
        public override float suma { get; set; }

        [Column("data_obr")]
        public override string data_obr { get; set; }

        [Column("opis")]
        public override string opis { get; set; }

        [Column("nr_kontr")]
        public override int nr_kontr { get; set; }

        [Column("kod_wplat")]
        public override int kod_wplat { get; set; }
    }
}