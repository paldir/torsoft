using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class HtmlInputHidden : System.Web.UI.HtmlControls.HtmlInputHidden
    {
        public HtmlInputHidden(string id, string value)
        {
            ID = id;
            Value = value;
        }
    }
}