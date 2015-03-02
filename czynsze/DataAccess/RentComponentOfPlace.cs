using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("skl_cz", Schema = "public")]
    public class RentComponentOfPlace
    {
        [Key, Column("kod_lok", Order = 0)]
        public int kod_lok { get; set; }

        [Key, Column("nr_lok", Order = 1)]
        public int nr_lok { get; set; }

        [Key, Column("nr_skl", Order = 2)]
        public int nr_skl { get; set; }

        [Column("dan_p")]
        public float dan_p { get; set; }

        [Column("dat_od")]
        public string dat_od { get; set; }

        [Column("dat_do")]
        public string dat_do { get; set; }
    }
}