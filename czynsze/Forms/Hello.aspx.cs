using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class Hello : System.Web.UI.Page
    {
        public static DateTime date;
        public static List<string> siteMapPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = Session["nazwa_1"].ToString();
            user.InnerText = Session["user"].ToString();
            month.InnerText = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Forms.Hello.date.Month - 1].ToString() + " " + Forms.Hello.date.Year.ToString();

            byte[] tablica = new byte[10];
            Random rand = new Random();

            rand.NextBytes(tablica);
        }
    }
}