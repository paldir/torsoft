using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

namespace czynsze.Formularze
{
    public partial class WalidacjaUzytkownika : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string użytkownik = Request.Params["uzytkownik"];
            string hasło = Request.Params["haslo"];
            bool walidacjaPomyślna = false;
            DostępDoBazy.Użytkownik wybranyUżytkownik;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                wybranyUżytkownik = db.Użytkownicy.FirstOrDefault(u => u.uzytkownik == użytkownik);

            if (wybranyUżytkownik != null)
                if (Enumerable.SequenceEqual(Encoding.UTF8.GetBytes(hasło), Encoding.UTF8.GetBytes(wybranyUżytkownik.haslo.Trim()).Select(b => (byte)(b - 10)).ToArray()))
                    walidacjaPomyślna = true;

            if (walidacjaPomyślna)
            {
                Session["uzytkownik"] = użytkownik;
                Response.Redirect("Start.aspx");
            }
            else
                Response.Redirect("../Logowanie.aspx?reason=" + Enumeratory.PowódPrzeniesieniaNaStronęLogowania.NiepoprawneDaneUwierzytelniające);
        }
    }
}