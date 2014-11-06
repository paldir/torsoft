using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class ChangeSettlementTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Forms.Hello.siteMapPath.Clear();
            
            List<string> texts = new List<string>() { "CZYNSZE" };
            List<string> values = new List<string>() { EnumP.SettlementTable.Czynsze.ToString() };

            if (Hello.numberOfSets >= 1)
            {
                texts.Add(Hello.namesOfSets[1]);
                values.Add(EnumP.SettlementTable.SecondSet.ToString());
            }

            if (Hello.numberOfSets == 3)
            {
                texts.Add(Hello.namesOfSets[2]);
                values.Add(EnumP.SettlementTable.ThirdSet.ToString());
            }

            Button button = new Button();
            button.Text = "Zmień";
            button.Click += button_Click;

            placeOfRadioButtons.Controls.Add(new ControlsP.RadioButtonListP("list", "numberOfSets", texts, values, Hello.currentSet.ToString(), true, false));
            placeOfButton.Controls.Add(button);
        }

        void button_Click(object sender, EventArgs e)
        {
            Hello.currentSet = (EnumP.SettlementTable)Enum.Parse(typeof(EnumP.SettlementTable), ((RadioButtonList)placeOfRadioButtons.FindControl("numberOfSets")).SelectedValue);

            Response.Redirect("Hello.aspx");
        }
    }
}