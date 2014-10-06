using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("fk_tuz", Schema = "public")]
    public class User
    {
        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("symbol")]
        public string symbol { get; set; }

        [Column("nazwisko")]
        public string nazwisko { get; set; }

        [Column("imie")]
        public string imie { get; set; }

        [Column("uzytkownik")]
        public string uzytkownik { get; set; }

        [Column("haslo")]
        public string haslo { get; set; }

        public string[] ImportantFields()
        {
            return new string[]
            {
                __record.ToString(),
                symbol,
                nazwisko,
                imie
            };
        }

        public string[] AllFields()
        {
            return new string[]
            {
                __record.ToString(),
                symbol.Trim(),
                nazwisko.Trim(),
                imie.Trim(),
                uzytkownik.Trim(),
                haslo.Trim()
            };
        }

        public void Set(string[] record)
        {
            symbol = record[1];
            nazwisko = record[2];
            imie = record[3];
            uzytkownik = record[4];
            haslo = record[5];
        }

        public static string Validate(EnumP.Action action, ref string[] record)
        {
            string result = String.Empty;
            List<string> recordList = record.ToList();

            switch (action)
            {
                case EnumP.Action.Dodaj:
                    if (record[1].Length > 0 && record[2].Length > 0)
                    {
                        using (DataAccess.Czynsze_Entities db = new Czynsze_Entities())
                            if (db.users.ToList().Count(u => u.nazwisko.Trim() == recordList.ElementAt(1) && u.imie.Trim() == recordList.ElementAt(2)) > 0)
                                result += "Użytkownik o podanym nazwisku i imieniu już istnieje! <br />";

                        recordList.Insert(1, record[1].First().ToString() + record[2].First().ToString());
                        recordList.Insert(4, record[1] + " " + record[2]);
                    }
                    else
                    {
                        result += "Należy podać nazwisko i imię! <br />";

                        recordList.Insert(1, String.Empty);
                        recordList.Insert(4, String.Empty);
                    }
                    break;
            }

            if (record[3].Length > 0)
            {
                if (record[3] != record[4])
                    result += "Podane hasła nie są identyczne! <br />";
            }
            else
                result += "Hasło nie może być puste! <br />";

            record = recordList.ToArray();

            return result;
        }
    }
}