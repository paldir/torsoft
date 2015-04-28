using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class HtmlGenericControl : System.Web.UI.HtmlControls.HtmlGenericControl
    {
        public HtmlGenericControl(string nazwaTagu, string klasaCss)
        {
            TagName = nazwaTagu;

            Attributes.Add("class", klasaCss);
        }
    }
}