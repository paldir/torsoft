using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class RadioButtonP : RadioButton
    {
        public RadioButtonP(string cSSClass, string id, string groupName)
        {
            CssClass = cSSClass;
            ID = id;
            GroupName = groupName;
        }
    }
}