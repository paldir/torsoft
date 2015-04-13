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
            string range = GetParamValue<string>("RentAmount");
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
                }
                else
                {
                    Response.Redirect(String.Format("ReportConfiguration.aspx?{0}report=dummy&fromBuilding={1}&fromPlace={2}&toBuilding={3}&toPlace={4}", Enums.Report.CurrentRentAmountOfPlaces, minBuilding, minPlace, maxBuilding, maxPlace));
                }
            }
        }
    }
}