using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class PostepGeneracjiNaleznosci : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            progress.InnerText = String.Format("{0} %", GeneracjaNaleznosci.PostępPrzetwarzaniaNależności);

            if (GeneracjaNaleznosci.PostępPrzetwarzaniaNależności == 100 || !String.IsNullOrEmpty(GeneracjaNaleznosci.BłądPrzetwarzaniaNależności))
            {
                timer.Enabled = false;
                string message;

                if (GeneracjaNaleznosci.PostępPrzetwarzaniaNależności == 100)
                    message = "Generacja należności zakończona pomyślnie.";
                else
                    message = String.Format("{0}<br />Prosimy o kontakt z firmą. ", GeneracjaNaleznosci.BłądPrzetwarzaniaNależności);

                info.Controls.Clear();
                info.Controls.Add(new LiteralControl(String.Format("{0}<br />", message)));
                info.Controls.Add(new Kontrolki.Button("button", "back", "Powrót do aplikacji", "Start.aspx"));
                updatePanel.Update();
            }
        }
    }
}