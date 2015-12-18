using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class LinkButton : System.Web.UI.WebControls.LinkButton
    {
        public LinkButton(string klasaCss, string id, string tekst)
        {
            CssClass = klasaCss;
            ID = id;
            Text = tekst;
        }
    }
}