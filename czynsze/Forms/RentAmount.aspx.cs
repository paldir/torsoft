using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class RentAmount : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Enums.RentAmount time = GetParamValue<Enums.RentAmount>("mode");
            string range = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("RentAmount"));
            Hello.SiteMapPath = new List<string>() { "Raporty", "Kwota czynszu" };

            switch (time)
            {
                case Enums.RentAmount.Current:
                    Hello.SiteMapPath.Add("Bieżąca");

                    break;

                case Enums.RentAmount.ForMonth:
                    Hello.SiteMapPath.Add("Za dany miesiąc");

                    break;

                default:

                    break;
            }

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                string minBuilding = (db.buildings.Any() ? db.buildings.Min(b => b.kod_1) : 0).ToString();
                string minPlace = (db.places.Any() ? db.places.Min(p => p.nr_lok) : 0).ToString();
                string maxBuilding = (db.buildings.Any() ? db.buildings.Max(b => b.kod_1) : 0).ToString();
                string maxPlace = (db.places.Any() ? db.places.Max(p => p.nr_lok) : 0).ToString();
                string minCommunity = (db.communities.Any() ? db.communities.Min(c => c.kod) : 0).ToString();
                string maxCommunity = (db.communities.Any() ? db.communities.Max(c => c.kod) : 0).ToString();

                if (String.IsNullOrEmpty(range))
                {
                    form.Controls.Add(new MyControls.HtmlInputHidden("mode", time.ToString()));

                    placeOfPlaces.Controls.Add(new MyControls.Button("button", "allPlacesRentAmount", "Zestawienie wszystkich lokali", String.Empty));
                    AddNewLine(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new MyControls.Button("button", "fromToPlaceRentAmount", "Od-do żądanego lokalu", String.Empty));
                    AddNewLine(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new MyControls.Label("label", "fromBuildingOfPlace", "Numer budynku pierwszego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new MyControls.TextBox("field", "fromBuildingOfPlace", minBuilding, MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                    placeOfPlaces.Controls.Add(new MyControls.Label("label", "fromPlace", " Numer pierwszego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new MyControls.TextBox("field", "fromPlace", minPlace, MyControls.TextBox.TextBoxMode.Number, 3, 1, true));
                    AddNewLine(placeOfPlaces);
                    placeOfPlaces.Controls.Add(new MyControls.Label("label", "toBuildingOfPlace", "Numer budynku ostatniego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new MyControls.TextBox("field", "toBuildingOfPlace", maxBuilding, MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                    placeOfPlaces.Controls.Add(new MyControls.Label("label", "toPlace", " Numer ostatniego lokalu: ", String.Empty));
                    placeOfPlaces.Controls.Add(new MyControls.TextBox("field", "toPlace", maxPlace, MyControls.TextBox.TextBoxMode.Number, 3, 1, true));

                    placeOfBuildings.Controls.Add(new MyControls.Button("button", "allBuildingsRentAmount", "Zestawienie wszystkich budynków", String.Empty));
                    AddNewLine(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new MyControls.Button("button", "fromToBuildingRentAmount", "Od-do żądanego budynku", String.Empty));
                    AddNewLine(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new MyControls.Label("label", "fromBuilding", "Numer pierwszego budynku: ", String.Empty));
                    placeOfBuildings.Controls.Add(new MyControls.TextBox("field", "fromBuilding", minBuilding, MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                    AddNewLine(placeOfBuildings);
                    placeOfBuildings.Controls.Add(new MyControls.Label("label", "toBuilding", "Numer ostatniego budynku: ", String.Empty));
                    placeOfBuildings.Controls.Add(new MyControls.TextBox("field", "toBuilding", maxBuilding, MyControls.TextBox.TextBoxMode.Number, 5, 1, true));

                    placeOfCommunities.Controls.Add(new MyControls.Button("button", "allCommunitiesRentAmount", "Zestawienie wszystkich wspólnot", String.Empty));
                    AddNewLine(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new MyControls.Button("button", "fromToCommunityRentAmount", "Od-do żądanej wspólnoty", String.Empty));
                    AddNewLine(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new MyControls.Label("label", "fromCommunity", "Numer pierwszej wspólnoty: ", String.Empty));
                    placeOfCommunities.Controls.Add(new MyControls.TextBox("field", "fromCommunity", minCommunity, MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                    AddNewLine(placeOfCommunities);
                    placeOfCommunities.Controls.Add(new MyControls.Label("label", "toCommunity", "Numer ostatniej wspólnoty: ", String.Empty));
                    placeOfCommunities.Controls.Add(new MyControls.TextBox("field", "toCommunity", maxCommunity, MyControls.TextBox.TextBoxMode.Number, 5, 1, true));
                }
                else
                {
                    range = range.Substring(range.LastIndexOf('$') + 1).Replace("RentAmount", String.Empty);
                    string kod_1_1, kod_1_2, nr1, nr2, kod1, kod2;
                    kod_1_1 = kod_1_2 = nr1 = nr2 = kod1 = kod2 = "0";
                    Enums.Report report = (Enums.Report)(-1);

                    switch (range)
                    {
                        case "allPlaces":
                            report = Enums.Report.CurrentRentAmountOfPlaces;
                            kod_1_1 = minBuilding;
                            nr1 = minPlace;
                            kod_1_2 = maxBuilding;
                            nr2 = maxPlace;

                            break;

                        case "fromToPlace":
                            report = Enums.Report.CurrentRentAmountOfPlaces;
                            kod_1_1 = GetParamValue<string>("fromBuildingOfPlace");
                            nr1 = GetParamValue<string>("fromPlace");
                            kod_1_2 = GetParamValue<string>("toBuildingOfPlace");
                            nr2 = GetParamValue<string>("toPlace");

                            break;

                        case "allBuildings":
                            report = Enums.Report.CurrentRentAmountOfBuildings;
                            kod_1_1 = minBuilding;
                            kod_1_2 = maxBuilding;

                            break;

                        case "fromToBuilding":
                            report = Enums.Report.CurrentRentAmountOfCommunities;
                            kod_1_1 = GetParamValue<string>("fromBuilding");
                            kod_1_2 = GetParamValue<string>("toBuilding");

                            break;
                    }

                    Response.Redirect(String.Format("ReportConfiguration.aspx?{0}report=dummy&fromBuilding={1}&fromPlace={2}&toBuilding={3}&toPlace={4}&fromCommunity={5}&toCommunity={6}", report, kod_1_1, nr1, kod_1_2, nr2, kod1, kod2));
                }
            }
        }
    }
}