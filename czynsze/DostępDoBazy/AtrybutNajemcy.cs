using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("cechy_n", Schema = "public")]
    public class AtrybutNajemcy : AtrybutObiektu
    {
        public static List<AktywnyNajemca> Najemcy { get; set; }
        
        public override string kod_powiaz
        {
            get
            {
                Najemca najemca = Najemcy.Single(n => n.nr_kontr == Int32.Parse(kod_powiaz_));

                return najemca.__record.ToString();
            }

            set
            {
                Najemca najemca = Najemcy.Single(n => n.__record == Int32.Parse(value));
                kod_powiaz_ = najemca.nr_kontr.ToString();
            }
        }
    }
}