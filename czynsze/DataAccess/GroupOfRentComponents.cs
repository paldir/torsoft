using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("grup_cz", Schema="public")]
    public class GroupOfRentComponents
    {
        [Key, Column("kod"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod { get; set; }

        [Column("nazwa")]
        public string nazwa { get; set; }

        public string[] ImportantFields()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                nazwa
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                kod.ToString(),
                nazwa.Trim()
            };
        }

        public static string Validate(EnumP.Action action, string[] record)
        {
            string result = String.Empty;
            int kod;

            if (action == EnumP.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.groupsOfRentComponents.Count(a => a.kod == kod) != 0)
                                result += "Istnieje już grupa składników czynszu o podanym kodzie! <br />";
                    }
                    catch { result += "Kod grupy składników czynszu musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod grupy składników czynszu! <br />";
            }

            if (action == EnumP.Action.Usuń)
            {
                //
                //TODO
                //
            }

            return result;
        }

        public void Set(string[] record)
        {
            kod = Convert.ToInt16(record[0]);
            nazwa = record[1];
        }
    }
}