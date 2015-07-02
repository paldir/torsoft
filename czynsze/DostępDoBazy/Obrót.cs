﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    public abstract class Obrót : IRekord, IPozycjaDoAnalizy
    {
        public const string Rok = "14";

        public abstract int __record { get; set; }

        public abstract decimal suma { get; set; }

        public abstract DateTime data_obr { get; set; }

        public abstract string opis { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int kod_wplat { get; set; }

        public abstract string nr_dowodu { get; set; }

        public abstract int pozycja_d { get; set; }

        public abstract string uwagi { get; set; }

        public DateTime Data
        {
            get { return data_obr; }
        }

        public decimal Kwota
        {
            get { return suma; }
        }

        public decimal Ilość
        {
            get { return 0; }
        }

        public decimal Stawka
        {
            get { return 0; }
        }

        public int IdInformacji
        {
            get { return kod_wplat; }
        }

        DostępDoBazy.AktywnyLokal _lokal;

        public int KodBudynku
        {
            get
            {
                UstawLokal();

                return _lokal.kod_lok;
            }
        }

        public int NrLokalu
        {
            get
            {
                UstawLokal();

                return _lokal.nr_lok;
            }
        }

        public static DostępDoBazy.CzynszeKontekst BazaDanych { get; set; }

        //public static Dictionary<Enums.SettlementTable, List<Turnover>> SettlementTableToListOfTurnovers { get; private set; }

        enum Konto { Wn, Ma };

        /*static Turnover()
        {
            SettlementTableToListOfTurnovers = new Dictionary<Enums.SettlementTable, List<Turnover>>();

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.Czynsze, new Czynsze_Entities().turnoversFor14.ToList().Cast<DostępDoBazy.Turnover>().ToList());
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.SecondSet, new Czynsze_Entities().turnoversFor14From2ndSet.ToList().Cast<DostępDoBazy.Turnover>().ToList());
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.ThirdSet, new Czynsze_Entities().turnoversFor14From3rdSet.ToList().Cast<DostępDoBazy.Turnover>().ToList());
            }
        }*/

        public string[] WażnePola()
        {
            string data_obr = null;

            if (this.data_obr != null)
                data_obr = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.data_obr);

            return new string[] 
            {
                __record.ToString(),
                String.Format("{0:N}", suma),
                data_obr,
                DateTime.Today.ToShortDateString(),
                opis,
                nr_dowodu,
                pozycja_d.ToString(),
                uwagi
            };
        }

        public string[] WażnePolaDoNależnościIObrotówNajemcy()
        {
            RodzajPłatności typ;
            Konto konto = Konto.Wn;
            int mnożnik = 1;
            string wn = String.Empty;
            string ma = String.Empty;
            string data_obr = null;

            using (CzynszeKontekst db = new CzynszeKontekst())
                typ = db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == kod_wplat);

            switch (typ.s_rozli)
            {
                case 1:
                    switch (typ.tn_odset)
                    {
                        case 0:
                            konto = Konto.Wn;
                            mnożnik = -1;

                            break;

                        case 1:
                            konto = Konto.Ma;

                            break;
                    }

                    break;

                case 2:
                    konto = Konto.Wn;

                    break;

                case 3:
                    konto = Konto.Ma;
                    mnożnik = -1;

                    break;
            }

            string suma = (this.suma * mnożnik).ToString("F2");

            switch (konto)
            {
                case Konto.Wn:
                    wn = suma;

                    break;

                case Konto.Ma:
                    ma = suma;

                    break;
            }

            if (this.data_obr != null)
                data_obr = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.data_obr);

            return new string[]
            {
                __record.ToString(),
                wn,
                ma,
                data_obr,
                opis
            };
        }

        public string[] WszystkiePola()
        {
            string data_obr = null;

            if (this.data_obr != null)
                data_obr = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.data_obr);

            return new string[]
            {
                __record.ToString(),
                suma.ToString(),
                data_obr,
                DateTime.Today.ToShortDateString(),
                kod_wplat.ToString(),
                nr_dowodu.Trim(),
                pozycja_d.ToString(),
                uwagi.Trim(),
                nr_kontr.ToString()
            };
        }

        public void Ustaw(string[] rekord)
        {
            __record = Int32.Parse(rekord[0]);
            suma = Decimal.Parse(rekord[1]);

            if (!String.IsNullOrEmpty(rekord[2]))
                data_obr = Convert.ToDateTime(rekord[2]);

            //data_NO = record[3];
            kod_wplat = Int32.Parse(rekord[4]);
            nr_dowodu = rekord[5];
            pozycja_d = Int32.Parse(rekord[6]);
            uwagi = rekord[7];
            nr_kontr = Int32.Parse(rekord[8]);

            using (CzynszeKontekst db = new CzynszeKontekst())
                opis = db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                wynik += CzynszeKontekst.WalidujFloat("Kwota", ref rekord[1]);
                wynik += CzynszeKontekst.WalidujDatę("Data", ref rekord[2]);
                wynik += CzynszeKontekst.WalidujDatę("Data NO", ref rekord[3]);
                wynik += CzynszeKontekst.WalidujInt("Pozycja", ref rekord[6]);
            }

            return wynik;
        }

        void UstawLokal()
        {
            if (_lokal == null)
                _lokal = BazaDanych.AktywneLokale.FirstOrDefault(l => l.nr_kontr == nr_kontr);

            if (_lokal == null)
            {
                _lokal = new AktywnyLokal();
                _lokal.kod_lok = _lokal.nr_lok = 0;
            }
        }
    }
}