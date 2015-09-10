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
        public override string kod_powiaz
        {
            get
            {
                int kodLokalu = Int32.Parse(kod_powiaz_NIE_UŻYWAĆ.Substring(0, 5));
                int nrLokalu = Int32.Parse(kod_powiaz_NIE_UŻYWAĆ.Substring(5));
                DostępDoBazy.Lokal lokal = Sesja.Obecna.MagazynRekordów.KodINumerNaLokal[kodLokalu][nrLokalu];

                return lokal.__record.ToString();
            }

            set
            {
                DostępDoBazy.AktywnyLokal lokal = Sesja.Obecna.MagazynRekordów.KluczNaLokal[Int32.Parse(value)];
                kod_powiaz_NIE_UŻYWAĆ = String.Format("{0, 5}{1, 3}", lokal.kod_lok, lokal.nr_lok);
            }
        }
    }
}