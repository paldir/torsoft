using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("typ_naje", Schema = "public")]
    public class TypeOfTenant : IRecord
    {
        [Key, Column("kod_najem"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_najem { get; set; }

        [Column("r_najemcy")]
        public string r_najemcy { get; set; }

        public string[] ImportantFieldsForDropDown()
        {
            return new string[] 
            { 
                kod_najem.ToString(), 
                r_najemcy 
            };
        }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_najem.ToString(),
                kod_najem.ToString(), 
                r_najemcy 
            };
        }

        public string[] AllFields()
        {
            return new string[] 
            { 
                kod_najem.ToString(), 
                r_najemcy.Trim() 
            };
        }

        public void Set(string[] record)
        {
            kod_najem = Convert.ToInt16(record[0]);
            r_najemcy = record[1];
        }

        public IRecord Find(Czynsze_Entities dataBase, int id)
        {
            return dataBase.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            int kod_najem;

            if (action == Enums.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod_najem = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.typesOfTenant.Count(t => t.kod_najem == kod_najem) != 0)
                                result += "Istnieje już rodzaj najemców o podanym kodzie! <br />";
                    }
                    catch { result += "Kod rodzaju najemców musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod rodzaju najemców! <br />";
            }

            if (action == Enums.Action.Usuń)
            {
                kod_najem = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.tenants.Count(t => t.kod_najem == kod_najem) > 0)
                        result += "Nie można usunąć rodzaju najemców, jeśli jest on używany! <br />";
            }

            return result;
        }
    }
}