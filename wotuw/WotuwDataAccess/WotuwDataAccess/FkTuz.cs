using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WotuwDataAccess
{
    [Table("fk_tuz", Schema = "public")]
    public class FkTuz
    {
        [Key, Column("id"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int id { get; set; }

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

        public static bool AccessGranted(string login, string haslo)
        {
            FkTuz fkTuz = Record.DataBase.FkTuz.FirstOrDefault(f => f.uzytkownik.Trim() == login.Trim());

            if (fkTuz != null)
            {
                IEnumerable<byte> correctPassword = Encoding.UTF8.GetBytes(fkTuz.haslo.Trim()).Select(p => (byte)(p - 10));
                byte[] password = Encoding.UTF8.GetBytes(haslo);

                if (correctPassword.Count() == password.Length && Enumerable.SequenceEqual(correctPassword, password))
                    return true;
            }

            return false;
        }
    }
}