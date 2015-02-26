using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("tyt_praw", Schema = "public")]
    public class Title : IRecord
    {
        [Key, Column("kod_praw"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_praw { get; set; }

        [Column("tyt_prawny")]
        public string tyt_prawny { get; set; }

        public string[] ImportantFieldsForDropDown()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                tyt_prawny 
            };
        }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                kod_praw.ToString(), 
                tyt_prawny 
            };
        }

        public string[] AllFields()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                tyt_prawny.Trim()
            };
        }

        public void Set(string[] record)
        {
            kod_praw = Convert.ToInt16(record[0]);
            tyt_prawny = record[1];
        }

        public IRecord Find(Czynsze_Entities dataBase, int id)
        {
            return dataBase.titles.FirstOrDefault(t => t.kod_praw == id);
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            int kod_praw;

            if (action == Enums.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod_praw = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.titles.Count(t => t.kod_praw == kod_praw) != 0)
                                result += "Istnieje już tytuł prawny do lokali o podanym kodzie! <br />";
                    }
                    catch { result += "Kod tytułu prawnego do lokali musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod tytułu prawnego do lokali! <br />";
            }

            if (action == Enums.Action.Usuń)
            {
                kod_praw = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.places.Count(t => t.kod_praw == kod_praw) > 0)
                        result += "Nie można usunąć tytułu prawnego do lokali, jeśli jest on używany! <br />";
            }

            return result;
        }
    }
}