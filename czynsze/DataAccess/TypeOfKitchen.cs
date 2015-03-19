using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("typ_kuch", Schema = "public")]
    public class TypeOfKitchen : IRecord
    {
        [Key, Column("kod_kuch"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_kuch { get; set; }

        [Column("typ_kuch")]
        public string typ_kuch { get; set; }

        public string[] ImportantFieldsForDropDown()
        {
            return new string[] 
            { 
                kod_kuch.ToString(), 
                typ_kuch 
            };
        }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_kuch.ToString(), 
                kod_kuch.ToString(), 
                typ_kuch 
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                kod_kuch.ToString(),
                typ_kuch.Trim()
            };
        }

        public void Set(string[] record)
        {
            kod_kuch = Convert.ToInt16(record[0]);
            typ_kuch = record[1];
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            int kod_kuch;

            if (action == Enums.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod_kuch = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.typesOfKitchen.Any(t => t.kod_kuch == kod_kuch))
                                result += "Istnieje już rodzaj kuchni o podanym kodzie! <br />";
                    }
                    catch { result += "Kod rodzaju kuchni musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod rodzaju kuchni! <br />";
            }

            if (action == Enums.Action.Usuń)
            {
                kod_kuch = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.places.Any(p => p.kod_kuch == kod_kuch))
                        result += "Nie można usunąć rodzaju kuchni, jeśli jest on używany! <br />";
            }

            return result;
        }
    }
}