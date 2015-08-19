using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.Kontrolki
{
    public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList
    {
        public CheckBoxList(string klasaCss, string id, List<string> tekst, List<string> wartości, List<string> wybraneWartości, bool włączone)
        {
            CssClass = klasaCss;
            ID = id;
            Enabled = włączone;

            for (int i = 0; i < wartości.Count; i++)
                Items.Add(new ListItem(tekst[i], wartości[i]));

            foreach (string selectedValue in wybraneWartości)
            {
                ListItem pozycja = Items.FindByValue(selectedValue);

                if (pozycja != null)
                    pozycja.Selected = true;
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(new PisarzTekstuHtml(writer, ID));
        }
    }
}