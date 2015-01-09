using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.MyControls
{
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        public enum TextBoxMode { SingleLine, MultiLine, Date, Number, Float, Password };
        
        public TextBox(string cSSClass, string id, string text, TextBoxMode textMode, int maxLength, int rows, bool enabled)
        {
            CssClass = cSSClass;
            ID = id;
            Text = text;

            switch (textMode)
            {
                case TextBoxMode.MultiLine:
                    TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;

                    Attributes.Add("maxlength", maxLength.ToString());

                    break;

                case TextBoxMode.Number:
                    Attributes.Add("onkeypress", "return isInteger(event)");

                    break;

                case TextBoxMode.Float:
                    Attributes.Add("onkeypress", "return isFloat(event)");

                    break;

                case TextBoxMode.Date:
                    Attributes.Add("onkeypress", "return isDate(event)");

                    break;

                case TextBoxMode.Password:
                    TextMode = System.Web.UI.WebControls.TextBoxMode.Password;

                    break;
            }
            
            MaxLength = maxLength; Columns = maxLength / rows;
            Rows = rows;
            Enabled = enabled;
        }
    }
}