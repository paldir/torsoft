using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

namespace czynsze.Formularze
{
    public partial class UserValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Request.Params["uzytkownik"];
            string password = Request.Params["haslo"];
            bool validated = false;
            DostępDoBazy.Użytkownik typedUser;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                typedUser = db.Użytkownicy.FirstOrDefault(u => u.uzytkownik == user);

            if (typedUser != null)
                if (Enumerable.SequenceEqual(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(typedUser.haslo.Trim()).Select(b => (byte)(b - 10)).ToArray()))
                    validated = true;

            if (validated)
            {
                Session["user"] = user;
                Response.Redirect("Hello.aspx");
            }
            else
                Response.Redirect("../Login.aspx?reason=" + Enumeratory.PowódPrzeniesieniaNaStronęLogowania.NiepoprawneDaneUwierzytelniające);
        }
    }
}