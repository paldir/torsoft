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
            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                companyName.InnerText = db.configurations.Select(c => c.naz_wiz).FirstOrDefault();

        }
    }
}