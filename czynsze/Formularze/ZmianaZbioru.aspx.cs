using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class ZmianaZbioru : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Start.ŚcieżkaStrony.Wyczyść();

            List<string> texts = new List<string>() { "CZYNSZE" };
            List<string> values = new List<string>() { Enumeratory.Zbiór.Czynsze.ToString() };

            if (Start.LiczbaZbiorów >= 1)
            {
                texts.Add(Start.NazwyZbiorów[1]);
                values.Add(Enumeratory.Zbiór.Drugi.ToString());
            }

            if (Start.LiczbaZbiorów == 3)
            {
                texts.Add(Start.NazwyZbiorów[2]);
                values.Add(Enumeratory.Zbiór.Trzeci.ToString());
            }

            Button button = new Button();
            button.Text = "Zmień";
            button.Click += button_Click;

            placeOfRadioButtons.Controls.Add(new Kontrolki.RadioButtonList("list", "numberOfSets", texts, values, true, false, Start.AktywnyZbiór.ToString()));
            placeOfButton.Controls.Add(button);
        }

        void button_Click(object sender, EventArgs e)
        {
            Start.AktywnyZbiór = (Enumeratory.Zbiór)Enum.Parse(typeof(Enumeratory.Zbiór), ((RadioButtonList)placeOfRadioButtons.FindControl("numberOfSets")).SelectedValue);

            Response.Redirect("Start.aspx");
        }
    }
}