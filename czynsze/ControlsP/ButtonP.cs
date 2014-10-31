using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class ButtonP : Button
    {
        public ButtonP(string cSSClass, string id, string text, string postBackUrl)
        {
            CssClass = cSSClass;
            ID = id;
            Text = text;
            PostBackUrl = postBackUrl;
        }
    }
}