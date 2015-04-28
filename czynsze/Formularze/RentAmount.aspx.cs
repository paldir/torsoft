using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class RentAmount : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Enumeratory.KwotaCzynszu time = GetParamValue<Enumeratory.KwotaCzynszu>("mode");
            string range = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("RentAmount"));
            Hello.SiteMapPath = new List<string>() { "Raporty", "Kwota czynszu" };

            switch (time)
            {
                case Enumeratory.KwotaCzynszu.Bieżąca:
                    Hello.SiteMapPath.Add("Bieżąca");

                    break;

                case Enumeratory.KwotaCzynszu.ZaDanyMiesiąc:
                    Hello.SiteMapPath.Add("Za dany miesiąc");

                    break;

                default:

                    break;
            }

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                string minBuilding = (db.Budynki.Any() ? db.Budynki.Min(b => b.kod_1) : 0).ToString();
                string minPlace = (db.AktywneLokale.Any() ? db.AktywneLokale.Min(p => p.nr_lok) : 0).ToString();
                string maxBuilding = (db.Budynki.Any() ? db.Budynki.Max(b => b.kod_1) : 0).ToString();
                string maxPlace = (db.AktywneLokale.Any() ? db.AktywneLokale.Max(p => p.nr_lok) : 0).ToString();
                string minCommunity = (db.Wspólnoty.Any() ? db.Wspólnoty.Min(c => c.kod) : 0).ToString();
                string maxCommunity = (db.Wspólnoty.Any() ? db.Wspólnoty.Max(c => c.kod) : 0).ToString();

                if (String.IsNullOrEmpty(range))
                {
                    form.Controls.Add(new Kontrolki.HtmlInputHidden("mode", time.ToString()));

                    placeOfPlaces.Controls.Add(new Kontrolki.Button("button", "allPlacesRentAmount", "Zestawienie wszystkich lokali", String.Empty));
                    AddNewLine(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Button("button", "fromToPlaceRentAmount", "Od-do żądanego lokalu", String.Empty));
                    AddNewLine(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "fromBuildingOfPlace", "Numer budynku pierwszego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "fromBuildingOfPlace", minBuilding, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "fromPlace", " Numer pierwszego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "fromPlace", minPlace, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));
                    AddNewLine(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "toBuildingOfPlace", "Numer budynku ostatniego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "toBuildingOfPlace", maxBuilding, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    placeOfPlaces.Controls.Add(new Kontrolki.Label("label", "toPlace", " Numer ostatniego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new Kontrolki.TextBox("field", "toPlace", maxPlace, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 3, 1, true));

                    placeOfBuildings.Controls.Add(new Kontrolki.Button("button", "allBuildingsRentAmount", "Zestawienie wszystkich budynków", String.Empty));
                    AddNewLine(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Button("button", "fromToBuildingRentAmount", "Od-do żądanego budynku", String.Empty));
                    AddNewLine(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Label("label", "fromBuilding", "Numer pierwszego budynku: ", String.Empty));
                    placeOfBuildings.Controls.Add(new Kontrolki.TextBox("field", "fromBuilding", minBuilding, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    AddNewLine(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new Kontrolki.Label("label", "toBuilding", "Numer ostatniego budynku: ", String.Empty));
                    placeOfBuildings.Controls.Add(new Kontrolki.TextBox("field", "toBuilding", maxBuilding, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));

                    placeOfCommunities.Controls.Add(new Kontrolki.Button("button", "allCommunitiesRentAmount", "Zestawienie wszystkich wspólnot", String.Empty));
                    AddNewLine(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Button("button", "fromToCommunityRentAmount", "Od-do żądanej wspólnoty", String.Empty));
                    AddNewLine(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Label("label", "fromCommunity", "Numer pierwszej wspólnoty: ", String.Empty));
                    placeOfCommunities.Controls.Add(new Kontrolki.TextBox("field", "fromCommunity", minCommunity, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                    AddNewLine(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new Kontrolki.Label("label", "toCommunity", "Numer ostatniej wspólnoty: ", String.Empty));
                    placeOfCommunities.Controls.Add(new Kontrolki.TextBox("field", "toCommunity", maxCommunity, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true));
                }
                else
                {
                    range = range.Substring(range.LastIndexOf('$') + 1).Replace("RentAmount", String.Empty);
                    string kod_1_1, kod_1_2, nr1, nr2, kod1, kod2;
                    kod_1_1 = kod_1_2 = nr1 = nr2 = kod1 = kod2 = "0";
                    Enumeratory.Raport report = (Enumeratory.Raport)(-1);

                    switch (range)
                    {
                        case "allPlaces":
                            report = Enumeratory.Raport.BieżącaKwotaCzynszuLokali;
                            kod_1_1 = minBuilding;
                            nr1 = minPlace;
                            kod_1_2 = maxBuilding;
                            nr2 = maxPlace;

                            break;

                        case "fromToPlace":
                            report = Enumeratory.Raport.BieżącaKwotaCzynszuLokali;
                            kod_1_1 = GetParamValue<string>("fromBuildingOfPlace");
                            nr1 = GetParamValue<string>("fromPlace");
                            kod_1_2 = GetParamValue<string>("toBuildingOfPlace");
                            nr2 = GetParamValue<string>("toPlace");

                            break;

                        case "allBuildings":
                            report = Enumeratory.Raport.BieżącaKwotaCzynszuBudynków;
                            kod_1_1 = minBuilding;
                            kod_1_2 = maxBuilding;

                            break;

                        case "fromToBuilding":
                            report = Enumeratory.Raport.BieżącaKwotaCzynszuBudynków;
                            kod_1_1 = GetParamValue<string>("fromBuilding");
                            kod_1_2 = GetParamValue<string>("toBuilding");

                            break;

                        case "allCommunities":
                            report = Enumeratory.Raport.BieżącaKwotaCzynszuWspólnot;
                            kod1 = minCommunity;
                            kod2 = maxCommunity;

                            break;

                        case "fromToCommunity":
                            report = Enumeratory.Raport.BieżącaKwotaCzynszuWspólnot;
                            kod1 = GetParamValue<string>("fromCommunity");
                            kod2 = GetParamValue<string>("toCommunity");

                            break;
                    }

                    Response.Redirect(String.Format("ReportConfiguration.aspx?{0}report=dummy&fromBuilding={1}&fromPlace={2}&toBuilding={3}&toPlace={4}&fromCommunity={5}&toCommunity={6}", report, kod_1_1, nr1, kod_1_2, nr2, kod1, kod2));
                }
            }
        }
    }
}