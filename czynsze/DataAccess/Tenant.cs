using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("najemcy", Schema="public")]
    public class Tenant
    {
        [Key, Column("nr_kontr")]
        public int nr_kontr { get; set; }

        [Column("nazwisko")]
        public string nazwisko { get; set; }

        [Column("imie")]
        public string imie { get; set; }

        [Column("adres_1")]
        public string adres_1 { get; set; }

        [Column("adres_2")]
        public string adres_2 { get; set; }

        public string[] ImportantFields()
        {
            return new string[] { nr_kontr.ToString(), nazwisko, imie, adres_1, adres_2 };
        }
    }
}