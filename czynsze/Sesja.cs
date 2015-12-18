using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public class Sesja
    {
        const string _klucz = "__mojaSesja__";

        Sesja()
        {
        }

        public static Sesja Obecna
        {
            get
            {
                Sesja sesja = HttpContext.Current.Session[_klucz] as Sesja;

                if (sesja == null)
                    HttpContext.Current.Session[_klucz] = sesja = new Sesja();

                return sesja;
            }
        }

        public string AktualnieZalogowanyUżytkownik { get; set; }
        public Enumeratory.Analiza TrybAnalizy { get; set; }
        public List<int> NumeryGrupWybranychSkładnikówCzynszu { get; set; }
        public List<DostępDoBazy.AtrybutObiektu> AtrybutyObiektu { get; set; }
        public List<DostępDoBazy.BudynekWspólnoty> BudynkiWspólnoty { get; set; }
        public Enumeratory.WykazWedługSkładnika TrybWykazuWgSkładnika { get; set; }
        public DateTime DataWykazuWgSkładnika { get; set; }
        public List<string> NagłówkiRaportu { get; set; }
        public List<List<string[]>> TabeleRaportu { get; set; }
        public List<string> PodpisyRaportu { get; set; }
        public string FormatRaportu { get; set; }
        public string TytułRaportu { get; set; }
        public List<string> GotowaDefinicjaHtmlRaportu { get; set; }
        public DostępDoBazy.Rekord Rekord { get; set; }
        public List<DostępDoBazy.SkładnikCzynszuLokalu> SkładnikiCzynszuLokalu { get; set; }
        public List<DostępDoBazy.Plik> PlikiObiektu { get; set; }
        public MagazynRekordów MagazynRekordów { get; set; }

        public void Wyczyść()
        {
            HttpContext.Current.Session[_klucz] = null;
        }
    }

    public class MagazynRekordów
    {
        public List<DostępDoBazy.AktywnyLokal> Lokale { get; set; }
        public List<DostępDoBazy.AktywnyNajemca> Najemcy { get; set; }
        public List<DostępDoBazy.Wspólnota> Wspólnoty { get; set; }
        public List<DostępDoBazy.Budynek> Budynki { get; set; }
        public List<DostępDoBazy.TypLokalu> TypyLokali { get; set; }
        public List<DostępDoBazy.SkładnikCzynszu> SkładnikiCzynszu { get; set; }

        public SortedDictionary<int, SortedDictionary<int, DostępDoBazy.Lokal>> KodINumerNaLokal { get; set; }
        public Dictionary<int, DostępDoBazy.AktywnyLokal> KluczNaLokal { get; set; }

        public SortedDictionary<int, DostępDoBazy.Budynek> KodNaBudynek { get; set; }
        public Dictionary<int, DostępDoBazy.Budynek> KluczNaBudynek { get; set; }

        public SortedDictionary<int, DostępDoBazy.AktywnyNajemca> NrKontrNaNajemcę { get; set; }
        public Dictionary<int, DostępDoBazy.AktywnyNajemca> KluczNaNajemcę { get; set; }

        public SortedDictionary<int, DostępDoBazy.Wspólnota> KodNaWspólnotę { get; set; }
        public Dictionary<int, DostępDoBazy.Wspólnota> KluczNaWspólnotę { get; set; }

        public Dictionary<int, DostępDoBazy.TypLokalu> KodNaTypLokalu { get; set; }
        public Dictionary<int, DostępDoBazy.SkładnikCzynszu> NumerSkładnikaNaSkładnikCzynszu { get; set; }

        public MagazynRekordów()
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {            
                Lokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                Najemcy = db.AktywniNajemcy.OrderBy(n => n.nr_kontr).ToList();
                Wspólnoty = db.Wspólnoty.OrderBy(w => w.kod).ToList();
                Budynki = db.Budynki.OrderBy(b => b.kod_1).ToList();
                TypyLokali = db.TypyLokali.OrderBy(t => t.kod_typ).ToList();
                SkładnikiCzynszu = db.SkładnikiCzynszu.OrderBy(s => s.nr_skl).ToList();

                KluczNaLokal = new Dictionary<int, DostępDoBazy.AktywnyLokal>();
                KodINumerNaLokal = new SortedDictionary<int, SortedDictionary<int, DostępDoBazy.Lokal>>();

                foreach (DostępDoBazy.AktywnyLokal lokal in db.AktywneLokale)
                {
                    int kodBudynku = lokal.kod_lok;

                    if (!KodINumerNaLokal.ContainsKey(kodBudynku))
                        KodINumerNaLokal.Add(kodBudynku, new SortedDictionary<int, DostępDoBazy.Lokal>());

                    KodINumerNaLokal[kodBudynku].Add(lokal.nr_lok, lokal);
                    KluczNaLokal.Add(lokal.__record, lokal);
                }

                KodNaBudynek = new SortedDictionary<int, DostępDoBazy.Budynek>();
                KluczNaBudynek = new Dictionary<int, DostępDoBazy.Budynek>();
                
                foreach (DostępDoBazy.Budynek budynek in db.Budynki)
                {
                    KodNaBudynek.Add(budynek.kod_1, budynek);
                    KluczNaBudynek.Add(budynek.__record, budynek);
                }

                NrKontrNaNajemcę = new SortedDictionary<int, DostępDoBazy.AktywnyNajemca>();
                KluczNaNajemcę = new Dictionary<int, DostępDoBazy.AktywnyNajemca>();

                foreach(DostępDoBazy.AktywnyNajemca najemca in db.AktywniNajemcy)
                {
                    NrKontrNaNajemcę.Add(najemca.nr_kontr, najemca);
                    KluczNaNajemcę.Add(najemca.__record, najemca);
                }

                KodNaWspólnotę = new SortedDictionary<int, DostępDoBazy.Wspólnota>();
                KluczNaWspólnotę = new Dictionary<int, DostępDoBazy.Wspólnota>();

                foreach (DostępDoBazy.Wspólnota wspólnota in db.Wspólnoty)
                {
                    KodNaWspólnotę.Add(wspólnota.kod, wspólnota);
                    KluczNaWspólnotę.Add(wspólnota.__record, wspólnota);
                }

                KodNaTypLokalu = TypyLokali.ToDictionary(t => t.kod_typ);
                NumerSkładnikaNaSkładnikCzynszu = SkładnikiCzynszu.ToDictionary(s => s.nr_skl);
            }
        }
    }
}