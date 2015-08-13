using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Reflection;

namespace czynsze.DostępDoBazy
{
    public class CzynszeKontekst : DbContext
    {
        public CzynszeKontekst() : base(nameOrConnectionString: "czynsze_connectionString") { }
        public CzynszeKontekst(System.Data.Entity.Infrastructure.DbCompiledModel model) : base("czynsze_connectionString", model) { }
        public DbSet<Budynek> Budynki { get; set; }
        public DbSet<AktywnyLokal> AktywneLokale { get; set; }
        public DbSet<NieaktywnyLokal> NieaktywneLokale { get; set; }
        public DbSet<TypLokalu> TypyLokali { get; set; }
        public DbSet<TypKuchni> TypyKuchni { get; set; }
        public DbSet<AktywnyNajemca> AktywniNajemcy { get; set; }
        public DbSet<TytułPrawny> TytułyPrawne { get; set; }
        public DbSet<TypNajemcy> TypyNajemców { get; set; }
        public DbSet<SkładnikCzynszu> SkładnikiCzynszu { get; set; }
        public DbSet<SkładnikCzynszuLokalu> SkładnikiCzynszuLokalu { get; set; }
        public DbSet<Konfiguracja> Konfiguracje { get; set; }
        public DbSet<Użytkownik> Użytkownicy { get; set; }
        public DbSet<Wspólnota> Wspólnoty { get; set; }
        public DbSet<RodzajPłatności> RodzajePłatności { get; set; }
        public DbSet<StawkaVat> StawkiVat { get; set; }
        public DbSet<Atrybut> Atrybuty { get; set; }
        public DbSet<AtrybutLokalu> AtrybutyLokali { get; set; }
        public DbSet<AtrybutNajemcy> AtrybutyNajemców { get; set; }
        public DbSet<AtrybutBudynku> AtrybutyBudynków { get; set; }
        public DbSet<AtrybutWspólnoty> AtrybutyWspólnot { get; set; }
        public DbSet<GrupaSkładnikówCzynszu> GrupySkładnikówCzynszu { get; set; }
        public DbSet<GrupaFinansowa> GrupyFinansowe { get; set; }
        public DbSet<NieaktywnyNajemca> NieaktywniNajemcy { get; set; }
        public DbSet<Należność1> Należności1 { get; set; }
        public DbSet<Obrót1> Obroty1 { get; set; }
        public DbSet<Należność2> Należności2 { get; set; }
        public DbSet<Obrót2> Obroty2 { get; set; }
        public DbSet<Należność3> Należności3 { get; set; }
        public DbSet<Obrót3> Obroty3 { get; set; }
        public DbSet<Zamknięty> Zamknięte { get; set; }
        public DbSet<BudynekWspólnoty> BudynkiWspólnot { get; set; }
        public DbSet<Treść> Treści { get; set; }

        public const string FormatDaty = "{0:yyyy-MM-dd}";
        //public static CzynszeKontekst BazaDanych = new CzynszeKontekst();

        /*static readonly Destruktor _destruktor = new Destruktor();

        class Destruktor
        {
            ~Destruktor()
            {
                BazaDanych.Dispose();
            }
        }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            int rok = DateTime.Today.Year % 1000;

            modelBuilder.Entity<Należność1>().ToTable(String.Concat("nal_", rok, "__"), "public");
            modelBuilder.Entity<Należność2>().ToTable(String.Concat("nak_", rok, "__"), "public");
            modelBuilder.Entity<Należność3>().ToTable(String.Concat("nam_", rok, "__"), "public");
            modelBuilder.Entity<Obrót1>().ToTable(String.Concat("obr_", rok, "__"), "public");
            modelBuilder.Entity<Obrót2>().ToTable(String.Concat("obk_", rok, "__"), "public");
            modelBuilder.Entity<Obrót3>().ToTable(String.Concat("obm_", rok, "__"), "public");
        }

        public static void ZmieńRok(int rok)
        {
            rok = rok % 1000;

            DbModelBuilder _budowniczyModelu = new DbModelBuilder();
            Type typKonfiguracjiTypuEncji = typeof(System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<>);
            MethodInfo metodaDodawaniaKonfiguracji = typeof(System.Data.Entity.ModelConfiguration.Configuration.ConfigurationRegistrar).GetMethods().First(m => m.Name == "Add");

            foreach (var tmp in typeof(DostępDoBazy.CzynszeKontekst).GetProperties().Where(w => w.PropertyType.IsGenericType))
            {
                Type typEncji = tmp.PropertyType.GetGenericArguments().Single();
                Type ogólnyKonfiguracjiTypuEncji = typKonfiguracjiTypuEncji.MakeGenericType(typEncji);
                object konfiguracjaTypuEncji = Activator.CreateInstance(ogólnyKonfiguracjiTypuEncji);
                MethodInfo ogólnaMetodaDodawaniaKonfiguracji = metodaDodawaniaKonfiguracji.MakeGenericMethod(typEncji);

                ogólnaMetodaDodawaniaKonfiguracji.Invoke(_budowniczyModelu.Configurations, new object[] { konfiguracjaTypuEncji });

                //_budowniczyModelu.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DostępDoBazy.Budynek>());
            }
        }

        public static string WalidujInt(string nazwa, ref string całkowita)
        {
            string wynik = "";

            if (całkowita != null && całkowita.Length > 0)
                try { Int32.Parse(całkowita); }
                catch { wynik += nazwa + " musi być liczbą całkowitą! <br />"; }
            else
                całkowita = "0";

            return wynik;
        }

        public static string WalidujFloat(string nazwa, ref string pojedynczaPrecyzja)
        {
            string wynik = "";

            if (pojedynczaPrecyzja != null && pojedynczaPrecyzja.Length > 0)
                try { Single.Parse(pojedynczaPrecyzja); }
                catch { wynik += nazwa + " musi być liczbą! <br />"; }
            else
                pojedynczaPrecyzja = "0";

            return wynik;
        }

        public static string WalidujDatę(string nazwa, ref string data)
        {
            string wynik = "";

            if (!String.IsNullOrEmpty(data) && data != "-1")
                try { Convert.ToDateTime(data); }
                catch { wynik += nazwa + " musi mieć format rrrr-mm-dd! <br />"; }
            else
                data = null;

            return wynik;
        }

        public static readonly Dictionary<int, string> NumerMiesiącaNaNazwęBezPolskichZnaków = new Dictionary<int, string>()
        {
            {1, "styczen"},
            {2, "luty"},
            {3, "marzec"},
            {4, "kwiecien"},
            {5, "maj"},
            {6, "czerwiec"},
            {7, "lipiec"},
            {8, "sierpien"},
            {9, "wrzesien"},
            {10, "pazdziernik"},
            {11, "listopad"},
            {12, "grudzien"}
        };

        public static readonly Dictionary<int, string> NumerMiesiącaNaNazwęZPolskimiZnakami = new Dictionary<int, string>()
        {
            {1, "styczeń"},
            {2, "luty"},
            {3, "marzec"},
            {4, "kwiecień"},
            {5, "maj"},
            {6, "czerwiec"},
            {7, "lipiec"},
            {8, "sierpień"},
            {9, "wrzesień"},
            {10, "październik"},
            {11, "listopad"},
            {12, "grudzień"}
        };
    }
}