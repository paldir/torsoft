using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    public class Najemca : IRekord
    {
        [PrzyjaznaNazwaPola("nr kontr.")]
        [Key, DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int nr_kontr { get; set; }

        [PrzyjaznaNazwaPola("nazwisko")]
        public string nazwisko { get; set; }

        [PrzyjaznaNazwaPola("imię")]
        public string imie { get; set; }

        [PrzyjaznaNazwaPola("adres")]
        public string adres_1 { get; set; }

        [PrzyjaznaNazwaPola("adres cd.")]
        public string adres_2 { get; set; }

        [PrzyjaznaNazwaPola("najemca")]
        public int kod_najem { get; set; }

        [PrzyjaznaNazwaPola("nr dowodu osobistego")]
        public string nr_dow { get; set; }

        [PrzyjaznaNazwaPola("pesel")]
        public string pesel { get; set; }

        [PrzyjaznaNazwaPola("zakład pracy")]
        public string nazwa_z { get; set; }

        [PrzyjaznaNazwaPola("login/e-mail")]
        public string e_mail { get; set; }

        [PrzyjaznaNazwaPola("hasło")]
        public string l__has { get; set; }

        string uwagi_1 { get; set; }

        string uwagi_2 { get; set; }

        public int id { get { return nr_kontr; } }

        [PrzyjaznaNazwaPola("uwagi")]
        [NotMapped]
        public string uwagi
        {
            get { return String.Concat(uwagi_1, uwagi_2).Trim(); }

            set
            {
                string uwagi = value.PadRight(120);
                uwagi_1 = uwagi.Substring(0, 60).Trim();
                uwagi_2 = uwagi.Substring(60, 60).Trim();
            }
        }

        public static List<AktywnyLokal> AktywneLokale { get; set; }

        public string[] PolaDoTabeli()
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
                uwagi
            };
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
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
            uwagi = rekord[11];
        }

        public string[] ZLokalem()
        {
            string kod;
            string nr;
            string lokal;

            //using (DostępDoBazy.Czynsze_Entities db = new Czynsze_Entities())
            {
                DostępDoBazy.Lokal place = AktywneLokale.FirstOrDefault(p => p.nr_kontr == nr_kontr);

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