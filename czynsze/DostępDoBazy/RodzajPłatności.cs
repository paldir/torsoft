using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("t_wplat", Schema = "public")]
    public class RodzajPłatności : Rekord, IInformacjeOPozycji
    {
        [Display(Name = "kod")]
        public int kod_wplat { get; set; }

        [Display(Name = "rodzaj wpłaty lub wypłaty")]
        public string typ_wplat { get; set; }

        [Display(Name = "sposób rozliczenia")]
        public int s_rozli { get; set; }

        [Display(Name = "grupa składników czynszu")]
        public int kod { get; set; }

        [Display(Name = "czy naliczać odsetki")]
        public int tn_odset { get; set; }

        [Display(Name = "czy liczyć odsetki na nocie")]
        public int nota_odset { get; set; }

        [Display(Name = "rodzaj ewidencji")]
        public int rodz_e { get; set; }

        [Display(Name = "VAT")]
        public string vat { get; set; }

        [Display(Name = "SWW")]
        public string sww { get; set; }

        public int IdInformacji
        {
            get { return -kod_wplat; }
        }

        public string Nazwa
        {
            get { return typ_wplat; }
        }

        public int RodzajEwidencji
        {
            get { return rodz_e; }
        }

        public int Grupa
        {
            get { return kod; }
        }

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

        public override IEnumerable<string> PolaDoTabeli()
        {
            return base.PolaDoTabeli().Concat(new string[]
            {
                kod_wplat.ToString(),
                typ_wplat,
                Rozpoznaj_s_rozli(),
                Rozpoznaj_tn_odset(),
                Rozpoznaj_nota_odset()
            });
        }

        /*public override void Ustaw(string[] record)
        {
            kod_wplat = Int32.Parse(record[0]);
            typ_wplat = record[1];
            rodz_e = Int32.Parse(record[2]);
            s_rozli = Int32.Parse(record[3]);
            kod = Int32.Parse(record[4]);
            tn_odset = Int32.Parse(record[5]);
            nota_odset = Int32.Parse(record[6]);
            vat = record[7];
            sww = record[8];
        }

        public override string Waliduj(Enumeratory.Akcja action, string[] record)
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
                        if (db.Obroty1.Any(t => t.kod_wplat == kod_wplat) || db.Obroty2.Any(t => t.kod_wplat == kod_wplat) || db.Obroty3.Any(t => t.kod_wplat == kod_wplat))
                            result += "Nie można usunąć typu wpłaty lub wypłaty, jeśli jest on używany! <br />";

                    break;
            }

            return result;
        }*/

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