using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;

namespace czynsze.Formularze
{
    public partial class SkladnikiCzynszu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?{0}raport=dummy", Enumeratory.Raport.SkładnikiCzynszu));
        }
    }
}