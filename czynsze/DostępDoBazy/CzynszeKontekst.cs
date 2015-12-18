﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace czynsze.DostępDoBazy
{
    public class CzynszeKontekst : DbContext
    {
        CzynszeKontekst(object o) : base(nameOrConnectionString: "czynsze_connectionString") { }
        public CzynszeKontekst() : base("czynsze_connectionString", _model) { }
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
        public DbSet<PlikLokalu> PlikiLokalów { get; set; }
        public DbSet<PlikBudynku> PlikiBudynków { get; set; }

        static DbCompiledModel _model;
        public const string FormatDaty = "{0:yyyy-MM-dd}";
        static Dictionary<Type, string> _typEncjiNaNazwęTabeli;
        static int _rok;
        public static int Rok
        {
            get { return _rok; }

            set
            {
                int _poprzedniRok = _rok;
                _rok = value % 1000;

                if (_rok != _poprzedniRok)
                    AktualizujModel();
            }
        }

        /*static readonly Destruktor _destruktor = new Destruktor();

        class Destruktor
        {
            ~Destruktor()
            {
                BazaDanych.Dispose();
            }
        }*/

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            DodajNależnościIObroty(DateTime.Today.Year % 1000, modelBuilder);

            using (CzynszeKontekst db = new CzynszeKontekst(new object()))
                _model = modelBuilder.Build(db.Database.Connection).Compile();
        }*/

        static CzynszeKontekst()
        {
            Rok = DateTime.Today.Year;
        }

        public override int SaveChanges()
        {
            /*IEnumerable<DbEntityEntry> pozycjeModyfikowanychEncji = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            List<string> modyfikowaneTabele = new List<string>();
            Type typAtrybutuTabeli = typeof(TableAttribute);

            foreach (DbEntityEntry modyfikowanaEncja in pozycjeModyfikowanychEncji)
            {
                Type typEncji = modyfikowanaEncja.Entity.GetType();
                string nazwaTabeli;

                if (Attribute.IsDefined(typEncji, typAtrybutuTabeli))
                    nazwaTabeli = (Attribute.GetCustomAttribute(typEncji, typAtrybutuTabeli) as TableAttribute).Name;
                else
                    nazwaTabeli = _typEncjiNaNazwęTabeli[typEncji];

                if (!modyfikowaneTabele.Contains(nazwaTabeli))
                    modyfikowaneTabele.Add(nazwaTabeli);
            }*/

            int kod = base.SaveChanges();
            /*XmlDocument upsize = new XmlDocument();

            upsize.Load(System.Web.Configuration.WebConfigurationManager.AppSettings["ścieżkaCzynszeUpsize"]);

            foreach (string nazwaTabeli in modyfikowaneTabele)
            {
                XmlNodeList węzłyIndeksów = upsize.SelectSingleNode(String.Format("/config/table[@name='{0}']", nazwaTabeli)).SelectNodes("order");
                List<string> listaArgumentów = new List<string>() { nazwaTabeli };

                if (węzłyIndeksów.Count != 0)
                {
                    foreach (XmlNode węzełIndeksu in węzłyIndeksów)
                        listaArgumentów.Add(System.IO.Path.GetFileNameWithoutExtension(węzełIndeksu.InnerText));

                    string argumenty = String.Join(" ", listaArgumentów);

                    using (Process proces = Process.Start(new ProcessStartInfo("NaprawaIndeksow.exe", argumenty)))
                    {
                        proces.WaitForExit();
                    }
                }

            }*/

            return kod;
        }

        /*protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            System.Data.Entity.Validation.DbEntityValidationResult wynikWalidacji = base.ValidateEntity(entityEntry, items);

            entityEntry.
        }*/

        static void AktualizujModel()
        {
            DbModelBuilder budowniczyModelu = new DbModelBuilder();
            Type typKonfiguracjiTypuEncji = typeof(System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<>);
            MethodInfo metodaDodawaniaKonfiguracji = typeof(System.Data.Entity.ModelConfiguration.Configuration.ConfigurationRegistrar).GetMethods().First(m => String.Equals(m.Name, "Add"));
            Type typOgólnyDbSet = typeof(DbSet<>).GetGenericTypeDefinition();
            List<PropertyInfo> właściwościTypuDbSet = new List<PropertyInfo>();

            foreach (PropertyInfo właściwość in typeof(CzynszeKontekst).GetProperties())
            {
                Type typWłaściwości = właściwość.PropertyType;

                if (typWłaściwości.IsGenericType && typWłaściwości.GetGenericTypeDefinition() == typOgólnyDbSet)
                    właściwościTypuDbSet.Add(właściwość);
            }

            foreach (PropertyInfo właściwośćDbSet in właściwościTypuDbSet)
            {
                Type typEncji = właściwośćDbSet.PropertyType.GetGenericArguments().Single();
                Type ogólnyTypKonfiguracjiTypuEncji = typKonfiguracjiTypuEncji.MakeGenericType(typEncji);
                object konfiguracjaTypuEncji = Activator.CreateInstance(ogólnyTypKonfiguracjiTypuEncji);
                MethodInfo ogólnaMetodaDodawaniaKonfiguracji = metodaDodawaniaKonfiguracji.MakeGenericMethod(typEncji);

                ogólnaMetodaDodawaniaKonfiguracji.Invoke(budowniczyModelu.Configurations, new object[] { konfiguracjaTypuEncji });
            }

            DodajNależnościIObroty(budowniczyModelu);

            using (CzynszeKontekst db = new CzynszeKontekst(new object()))
                _model = budowniczyModelu.Build(db.Database.Connection).Compile();
        }

        static void DodajNależnościIObroty(DbModelBuilder budowniczyModelu)
        {
            List<string> nazwyTabel = new List<string>();

            nazwyTabel.Add(String.Concat("nal_", Rok, "__"));
            nazwyTabel.Add(String.Concat("nak_", Rok, "__"));
            nazwyTabel.Add(String.Concat("nam_", Rok, "__"));
            nazwyTabel.Add(String.Concat("obr_", Rok, "__"));
            nazwyTabel.Add(String.Concat("obk_", Rok, "__"));
            nazwyTabel.Add(String.Concat("obm_", Rok, "__"));


            budowniczyModelu.Entity<Należność1>().ToTable(nazwyTabel[0], "public");
            budowniczyModelu.Entity<Należność2>().ToTable(nazwyTabel[1], "public");
            budowniczyModelu.Entity<Należność3>().ToTable(nazwyTabel[2], "public");
            budowniczyModelu.Entity<Obrót1>().ToTable(nazwyTabel[3], "public");
            budowniczyModelu.Entity<Obrót2>().ToTable(nazwyTabel[4], "public");
            budowniczyModelu.Entity<Obrót3>().ToTable(nazwyTabel[5], "public");

            _typEncjiNaNazwęTabeli = new Dictionary<Type, string>();

            _typEncjiNaNazwęTabeli.Add(typeof(Należność1), nazwyTabel[0]);
            _typEncjiNaNazwęTabeli.Add(typeof(Należność2), nazwyTabel[1]);
            _typEncjiNaNazwęTabeli.Add(typeof(Należność3), nazwyTabel[2]);
            _typEncjiNaNazwęTabeli.Add(typeof(Obrót1), nazwyTabel[3]);
            _typEncjiNaNazwęTabeli.Add(typeof(Obrót2), nazwyTabel[4]);
            _typEncjiNaNazwęTabeli.Add(typeof(Obrót3), nazwyTabel[5]);
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

        public static readonly Dictionary<Enumeratory.Tabela, Type> TabelaNaTypRekordu = new Dictionary<Enumeratory.Tabela, Type>()
        {
            {Enumeratory.Tabela.AktywneLokale, typeof(AktywnyLokal)},
            {Enumeratory.Tabela.AktywniNajemcy, typeof(AktywnyNajemca)},
            {Enumeratory.Tabela.Atrybuty, typeof(Atrybut)},
            {Enumeratory.Tabela.Budynki, typeof(Budynek)},
            {Enumeratory.Tabela.GrupyFinansowe, typeof(GrupaFinansowa)},
            {Enumeratory.Tabela.GrupySkładnikowCzynszu, typeof(GrupaSkładnikówCzynszu)},
            {Enumeratory.Tabela.NieaktywneLokale, typeof(NieaktywnyLokal)},
            {Enumeratory.Tabela.NieaktywniNajemcy, typeof(NieaktywnyNajemca)},
            {Enumeratory.Tabela.RodzajeNajemcy, typeof(TypNajemcy)},
            {Enumeratory.Tabela.SkladnikiCzynszu, typeof(SkładnikCzynszu)},
            {Enumeratory.Tabela.StawkiVat, typeof(StawkaVat)},
            {Enumeratory.Tabela.TypyKuchni, typeof(TypKuchni)},
            {Enumeratory.Tabela.TypyLokali, typeof(TypLokalu)},
            {Enumeratory.Tabela.TypyWplat, typeof(RodzajPłatności)},
            {Enumeratory.Tabela.TytulyPrawne, typeof(TytułPrawny)},
            {Enumeratory.Tabela.Uzytkownicy, typeof(Użytkownik)},
            {Enumeratory.Tabela.Wspolnoty, typeof(Wspólnota)},
            {Enumeratory.Tabela.ObrotyNajemcy, typeof(Obrót1)}
        };
    }
}