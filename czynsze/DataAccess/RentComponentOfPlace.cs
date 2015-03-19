using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("skl_cz", Schema = "public")]
    public class RentComponentOfPlace
    {
        [Key, Column("kod_lok", Order = 0)]
        public int kod_lok { get; set; }

        [Key, Column("nr_lok", Order = 1)]
        public int nr_lok { get; set; }

        [Key, Column("nr_skl", Order = 2)]
        public int nr_skl { get; set; }

        [Column("dan_p")]
        public float dan_p { get; set; }

        [Column("dat_od")]
        public Nullable<DateTime> dat_od { get; set; }

        [Column("dat_do")]
        public Nullable<DateTime> dat_do { get; set; }

        public string[] ImportantFields()
        {
            RentComponent rentComponent;
            Place place;

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == nr_skl);
                place = db.places.FirstOrDefault(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok);

                if (place == null)
                    place = db.inactivePlaces.FirstOrDefault(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok);
            }

            float ilosc = 0;
            float stawka = rentComponent.stawka;

            switch (rentComponent.s_zaplat)
            {
                case 1:
                    ilosc = place.pow_uzyt;

                    break;

                case 2:
                    ilosc = dan_p;

                    break;

                case 3:
                    ilosc = (float)place.il_osob;

                    break;

                case 4:
                    ilosc = 1;

                    break;

                case 5:
                    DateTime date=Forms.Hello.Date;

                    ilosc = DateTime.DaysInMonth(date.Year, date.Month);

                    break;

                case 6:
                    ilosc = 1;

                    switch (place.il_osob)
                    {
                        case 0:
                            stawka = rentComponent.stawka_00;

                            break;

                        case 1:
                            stawka = rentComponent.stawka_01;

                            break;

                        case 2:
                            stawka = rentComponent.stawka_02;

                            break;

                        case 3:
                            stawka = rentComponent.stawka_03;

                            break;

                        case 4:
                            stawka = rentComponent.stawka_04;

                            break;

                        case 5:
                            stawka = rentComponent.stawka_05;

                            break;

                        case 6:
                            stawka = rentComponent.stawka_06;

                            break;

                        case 7:
                            stawka = rentComponent.stawka_07;

                            break;

                        case 8:
                            stawka = rentComponent.stawka_08;

                            break;

                        default:
                            stawka = rentComponent.stawka_09;

                            break;
                    }

                    break;
            }

            return new string[]
            {
                nr_skl.ToString(),
                rentComponent.nazwa,
                stawka.ToString("F2"),
                ilosc.ToString("F2"),
                (ilosc*stawka).ToString("F2")
            };
        }

        public void Set(string[] record)
        {
            kod_lok = Convert.ToInt16(record[0]);
            nr_lok = Convert.ToInt16(record[1]);
            nr_skl = Convert.ToInt16(record[2]);
            dan_p = Convert.ToSingle(record[3]);

            if (record[4] != null)
                dat_od = Convert.ToDateTime(record[4]);

            if (record[5] != null)
                dat_do = Convert.ToDateTime(record[5]);
        }

        public static bool Validate(string[] record, Enums.Action action)
        {
            if (!String.IsNullOrEmpty(Czynsze_Entities.ValidateFloat("dan_p", ref record[3])))
                return false;

            if (!String.IsNullOrEmpty(Czynsze_Entities.ValidateDate("dat_od", ref record[4])))
                return false;

            if (!String.IsNullOrEmpty(Czynsze_Entities.ValidateDate("dat_do", ref record[5])))
                return false;

            switch (action)
            {
                case Enums.Action.Dodaj:
                    using (Czynsze_Entities db = new Czynsze_Entities())
                        if (db.rentComponentsOfPlaces.ToList().Any(c => c.kod_lok == Convert.ToInt16(record[0]) && c.nr_lok == Convert.ToInt16(record[1]) && c.nr_skl == Convert.ToInt16(record[2])))
                            return false;

                    break;
            }

            return true;
        }
    }
}