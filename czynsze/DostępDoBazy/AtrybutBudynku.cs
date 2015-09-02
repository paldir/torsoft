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
        public static List<Budynek> Budynki { get; set; }

        public override string kod_powiaz
        {
            get
            {
                Budynek budynek = Budynki.Single(b => b.kod_1 == Int32.Parse(kod_powiaz_));

                return budynek.__record.ToString();
            }

            set
            {
                Budynek budynek = Budynki.Single(b => b.__record == Int32.Parse(value));
                kod_powiaz_ = budynek.kod_1.ToString();
            }
        }
    }
}