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
        public override string kod_powiaz
        {
            get
            {
                Najemca najemca = Sesja.Obecna.MagazynRekordów.NrKontrNaNajemcę[Int32.Parse(kod_powiaz_NIE_UŻYWAĆ)];

                return najemca.__record.ToString();
            }

            set
            {
                Najemca najemca = Sesja.Obecna.MagazynRekordów.KluczNaNajemcę[Int32.Parse(value)];
                kod_powiaz_NIE_UŻYWAĆ = najemca.nr_kontr.ToString();
            }
        }
    }
}