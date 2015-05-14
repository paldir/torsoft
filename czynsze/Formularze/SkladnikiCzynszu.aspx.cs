using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;

namespace czynsze.Formularze
{
    public partial class SkladnikiCzynszu : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string zakres = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("przycisk"));
            Start.ŚcieżkaStrony = new List<string>() { "Raporty", "Składniki czynszu" };

            if (String.IsNullOrEmpty(zakres))
                using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                {
                    IEnumerable<DostępDoBazy.AktywnyLokal> lokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok);
                    DostępDoBazy.AktywnyLokal pierwszyLokal = lokale.First();
                    DostępDoBazy.AktywnyLokal ostatniLokal = lokale.Last();
                    string minimalnyBudynek = pierwszyLokal.kod_lok.ToString();
                    string minimalnyLokal = pierwszyLokal.nr_lok.ToString();
                    string maksymalnyBudynek = ostatniLokal.kod_lok.ToString();
                    string maksymalnyLokal = ostatniLokal.nr_lok.ToString();

                    pojemnikRadio.Controls.Add(new Kontrolki.Label("label", "stawka", "Wybór stawki: ", String.Empty));

                    pojemnikRadio.Controls.Add(new Kontrolki.DropDownList("field", "stawka", new List<string[]> 
                    {
                        new string[] 
                        { 
                            Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła.ToString(), 
                            "Stawka" 
                        }, 
                        new string[] 
                        { 
                            Enumeratory.Raport.SkładnikiCzynszuStawkaInformacyjna.ToString(), 
                            "Stawka informacyjna" 
                        } 
                    }, Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła.ToString(), true, false));

                    List<string[]> lokaleDoListy = new List<string[]>();

                    foreach (DostępDoBazy.AktywnyLokal lokal in lokale)
                    {
                        string id = String.Format("{0}-{1}", lokal.kod_lok, lokal.nr_lok);

                        lokaleDoListy.Add(new string[] { id, id, lokal.adres, lokal.adres_2 });
                    }

                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "odLokalu", "Pierwszy lokal: ", String.Empty));
                    pojemnikReszty.Controls.Add(new Kontrolki.DropDownList("field", "odLokalu", lokaleDoListy, String.Format("{0}-{1}", minimalnyBudynek, minimalnyLokal), true, false));
                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "doLokalu", "Ostatni lokal: ", String.Empty));
                    pojemnikReszty.Controls.Add(new Kontrolki.DropDownList("field", "doLokalu", lokaleDoListy, String.Format("{0}-{1}", maksymalnyBudynek, maksymalnyLokal), true, false));
                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Button("button", "przycisk", "Wybierz", String.Empty));
                }
            else
            {
                Enumeratory.Raport raport = PobierzWartośćParametru<Enumeratory.Raport>("stawka");
                string[] odLokalu = PobierzWartośćParametru<string>("odLokalu").Split('-');
                string[] doLokalu = PobierzWartośćParametru<string>("doLokalu").Split('-');

                Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?{0}raport=dummy&odBudynku={1}&odLokalu={2}&doBudynku={3}&doLokalu={4}", raport, odLokalu[0], odLokalu[1], doLokalu[0], doLokalu[1]));
            }
        }
    }
}