using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("t_wplat", Schema = "public")]
    public class TypeOfPayment : IRecord
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

        string Recognize_s_rozli()
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

        string Recognize_tn_odset()
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

        string Recognize_nota_odset()
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

        public string[] ImportantFields()
        {
            return new string[]
            {
                kod_wplat.ToString(),
                kod_wplat.ToString(),
                typ_wplat,
                Recognize_s_rozli(),
                Recognize_tn_odset(),
                Recognize_nota_odset()
            };
        }

        public string[] AllFields()
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

        public void Set(string[] record)
        {
            kod_wplat = Convert.ToInt16(record[0]);
            typ_wplat = record[1];
            rodz_e = Convert.ToInt16(record[2]);
            s_rozli = Convert.ToInt16(record[3]);
            tn_odset = Convert.ToInt16(record[4]);
            nota_odset = Convert.ToInt16(record[5]);
            vat = record[6];
            sww = record[7];
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            int kod_wplat;

            switch (action)
            {
                case Enums.Action.Dodaj:
                    if (record[0].Length > 0)
                    {
                        try
                        {
                            kod_wplat = Convert.ToInt16(record[0]);

                            using (Czynsze_Entities db = new Czynsze_Entities())
                                if (db.typesOfPayment.Any(t => t.kod_wplat == kod_wplat))
                                    result += "Istnieje już rodzaj wpłaty lub wypłaty o podanym kodzie! <br />";
                        }
                        catch { result += "Kod rodzaju wpłaty lub wypłaty musi być liczbą całkowitą! <br />"; }
                    }
                    else
                        result += "Należy podać kod rodzaju wpłaty lub wypłaty! <br />";

                    break;

                case Enums.Action.Usuń:
                    kod_wplat = Convert.ToInt16(record[0]);

                    using (Czynsze_Entities db = new Czynsze_Entities())
                        if (db.turnoversFrom1stSet.Any(t => t.kod_wplat == kod_wplat) || db.turnoversFrom2ndSet.Any(t => t.kod_wplat == kod_wplat) || db.turnoversFrom3rdSet.Any(t => t.kod_wplat == kod_wplat))
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