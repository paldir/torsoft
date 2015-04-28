using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class ChangeSettlementTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Hello.SiteMapPath.Clear();

            List<string> texts = new List<string>() { "CZYNSZE" };
            List<string> values = new List<string>() { Enumeratory.Zbiór.Czynsze.ToString() };

            if (Hello.NumberOfSets >= 1)
            {
                texts.Add(Hello.NamesOfSets[1]);
                values.Add(Enumeratory.Zbiór.Drugi.ToString());
            }

            if (Hello.NumberOfSets == 3)
            {
                texts.Add(Hello.NamesOfSets[2]);
                values.Add(Enumeratory.Zbiór.Trzeci.ToString());
            }

            Button button = new Button();
            button.Text = "Zmień";
            button.Click += button_Click;

            placeOfRadioButtons.Controls.Add(new Kontrolki.RadioButtonList("list", "numberOfSets", texts, values, Hello.CurrentSet.ToString(), true, false));
            placeOfButton.Controls.Add(button);
        }

        void button_Click(object sender, EventArgs e)
        {
            Hello.CurrentSet = (Enumeratory.Zbiór)Enum.Parse(typeof(Enumeratory.Zbiór), ((RadioButtonList)placeOfRadioButtons.FindControl("numberOfSets")).SelectedValue);

            Response.Redirect("Hello.aspx");
        }
    }
}