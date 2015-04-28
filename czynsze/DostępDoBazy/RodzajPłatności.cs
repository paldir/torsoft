using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("t_wplat", Schema = "public")]
    public class RodzajPłatności : IRekord
    {
        [Key, Column("kod_wplat"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_wplat { get; set; }

        [Column("typ_wplat")]
        public string typ_wplat { get; set; }

        [Column("s_rozli")]
        public int s_rozli { get; set; }

        [Column("tn_odset")]
        public int tn_odset { get; set; }

        [Column("nota_odset")]
        public int nota_odset { get; set; }

        [Column("rodz_e")]
        public int rodz_e { get; set; }

        [Column("vat")]
        public string vat { get; set; }

        [Column("sww")]
        public string sww { get; set; }

        string Rozpoznaj_s_rozli()
        {
            switch (s_rozli)
            {
                case 1:
                    return "Zmniejszenia";
                case 2:
                    return "Zwiększenia";
                case 3:
                    return "Zwrot";
                default:
                    return String.Empty;
            }
        }

        string Rozpoznaj_tn_odset()
        {
            switch (tn_odset)
            {
                case 0:
                    return "Nie";
                case 1:
                    return "Tak";
                default:
                    return String.Empty;
            }
        }

        string Rozpoznaj_nota_odset()
        {
            switch (nota_odset)
            {
                case 0:
                    return "Nie";
                case 1:
                    return "Tak";
                default:
                    return String.Empty;
            }
        }

        public string[] WażnePola()
        {
            return new string[]
            {
                kod_wplat.ToString(),
                kod_wplat.ToString(),
                typ_wplat,
                Rozpoznaj_s_rozli(),
                Rozpoznaj_tn_odset(),
                Rozpoznaj_nota_odset()
            };
        }

        public string[] WszystkiePola()
        {
            return new string[]
            {
                kod_wplat.ToString(),
                typ_wplat.Trim(),
                rodz_e.ToString(),
                s_rozli.ToString(),
                tn_odset.ToString(),
                nota_odset.ToString(),
                vat.Trim(),
                sww.Trim()
            };
        }

        public void Ustaw(string[] record)
        {
            kod_wplat = Int32.Parse(record[0]);
            typ_wplat = record[1];
            rodz_e = Int32.Parse(record[2]);
            s_rozli = Int32.Parse(record[3]);
            tn_odset = Int32.Parse(record[4]);
            nota_odset = Int32.Parse(record[5]);
            vat = record[6];
            sww = record[7];
        }

        public string Waliduj(Enumeratory.Akcja action, string[] record)
        {
            string result = String.Empty;
            int kod_wplat;

            switch (action)
            {
                case Enumeratory.Akcja.Dodaj:
                    if (record[0].Length > 0)
                    {
                        try
                        {
                            kod_wplat = Int32.Parse(record[0]);

                            using (CzynszeKontekst db = new CzynszeKontekst())
                                if (db.RodzajePłatności.Any(t => t.kod_wplat == kod_wplat))
                                    result += "Istnieje już rodzaj wpłaty lub wypłaty o podanym kodzie! <br />";
                        }
                        catch { result += "Kod rodzaju wpłaty lub wypłaty musi być liczbą całkowitą! <br />"; }
                    }
                    else
                        result += "Należy podać kod rodzaju wpłaty lub wypłaty! <br />";

                    break;

                case Enumeratory.Akcja.Usuń:
                    kod_wplat = Int32.Parse(record[0]);

                    using (CzynszeKontekst db = new CzynszeKontekst())
                        if (db.ObrotyZPierwszegoZbioru.Any(t => t.kod_wplat == kod_wplat) || db.ObrotyZDrugiegoZbioru.Any(t => t.kod_wplat == kod_wplat) || db.ObrotyZTrzeciegoZbioru.Any(t => t.kod_wplat == kod_wplat))
                            result += "Nie można usunąć typu wpłaty lub wypłaty, jeśli jest on używany! <br />";

                    break;
            }

            return result;
        }

        public string[] ImportantFieldsForDropdown()
        {
            return new string[]
            {
                kod_wplat.ToString(),
                kod_wplat.ToString(),
                typ_wplat
            };
        }
    }
}