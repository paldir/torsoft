using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    [System.ComponentModel.DataAnnotations.Schema.Table("pliki_lokal", Schema = "public")]
    public class PlikLokalu : Plik
    {
        public override int id_obiektu
        {
            get
            {
                DostępDoBazy.AktywnyLokal lokal = Sesja.Obecna.MagazynRekordów.Lokale.Single(l => l.nr_system == id_obiektu_NIE_UŻYWAĆ);

                return lokal.__record;
            }

            set
            {
                DostępDoBazy.AktywnyLokal lokal = Sesja.Obecna.MagazynRekordów.KluczNaLokal[value];
                id_obiektu_NIE_UŻYWAĆ = lokal.nr_system;
            }
        }
    }
}