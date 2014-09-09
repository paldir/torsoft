using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("typ_lok", Schema = "public")]
    public class TypeOfPlace
    {
        [Key, Column("kod_typ")]
        public int kod_typ { get; set; }

        [Column("typ_lok")]
        public string typ_lok { get; set; }

        public string[] ImportantFields()
        {
            return new string[] { kod_typ.ToString(), typ_lok };
        }
    }
}