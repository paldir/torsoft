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

        public override string kod_powiaz
        {
            get
            {
                Budynek budynek = Sesja.Obecna.MagazynRekordów.KodNaBudynek[Int32.Parse(kod_powiaz_NIE_UŻYWAĆ)];

                return budynek.__record.ToString();
            }

            set
            {
                Budynek budynek = Sesja.Obecna.MagazynRekordów.KluczNaBudynek[Int32.Parse(value)];
                kod_powiaz_NIE_UŻYWAĆ = budynek.kod_1.ToString();
            }
        }
    }
}