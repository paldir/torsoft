using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class HtmlInputRadioButton : System.Web.UI.HtmlControls.HtmlInputRadioButton
    {
        public HtmlInputRadioButton(string klasaCss, string id, string nazwa, string wartość, bool zaznaczonaWartość)
        {
            ID = id;
            Name = nazwa;
            Value = wartość;
            Checked = zaznaczonaWartość;

            Attributes.Add("class", klasaCss);
        }
    }
}