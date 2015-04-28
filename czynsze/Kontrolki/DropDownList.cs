using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.Kontrolki
{
    public class DropDownList : System.Web.UI.WebControls.DropDownList
    {
        public DropDownList(string klasaCss, string id, List<string[]> wiersze, string wybranaWartość, bool włączone, bool dodaćPustąPozycję)
        {
            CssClass = klasaCss;
            ID = id;
            Enabled = włączone;

            if (dodaćPustąPozycję)
                Items.Add(new ListItem(String.Empty, "0"));

            foreach (string[] row in wiersze)
            {
                string wartość = row[0];
                string tekst = String.Empty;

                for (int i = 1; i < row.Length; i++)
                    tekst += row[i] + ", ";

                tekst = tekst.Remove(tekst.Length - 2, 2);

                Items.Add(new ListItem(tekst, wartość));
            }

            SelectedValue = wybranaWartość;
        }
    }
}