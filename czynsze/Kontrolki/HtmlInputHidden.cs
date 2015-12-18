using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class HtmlInputHidden : System.Web.UI.HtmlControls.HtmlInputHidden
    {
        public HtmlInputHidden(string id, object wartość)
        {
            ID = id;

            if (wartość != null)
                Value = wartość.ToString();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(new PisarzTekstuHtml(writer, ID));
        }
    }
}