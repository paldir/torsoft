﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class RadioButtonListP : RadioButtonList
    {
        public RadioButtonListP(string cSSClass, string id, List<string> texts, List<string> values, string selectedValue, bool enabled, bool autoPostBack)
        {
            CssClass = cSSClass;
            ID = id;
            Enabled = enabled;
            AutoPostBack = autoPostBack;

            for (int i = 0; i < texts.Count; i++)
                Items.Add(new ListItem(texts[i], values[i]));

            SelectedValue = selectedValue;
        }
    }
}