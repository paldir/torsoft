﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.Controls
{
    public class RadioButtonP : RadioButton
    {
        public RadioButtonP(string cSSClass, string id, string groupName)
        {
            this.CssClass = cSSClass;
            this.ID = id;
            this.GroupName = groupName;
        }
    }
}