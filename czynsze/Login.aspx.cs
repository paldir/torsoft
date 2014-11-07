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
            {
                Forms.Hello.companyName = companyName.InnerText = db.configurations.FirstOrDefault().nazwa_1;
                Forms.Hello.numberOfSets = db.configurations.FirstOrDefault().p_32;
                Forms.Hello.namesOfSets = new string[] { "CZYNSZE", db.configurations.FirstOrDefault().nazwa_2zb, db.configurations.FirstOrDefault().nazwa_3zb };
            }

            Forms.Hello.date = DateTime.Today;
            Forms.Hello.siteMapPath = new List<string>();
            Forms.Hello.currentSet = EnumP.SettlementTable.Czynsze;

            //Response.Redirect("Forms/UserValidation.aspx?uzytkownik=Zaw Pat&haslo=148,34,6,255");
        }
    }
}