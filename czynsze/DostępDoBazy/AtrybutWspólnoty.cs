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
        public override string kod_powiaz
        {
            get
            {
                Wspólnota wspólnota = Sesja.Obecna.MagazynRekordów.KodNaWspólnotę[Int32.Parse(kod_powiaz_NIE_UŻYWAĆ)];

                return wspólnota.__record.ToString();
            }
            set
            {
                Wspólnota wspólnota = Sesja.Obecna.MagazynRekordów.KluczNaWspólnotę[Int32.Parse(value)];
                kod_powiaz_NIE_UŻYWAĆ = wspólnota.kod.ToString();
            }
        }
    }
}