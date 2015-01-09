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
        public static string CompanyName { get; set; }
        public static DateTime Date { get; set; }
        public static List<string> SiteMapPath { get; set; }
        public static int NumberOfSets { get; set; }
        public static Enums.SettlementTable CurrentSet { get; set; }
        public static string[] NamesOfSets { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = CompanyName;
            user.InnerText = Session["user"].ToString();
            month.InnerText = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Date.Month - 1].ToString() + " " + Date.Year.ToString();
        }
    }
}