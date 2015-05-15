using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class WykazWgSkladnika : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Start.ŚcieżkaStrony = new List<string>() { "Raporty", "Wykaz wg składnika" };

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                int minimalnyBudynek;
                int minimalnyLokal;
                int maksymalnyBudynek;
                int maksymalnyLokal;

                List<DostępDoBazy.SkładnikCzynszu> składnikiCzynszu = db.SkładnikiCzynszu.OrderBy(s => s.nr_skl).ToList();

                pojemnikSkladnika.Controls.Add(new Kontrolki.Label("label", "skladnik", "Składnik: ", String.Empty));
                pojemnikSkladnika.Controls.Add(new Kontrolki.DropDownList("field", "skladnik", składnikiCzynszu.Select(s => s.WażnePolaDoRozwijanejListy()).ToList(), składnikiCzynszu.First().nr_skl.ToString(), true, false));
                DodajNowąLinię(pojemnikReszty);
                DodajWybórLokali(pojemnikReszty, out minimalnyBudynek, out minimalnyLokal, out maksymalnyBudynek, out maksymalnyLokal);
                DodajNowąLinię(pojemnikReszty);
                pojemnikReszty.Controls.Add(new Kontrolki.Button("button", "przycisk", "Wybierz", String.Empty));
            }
        }
    }
}