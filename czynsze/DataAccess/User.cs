﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace czynsze.DataAccess
{
    [Table("fk_tuz", Schema = "public")]
    public class User : IRecord
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
            uzytkownik = record[2] + " " + record[3];
            haslo = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(record[4]).Select(b => (byte)(b + 10)).ToArray());
        }

        public string Validate(Enums.Action action, string[] record)
        {
            string result = String.Empty;
            List<string> recordList = record.ToList();

            switch (action)
            {
                case Enums.Action.Dodaj:
                    if (record[2].Length > 0 && record[3].Length > 0)
                    {
                        using (DataAccess.Czynsze_Entities db = new Czynsze_Entities())
                            if (db.users.ToList().Any(u => u.nazwisko.Trim() == recordList.ElementAt(1) && u.imie.Trim() == recordList.ElementAt(2)))
                                result += "Użytkownik o podanym nazwisku i imieniu już istnieje! <br />";
                    }
                    else
                        result += "Należy podać nazwisko i imię! <br />";

                    break;
            }

            if (action != Enums.Action.Usuń)
            {
                if (record[4].Length > 0)
                {
                    if (record[4] != record[5])
                        result += "Podane hasła nie są identyczne! <br />";
                }
                else
                    result += "Hasło nie może być puste! <br />";
            }

            return result;
        }
    }
}