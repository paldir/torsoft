using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class DropDownListP : DropDownList
    {
        public DropDownListP(string cSSClass, string id, List<string[]> rows, string selectedValue, bool enabled, bool addEmptyItem)
        {
            CssClass = cSSClass;
            ID = id;
            Enabled = enabled;

            if (addEmptyItem)
                Items.Add(new ListItem(String.Empty, "0"));

            foreach (string[] row in rows)
            {
                string value = row[0];
                string text = "";

                for (int i = 1; i < row.Length; i++)
                    text += row[i] + ", ";

                text = text.Remove(text.Length - 2, 2);

                Items.Add(new ListItem(text, value));
            }

            SelectedValue = selectedValue;
        }
    }
}