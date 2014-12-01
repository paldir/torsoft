using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace czynsze.ControlsP
{
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        public enum TextBoxModeP { SingleLine, MultiLine, Date, Number, Float, Password };
        
        public TextBox(string cSSClass, string id, string text, TextBoxModeP textMode, int maxLength, int rows, bool enabled)
        {
            CssClass = cSSClass;
            ID = id;
            Text = text;

            switch (textMode)
            {
                case TextBoxModeP.MultiLine:
                    TextMode = TextBoxMode.MultiLine;

                    Attributes.Add("maxlength", maxLength.ToString());

                    break;

                case TextBoxModeP.Number:
                    Attributes.Add("onkeypress", "return isInteger(event)");

                    break;

                case TextBoxModeP.Float:
                    Attributes.Add("onkeypress", "return isFloat(event)");

                    break;

                case TextBoxModeP.Date:
                    Attributes.Add("onkeypress", "return isDate(event)");

                    break;

                case TextBoxModeP.Password:
                    TextMode = TextBoxMode.Password;

                    break;
            }
            
            MaxLength = maxLength; Columns = maxLength / rows;
            Rows = rows;
            Enabled = enabled;
        }
    }
}