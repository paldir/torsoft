using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class Button : System.Web.UI.WebControls.Button
    {
        public Button(string cSSClass, string id, string text, string postBackUrl)
        {
            CssClass = cSSClass;
            ID = id;
            Text = text;
            PostBackUrl = postBackUrl;
        }
    }
}