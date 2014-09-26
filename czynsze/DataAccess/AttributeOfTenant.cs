using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("cechy_n", Schema = "public")]
    public class AttributeOfTenant : AttributeOfObject
    {
        [Key, Column("__record")]
        public override int __record { get; set; }

        [Column("kod")]
        public override int kod { get; set; }

        [Column("kod_powiaz")]
        public override string kod_powiaz { get; set; }

        [Column("wartosc_n")]
        public override float wartosc_n { get; set; }

        [Column("wartosc_s")]
        public override string wartosc_s { get; set; }
    }
}