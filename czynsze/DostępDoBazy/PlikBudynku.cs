using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    [System.ComponentModel.DataAnnotations.Schema.Table("pliki_budynek", Schema = "public")]
    public class PlikBudynku : Plik
    {
        public override int id_obiektu
        {
            get
            {
                DostępDoBazy.Budynek budynek = Sesja.Obecna.MagazynRekordów.KodNaBudynek[id_obiektu_W_BAZIE];

                return budynek.__record;
            }

            set
            {
                DostępDoBazy.Budynek budynek = Sesja.Obecna.MagazynRekordów.KluczNaBudynek[value];
                id_obiektu_W_BAZIE = budynek.kod_1;
            }
        }
    }
}