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
        List<string> siteMapPath
        {
            get
            {
                if (Session["siteMapPath"] == null)
                    return new List<string>();

                return (List<string>)Session["siteMapPath"];
            }
            set { Session["siteMapPath"] = value; }
        }

        DateTime date
        {
            get
            {
                if (Session["date"] == null)
                    return DateTime.Today;

                return (DateTime)Session["date"];
            }
        }
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("../Login.aspx?reason=" + EnumP.ReasonOfRedirectToLoginPage.NotLoggedInOrSessionExpired);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user.InnerText = Session["user"].ToString();
            placeOfSiteMapPath.InnerHtml = String.Empty;

            foreach (string siteMapNode in siteMapPath)
                placeOfSiteMapPath.InnerHtml += siteMapNode + " > ";

            if (siteMapPath.Count > 0)
                placeOfSiteMapPath.InnerHtml = placeOfSiteMapPath.InnerHtml.Remove(placeOfSiteMapPath.InnerHtml.Length - 3);
            /*else
                placeOfSiteMapPath.Visible = false;*/

            placeOfSelectedDate.InnerHtml = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[date.Month - 1].ToString() + "." + date.Year.ToString();
        }
    }
}