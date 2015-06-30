﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    public abstract class Należność : IPozycjaDoAnalizy
    {
        public const string Rok = "14";

        public abstract int __record { get; set; }

        public abstract decimal Kwota { get; set; }

        public abstract DateTime Data { get; set; }

        public abstract string opis { get; set; }

        public abstract int kod_lok { get; set; }

        public abstract int nr_lok { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int ZewnętrzneId { get; set; }

        public abstract decimal Stawka { get; set; }

        public abstract decimal Ilość { get; set; }

        public string[] WażnePola()
        {
            return new string[]
            {
                __record.ToString(),
                Kwota.ToString("F2"),
                String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, Data),
                opis,
                kod_lok.ToString(),
                nr_lok.ToString()
            };
        }

        public string[] WażnePolaDoNależnościIObrotówNajemcy()
        {
            return new string[]
            {
                (-1*__record).ToString(),
                Kwota.ToString("F2"),
                String.Empty,
                String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, Data),
                opis
            };
        }

        public void Ustaw(decimal kwota_nal, DateTime data_nal, string opis, int nr_kontr, int nr_skl, int kod_lok, int nr_lok, decimal stawka, decimal ilosc)
        {
            this.Kwota = kwota_nal;
            this.Data = data_nal;
            this.opis = opis;
            this.nr_kontr = nr_kontr;
            this.ZewnętrzneId = nr_skl;
            this.kod_lok = kod_lok;
            this.nr_lok = nr_lok;
            this.Stawka = stawka;
            this.Ilość = ilosc;
        }
    }
}