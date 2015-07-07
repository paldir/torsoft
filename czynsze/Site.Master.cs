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
            if (Session["uzytkownik"] == null)
                Response.Redirect("../Logowanie.aspx?przyczyna=" + Enumeratory.PowódPrzeniesieniaNaStronęLogowania.NiezalogowanyLubSesjaWygasla);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user.InnerText = Session["uzytkownik"].ToString();
            placeOfSiteMapPath.InnerHtml = String.Empty;

            foreach (string siteMapNode in Formularze.Start.ŚcieżkaStrony)
                placeOfSiteMapPath.InnerHtml += siteMapNode + " > ";

            if (Formularze.Start.ŚcieżkaStrony.Any())
                placeOfSiteMapPath.InnerHtml = placeOfSiteMapPath.InnerHtml.Remove(placeOfSiteMapPath.InnerHtml.Length - 3);

            placeOfSelectedDate.InnerHtml = DostępDoBazy.CzynszeKontekst.NumerMiesiącaNaNazwęZPolskimiZnakami[Formularze.Start.Data.Month].ToString() + " " + Formularze.Start.Data.Year.ToString();
            placeOfCurrentSet.InnerHtml = Formularze.Start.NazwyZbiorów[(int)Formularze.Start.AktywnyZbiór];
        }
    }
}