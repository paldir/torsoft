using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class TextBoxP : TextBox
    {
        public enum TextBoxModeP { SingleLine, MultiLine, Date, Number, Float, Password };
        
        public TextBoxP(string cSSClass, string id, string text, TextBoxModeP textMode, int maxLength, int rows, bool enabled)
        {
            this.CssClass = cSSClass;
            this.ID = id;
            this.Text = text;

            switch (textMode)
            {
                case TextBoxModeP.MultiLine:
                    this.TextMode = TextBoxMode.MultiLine;

                    this.Attributes.Add("maxlength", maxLength.ToString());
                    break;
                case TextBoxModeP.Number:
                    this.Attributes.Add("onkeypress", "return isInteger(event)");
                    break;
                case TextBoxModeP.Float:
                    this.Attributes.Add("onkeypress", "return isFloat(event)");
                    break;
                case TextBoxModeP.Date:
                    this.Attributes.Add("onkeypress", "return isDate(event)");
                    break;
                case TextBoxModeP.Password:
                    this.TextMode = TextBoxMode.Password;
                    break;
            }
            
            this.MaxLength = maxLength; this.Columns = maxLength / rows;
            this.Rows = rows;
            this.Enabled = enabled;
        }
    }
}