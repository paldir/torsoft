using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class TextBox : System.Web.UI.WebControls.TextBox, IKontrolkaZWartością
    {
        public enum TextBoxMode { PojedynczaLinia, KilkaLinii, Data, LiczbaCałkowita, LiczbaNiecałkowita, Hasło };

        public string Wartość
        {
            get { return Text; }
            set { Text = value; }
        }

        public TextBox() { }

        public TextBox(string klasaCss, string id, TextBoxMode tryb, int długośćMaksymalna, int liczbaWierszy, bool włączony)
        {
            CssClass = klasaCss;
            ID = id;

            switch (tryb)
            {
                case TextBoxMode.KilkaLinii:
                    TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;

                    Attributes.Add("maxlength", długośćMaksymalna.ToString());

                    break;

                case TextBoxMode.LiczbaCałkowita:
                    Attributes.Add("onkeypress", "return isInteger(event)");

                    break;

                case TextBoxMode.LiczbaNiecałkowita:
                    Attributes.Add("onkeypress", "return isFloat(event)");

                    break;

                case TextBoxMode.Data:
                    Attributes.Add("onkeypress", "return isDate(event)");

                    break;

                case TextBoxMode.Hasło:
                    TextMode = System.Web.UI.WebControls.TextBoxMode.Password;

                    break;
            }

            MaxLength = długośćMaksymalna; Columns = długośćMaksymalna / liczbaWierszy;
            Rows = liczbaWierszy;
            Enabled = włączony;
        }

        public TextBox(string klasaCss, string id, TextBoxMode tryb, int długośćMaksymalna, int liczbaWierszy, bool włączony, string tekst)
            : this(klasaCss, id, tryb, długośćMaksymalna, liczbaWierszy, włączony)
        {
            Text = tekst;
        }
    }
}