using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.ControlsP
{
    public class HtmlIframeP : System.Web.UI.HtmlControls.HtmlIframe
    {
        public HtmlIframeP(string className, string id, string src, string hidden)
        {
            this.ID = id;
            this.Src = src;

            this.Attributes.Add("class", className);
            this.Attributes.Add("hidden", hidden);
        }
    }
}