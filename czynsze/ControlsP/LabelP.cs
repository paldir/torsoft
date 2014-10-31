using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class LabelP : Label
    {
        public LabelP(string cSSClass, string associatedControlId, string text, string id)
        {
            CssClass = cSSClass;
            AssociatedControlID = associatedControlId;
            Text = text;
            ID = id;
        }
    }
}