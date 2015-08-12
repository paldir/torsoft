using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("cechy_l", Schema = "public")]
    public class AtrybutLokalu : AtrybutObiektu
    {
        [Key, Column("__record")]
        public override int __record { get; set; }

        [Column("kod")]
        public override int kod { get; set; }

        [Column("kod_powiaz")]
        string kod_powiaz_ { get; set; }

        public override string kod_powiaz
        {
            get
            {
                if (String.IsNullOrEmpty(kod_powiaz_))
                    return "0";
                else
                {
                    string[] kod = new string[] { kod_powiaz_.Substring(0, 5), kod_powiaz_.Substring(5, 5) };

                    using (DostępDoBazy.CzynszeKontekst db = new CzynszeKontekst())
                    {
                        DostępDoBazy.Lokal lokal = db.AktywneLokale.AsEnumerable().Single(l => l.kod_lok == Int32.Parse(kod[0]) && l.nr_lok == Int32.Parse(kod[1]));

                        return lokal.nr_system.ToString();
                    }
                }
            }

            set
            {
                try
                {
                    using (DostępDoBazy.CzynszeKontekst db = new CzynszeKontekst())
                    {
                        DostępDoBazy.Lokal lokal = db.AktywneLokale.AsEnumerable().Single(l => l.nr_system.ToString() == value);
                        kod_powiaz_ = String.Format("{0, 5} {1, 5}", lokal.kod_lok, lokal.nr_lok);
                    }
                }
                catch (Exception) { kod_powiaz_ = value; }
            }
        }

        [Column("wartosc_n")]
        public override float wartosc_n { get; set; }

        [Column("wartosc_s")]
        public override string wartosc_s { get; set; }
    }
}