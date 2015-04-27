using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    public abstract class Najemca : IRekord
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

        public static List<AktywnyLokal> Places { get; set; }

        public string[] WażnePola()
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

        public string[] WszystkiePola()
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

        public string Waliduj(Enums.Akcja akcja, string[] rekord)
        {
            return String.Empty;
        }

        public void Ustaw(string[] rekord)
        {
            nr_kontr = Int32.Parse(rekord[0]);
            kod_najem = Int32.Parse(rekord[1]);
            nazwisko = rekord[2];
            imie = rekord[3];

            using (CzynszeKontekst db = new CzynszeKontekst())
            {
                List<AktywnyLokal> places = db.AktywneLokale.Where(p => p.nr_kontr == nr_kontr).ToList();

                foreach (AktywnyLokal place in places)
                {
                    place.nazwisko = nazwisko;
                    place.imie = imie;
                }

                db.SaveChanges();
            }

            adres_1 = rekord[4];
            adres_2 = rekord[5];
            nr_dow = rekord[6];
            pesel = rekord[7];
            nazwa_z = rekord[8];
            e_mail = rekord[9];
            l__has = rekord[10];

            rekord[11] = rekord[11].PadRight(120);

            uwagi_1 = rekord[11].Substring(0, 60).Trim();
            uwagi_2 = rekord[11].Substring(60, 60).Trim();
        }

        public string[] ZLokalem()
        {
            string kod;
            string nr;
            string lokal;

            //using (DostępDoBazy.Czynsze_Entities db = new Czynsze_Entities())
            {
                DostępDoBazy.Lokal place = Places.FirstOrDefault(p => p.nr_kontr == nr_kontr);

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