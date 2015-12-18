using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;

namespace czynsze.DostępDoBazy
{
    public abstract class Plik
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int __record { get; set; }

        [Column("id_obiektu")]
        public int id_obiektu_W_BAZIE { get; protected set; }

        [NotMapped]
        public abstract int id_obiektu { get; set; }

        public string plik { get; set; }

        public string nazwa_pliku { get; set; }

        public string opis { get; set; }
    }
}