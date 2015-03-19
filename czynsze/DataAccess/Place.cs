using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DataAccess
{
    public abstract class Place : IRecord
    {
        public abstract int nr_system { get; set; }

        public abstract int kod_lok { get; set; }

        public abstract int nr_lok { get; set; }

        public abstract int kod_typ { get; set; }

        public abstract float pow_uzyt { get; set; }

        public abstract string nazwisko { get; set; }

        public abstract string imie { get; set; }

        public abstract string adres { get; set; }

        public abstract string adres_2 { get; set; }

        public abstract float pow_miesz { get; set; }

        public abstract float udzial { get; set; }

        public abstract Nullable<DateTime> dat_od { get; set; }

        public abstract Nullable<DateTime> dat_do { get; set; }

        public abstract float p_1 { get; set; }

        public abstract float p_2 { get; set; }

        public abstract float p_3 { get; set; }

        public abstract float p_4 { get; set; }

        public abstract float p_5 { get; set; }

        public abstract float p_6 { get; set; }

        public abstract Nullable<int> kod_kuch { get; set; }

        public abstract Nullable<int> nr_kontr { get; set; }

        public abstract Nullable<int> il_osob { get; set; }

        public abstract Nullable<int> kod_praw { get; set; }

        public abstract string uwagi_1 { get; set; }

        public abstract string uwagi_2 { get; set; }

        public abstract string uwagi_3 { get; set; }

        public abstract string uwagi_4 { get; set; }

        public string[] ImportantFields()
        {
            string kod_typ = String.Empty;

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                TypeOfPlace typeOfPlace = db.typesOfPlace.Where(t => t.kod_typ == this.kod_typ).FirstOrDefault();

                if (typeOfPlace != null)
                    kod_typ = typeOfPlace.typ_lok;
            }

            return new string[] 
            { 
                nr_system.ToString(), 
                kod_lok.ToString(), 
                nr_lok.ToString(), 
                kod_typ, 
                pow_uzyt.ToString("F2"), 
                nazwisko, 
                imie 
            };
        }

        public string[] AllFields()
        {
            string dat_od, dat_do;

            if (this.dat_od == null)
                dat_od = null;
            else
                dat_od = String.Format(DataAccess.Czynsze_Entities.DateFormat, this.dat_od);

            if (this.dat_do == null)
                dat_do = null;
            else
                dat_do = String.Format(DataAccess.Czynsze_Entities.DateFormat, this.dat_do);

            return new string[] 
            { 
                nr_system.ToString(), 
                kod_lok.ToString(), 
                nr_lok.ToString(), 
                kod_typ.ToString(), 
                adres.Trim(), 
                adres_2.Trim(), 
                pow_uzyt.ToString("F2"),
                pow_miesz.ToString("F2"), 
                udzial.ToString("F2"), 
                dat_od,
                dat_do,
                p_1.ToString("F2"),
                p_2.ToString("F2"),
                p_3.ToString("F2"), 
                p_4.ToString("F2"), 
                p_5.ToString("F2"), 
                p_6.ToString("F2"),
                kod_kuch.ToString(), 
                nr_kontr.ToString(), 
                il_osob.ToString(), 
                kod_praw.ToString(), 
                String.Concat(uwagi_1.Trim(), uwagi_2.Trim(), uwagi_3.Trim(), uwagi_4.Trim()) 
            };
        }

        public void Set(string[] record)
        {
            nr_system = Convert.ToInt16(record[0]);
            kod_lok = Convert.ToInt16(record[1]);
            nr_lok = Convert.ToInt16(record[2]);
            kod_typ = Convert.ToInt16(record[3]);
            adres = record[4];
            adres_2 = record[5];
            pow_uzyt = Convert.ToSingle(record[6]);
            pow_miesz = Convert.ToSingle(record[7]);
            udzial = Convert.ToSingle(record[8]);

            if (record[9] != null)
                dat_od = Convert.ToDateTime(record[9]);

            if (record[10] != null)
                dat_do = Convert.ToDateTime(record[10]);

            p_1 = Convert.ToSingle(record[11]);
            p_2 = Convert.ToSingle(record[12]);
            p_3 = Convert.ToSingle(record[13]);
            p_4 = Convert.ToSingle(record[14]);
            p_5 = Convert.ToSingle(record[15]);
            p_6 = Convert.ToSingle(record[16]);
            kod_kuch = Convert.ToInt16(record[17]);
            nr_kontr = Convert.ToInt16(record[18]);

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                ActiveTenant tenant = db.tenants.Where(t => t.nr_kontr == nr_kontr).FirstOrDefault();

                if (tenant == null)
                    nazwisko = imie = String.Empty;
                else
                {
                    nazwisko = tenant.nazwisko;
                    imie = tenant.imie;
                }
            }

            il_osob = Convert.ToInt16(record[19]);
            kod_praw = Convert.ToInt16(record[20]);

            record[21] = record[21].PadRight(240);

            uwagi_1 = record[21].Substring(0, 60).Trim();
            uwagi_2 = record[21].Substring(60, 60).Trim();
            uwagi_3 = record[21].Substring(120, 60).Trim();
            uwagi_4 = record[21].Substring(180, 60).Trim();
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = "";
            int kod_lok, nr_lok;

            if (action == Enums.Action.Dodaj)
            {
                if (record[2].Length > 0)
                {
                    try
                    {
                        kod_lok = Convert.ToInt16(record[1]);
                        nr_lok = Convert.ToInt16(record[2]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.places.Where(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok).Any())
                                result += "W wybranym budynku istnieje już lokal o danym numerze! <br />";
                    }
                    catch { result += "Nr lokalu musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać numer lokalu! <br />";
            }

            if (action != Enums.Action.Przenieś)
            {
                result += Czynsze_Entities.ValidateFloat("Powierzchnia użytkowa", ref record[6]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia mieszkalna", ref record[7]);
                result += Czynsze_Entities.ValidateFloat("Udział", ref record[8]);
                result += Czynsze_Entities.ValidateDate("Początek zakresu dat", ref record[9]);
                result += Czynsze_Entities.ValidateDate("Koniec zakresu dat", ref record[10]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia I pokoju", ref record[11]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia II pokoju", ref record[12]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia III pokoju", ref record[13]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia IV pokoju", ref record[14]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia V pokoju", ref record[15]);
                result += Czynsze_Entities.ValidateFloat("Powierzchnia VI pokoju", ref record[16]);
                result += Czynsze_Entities.ValidateInt("Ilość osób", ref record[19]);
            }

            return result;
        }
    }
}