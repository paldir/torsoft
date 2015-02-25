using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("typ_lok", Schema = "public")]
    public class TypeOfPlace : IRecord
    {
        [Key, Column("kod_typ"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int kod_typ { get; set; }

        [Column("typ_lok")]
        public string typ_lok { get; set; }

        public string[] ImportantFieldsForDropDown()
        {
            return new string[] 
            { 
                kod_typ.ToString(), 
                typ_lok 
            };
        }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_typ.ToString(),
                kod_typ.ToString(), 
                typ_lok 
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                kod_typ.ToString(),
                typ_lok.Trim()
            };
        }

        public void Set(string[] record)
        {
            kod_typ = Convert.ToInt16(record[0]);
            typ_lok = record[1];
        }

        public void Add(Czynsze_Entities dataBase)
        {
            dataBase.typesOfPlace.Add(this);
        }

        public IRecord Find(Czynsze_Entities dataBase, int id)
        {
            return dataBase.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);
        }

        public void Remove(Czynsze_Entities dataBase)
        {
            dataBase.typesOfPlace.Remove(this);
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            int kod_typ;

            if (action == Enums.Action.Dodaj)
            {
                if (record[0].Length > 0)
                {
                    try
                    {
                        kod_typ = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.typesOfPlace.Count(t => t.kod_typ == kod_typ) != 0)
                                result += "Istnieje już typ lokali o podanym kodzie! <br />";
                    }
                    catch { result += "Kod typu lokali musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać kod typu lokali! <br />";
            }

            if (action == Enums.Action.Usuń)
            {
                kod_typ = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.places.Count(p => p.kod_typ == kod_typ) > 0)
                        result += "Nie można usunąć typu lokali, jeśli jest on używany! <br />";
            }

            return result;
        }
    }
}