using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList
    {
        public RadioButtonList(string klasaCss, string id, List<string> tekst, List<string> wartości, string wybranaWartość, bool włączony, bool automatycznyPostBack)
        {
            CssClass = klasaCss;
            ID = id;
            Enabled = włączony;
            AutoPostBack = automatycznyPostBack;

            for (int i = 0; i < tekst.Count; i++)
                Items.Add(new System.Web.UI.WebControls.ListItem(tekst[i], wartości[i]));

            SelectedValue = wybranaWartość;
        }
    }
}