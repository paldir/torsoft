using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("tabl_zak", Schema = "public")]
    public class Completed
    {
        [Key, Column("__record"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int __record { get; set; }

        [Column("rok")]
        public int rok { get; set; }

        [Column("miesiac")]
        public int miesiac { get; set; }

        [Column("z_rok")]
        public bool z_rok { get; set; }

        [Column("z_mies")]
        public bool z_mies { get; set; }
    }
}