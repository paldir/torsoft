using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.MyControls
{
    public class Label : System.Web.UI.WebControls.Label
    {
        public Label(string cSSClass, string associatedControlId, string text, string id)
        {
            CssClass = cSSClass;
            AssociatedControlID = associatedControlId;
            Text = text;
            ID = id;
        }
    }
}