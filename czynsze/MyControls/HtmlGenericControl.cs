using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.MyControls
{
    public class HtmlGenericControl : System.Web.UI.HtmlControls.HtmlGenericControl
    {
        public HtmlGenericControl(string tagName, string className)
        {
            TagName = tagName;

            Attributes.Add("class", className);
        }
    }
}