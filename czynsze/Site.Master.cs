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
                Response.Redirect("../Login.aspx?reason=" + Enumeratory.PowódPrzeniesieniaNaStronęLogowania.NiezalogowanyLubSesjaWygasła);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user.InnerText = Session["user"].ToString();
            placeOfSiteMapPath.InnerHtml = String.Empty;

            foreach (string siteMapNode in Formularze.Hello.SiteMapPath)
                placeOfSiteMapPath.InnerHtml += siteMapNode + " > ";

            if (Formularze.Hello.SiteMapPath.Any())
                placeOfSiteMapPath.InnerHtml = placeOfSiteMapPath.InnerHtml.Remove(placeOfSiteMapPath.InnerHtml.Length - 3);

            placeOfSelectedDate.InnerHtml = System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Formularze.Hello.Date.Month - 1].ToString() + " " + Formularze.Hello.Date.Year.ToString();
            placeOfCurrentSet.InnerHtml = Formularze.Hello.NamesOfSets[(int)Formularze.Hello.CurrentSet];
        }
    }
}