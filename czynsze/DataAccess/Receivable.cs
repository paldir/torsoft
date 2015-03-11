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

        public abstract DateTime data_nal { get; set; }

        public abstract string opis { get; set; }

        public abstract int kod_lok { get; set; }

        public abstract int nr_lok { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int nr_skl { get; set; }

        public abstract float stawka { get; set; }

        public abstract float ilosc { get; set; }

        public string[] ImportantFields()
        {
            return new string[]
            {
                __record.ToString(),
                kwota_nal.ToString("F2"),
                String.Format(DataAccess.Czynsze_Entities.DateFormat, data_nal),
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
                String.Format(DataAccess.Czynsze_Entities.DateFormat, data_nal),
                opis
            };
        }

        public void Set(float kwota_nal, DateTime data_nal, string opis, int nr_kontr, int nr_skl, int kod_lok, int nr_lok, float stawka, float ilosc)
        {
            this.kwota_nal = kwota_nal;
            this.data_nal = data_nal;
            this.opis = opis;
            this.nr_kontr = nr_kontr;
            this.nr_skl = nr_skl;
            this.kod_lok = kod_lok;
            this.nr_lok = nr_lok;
            this.stawka = stawka;
            this.ilosc = ilosc;
        }
    }
}