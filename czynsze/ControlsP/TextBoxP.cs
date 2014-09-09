using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class TextBoxP : TextBox
    {
        public TextBoxP(string cSSClass, string id, string text, TextBoxMode textMode, int maxLength, int rows, bool enabled)
        {
            this.CssClass = cSSClass;
            this.ID = id;
            this.Text = text;

            switch (textMode)
            {
                case TextBoxMode.MultiLine:
                    this.TextMode = textMode;

                    this.Attributes.Add("maxlength", maxLength.ToString());
                    break;
            }
            
            this.MaxLength = maxLength; this.Columns = maxLength / rows;
            this.Rows = rows;
            this.Enabled = enabled;
        }
    }
}