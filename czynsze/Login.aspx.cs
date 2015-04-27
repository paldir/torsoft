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

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Forms.Hello.CompanyName = companyName.InnerText = db.Konfiguracje.FirstOrDefault().nazwa_1;
                Forms.Hello.NumberOfSets = db.Konfiguracje.FirstOrDefault().p_32;
                Forms.Hello.NamesOfSets = new string[] { "CZYNSZE", db.Konfiguracje.FirstOrDefault().nazwa_2zb, db.Konfiguracje.FirstOrDefault().nazwa_3zb };
            }

            Forms.Hello.Date = DateTime.Today;
            Forms.Hello.SiteMapPath = new List<string>();
            Forms.Hello.CurrentSet = Enums.SettlementTable.Czynsze;

            Response.Redirect("Forms/UserValidation.aspx?uzytkownik=admin&haslo=a");
        }
    }
}