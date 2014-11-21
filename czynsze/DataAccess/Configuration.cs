using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("konfig", Schema="public")]
    public class Configuration
    {
        [Key, Column("w_station")]
        public string w_station { get; set; }
        
        [Column("nazwa_1")]
        public string nazwa_1 { get; set; }

        [Column("nazwa_2zb")]
        public string nazwa_2zb { get; set; }

        [Column("nazwa_3zb")]
        public string nazwa_3zb { get; set; }

        [Column("p_20")]
        public int p_20 { get; set; }
        
        [Column("p_32")]
        public int p_32 { get; set; }

        [Column("p_37")]
        public int p_37 { get; set; }
    }
}