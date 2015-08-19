using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class Button : System.Web.UI.WebControls.Button
    {
        string _nazwa;

        public Button(string klasaCss, string id, string tekst, string url, string nazwa = null)
        {
            CssClass = klasaCss;
            ID = id;
            Text = tekst;
            PostBackUrl = url;

            if (String.IsNullOrEmpty(nazwa))
                _nazwa = ID;
            else
                _nazwa = nazwa;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(new PisarzTekstuHtml(writer, _nazwa));
        }
    }
}