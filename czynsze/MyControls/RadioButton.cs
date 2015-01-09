using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.MyControls
{
    public class RadioButton : System.Web.UI.WebControls.RadioButton
    {
        public RadioButton(string cSSClass, string id, string groupName)
        {
            CssClass = cSSClass;
            ID = id;
            GroupName = groupName;
        }
    }
}