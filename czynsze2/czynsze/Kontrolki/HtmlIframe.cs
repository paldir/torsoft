using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class HtmlIframe : System.Web.UI.HtmlControls.HtmlIframe
    {
        public HtmlIframe(string klasaCss, string id, string src, string ukryty)
        {
            ID = id;
            Src = src;

            Attributes.Add("class", klasaCss);
            Attributes.Add("hidden", ukryty);
        }
    }
}