using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace WotuwDataAccess
{
    public class WotuwEntities : DbContext
    {
        public WotuwEntities() : base(nameOrConnectionString: "wotuw_connectionString") { }
        public DbSet<Pacjent> Pacjenci { get; set; }
        public DbSet<Swiadczenie> Swiadczenia { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<FkTuz> FkTuz { get; set; }
    }
}