using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DataAccess
{
    public abstract class Receivable
    {
        public const string ReceivableYear = "14";
        
        public abstract int __record { get; set; }

        public abstract float kwota_nal { get; set; }

        public abstract string data_nal { get; set; }

        public abstract string opis { get; set; }

        public abstract int kod_lok { get; set; }

        public abstract int nr_lok { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int nr_skl { get; set; }

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