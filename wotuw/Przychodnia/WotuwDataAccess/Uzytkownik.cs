using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WotuwDataAccess
{
    [Table("uzytkownik", Schema = "public")]
    public class Uzytkownik
    {
        public static Uzytkownik CurrentUser { get; private set; }

        [Key, Column("id"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public long id { get; set; }

        [Column("dateIn")]
        public DateTime dateIn { get; set; }

        [Column("timeIn")]
        public string timeIn { get; set; }

        [Column("dateOut")]
        public DateTime? dateOut { get; set; }

        [Column("timeOut")]
        public string timeOut { get; set; }

        [Column("station")]
        public string station { get; set; }

        [Column("nazwisko")]
        public string nazwisko { get; set; }

        [Column("imie")]
        public string imie { get; set; }

        [Column("uwagi")]
        public string uwagi { get; set; }

        public static void RegisterUserIn(string nazwisko, string imie)
        {
            DateTime now = DateTime.Now;
            long newId = 1;
            WotuwEntities db = Record.DataBase;

            if (db.Uzytkownicy.Any())
                newId = db.Uzytkownicy.Max(u => u.id) + 1;

            CurrentUser = new Uzytkownik();
            CurrentUser.id = newId;
            CurrentUser.dateIn = now;
            CurrentUser.timeIn = now.ToLongTimeString();
            CurrentUser.station = Environment.MachineName;
            CurrentUser.nazwisko = nazwisko;
            CurrentUser.imie = imie;

            db.Uzytkownicy.Add(CurrentUser);
            db.SaveChanges();
        }

        public static void RegisterUserOut()
        {
            DateTime now = DateTime.Now;
            CurrentUser.dateOut = now;
            CurrentUser.timeOut = now.ToLongTimeString();

            Record.DataBase.SaveChanges();
            Record.DataBase.Dispose();
        }
    }
}