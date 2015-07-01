using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace czynsze.DostępDoBazy
{
    public class CzynszeKontekst : DbContext
    {
        public CzynszeKontekst() : base(nameOrConnectionString: "czynsze_connectionString") { }
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
        public DbSet<NależnośćZPierwszegoZbioru> NależnościZPierwszegoZbioru { get; set; }
        public DbSet<ObrótZPierwszegoZbioru> ObrotyZPierwszegoZbioru { get; set; }
        public DbSet<NależnośćZDrugiegoZbioru> NależnościZDrugiegoZbioru { get; set; }
        public DbSet<ObrótZDrugiegoZbioru> ObrotyZDrugiegoZbioru { get; set; }
        public DbSet<NależnośćZTrzeciegoZbioru> NależnościZTrzeciegoZbioru { get; set; }
        public DbSet<ObrótZTrzeciegoZbioru> ObrotyZTrzeciegoZbioru { get; set; }
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
    }
}