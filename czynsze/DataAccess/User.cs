using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("fk_tuz", Schema = "public")]
    public class User
    {
        [Key, Column("uzytkownik")]
        public string uzytkownik { get; set; }

        [Column("haslo")]
        public string haslo { get; set; }
    }
}