using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class KwotaCzynszu : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Enumeratory.KwotaCzynszu tryb = PobierzWartośćParametru<Enumeratory.KwotaCzynszu>("tryb");
            string zakres = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("KwotaCzynszu"));
            Hello.SiteMapPath = new List<string>() { "Raporty", "Kwota czynszu" };

            switch (tryb)
            {
                case Enumeratory.KwotaCzynszu.Biezaca:
                    Hello.SiteMapPath.Add("Bieżąca");

                    break;

                case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                    Hello.SiteMapPath.Add("Za dany miesiąc");

                    break;

                default:

                    break;
            }

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                string minimalnyBudynek = (db.Budynki.Any() ? db.Budynki.Min(b => b.kod_1) : 0).ToString();
                string minimalnyLokal = (db.AktywneLokale.Any() ? db.AktywneLokale.Min(p => p.nr_lok) : 0).ToString();
                string maksymalnyBudynek = (db.Budynki.Any() ? db.Budynki.Max(b => b.kod_1) : 0).ToString();
                string maksymalnyLokal = (db.AktywneLokale.Any() ? db.AktywneLokale.Max(p => p.nr_lok) : 0).ToString();
                string minimalnaWspólnota = (db.Wspólnoty.Any() ? db.Wspólnoty.Min(c => c.kod) : 0).ToString();
                string maksymalnaWspólnota = (db.Wspólnoty.Any() ? db.Wspólnoty.Max(c => c.kod) : 0).ToString();

                if (String.IsNullOrEmpty(zakres))
                {
                    form.Controls.Add(new Kontrolki.HtmlInputHidden("tryb", tryb.ToString()));

                    placeOfPlaces.Controls.Add(new Kontrolki.Button("button", "wszystkieLokaleKwotaCzynszu", "Zestawienie wszystkich lokali", String.Empty));
                    DodajNowąLinię(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Button("button", "odDoLokaluKwotaCzynszu", "Od-do żądanego lokalu", String.Empty));
                    DodajNowąLinię(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "odLokaluBudynku", "Numer budynku pierwszego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "odLokaluBudynku", minimalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "odLokalu", " Numer pierwszego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "odLokalu", minimalnyLokal, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));
                    DodajNowąLinię(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "doLokaluBudynku", "Numer budynku ostatniego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "doLokaluBudynku", maksymalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "doLokalu", " Numer ostatniego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "doLokalu", maksymalnyLokal, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));

                    placeOfBuildings.Controls.Add(new Kontrolki.Button("button", "wszystkieBudynkiKwotaCzynszu", "Zestawienie wszystkich budynków", String.Empty));
                    DodajNowąLinię(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Button("button", "odDoBudynkuKwotaCzynszu", "Od-do żądanego budynku", String.Empty));
                    DodajNowąLinię(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Label("label", "odBudynku", "Numer pierwszego budynku: ", String.Empty));
                    placeOfBuildings.Controls.Add(new Kontrolki.TextBox("field", "odBudynku", minimalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    DodajNowąLinię(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Label("label", "doBudynku", "Numer ostatniego budynku: ", String.Empty));
                    placeOfBuildings.Controls.Add(new Kontrolki.TextBox("field", "doBudynku", maksymalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));

                    placeOfCommunities.Controls.Add(new Kontrolki.Button("button", "wszystkieWspólnotyKwotaCzynszu", "Zestawienie wszystkich wspólnot", String.Empty));
                    DodajNowąLinię(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Button("button", "odDoWspólnotyKwotaCzynszu", "Od-do żądanej wspólnoty", String.Empty));
                    DodajNowąLinię(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Label("label", "odWpólnoty", "Numer pierwszej wspólnoty: ", String.Empty));
                    placeOfCommunities.Controls.Add(new Kontrolki.TextBox("field", "odWpólnoty", minimalnaWspólnota, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    DodajNowąLinię(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Label("label", "doWspólnoty", "Numer ostatniej wspólnoty: ", String.Empty));
                    placeOfCommunities.Controls.Add(new Kontrolki.TextBox("field", "doWspólnoty", maksymalnaWspólnota, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                }
                else
                {
                    zakres = zakres.Substring(zakres.LastIndexOf('$') + 1).Replace("KwotaCzynszu", String.Empty);
                    string kod_1_1, kod_1_2, nr1, nr2, kod1, kod2;
                    kod_1_1 = kod_1_2 = nr1 = nr2 = kod1 = kod2 = "0";
                    Enumeratory.Raport raport = (Enumeratory.Raport)(-1);

                    switch (zakres)
                    {
                        case "wszystkieLokale":
                            raport = Enumeratory.Raport.KwotaCzynszuLokali;
                            kod_1_1 = minimalnyBudynek;
                            nr1 = minimalnyLokal;
                            kod_1_2 = maksymalnyBudynek;
                            nr2 = maksymalnyLokal;

                            break;

                        case "odDoLokalu":
                            raport = Enumeratory.Raport.KwotaCzynszuLokali;
                            kod_1_1 = PobierzWartośćParametru<string>("odLokaluBudynku");
                            nr1 = PobierzWartośćParametru<string>("odLokalu");
                            kod_1_2 = PobierzWartośćParametru<string>("doLokaluBudynku");
                            nr2 = PobierzWartośćParametru<string>("doLokalu");

                            break;

                        case "wszystkieBudynki":
                            raport = Enumeratory.Raport.KwotaCzynszuBudynków;
                            kod_1_1 = minimalnyBudynek;
                            kod_1_2 = maksymalnyBudynek;

                            break;

                        case "odDoBudynku":
                            raport = Enumeratory.Raport.KwotaCzynszuBudynków;
                            kod_1_1 = PobierzWartośćParametru<string>("odBudynku");
                            kod_1_2 = PobierzWartośćParametru<string>("doBudynku");

                            break;

                        case "wszystkieWspólnoty":
                            raport = Enumeratory.Raport.KwotaCzynszuWspólnot;
                            kod1 = minimalnaWspólnota;
                            kod2 = maksymalnaWspólnota;

                            break;

                        case "odDoWspólnoty":
                            raport = Enumeratory.Raport.KwotaCzynszuWspólnot;
                            kod1 = PobierzWartośćParametru<string>("odWpólnoty");
                            kod2 = PobierzWartośćParametru<string>("doWspólnoty");

                            break;
                    }

                    Session["trybKwotyCzynszu"] = tryb;

                    Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?{0}raport=dummy&odBudynku={1}&odLokalu={2}&doBudynku={3}&doLokalu={4}&odWspólnoty={5}&doWspólnoty={6}", raport, kod_1_1, nr1, kod_1_2, nr2, kod1, kod2));
                }
            }
        }
    }
}