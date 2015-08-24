﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("cechy", Schema = "public")]
    public class Atrybut : Rekord
    {
        [Display(Name = "kod")]
        public int kod { get; set; }

        [Display(Name = "nazwa")]
        public string nazwa { get; set; }

        [Display(Name = "numeryczna/charakter")]
        public string nr_str { get; set; }

        public string zb_l { get; set; }

        public string zb_n { get; set; }

        public string zb_b { get; set; }

        public string zb_s { get; set; }

        [Display(Name = "dotyczy")]
        public string zb 
        {
            get { return null; } 
        }

        [Display(Name = "jednostka miary")]
        public string jedn { get; set; }

        float wartosc_n { get; set; }

        string wartosc_s { get; set; }

        [Display(Name = "wartość domyślna")]
        [NotMapped]
        public string wartosc
        {
            get
            {
                if (String.Equals(nr_str, "N"))
                    return wartosc_n.ToString().Trim();
                else
                    return wartosc_s;
            }

            set
            {
                if (String.Equals(nr_str, "N"))
                    wartosc_n = Single.Parse(value);
                else
                    wartosc_s = value;
            }
        }

        [Display(Name = "uwagi")]
        public string uwagi { get; set; }

        [Display(Name = "kod")]
        public override int id
        {
            get { return kod; }
            set { kod = value; }
        }

        public override string[] PolaDoTabeli()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                nazwa,
                nr_str,
                zb_l,
                zb_n,
                zb_b,
                zb_s
            };
        }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[]
            {
                kod.ToString(),
                nazwa
            };
        }

        public override string[] WszystkiePola()
        {
           return new string[]
            {
                kod.ToString(),
                nazwa.Trim(),
                nr_str,
                jedn.Trim(),
                wartosc,
                uwagi.Trim(),
                zb_l,
                zb_n,
                zb_b,
                zb_s
            };
        }

        public override void Ustaw(string[] record)
        {
            kod = Int32.Parse(record[0]);
            nazwa = record[1];
            nr_str = record[2];
            jedn = record[3];
            wartosc = record[4];
            uwagi = record[5];
            zb_l = record[6];
            zb_n = record[7];
            zb_b = record[8];
            zb_s = record[9];
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            int kod;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.Atrybuty.Any(a => a.kod == kod))
                                wynik += "Istnieje już cecha obiektów o podanym kodzie! <br />";
                    }
                    catch { wynik += "Kod cechy obiektów musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod cechy obiektów! <br />";
            }

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                if (String.Equals(rekord[2], "N"))
                    wynik += CzynszeKontekst.WalidujFloat("Wartość domyślna", ref rekord[4]);
                else
                    rekord[3] = String.Empty;

                for (int i = 0; i < 4; i++)
                    if (rekord[i + 6] != null)
                        rekord[i + 6] = "X";
            }
            else
            {
                kod = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                {
                    //
                    // TODO
                    //
                }
            }

            return wynik;
        }
    }
}