using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("typ_naje", Schema="public")]
    public class TypeOfTenant
    {
        [Key, Column("kod_najem")]
        public int kod_najem { get; set; }

        [Column("r_najemcy")]
        public string r_najemcy { get; set; }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_najem.ToString(), 
                r_najemcy 
            };
        }
    }
}