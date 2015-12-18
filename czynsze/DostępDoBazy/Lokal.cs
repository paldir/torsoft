﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    public class Lokal : Rekord
    {
        public int nr_system { get; private set; }

        [Display(Name = "budynek")]
        public int kod_lok { get; set; }

        [Display(Name = "nr lokalu")]
        public int nr_lok { get; set; }

        [Display(Name = "typ")]
        public int kod_typ { get; set; }

        [Display(Name = "powierzchnia użytkowa")]
        public float pow_uzyt { get; set; }

        [Display(Name = "nazwisko")]
        public string nazwisko { get; set; }

        [Display(Name = "imię")]
        public string imie { get; set; }

        [Display(Name = "adres")]
        public string adres { get; set; }

        [Display(Name = "adres cd.")]
        public string adres_2 { get; set; }

        [Display(Name = "powierzchnia mieszkalna")]
        public float pow_miesz { get; set; }

        [Display(Name = "udział")]
        public float udzial { get; set; }

        [Display(Name = "początek zakresu dat")]
        public Nullable<DateTime> dat_od { get; set; }

        [Display(Name = "koniec zakresu dat")]
        public Nullable<DateTime> dat_do { get; set; }

        [Display(Name = "powierzchnia I pokoju")]
        public float p_1 { get; set; }

        [Display(Name = "powierzchnia II pokoju")]
        public float p_2 { get; set; }

        [Display(Name = "powierzchnia III pokoju")]
        public float p_3 { get; set; }

        [Display(Name = "powierzchnia IV pokoju")]
        public float p_4 { get; set; }

        [Display(Name = "powierzchnia V pokoju")]
        public float p_5 { get; set; }

        [Display(Name = "powierzchnia VI pokoju")]
        public float p_6 { get; set; }

        [Display(Name = "typ kuchni")]
        public Nullable<int> kod_kuch { get; set; }

        [Display(Name = "najemca")]
        public Nullable<int> nr_kontr { get; set; }

        [Display(Name = "ilość osób")]
        public Nullable<int> il_osob { get; set; }

        [Display(Name = "tytuł prawny do lokalu")]
        public Nullable<int> kod_praw { get; set; }

        public string uwagi_1 { get; private set; }

        public string uwagi_2 { get; private set; }

        public string uwagi_3 { get; private set; }

        public string uwagi_4 { get; private set; }

        [Display(Name = "uwagi")]
        [NotMapped]
        public string uwagi
        {
            get { return String.Concat(uwagi_1, uwagi_2, uwagi_3, uwagi_4).Trim(); }

            set
            {
                string uwagi = value.PadRight(240);
                uwagi_1 = uwagi.Substring(0, 60).Trim();
                uwagi_2 = uwagi.Substring(60, 60).Trim();
                uwagi_3 = uwagi.Substring(120, 60).Trim();
                uwagi_4 = uwagi.Substring(180, 60).Trim();
            }
        }

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[] 
            { 
                kod_lok.ToString(), 
                nr_lok.ToString(), 
                Rozpoznaj_kod_typ(),
                pow_uzyt.ToString("F2"), 
                nazwisko, 
                imie 
            });
        }

        public string Rozpoznaj_kod_typ()
        {
            TypLokalu typLokalu;

            Sesja.Obecna.MagazynRekordów.KodNaTypLokalu.TryGetValue(kod_typ, out typLokalu);

            if (typLokalu == null)
                return String.Empty;
            else
                return typLokalu.typ_lok;
        }

        /*public override void Ustaw(string[] rekord)
        {
            nr_system = Int32.Parse(rekord[0]);
            kod_lok = Int32.Parse(rekord[1]);
            nr_lok = Int32.Parse(rekord[2]);
            kod_typ = Int32.Parse(rekord[3]);
            adres = rekord[4];
            adres_2 = rekord[5];
            pow_uzyt = Single.Parse(rekord[6]);
            pow_miesz = Single.Parse(rekord[7]);
            udzial = Single.Parse(rekord[8]);

            if (rekord[9] != null)
                dat_od = Convert.ToDateTime(rekord[9]);

            if (rekord[10] != null)
                dat_do = Convert.ToDateTime(rekord[10]);

            p_1 = Single.Parse(rekord[11]);
            p_2 = Single.Parse(rekord[12]);
            p_3 = Single.Parse(rekord[13]);
            p_4 = Single.Parse(rekord[14]);
            p_5 = Single.Parse(rekord[15]);
            p_6 = Single.Parse(rekord[16]);
            kod_kuch = null;
            nr_kontr = Int32.Parse(rekord[18]);

            using (CzynszeKontekst db = new CzynszeKontekst())
            {
                AktywnyNajemca najemca = db.AktywniNajemcy.Where(t => t.nr_kontr == nr_kontr).FirstOrDefault();

                if (najemca == null)
                    nazwisko = imie = String.Empty;
                else
                {
                    nazwisko = najemca.nazwisko;
                    imie = najemca.imie;
                }
            }

            il_osob = Int32.Parse(rekord[19]);
            kod_praw = Int32.Parse(rekord[20]);
            uwagi = rekord[21];
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = "";
            int kod_lok, nr_lok;

            if (akcja == Enumeratory.Akcja.Dodaj)
            {
                if (rekord[2].Length > 0)
                {
                    try
                    {
                        kod_lok = Int32.Parse(rekord[1]);
                        nr_lok = Int32.Parse(rekord[2]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.AktywneLokale.Where(p => p.kod_lok == kod_lok && p.nr_lok == nr_lok).Any())
                                wynik += "W wybranym budynku istnieje już lokal o danym numerze! <br />";
                    }
                    catch { wynik += "Nr lokalu musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać numer lokalu! <br />";
            }

            if (akcja != Enumeratory.Akcja.Przenieś)
            {
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia użytkowa", ref rekord[6]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia mieszkalna", ref rekord[7]);
                wynik += CzynszeKontekst.WalidujFloat("Udział", ref rekord[8]);
                wynik += CzynszeKontekst.WalidujDatę("Początek zakresu dat", ref rekord[9]);
                wynik += CzynszeKontekst.WalidujDatę("Koniec zakresu dat", ref rekord[10]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia I pokoju", ref rekord[11]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia II pokoju", ref rekord[12]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia III pokoju", ref rekord[13]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia IV pokoju", ref rekord[14]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia V pokoju", ref rekord[15]);
                wynik += CzynszeKontekst.WalidujFloat("Powierzchnia VI pokoju", ref rekord[16]);
                wynik += CzynszeKontekst.WalidujInt("Ilość osób", ref rekord[19]);
            }

            return wynik;
        }*/
    }
}