using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class UserValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Request.Params["uzytkownik"];
            int[] password = Request.Params["haslo"].ToString().Split(',').ToList().Select(a => Convert.ToInt32(a)).ToArray();
            string correctPassword;
            bool validated = false;

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                correctPassword = db.users.Where(u => u.uzytkownik == user).FirstOrDefault().haslo.Trim();

            if (password.Length == correctPassword.Length)
                validated = true;

            if (validated)
            {
                Session["user"] = user;
                Response.Redirect("List.aspx?table=Buildings");
            }
            else
                Response.Redirect("../Login.aspx");
        }
    }
}