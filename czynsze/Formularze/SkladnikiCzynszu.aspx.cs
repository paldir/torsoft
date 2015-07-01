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
            {
                int minimalnyBudynek;
                int minimalnyLokal;
                int maksymalnyBudynek;
                int maksymalnyLokal;

                pojemnikRadio.Controls.Add(new Kontrolki.Label("label", "stawka", "Wybór stawki: ", String.Empty));
                pojemnikRadio.Controls.Add(new Kontrolki.DropDownList("field", "stawka", new List<string[]> 
                    {
                        new string[] 
                        { 
                            Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla.ToString(), 
                            "Stawka" 
                        }, 
                        new string[] 
                        { 
                            Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna.ToString(), 
                            "Stawka informacyjna" 
                        } 
                    }, Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla.ToString(), true, false));


                DodajNowąLinię(pojemnikReszty);
                DodajWybórLokali(pojemnikReszty, out minimalnyBudynek, out minimalnyLokal, out maksymalnyBudynek, out maksymalnyLokal);
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