﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("bud_ws", Schema = "public")]
    public class BudynekWspólnoty
    {
        [Key]
        public int __record { get; set; }

        public int kod { get; set; }

        public int kod_1 { get; set; }

        public string uwagi { get; set; }

        public IEnumerable<string> PolaDoTabeli()
        {
            string adres;

            using (CzynszeKontekst db = new CzynszeKontekst())
            {
                Budynek budynek = db.Budynki.FirstOrDefault(b => b.kod_1 == kod_1);
                adres = budynek.adres + " " + budynek.adres_2;
            }

            return new string[]
            {
                kod_1.ToString(),
                adres,
                uwagi
            };
        }

        public void Ustaw(string[] rekord)
        {
            kod = Int32.Parse(rekord[0]);
            kod_1 = Int32.Parse(rekord[1]);
            uwagi = rekord[2];
        }
    }
}