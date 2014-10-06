using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("najemc_a", Schema = "public")]
    public class InactiveTenant : Tenant
    {
        [Key, Column("nr_kontr"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public override int nr_kontr { get; set; }

        [Column("nazwisko")]
        public override string nazwisko { get; set; }

        [Column("imie")]
        public override string imie { get; set; }

        [Column("adres_1")]
        public override string adres_1 { get; set; }

        [Column("adres_2")]
        public override string adres_2 { get; set; }

        [Column("kod_najem")]
        public override int kod_najem { get; set; }

        [Column("nr_dow")]
        public override string nr_dow { get; set; }

        [Column("pesel")]
        public override string pesel { get; set; }

        [Column("nazwa_z")]
        public override string nazwa_z { get; set; }

        [Column("e_mail")]
        public override string e_mail { get; set; }

        [Column("l__has")]
        public override string l__has { get; set; }

        [Column("uwagi_1")]
        public override string uwagi_1 { get; set; }

        [Column("uwagi_2")]
        public override string uwagi_2 { get; set; }

        /*public string[] ImportantFields()
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
        }*/
    }
}