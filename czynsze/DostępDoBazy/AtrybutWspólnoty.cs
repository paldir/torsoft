using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("cechy_s", Schema = "public")]
    public class AtrybutWspólnoty : AtrybutObiektu
    {
        public static List<Wspólnota> Wspólnoty { get; set; }

        public override string kod_powiaz
        {
            get
            {
                Wspólnota wspólnota = Wspólnoty.Single(w => w.kod == Int32.Parse(kod_powiaz_NIE_UŻYWAĆ));

                return wspólnota.__record.ToString();
            }
            set
            {
                Wspólnota wspólnota = Wspólnoty.Single(w => w.__record == Int32.Parse(value));
                kod_powiaz_NIE_UŻYWAĆ = wspólnota.kod.ToString();
            }
        }
    }
}