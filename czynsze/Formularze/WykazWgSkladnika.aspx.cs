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
            string przycisk = PobierzWartośćParametru<string>("przycisk");

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                if (String.IsNullOrEmpty(przycisk))
                {
                    int minimalnyBudynek;
                    int minimalnyLokal;
                    int maksymalnyBudynek;
                    int maksymalnyLokal;

                    List<DostępDoBazy.SkładnikCzynszu> składnikiCzynszu = db.SkładnikiCzynszu.OrderBy(s => s.nr_skl).ToList();

                    pojemnikSkladnika.Controls.Add(new Kontrolki.Label("label", "składnik", "Składnik: ", String.Empty));
                    pojemnikSkladnika.Controls.Add(new Kontrolki.DropDownList("field", "składnik", składnikiCzynszu.Select(s => s.WażnePolaDoRozwijanejListy()).ToList(), składnikiCzynszu.First().nr_skl.ToString(), true, false));
                    DodajNowąLinię(pojemnikReszty);
                    DodajWybórLokali(pojemnikReszty, out minimalnyBudynek, out minimalnyLokal, out maksymalnyBudynek, out maksymalnyLokal);
                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Button("button", "przycisk", "Wybierz", String.Empty));
                }
                else
                {
                    string[] pierwszyLokal = PobierzWartośćParametru<string>("odLokalu").Split('-');
                    string[] ostatniLokal = PobierzWartośćParametru<string>("doLokalu").Split('-');
                    int nrSkładnika = PobierzWartośćParametru<int>("składnik");

                    Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?{0}raport=dummy&odBudynku={1}&odLokalu={2}&doBudynku={3}&doLokalu={4}&nrSkladnika={5}", Enumeratory.Raport.WykazWgSkladnika, pierwszyLokal[0], pierwszyLokal[1], ostatniLokal[0], ostatniLokal[1], nrSkładnika));
                }
            }
        }
    }
}