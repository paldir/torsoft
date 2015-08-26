using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("cechy_b", Schema = "public")]
    public class AtrybutBudynku : AtrybutObiektu
    {
        /*[Key]
        public override int __record { get; set; }

        public override int kod { get; set; }

        public override string kod_powiaz { get; set; }

        public override float wartosc_n { get; set; }

        public override string wartosc_s { get; set; }*/
    }
}