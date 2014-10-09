using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DataAccess
{
    public abstract class Tenant
    {
        public abstract int nr_kontr { get; set; }

        public abstract string nazwisko { get; set; }

        public abstract string imie { get; set; }

        public abstract string adres_1 { get; set; }

        public abstract string adres_2 { get; set; }

        public abstract int kod_najem { get; set; }

        public abstract string nr_dow { get; set; }

        public abstract string pesel { get; set; }

        public abstract string nazwa_z { get; set; }

        public abstract string e_mail { get; set; }

        public abstract string l__has { get; set; }

        public abstract string uwagi_1 { get; set; }

        public abstract string uwagi_2 { get; set; }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                nr_kontr.ToString(), 
                nr_kontr.ToString(), 
                nazwisko, 
                imie, 
                adres_1, 
                adres_2 
            };
        }

        public string[] AllFields()
        {
            return new string[] 
            { 
                nr_kontr.ToString(), 
                kod_najem.ToString(), 
                nazwisko.Trim(), 
                imie.Trim(), 
                adres_1.Trim(),
                adres_2.Trim(), 
                nr_dow.Trim(), 
                pesel.Trim(), 
                nazwa_z.Trim(), 
                e_mail.Trim(), 
                l__has.Trim(), 
                String.Concat(uwagi_1.Trim(), uwagi_2.Trim()) 
            };
        }

        public void Set(string[] record)
        {
            nr_kontr = Convert.ToInt16(record[0]);
            kod_najem = Convert.ToInt16(record[1]);
            nazwisko = record[2];
            imie = record[3];

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                List<ActivePlace> places = db.places.Where(p => p.nr_kontr == nr_kontr).ToList();

                foreach (ActivePlace place in places)
                {
                    place.nazwisko = nazwisko;
                    place.imie = imie;
                }

                db.SaveChanges();
            }

            adres_1 = record[4];
            adres_2 = record[5];
            nr_dow = record[6];
            pesel = record[7];
            nazwa_z = record[8];
            e_mail = record[9];
            l__has = record[10];

            record[11] = record[11].PadRight(120);

            uwagi_1 = record[11].Substring(0, 60).Trim();
            uwagi_2 = record[11].Substring(60, 60).Trim();
        }

        public string[] WithPlace()
        {
            string kod;
            string nr;
            string lokal;

            using (DataAccess.Czynsze_Entities db = new Czynsze_Entities())
            {
                DataAccess.Place place = db.places.FirstOrDefault(p => p.nr_kontr == nr_kontr);

                if (place == null)
                    kod = nr = lokal = "0";
                else
                {
                    kod = place.kod_lok.ToString();
                    nr = place.nr_lok.ToString();
                    lokal = place.adres + " " + place.adres_2;
                }
            }


            return new string[] 
            { 
                nr_kontr.ToString(),
                nazwisko,
                imie,
                kod,
                nr,
                adres_1+" "+adres_2,
                lokal
            };
        }
    }
}