using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class Hello : System.Web.UI.Page
    {
        public static string CompanyName { get; set; }
        public static DateTime Date { get; set; }
        public static List<string> SiteMapPath { get; set; }
        public static int NumberOfSets { get; set; }
        public static Enumeratory.Zbiór CurrentSet { get; set; }
        public static string[] NamesOfSets { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = CompanyName;
            user.InnerText = Session["user"].ToString();
            month.InnerText = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Date.Month - 1].ToString() + " " + Date.Year.ToString();
        }

        public static string ExceptionMessage(Exception exception)
        {
            if (exception == null)
                return String.Empty;
            else
                return String.Format("{0}<br />{1}", exception.Message, ExceptionMessage(exception.InnerException));
        }
    }
}