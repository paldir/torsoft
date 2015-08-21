using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("wspol", Schema = "public")]
    public class Wspólnota : IRekord
    {
        [Key, DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        [Display(Name = "kod wspólnoty")]
        public int kod { get; set; }

        [Display(Name = "nazwa skrócona")]
        public string nazwa_skr { get; set; }

        [Display(Name = "ilość budynków")]
        public int il_bud { get; set; }

        [Display(Name = "ilości lokali")]
        public int il_miesz { get; set; }

        [Display(Name = "nazwa pełna")]
        public string nazwa_pel { get; set; }

        [Display(Name = "adres")]
        public string adres { get; set; }

        [Display(Name = "adres cd.")]
        public string adres_2 { get; set; }

        [Display(Name = "nr konta 1")]
        public string nr1_konta { get; set; }

        [Display(Name = "nr konta 2")]
        public string nr2_konta { get; set; }

        [Display(Name = "nr konta 3")]
        public string nr3_konta { get; set; }

        [Display(Name = "ścieżka do F-K")]
        public string sciezka_fk { get; set; }

        public string uwagi_1 { get; set; }

        public string uwagi_2 { get; set; }

        public string uwagi_3 { get; set; }

        public string uwagi_4 { get; set; }

        public string uwagi_5 { get; set; }

        public string uwagi_6 { get; set; }

        [Display(Name = "kod wspólnoty")]
        [NotMapped]
        public int id
        {
            get { return kod; }
            set { kod = value; }
        }

        [Display(Name = "uwagi")]
        [NotMapped]
        public string uwagi
        {
            get { return String.Concat(uwagi_1, uwagi_2, uwagi_3, uwagi_4, uwagi_5, uwagi_6).Trim(); }

            set
            {
                string uwagi = value.PadRight(420);
                uwagi_1 = uwagi.Substring(0, 70).Trim();
                uwagi_2 = uwagi.Substring(70, 70).Trim();
                uwagi_3 = uwagi.Substring(140, 70).Trim();
                uwagi_4 = uwagi.Substring(210, 70).Trim();
                uwagi_5 = uwagi.Substring(280, 70).Trim();
                uwagi_6 = uwagi.Substring(350).Trim();
            }
        }

        public string[] PolaDoTabeli()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                nazwa_skr,
                il_bud.ToString(),
                il_miesz.ToString()
            };
        }

        public string[] WszystkiePola()
        {
            return new string[]
            {
                kod.ToString(),
                il_bud.ToString(),
                il_miesz.ToString(),
                nazwa_pel.Trim(),
                nazwa_skr.Trim(),
                adres.Trim(),
                adres_2.Trim(),
                nr1_konta.Trim(),
                nr2_konta.Trim(),
                nr3_konta.Trim(),
                sciezka_fk.Trim(),
                uwagi
            };
        }

        public void Ustaw(string[] rekord)
        {
            kod = Int32.Parse(rekord[0]);
            il_bud = Int32.Parse(rekord[1]);
            il_miesz = Int32.Parse(rekord[2]);
            nazwa_pel = rekord[3];
            nazwa_skr = rekord[4];
            adres = rekord[5];
            adres_2 = rekord[6];
            nr1_konta = rekord[7];
            nr2_konta = rekord[8];
            nr3_konta = rekord[9];
            sciezka_fk = rekord[10];
            uwagi = rekord[11];
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = "";
            int kod;

            if (akcja == Enumeratory.Akcja.Dodaj)
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        kod = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.Wspólnoty.Any(c => c.kod == kod))
                                wynik += "Kod wspólnoty jest już używany! <br />";
                    }
                    catch { wynik += "Kod wspólnoty musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać kod wspólnoty! <br />";

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                wynik += CzynszeKontekst.WalidujInt("Ilość budynków", ref rekord[1]);
                wynik += CzynszeKontekst.WalidujInt("Ilość lokali", ref rekord[2]);
            }

            return wynik;
        }
    }
}