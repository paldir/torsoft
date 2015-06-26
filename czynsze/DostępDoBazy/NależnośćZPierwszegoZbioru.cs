using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("nal_" + Należność.Rok + "__", Schema = "public")]
    public class NależnośćZPierwszegoZbioru : Należność
    {
        [Key, Column("__record")]
        public override int __record { get; set; }

        [Column("kwota_nal")]
        public override decimal Kwota { get; set; }

        [Column("data_nal")]
        public override DateTime Data { get; set; }

        [Column("opis")]
        public override string opis { get; set; }

        [Column("kod_lok")]
        public override int kod_lok { get; set; }

        [Column("nr_lok")]
        public override int nr_lok { get; set; }

        [Column("nr_kontr")]
        public override int nr_kontr { get; set; }

        [Column("nr_skl")]
        public override int nr_skl { get; set; }

        [Column("stawka")]
        public override decimal Stawka { get; set; }

        [Column("ilosc")]
        public override decimal Ilość { get; set; }
    }
}