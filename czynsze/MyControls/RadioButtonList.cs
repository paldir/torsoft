using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.MyControls
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList
    {
        public RadioButtonList(string cSSClass, string id, List<string> texts, List<string> values, string selectedValue, bool enabled, bool autoPostBack)
        {
            CssClass = cSSClass;
            ID = id;
            Enabled = enabled;
            AutoPostBack = autoPostBack;

            for (int i = 0; i < texts.Count; i++)
                Items.Add(new System.Web.UI.WebControls.ListItem(texts[i], values[i]));

            SelectedValue = selectedValue;
        }
    }
}