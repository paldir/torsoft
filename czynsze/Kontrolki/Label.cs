using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class Label : System.Web.UI.WebControls.Label
    {
        public Label(string klasaCss, string idPowiązanejKontrolki, string tekst, string id)
        {
            CssClass = klasaCss;
            AssociatedControlID = idPowiązanejKontrolki;
            Text = tekst;
            ID = id;
        }
    }
}