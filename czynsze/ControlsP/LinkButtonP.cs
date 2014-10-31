using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class LinkButtonP : LinkButton
    {
        public LinkButtonP(string cSSClass, string id, string text)
        {
            CssClass = cSSClass;
            ID = id;
            Text = text;
        }
    }
}