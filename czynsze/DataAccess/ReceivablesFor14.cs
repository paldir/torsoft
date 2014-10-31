using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("nal_14__", Schema = "public")]
    public class ReceivablesFor14
    {
        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("kwota_nal")]
        public float kwota_nal { get; set; }

        [Column("data_nal")]
        public string data_nal { get; set; }

        [Column("opis")]
        public string opis { get; set; }

        [Column("kod_lok")]
        public int kod_lok { get; set; }

        [Column("nr_lok")]
        public int nr_lok { get; set; }

        [Column("nr_kontr")]
        public int nr_kontr { get; set; }

        public string[] ImportantFields()
        {
            return new string[]
            {
                __record.ToString(),
                kwota_nal.ToString("F2"),
                data_nal,
                opis,
                kod_lok.ToString(),
                nr_lok.ToString()
            };
        }

        public string[] ImportantFieldsForReceivablesAndTurnoversOfTenant()
        {
            return new string[]
            {
                (-1*__record).ToString(),
                kwota_nal.ToString("F2"),
                String.Empty,
                data_nal,
                opis
            };
        }
    }
}