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
            this.ID = id;
            this.Name = name;
            this.Value = value;
            this.Checked = checkedValue;

            this.Attributes.Add("class", className);
        }
    }
}