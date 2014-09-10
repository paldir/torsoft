using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("typ_kuch", Schema="public")]
    public class TypeOfKitchen
    {
        [Key, Column("kod_kuch")]
        public int kod_kuch { get; set; }

        [Column("typ_kuch")]
        public string typ_kuch { get; set; }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_kuch.ToString(), 
                typ_kuch 
            };
        }
    }
}