using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("bud_ws", Schema = "public")]
    public class CommunityBuilding
    {
        [Column("__record"), Key]
        public int __record { get; set; }

        [Column("kod")]
        public int kod { get; set; }

        [Column("kod_1")]
        public int kod_1 { get; set; }

        [Column("uwagi")]
        public string uwagi { get; set; }

        public string[] ImportantFields()
        {
            string adres;

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                Building building = db.buildings.FirstOrDefault(b => b.kod_1 == kod_1);
                adres = building.adres + " " + building.adres_2;
            }

            return new string[]
            {
                kod_1.ToString(),
                adres
            };
        }

        public void Set(string[] record)
        {
            kod = Convert.ToInt32(record[0]);
            kod_1 = Convert.ToInt32(record[1]);
            uwagi = record[2];
        }
    }
}