using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class DropDownListP: DropDownList
    {
        public DropDownListP(string cSSClass, string id, List<string[]> rows, string selectedValue, bool enabled)
        {
            this.CssClass = cSSClass;
            this.ID = id;
            this.Enabled = enabled;

            this.Items.Add(new ListItem(String.Empty, "0"));
            
            foreach (string[] row in rows)
            {
                string value = row[0];
                string text = "";

                for (int i = 1; i < row.Length; i++)
                    text += row[i] + ", ";

                text = text.Remove(text.Length - 2, 2);

                this.Items.Add(new ListItem(text, value));
            }

            this.SelectedValue = selectedValue;
        }
    }
}