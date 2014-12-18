using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class HtmlIframe : System.Web.UI.HtmlControls.HtmlIframe
    {
        public HtmlIframe(string className, string id, string src, string hidden)
        {
            ID = id;
            Src = src;

            Attributes.Add("class", className);
            Attributes.Add("hidden", hidden);
        }
    }
}