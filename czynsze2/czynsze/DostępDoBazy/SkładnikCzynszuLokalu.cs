﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("skl_cz", Schema = "public")]
    public class SkładnikCzynszuLokalu
    {
        [Key, Column(Order = 0)]
        public int kod_lok { get; set; }

        [Key, Column(Order = 1)]
        public int nr_lok { get; set; }

        [Key, Column(Order = 2)]
        public int nr_skl { get; set; }

        public float dan_p { get; set; }

        public Nullable<DateTime> dat_od { get; set; }

        public Nullable<DateTime> dat_do { get; set; }

        public IEnumerable<string> PolaDoTabeli()
        {
            SkładnikCzynszu składnikCzynszu;

            using (CzynszeKontekst db = new CzynszeKontekst())
                składnikCzynszu = db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == nr_skl);

            float ilosc;
            decimal stawka;

            Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

            string dat_od = String.Empty;
            string dat_do = String.Empty;

            if (this.dat_od.HasValue)
                dat_od = this.dat_od.Value.ToShortDateString();

            if (this.dat_do.HasValue)
                dat_do = this.dat_do.Value.ToShortDateString();

            decimal kwota = Decimal.Round(stawka * Convert.ToDecimal(ilosc), 2, MidpointRounding.AwayFromZero);

            return new string[]
            {
                nr_skl.ToString(),
                składnikCzynszu.nazwa,
                stawka.ToString("F2"),
                ilosc.ToString("F2"),
                String.Format("{0:N}", kwota),
                dat_od,
                dat_od
            };
        }

        public void Ustaw(string[] rekord)
        {
            kod_lok = Int32.Parse(rekord[0]);
            nr_lok = Int32.Parse(rekord[1]);
            nr_skl = Int32.Parse(rekord[2]);
            dan_p = Single.Parse(rekord[3]);

            if (rekord[4] != null)
                dat_od = Convert.ToDateTime(rekord[4]);

            if (rekord[5] != null)
                dat_do = Convert.ToDateTime(rekord[5]);
        }

        public void Rozpoznaj_ilosc_i_stawka(out float ilosc, out decimal stawka)
        {
            SkładnikCzynszu składnikCzynszu;
            Lokal lokal;
            MagazynRekordów rekordy = Sesja.Obecna.MagazynRekordów;

            składnikCzynszu = rekordy.NumerSkładnikaNaSkładnikCzynszu[nr_skl];

            rekordy.KodINumerNaLokal[kod_lok].TryGetValue(nr_lok, out lokal);

            if (lokal == null)
                using (CzynszeKontekst db = new CzynszeKontekst())
                    lokal = db.NieaktywneLokale.FirstOrDefault(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok);

            ilosc = 0;

            if (lokal == null)
                stawka = 0;
            else
            {
                stawka = składnikCzynszu.stawka;

                switch (składnikCzynszu.s_zaplat)
                {
                    case 1:
                        ilosc = lokal.pow_uzyt;

                        break;

                    case 2:
                        ilosc = dan_p;

                        break;

                    case 3:
                        int? ilośćOsób = lokal.il_osob;
                        ilosc = ilośćOsób.HasValue ? ilośćOsób.Value : 0;

                        break;

                    case 4:
                        ilosc = 1;

                        break;

                    case 5:
                        DateTime date = Formularze.Start.Data;
                        ilosc = DateTime.DaysInMonth(date.Year, date.Month);

                        break;

                    case 6:
                        ilosc = 1;

                        switch (lokal.il_osob)
                        {
                            case 0:
                                stawka = składnikCzynszu.stawka_00;

                                break;

                            case 1:
                                stawka = składnikCzynszu.stawka_01;

                                break;

                            case 2:
                                stawka = składnikCzynszu.stawka_02;

                                break;

                            case 3:
                                stawka = składnikCzynszu.stawka_03;

                                break;

                            case 4:
                                stawka = składnikCzynszu.stawka_04;

                                break;

                            case 5:
                                stawka = składnikCzynszu.stawka_05;

                                break;

                            case 6:
                                stawka = składnikCzynszu.stawka_06;

                                break;

                            case 7:
                                stawka = składnikCzynszu.stawka_07;

                                break;

                            case 8:
                                stawka = składnikCzynszu.stawka_08;

                                break;

                            default:
                                stawka = składnikCzynszu.stawka_09;

                                break;
                        }

                        break;
                }
            }
        }

        public static bool Waliduj(string[] rekord, Enumeratory.Akcja akcja)
        {
            if (!String.IsNullOrEmpty(CzynszeKontekst.WalidujFloat("dan_p", ref rekord[3])))
                return false;

            if (!String.IsNullOrEmpty(CzynszeKontekst.WalidujDatę("dat_od", ref rekord[4])))
                return false;

            if (!String.IsNullOrEmpty(CzynszeKontekst.WalidujDatę("dat_do", ref rekord[5])))
                return false;

            switch (akcja)
            {
                case Enumeratory.Akcja.Dodaj:
                    using (CzynszeKontekst db = new CzynszeKontekst())
                        if (db.SkładnikiCzynszuLokalu.ToList().Any(c => c.kod_lok == Int32.Parse(rekord[0]) && c.nr_lok == Int32.Parse(rekord[1]) && c.nr_skl == Int32.Parse(rekord[2])))
                            return false;

                    break;
            }

            return true;
        }
    }
}