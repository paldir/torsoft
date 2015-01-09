using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Site : System.Web.UI.MasterPage
    {        
        protected void Page_Init(object sender, EventArgs e)
        {   
            if (Session["user"] == null)
                Response.Redirect("../Login.aspx?reason=" + Enums.ReasonOfRedirectToLoginPage.NotLoggedInOrSessionExpired);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user.InnerText = Session["user"].ToString();
            placeOfSiteMapPath.InnerHtml = String.Empty;

            foreach (string siteMapNode in Forms.Hello.SiteMapPath)
                placeOfSiteMapPath.InnerHtml += siteMapNode + " > ";

            if (Forms.Hello.SiteMapPath.Count > 0)
                placeOfSiteMapPath.InnerHtml = placeOfSiteMapPath.InnerHtml.Remove(placeOfSiteMapPath.InnerHtml.Length - 3);

            placeOfSelectedDate.InnerHtml = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Forms.Hello.Date.Month - 1].ToString() + " " + Forms.Hello.Date.Year.ToString();
            placeOfCurrentSet.InnerHtml = Forms.Hello.NamesOfSets[(int)Forms.Hello.CurrentSet];
        }
    }
}