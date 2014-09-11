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
            this.CssClass = cSSClass;
            this.AssociatedControlID = associatedControlId;
            this.Text = text;
            this.ID = id;
        }
    }
}