using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Logowanie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Formularze.Hello.CompanyName = companyName.InnerText = db.Konfiguracje.FirstOrDefault().nazwa_1;
                Formularze.Hello.NumberOfSets = db.Konfiguracje.FirstOrDefault().p_32;
                Formularze.Hello.NamesOfSets = new string[] { "CZYNSZE", db.Konfiguracje.FirstOrDefault().nazwa_2zb, db.Konfiguracje.FirstOrDefault().nazwa_3zb };
            }

            Formularze.Hello.Date = DateTime.Today;
            Formularze.Hello.SiteMapPath = new List<string>();
            Formularze.Hello.CurrentSet = Enumeratory.Zbiór.Czynsze;

            Response.Redirect("Formularze/WalidacjaUzytkownika.aspx?uzytkownik=admin&haslo=a");
        }
    }
}