using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class Button : System.Web.UI.WebControls.Button
    {
        public Button(string klasaCss, string id, string tekst, string url)
        {
            CssClass = klasaCss;
            ID = id;
            Text = tekst;
            PostBackUrl = url;
        }
    }
}