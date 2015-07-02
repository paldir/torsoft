using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    public abstract class Należność : IPozycjaDoAnalizy
    {
        public const string Rok = "14";

        public abstract int __record { get; set; }

        public abstract decimal kwota_nal { get; set; }

        public abstract DateTime data_nal { get; set; }

        public abstract string opis { get; set; }

        public abstract int kod_lok { get; set; }

        public abstract int nr_lok { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int nr_skl { get; set; }

        public abstract decimal stawka { get; set; }

        public abstract decimal ilosc { get; set; }

        public DateTime Data
        {
            get { return data_nal; }
        }

        public decimal Kwota
        {
            get { return kwota_nal; }
        }

        public decimal Ilość
        {
            get { return ilosc; }
        }

        public decimal Stawka
        {
            get { return stawka; }
        }

        public int IdInformacji
        {
            get { return nr_skl; }
        }

        public int KodBudynku
        {
            get { return kod_lok; }
        }

        public int NrLokalu
        {
            get { return nr_lok; }
        }
            
        public string[] WażnePola()
        {
            return new string[]
            {
                __record.ToString(),
                kwota_nal.ToString("F2"),
                String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, data_nal),
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
                kwota_nal.ToString("F2"),
                String.Empty,
                String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, data_nal),
                opis
            };
        }

        public void Ustaw(decimal kwota_nal, DateTime data_nal, string opis, int nr_kontr, int nr_skl, int kod_lok, int nr_lok, decimal stawka, decimal ilosc)
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