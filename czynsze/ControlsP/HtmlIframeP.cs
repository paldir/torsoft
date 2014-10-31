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
            ID = id;
            Src = src;

            Attributes.Add("class", className);
            Attributes.Add("hidden", hidden);
            //Attributes.Add("onload", "src=src");
        }
    }
}