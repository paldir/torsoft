using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class CheckBoxListP : CheckBoxList
    {
        public CheckBoxListP(string cSSClass, string id, List<string> texts, List<string> values, List<string> selectedValues)
        {
            this.CssClass = cSSClass;
            this.ID = id;

            for (int i = 0; i < values.Count; i++)
                this.Items.Add(new ListItem(texts[i], values[i]));

            foreach (string selectedValue in selectedValues)
            {
                ListItem item = this.Items.FindByValue(selectedValue);

                if (item != null)
                    item.Selected = true;
            }
        }
    }
}