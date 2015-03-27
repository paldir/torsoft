using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class ProgressOfGenerationOfReceivables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void timer_Tick(object sender, EventArgs e)
        {
            progress.InnerText = String.Format("{0} %", GenerationOfReceivables.ProgressOfProcessingOfReceivables);

            if (GenerationOfReceivables.ProgressOfProcessingOfReceivables == 100 || !String.IsNullOrEmpty(GenerationOfReceivables.ErrorOfProcessingOfReceivables))
            {
                timer.Enabled = false;
                string message;

                if (GenerationOfReceivables.ProgressOfProcessingOfReceivables == 100)
                    message = "Generacja należności zakończona pomyślnie.";
                else
                    message = String.Format("{0}<br />Prosimy o kontakt z firmą. ", GenerationOfReceivables.ErrorOfProcessingOfReceivables);

                info.Controls.Clear();
                info.Controls.Add(new LiteralControl(String.Format("{0}<br />", message)));
                info.Controls.Add(new MyControls.Button("button", "back", "Powrót do aplikacji", "Hello.aspx"));
                updatePanel.Update();
            }
        }
    }
}