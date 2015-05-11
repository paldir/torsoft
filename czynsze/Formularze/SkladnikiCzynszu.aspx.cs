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
                    string minimalnyBudynek = (db.Budynki.Any() ? db.Budynki.Min(b => b.kod_1) : 0).ToString();
                    string minimalnyLokal = (db.AktywneLokale.Any() ? db.AktywneLokale.Min(p => p.nr_lok) : 0).ToString();
                    string maksymalnyBudynek = (db.Budynki.Any() ? db.Budynki.Max(b => b.kod_1) : 0).ToString();
                    string maksymalnyLokal = (db.AktywneLokale.Any() ? db.AktywneLokale.Max(p => p.nr_lok) : 0).ToString();

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

                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "odLokaluBudynku", "Numer budynku pierwszego lokalu: ", String.Empty));
                    pojemnikReszty.Controls.Add(new Kontrolki.TextBox("field", "odLokaluBudynku", minimalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "odLokalu", " Numer pierwszego lokalu: ", String.Empty));
                    pojemnikReszty.Controls.Add(new Kontrolki.TextBox("field", "odLokalu", minimalnyLokal, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));
                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "doLokaluBudynku", "Numer budynku ostatniego lokalu: ", String.Empty));
                    pojemnikReszty.Controls.Add(new Kontrolki.TextBox("field", "doLokaluBudynku", maksymalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "doLokalu", " Numer ostatniego lokalu: ", String.Empty));
                    pojemnikReszty.Controls.Add(new Kontrolki.TextBox("field", "doLokalu", maksymalnyLokal, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));
                    DodajNowąLinię(pojemnikReszty);
                    pojemnikReszty.Controls.Add(new Kontrolki.Button("button", "przycisk", "Wybierz", String.Empty));
                }
            else
            {
                Enumeratory.Raport raport = PobierzWartośćParametru<Enumeratory.Raport>("stawka");
                string kod_1_1 = PobierzWartośćParametru<string>("odLokaluBudynku");
                string nr1 = PobierzWartośćParametru<string>("odLokalu");
                string kod_1_2 = PobierzWartośćParametru<string>("doLokaluBudynku");
                string nr2 = PobierzWartośćParametru<string>("doLokalu");

                Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?{0}raport=dummy&odBudynku={1}&odLokalu={2}&doBudynku={3}&doLokalu={4}", raport, kod_1_1, nr1, kod_1_2, nr2));
            }
        }
    }
}