using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public class Sesja
    {
        const string _klucz = "__mojaSesja__";

        Sesja() { }

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

        public void Wyczyść()
        {
            HttpContext.Current.Session[_klucz] = null;
        }
    }
}