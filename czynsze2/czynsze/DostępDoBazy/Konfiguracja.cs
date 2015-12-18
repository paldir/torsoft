using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("konfig", Schema="public")]
    public class Konfiguracja
    {
        [Key]
        public string w_station { get; set; }
        
        public string nazwa_1 { get; set; }

        public string nazwa_2zb { get; set; }

        public string nazwa_3zb { get; set; }

        public int p_20 { get; set; }

        public int p_32 { get; set; }

        public int p_37 { get; set; }

        public int p_46 { get; set; }
    }
}