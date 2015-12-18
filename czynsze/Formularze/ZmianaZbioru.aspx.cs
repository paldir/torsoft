using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class ZmianaZbioru : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PobierzWartośćParametru<string>("zmiana")))
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

                Kontrolki.Button button = new Kontrolki.Button("field", "zmiana", "Zmień", String.Empty);

                placeOfRadioButtons.Controls.Add(new Kontrolki.RadioButtonList("list", "numberOfSets", texts, values, true, false, Start.AktywnyZbiór.ToString()));
                placeOfButton.Controls.Add(button);
            }
            else
            {
                Start.AktywnyZbiór = PobierzWartośćParametru<Enumeratory.Zbiór>("numberOfSets");

                Response.Redirect("Start.aspx");
            }
        }
    }
}