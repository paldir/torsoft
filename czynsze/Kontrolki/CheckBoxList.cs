using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.Kontrolki
{
    public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList, IKontrolkaZWartością
    {
        public string Wartość
        {
            get
            {
                List<string> wybraneWartości = new List<string>();

                foreach (ListItem pozycja in Items)
                    if (pozycja.Selected)
                        wybraneWartości.Add(pozycja.Value);

                return String.Join(",", wybraneWartości);
            }

            set
            {
                string[] wybraneWartości = value.Split(',');

                ZaznaczPozycje(wybraneWartości);
            }
        }

        public CheckBoxList(string klasaCss, string id, List<string> tekst, List<string> wartości, bool włączone, List<string> wybraneWartości = null)
        {
            CssClass = klasaCss;
            ID = id;
            Enabled = włączone;

            for (int i = 0; i < wartości.Count; i++)
                Items.Add(new ListItem(tekst[i], wartości[i]));

            if (wybraneWartości != null)
                ZaznaczPozycje(wybraneWartości);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(new PisarzTekstuHtml(writer, ID));
        }

        void ZaznaczPozycje(IEnumerable<string> wybraneWartości)
        {
            foreach (string selectedValue in wybraneWartości)
            {
                ListItem pozycja = Items.FindByValue(selectedValue);

                if (pozycja != null)
                    pozycja.Selected = true;
            }
        }
    }
}