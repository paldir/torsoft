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
        /*[Key, Column("__record")]
        public override int __record { get; set; }

        [Column("kod")]
        public override int kod { get; set; }*/

        public override string kod_powiaz_
        {
            get
            {
                if (String.IsNullOrEmpty(kod_powiaz))
                    return "0";
                else
                {
                    /*int wynik;

                    if (Int32.TryParse(kod_powiaz_, out wynik))
                        return kod_powiaz_;
                    else*/
                    {
                        string[] kod = new string[] { kod_powiaz.Substring(0, 5), kod_powiaz.Substring(5, 5) };
                        DostępDoBazy.Lokal lokal = Lokale.Single(l => l.kod_lok == Int32.Parse(kod[0]) && l.nr_lok == Int32.Parse(kod[1]));

                        return lokal.__record.ToString();
                    }
                }
            }

            set
            {
                try
                {
                    DostępDoBazy.Lokal lokal = Lokale.Single(l => l.__record == Int32.Parse(value));
                    kod_powiaz = String.Format("{0, 5} {1, 5}", lokal.kod_lok, lokal.nr_lok);
                }
                catch (Exception) { kod_powiaz = value; }
            }
        }

        public static List<AktywnyLokal> Lokale { get; set; }

        /*[Column("wartosc_n")]
        public override float wartosc_n { get; set; }

        [Column("wartosc_s")]
        public override string wartosc_s { get; set; }*/
    }
}