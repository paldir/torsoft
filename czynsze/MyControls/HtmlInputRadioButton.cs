using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.MyControls
{
    public class HtmlInputRadioButton : System.Web.UI.HtmlControls.HtmlInputRadioButton
    {
        public HtmlInputRadioButton(string className, string id, string name, string value, bool checkedValue)
        {
            ID = id;
            Name = name;
            Value = value;
            Checked = checkedValue;

            Attributes.Add("class", className);
        }
    }
}