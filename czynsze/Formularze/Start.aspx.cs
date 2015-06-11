using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class Start : System.Web.UI.Page
    {
        public static string NazwaFirmy { get; set; }
        public static DateTime Data { get; set; }
        public static List<string> ŚcieżkaStrony { get; set; }
        public static int LiczbaZbiorów { get; set; }
        public static Enumeratory.Zbiór AktywnyZbiór { get; set; }
        public static string[] NazwyZbiorów { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = NazwaFirmy;
            user.InnerText = Session["uzytkownik"].ToString();
            month.InnerText = String.Format(" USTAWIONEGO NA PAŁĘ W CELACH TESTOWYCH {0} {1}", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Data.Month - 1], Data.Year);
        }

        public static string ExceptionMessage(Exception wyjątek)
        {
            if (wyjątek == null)
                return String.Empty;
            else
                return String.Format("{0}<br />{1}", wyjątek.Message, ExceptionMessage(wyjątek.InnerException));
        }
    }
}