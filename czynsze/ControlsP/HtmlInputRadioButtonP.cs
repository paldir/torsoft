using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class HtmlInputRadioButtonP : System.Web.UI.HtmlControls.HtmlInputRadioButton
    {
        public HtmlInputRadioButtonP(string className, string id, string name, string value, bool checkedValue)
        {
            ID = id;
            Name = name;
            Value = value;
            Checked = checkedValue;

            Attributes.Add("class", className);
        }
    }
}