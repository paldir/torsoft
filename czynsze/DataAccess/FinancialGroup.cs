using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("grup_fi", Schema = "public")]
    public class FinancialGroup : IRecord
    {
        [Key, Column("kod"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod { get; set; }

        [Column("nazwa")]
        public string nazwa { get; set; }

        [Column("k_syn")]
        public string k_syn { get; set; }

        public string[] ImportantFields()
        {
            return new string[]
            {
                kod.ToString(),
                kod.ToString(),
                k_syn,
                nazwa
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                kod.ToString(),
                k_syn.Trim(),
                nazwa.Trim()
            };
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            int kod;

            if (action == Enums.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod = Convert.ToInt32(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.financialGroups.Any(t => t.kod == kod))
                                result += "Istnieje już grupa finansowa o podanym kodzie! <br />";
                    }
                    catch { result += "Kod grupy finansowej musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod grupy finansowej! <br />";
            }

            if (action == Enums.Action.Usuń)
            {
                //
                //TODO
                //
            }

            return result;
        }

        public void Set(string[] record)
        {
            kod = Convert.ToInt32(record[0]);
            k_syn = record[1];
            nazwa = record[2];
        }
    }
}