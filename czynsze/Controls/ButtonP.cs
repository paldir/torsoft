using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.Controls
{
    public class ButtonP : Button
    {
        public ButtonP(string cSSClass, string id, string text, string postBackUrl)
        {
            this.CssClass = cSSClass;
            this.ID = id;
            this.Text = text;
            this.PostBackUrl = postBackUrl;
        }
    }
}