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
            Start.ŚcieżkaStrony = new List<string>() { "Raporty", "Kwota czynszu" };

            switch (tryb)
            {
                case Enumeratory.KwotaCzynszu.Biezaca:
                    Start.ŚcieżkaStrony.Add("Bieżąca");

                    break;

                case Enumeratory.KwotaCzynszu.ZaDanyMiesiac:
                    Start.ŚcieżkaStrony.Add("Za dany miesiąc");

                    break;

                default:

                    break;
            }

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                IEnumerable<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok);
                string minimalnyBudynek = (db.Budynki.Any() ? db.Budynki.Min(b => b.kod_1) : 0).ToString();
                string minimalnyLokal = wszystkieLokale.First().nr_lok.ToString();
                string maksymalnyBudynek = (db.Budynki.Any() ? db.Budynki.Max(b => b.kod_1) : 0).ToString();
                string maksymalnyLokal = wszystkieLokale.Last().nr_lok.ToString();
                string minimalnaWspólnota = (db.Wspólnoty.Any() ? db.Wspólnoty.Min(c => c.kod) : 0).ToString();
                string maksymalnaWspólnota = (db.Wspólnoty.Any() ? db.Wspólnoty.Max(c => c.kod) : 0).ToString();

                if (String.IsNullOrEmpty(zakres))
                {
                    List<string[]> budynki = db.Budynki.OrderBy(b => b.kod_1).ToList().Select(b => new string[] { b.kod_1.ToString(), b.kod_1.ToString(), b.adres, b.adres_2 }).ToList();
                    List<string[]> wspólnoty = db.Wspólnoty.OrderBy(w => w.kod).ToList().Select(w => new string[] { w.kod.ToString(), w.kod.ToString(), w.nazwa_skr, w.adres, w.adres_2 }).ToList();
                    List<string[]> lokale = new List<string[]>();

                    foreach (DostępDoBazy.AktywnyLokal lokal in wszystkieLokale)
                    {
                        string id = String.Format("{0}-{1}", lokal.kod_lok, lokal.nr_lok);

                        lokale.Add(new string[] { id, id, lokal.adres, lokal.adres_2 });
                    }

                    form.Controls.Add(new Kontrolki.HtmlInputHidden("tryb", tryb.ToString()));

                    placeOfPlaces.Controls.Add(new Kontrolki.Button("button", "wszystkieLokaleKwotaCzynszu", "Zestawienie wszystkich lokali", String.Empty));
                    DodajNowąLinię(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Button("button", "odDoLokaluKwotaCzynszu", "Od-do żądanego lokalu", String.Empty));
                    DodajNowąLinię(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "odLokalu", "Pierwszy lokal: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.DropDownList("field", "odLokalu", lokale, String.Format("{0}-{1}", minimalnyBudynek, minimalnyLokal), true, false));
                    DodajNowąLinię(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "doLokalu", "Ostatni lokal: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.DropDownList("field", "doLokalu", lokale, String.Format("{0}-{1}", maksymalnyBudynek, maksymalnyLokal), true, false));

                    placeOfBuildings.Controls.Add(new Kontrolki.Button("button", "wszystkieBudynkiKwotaCzynszu", "Zestawienie wszystkich budynków", String.Empty));
                    DodajNowąLinię(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Button("button", "odDoBudynkuKwotaCzynszu", "Od-do żądanego budynku", String.Empty));
                    DodajNowąLinię(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Label("label", "odBudynku", "Pierwszy budynek: ", String.Empty));
                    //placeOfBuildings.Controls.Add(new Kontrolki.TextBox("field", "odBudynku", minimalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfBuildings.Controls.Add(new Kontrolki.DropDownList("field", "odBudynku", budynki, minimalnyBudynek, true, false));
                    DodajNowąLinię(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Label("label", "doBudynku", "Ostatni budynek: ", String.Empty));
                    //placeOfBuildings.Controls.Add(new Kontrolki.TextBox("field", "doBudynku", maksymalnyBudynek, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfBuildings.Controls.Add(new Kontrolki.DropDownList("field", "doBudynku", budynki, maksymalnyBudynek, true, false));

                    placeOfCommunities.Controls.Add(new Kontrolki.Button("button", "wszystkieWspólnotyKwotaCzynszu", "Zestawienie wszystkich wspólnot", String.Empty));
                    DodajNowąLinię(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Button("button", "odDoWspólnotyKwotaCzynszu", "Od-do żądanej wspólnoty", String.Empty));
                    DodajNowąLinię(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Label("label", "odWspólnoty", "Pierwsza wspólnota: ", String.Empty));
                    //placeOfCommunities.Controls.Add(new Kontrolki.TextBox("field", "odWpólnoty", minimalnaWspólnota, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfCommunities.Controls.Add(new Kontrolki.DropDownList("field", "odWspólnoty", wspólnoty, minimalnaWspólnota, true, false));
                    DodajNowąLinię(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Label("label", "doWspólnoty", "Ostatnia wspólnota: ", String.Empty));
                    //placeOfCommunities.Controls.Add(new Kontrolki.TextBox("field", "doWspólnoty", maksymalnaWspólnota, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfCommunities.Controls.Add(new Kontrolki.DropDownList("field", "doWspólnoty", wspólnoty, maksymalnaWspólnota, true, false));
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
                            string[] odLokalu = PobierzWartośćParametru<string>("odLokalu").Split('-');
                            string[] doLokalu = PobierzWartośćParametru<string>("doLokalu").Split('-');
                            kod_1_1 = odLokalu[0];
                            nr1 = odLokalu[1];
                            kod_1_2 = doLokalu[0];
                            nr2 = doLokalu[1];

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