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
    }
}