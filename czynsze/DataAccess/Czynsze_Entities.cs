﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace czynsze.DataAccess
{
    public class Czynsze_Entities: DbContext
    {
        public Czynsze_Entities() : base(nameOrConnectionString: "czynsze_connectionString") { }
        public DbSet<Building> buildings { get; set; }
        public DbSet<Place> places { get; set; }
        public DbSet<TypeOfPlace> typesOfPlace { get; set; }
        public DbSet<TypeOfKitchen> typesOfKitchen { get; set; }
        public DbSet<Tenant> tenants { get; set; }
        public DbSet<Title> titles { get; set; }
        public DbSet<TypeOfTenant> typesOfTenant { get; set; }
        public DbSet<RentComponent> rentComponents { get; set; }
        public DbSet<RentComponentOfPlace> rentComponentsOfPlaces { get; set; }
        public DbSet<Configuration> configurations { get; set; }

        public static string ValidateInt(string name, ref string integer)
        {
            string result = "";

            if (integer.Length > 0)
                try { Convert.ToInt16(integer); }
                catch { result += name + " musi być liczbą całkowitą! <br />"; }
            else
                integer = "0";

            return result;
        }

        public static string ValidateFloat(string name, ref string single)
        {
            string result = "";

            if (single.Length > 0)
                try { Convert.ToSingle(single); }
                catch { result += name + " musi być liczbą! <br />"; }
            else
                single = "0";

            return result;
        }

        public static string ValidateDate(string name, ref string date)
        {
            string result = "";

            if (date.Length > 0)
                try { Convert.ToDateTime(date); }
                catch { result += name + " musi mieć format rrrr-mm-dd! <br />"; }
            else
                date = DateTime.Today.ToShortDateString();

            return result;
        }
    }
}