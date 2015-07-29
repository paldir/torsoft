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
        [Key, Column("kod"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod { get; set; }

        [Column("nazwa_skr")]
        public string nazwa_skr { get; set; }

        [Column("il_bud")]
        public int il_bud { get; set; }

        [Column("il_miesz")]
        public int il_miesz { get; set; }

        [Column("nazwa_pel")]
        public string nazwa_pel { get; set; }

        [Column("adres")]
        public string adres { get; set; }

        [Column("adres_2")]
        public string adres_2 { get; set; }

        [Column("nr1_konta")]
        public string nr1_konta { get; set; }

        [Column("nr2_konta")]
        public string nr2_konta { get; set; }

        [Column("nr3_konta")]
        public string nr3_konta { get; set; }

        [Column("sciezka_fk")]
        public string sciezka_fk { get; set; }

        [Column("uwagi_1")]
        public string uwagi_1 { get; set; }

        [Column("uwagi_2")]
        public string uwagi_2 { get; set; }

        [Column("uwagi_3")]
        public string uwagi_3 { get; set; }

        [Column("uwagi_4")]
        public string uwagi_4 { get; set; }

        [Column("uwagi_5")]
        public string uwagi_5 { get; set; }

        [Column("uwagi_6")]
        public string uwagi_6 { get; set; }

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
                String.Concat(uwagi_1.Trim(), uwagi_2.Trim(), uwagi_3.Trim(), uwagi_4.Trim(), uwagi_5.Trim(), uwagi_6.Trim())
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

            rekord[11] = rekord[11].PadRight(420);

            uwagi_1 = rekord[11].Substring(0, 70).Trim();
            uwagi_2 = rekord[11].Substring(70, 70).Trim();
            uwagi_3 = rekord[11].Substring(140, 70).Trim();
            uwagi_4 = rekord[11].Substring(210, 70).Trim();
            uwagi_5 = rekord[11].Substring(280, 70).Trim();
            uwagi_6 = rekord[11].Substring(350).Trim();
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