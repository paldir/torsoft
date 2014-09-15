using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class UserValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Request.Params["uzytkownik"];
            string password = Request.Params["haslo"];

            /*using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                int tmp = db.users.Where(u => u.uzytkownik == user && u.haslo == password).Count();
            }*/

            Session["user"] = user;

            Response.Redirect("List.aspx?table=Buildings");
        }
    }
}