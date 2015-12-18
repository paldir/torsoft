using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Logowanie : Formularze.Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WartościSesji.Wyczyść();

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Formularze.Start.NazwaFirmy = companyName.InnerText = db.Konfiguracje.FirstOrDefault().nazwa_1;
                Formularze.Start.LiczbaZbiorów = db.Konfiguracje.FirstOrDefault().p_32;
                Formularze.Start.NazwyZbiorów = new string[] { "CZYNSZE", db.Konfiguracje.FirstOrDefault().nazwa_2zb, db.Konfiguracje.FirstOrDefault().nazwa_3zb };
            }

            Formularze.Start.Data = DateTime.Today;
            Formularze.Start.ŚcieżkaStrony = new ŚcieżkaStrony();
            Formularze.Start.AktywnyZbiór = Enumeratory.Zbiór.Czynsze;

            Response.Redirect("Formularze/WalidacjaUzytkownika.aspx?uzytkownik=TORSOFT TORSOFT&haslo=JK");
        }
    }
}