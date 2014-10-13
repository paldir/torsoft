using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                Session["nazwa_1"] = companyName.InnerText = db.configurations.Select(c => c.nazwa_1).FirstOrDefault();

            Forms.Hello.date = DateTime.Today;
            Forms.Hello.siteMapPath = new List<string>();

            Response.Redirect("Forms/UserValidation.aspx?uzytkownik=Zaw Pat&haslo=148,34,6,255");
        }
    }
}