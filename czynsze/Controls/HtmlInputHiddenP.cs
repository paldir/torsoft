using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Controls
{
    public class HtmlInputHiddenP : System.Web.UI.HtmlControls.HtmlInputHidden
    {
        public HtmlInputHiddenP(string id, string value)
        {
            this.ID = id;
            this.Value = value;
        }
    }
}