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
        public static string companyName;
        public static DateTime date;
        public static List<string> siteMapPath;
        public static int settlementTables;
        public static EnumP.SettlementTable currentSettlementTable;
        public static string[] namesOfSets;

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = companyName;
            user.InnerText = Session["user"].ToString();
            month.InnerText = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[date.Month - 1].ToString() + " " + date.Year.ToString();
        }
    }
}