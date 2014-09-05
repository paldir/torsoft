using System;
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
    }
}