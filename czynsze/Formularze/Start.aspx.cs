using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using System.Reflection.Emit;

namespace czynsze.Formularze
{
    public partial class Start : Strona
    {
        public static string NazwaFirmy { get; set; }
        public static DateTime Data { get; set; }
        public static ŚcieżkaStrony ŚcieżkaStrony { get; set; }
        public static int LiczbaZbiorów { get; set; }
        public static Enumeratory.Zbiór AktywnyZbiór { get; set; }
        public static string[] NazwyZbiorów { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = NazwaFirmy;
            user.InnerText = WartościSesji.AktualnieZalogowanyUżytkownik;
            month.InnerText = String.Format("{0} {1}", DostępDoBazy.CzynszeKontekst.NumerMiesiącaNaNazwęZPolskimiZnakami[Data.Month], Data.Year);
            WartościSesji.MagazynRekordów = new MagazynRekordów();
        }

        public static string KomunikatWyjątku(Exception wyjątek)
        {
            if (wyjątek == null)
                return String.Empty;
            else
                return String.Concat(wyjątek.Message, "<br />", KomunikatWyjątku(wyjątek.InnerException));
        }
    }
}