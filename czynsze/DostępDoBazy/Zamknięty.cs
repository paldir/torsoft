using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("tabl_zak", Schema = "public")]
    public class Zamknięty
    {
        [Key, DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int __record { get; set; }

        public int rok { get; set; }

        public int miesiac { get; set; }

        public bool z_rok { get; set; }

        public bool z_mies { get; set; }
    }
}