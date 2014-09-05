using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Rekord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.Form[Request.Form.AllKeys.FirstOrDefault(k => k.EndsWith("$id"))];
            string action = Request.Form[Request.Form.AllKeys.FirstOrDefault(k => k.EndsWith("action"))];
            string table = Request.Form[Request.Form.AllKeys.FirstOrDefault(k => k.EndsWith("$table"))];
        }
    }
}