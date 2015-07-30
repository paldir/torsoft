using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList, IKontrolkaZWartością
    {
        public string Wartość
        {
            get { return SelectedValue; }
            set { SelectedValue = value; }
        }

        public RadioButtonList(string klasaCss, string id, List<string> tekst, List<string> wartości, bool włączony, bool automatycznyPostBack)
        {
            CssClass = klasaCss;
            ID = id;
            Enabled = włączony;
            AutoPostBack = automatycznyPostBack;

            for (int i = 0; i < tekst.Count; i++)
                Items.Add(new System.Web.UI.WebControls.ListItem(tekst[i], wartości[i]));
        }

        public RadioButtonList(string klasaCss, string id, List<string> tekst, List<string> wartości, bool włączony, bool automatycznyPostBack, string wybranaWartość)
            : this(klasaCss, id, tekst, wartości, włączony, automatycznyPostBack)
        {
            SelectedValue = wybranaWartość;
        }
    }
}