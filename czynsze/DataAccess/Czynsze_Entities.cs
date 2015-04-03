using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace czynsze.DataAccess
{
    public class Czynsze_Entities : DbContext
    {
        public Czynsze_Entities() : base(nameOrConnectionString: "czynsze_connectionString") { }
        public DbSet<Building> buildings { get; set; }
        public DbSet<ActivePlace> places { get; set; }
        public DbSet<InactivePlace> inactivePlaces { get; set; }
        public DbSet<TypeOfPlace> typesOfPlace { get; set; }
        public DbSet<TypeOfKitchen> typesOfKitchen { get; set; }
        public DbSet<ActiveTenant> tenants { get; set; }
        public DbSet<Title> titles { get; set; }
        public DbSet<TypeOfTenant> typesOfTenant { get; set; }
        public DbSet<RentComponent> rentComponents { get; set; }
        public DbSet<RentComponentOfPlace> rentComponentsOfPlaces { get; set; }
        public DbSet<Configuration> configurations { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Community> communities { get; set; }
        public DbSet<TypeOfPayment> typesOfPayment { get; set; }
        public DbSet<VatRate> vatRates { get; set; }
        public DbSet<Attribute> attributes { get; set; }
        public DbSet<AttributeOfPlace> attributesOfPlaces { get; set; }
        public DbSet<AttributeOfTenant> attributesOfTenants { get; set; }
        public DbSet<AttributeOfBuilding> attributesOfBuildings { get; set; }
        public DbSet<AttributeOfCommunity> attributesOfCommunities { get; set; }
        public DbSet<GroupOfRentComponents> groupsOfRentComponents { get; set; }
        public DbSet<FinancialGroup> financialGroups { get; set; }
        public DbSet<InactiveTenant> inactiveTenants { get; set; }
        public DbSet<ReceivableFrom1stSet> receivablesFrom1stSet { get; set; }
        public DbSet<TurnoverFrom1stSet> turnoversFrom1stSet { get; set; }
        public DbSet<ReceivableFrom2ndSet> receivablesFrom2ndSet { get; set; }
        public DbSet<TurnoverFrom2ndSet> turnoversFrom2ndSet { get; set; }
        public DbSet<ReceivableFrom3rdSet> receivablesFrom3rdSet { get; set; }
        public DbSet<TurnoverFrom3rdSet> turnoversFrom3rdSet { get; set; }
        public DbSet<Completed> completed { get; set; }
        public DbSet<CommunityBuilding> buildingsOfCommunities { get; set; }

        public const string DateFormat = "{0:yyyy-MM-dd}";
        
        public static string ValidateInt(string name, ref string integer)
        {
            string result = "";

            if (integer != null && integer.Length > 0)
                try { Convert.ToInt32(integer); }
                catch { result += name + " musi być liczbą całkowitą! <br />"; }
            else
                integer = "0";

            return result;
        }

        public static string ValidateFloat(string name, ref string single)
        {
            string result = "";

            if (single != null && single.Length > 0)
                try { Convert.ToSingle(single); }
                catch { result += name + " musi być liczbą! <br />"; }
            else
                single = "0";

            return result;
        }

        public static string ValidateDate(string name, ref string date)
        {
            string result = "";

            if (!String.IsNullOrEmpty(date) && date != "-1")
                try { Convert.ToDateTime(date); }
                catch { result += name + " musi mieć format rrrr-mm-dd! <br />"; }
            else
                date = null;

            return result;
        }
    }
}