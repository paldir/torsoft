using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class LinkButton : System.Web.UI.WebControls.LinkButton
    {
        public LinkButton(string cSSClass, string id, string text)
        {
            CssClass = cSSClass;
            ID = id;
            Text = text;
        }
    }
}