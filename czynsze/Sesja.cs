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
        public List<DostępDoBazy.Plik> Pliki { get; set; }
        public MagazynRekordów MagazynRekordów { get; set; }

        public void Wyczyść()
        {
            HttpContext.Current.Session[_klucz] = null;
        }
    }

    public class MagazynRekordów
    {
        /*public*/ List<DostępDoBazy.AktywnyLokal> Lokale { get; set; }
        public List<DostępDoBazy.AktywnyNajemca> Najemcy { get; set; }
        public List<DostępDoBazy.Wspólnota> Wspólnoty { get; set; }
        public List<DostępDoBazy.Budynek> Budynki { get; set; }
        public List<DostępDoBazy.TypLokalu> TypyLokali { get; set; }
        public List<DostępDoBazy.SkładnikCzynszu> SkładnikiCzynszu { get; set; }

        public Dictionary<int, Dictionary<int, DostępDoBazy.AktywnyLokal>> KodINumerNaLokal { get; set; }
        public Dictionary<int, DostępDoBazy.AktywnyLokal> KluczNaLokal { get; set; }

        public Dictionary<int, DostępDoBazy.Budynek> KodNaBudynek { get; set; }
        public Dictionary<int, DostępDoBazy.Budynek> KluczNaBudynek { get; set; }

        public Dictionary<int, DostępDoBazy.AktywnyNajemca> NrKontrNaNajemcę { get; set; }
        public Dictionary<int, DostępDoBazy.AktywnyNajemca> KluczNaNajemcę { get; set; }

        public Dictionary<int, DostępDoBazy.Wspólnota> KodNaWspólnotę { get; set; }
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
                KodINumerNaLokal = new Dictionary<int, Dictionary<int, DostępDoBazy.AktywnyLokal>>();

                KodNaBudynek = Budynki.ToDictionary(b => b.kod_1);
                KluczNaBudynek = Budynki.ToDictionary(b => b.__record);

                NrKontrNaNajemcę = Najemcy.ToDictionary(n => n.nr_kontr);
                KluczNaNajemcę = Najemcy.ToDictionary(n => n.__record);

                KodNaWspólnotę = Wspólnoty.ToDictionary(w => w.kod);
                KluczNaWspólnotę = Wspólnoty.ToDictionary(w => w.__record);

                KodNaTypLokalu = TypyLokali.ToDictionary(t => t.kod_typ);
                NumerSkładnikaNaSkładnikCzynszu = SkładnikiCzynszu.ToDictionary(s => s.nr_skl);

                foreach (DostępDoBazy.AktywnyLokal lokal in db.AktywneLokale)
                {
                    int kodBudynku = lokal.kod_lok;

                    if (!KodINumerNaLokal.ContainsKey(kodBudynku))
                        KodINumerNaLokal.Add(kodBudynku, new Dictionary<int, DostępDoBazy.AktywnyLokal>());

                    KodINumerNaLokal[kodBudynku].Add(lokal.nr_lok, lokal);
                    KluczNaLokal.Add(lokal.__record, lokal);
                }
            }
        }
    }
}