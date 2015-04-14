using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class RentComponentsOfPlace : Page
    {
        List<DataAccess.RentComponentOfPlace> rentComponentsOfPlace
        {
            get { return (List<DataAccess.RentComponentOfPlace>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int kod_lok = GetParamValue<int>("kod_lok");
            int nr_lok = GetParamValue<int>("nr_lok");
            List<string[]> rows = new List<string[]>();
            string window = GetParamValue<string>("ShowWindow");
            Enums.Action parentAction = GetParamValue<Enums.Action>("parentAction");
            Enums.Action childAction = GetParamValue<Enums.Action>("ChildAction");
            int id = GetParamValue<int>("id");
            string postBackUrl = "RentComponentsOfPlace.aspx";
            List<int> allComponentsWithAmount;
            List<int> componentsWithAmount = new List<int>();
            DataAccess.RentComponentOfPlace currentRentComponent = null;

            if (id != 0)
                currentRentComponent = rentComponentsOfPlace.ElementAt(id - 1);

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                allComponentsWithAmount = db.rentComponents.Where(c => c.s_zaplat == 2).Select(c => c.nr_skl).ToList();

            if ((int)childAction != 0)
            {
                string[] record = new string[]
                {
                    kod_lok.ToString(),
                    nr_lok.ToString(),
                    GetParamValue<string>("nr_skl"),
                    GetParamValue<string>("dan_p"),
                    GetParamValue<string>("dat_od"),
                    GetParamValue<string>("dat_do")
                };
                
                switch (childAction)
                {
                    case Enums.Action.Dodaj:
                        if (DataAccess.RentComponentOfPlace.Validate(record, childAction))
                        {
                            DataAccess.RentComponentOfPlace rentComponentOfPlace = new DataAccess.RentComponentOfPlace();

                            rentComponentOfPlace.Set(record);
                            rentComponentsOfPlace.Add(rentComponentOfPlace);
                        }

                        break;

                    case Enums.Action.Edytuj:
                        if (DataAccess.RentComponentOfPlace.Validate(record, childAction))
                            currentRentComponent.Set(record);

                        break;

                    case Enums.Action.Usuń:
                        rentComponentsOfPlace.Remove(currentRentComponent);

                        break;
                }
            }

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                DataAccess.RentComponentOfPlace.Places = db.places.ToList();
                DataAccess.RentComponentOfPlace.RentComponents = db.rentComponents.ToList();
            }

            for (int i = 0; i < rentComponentsOfPlace.Count; i++)
            {
                string index = (i + 1).ToString();

                rows.Add(new string[] { index, index }.Concat(rentComponentsOfPlace.ElementAt(i).ImportantFields()).ToArray());
            }

            DataAccess.RentComponentOfPlace.Places = null;
            DataAccess.RentComponentOfPlace.RentComponents = null;

            ViewState["componentsWithAmount"] = allComponentsWithAmount;
            ViewState["id"] = id;

            form.Controls.Add(new MyControls.HtmlInputHidden("parentAction", parentAction.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("kod_lok", kod_lok.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("nr_lok", nr_lok.ToString()));

            if (window == null)
                switch (parentAction)
                {
                    case Enums.Action.Dodaj:
                    case Enums.Action.Edytuj:
                        placeOfButtons.Controls.Add(new MyControls.Button("button", "addShowWindow", "Dodaj", postBackUrl));
                        placeOfButtons.Controls.Add(new MyControls.Button("button", "removeChildAction", "Usuń", postBackUrl));
                        placeOfButtons.Controls.Add(new MyControls.Button("button", "editShowWindow", "Edytuj", postBackUrl));

                        break;
                }
            else
            {
                string firstLabel;
                string amount;
                Control firstControl;
                string textOfSaveButton;
                string dat_od = String.Empty;
                string dat_do = String.Empty;

                if (window == "Dodaj")
                {
                    firstLabel = "Nowy składnik: ";
                    amount = String.Empty;
                    textOfSaveButton = "Dodaj";

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        firstControl = new MyControls.DropDownList("field", "nr_skl", db.rentComponents.OrderBy(c => c.nr_skl).ToList().Select(c => c.ImportantFieldsForDropdown()).ToList(), String.Empty, true, false);
                }
                else
                {
                    firstLabel = "Nazwa składnika: ";
                    amount = currentRentComponent.dan_p.ToString("F2");
                    textOfSaveButton = "Edytuj";

                    if (currentRentComponent.dat_od != null)
                        dat_od = String.Format("{0:yyyy-MM-dd}", currentRentComponent.dat_od);

                    if (currentRentComponent.dat_do != null)
                        dat_do = String.Format("{0:yyyy-MM-dd}", currentRentComponent.dat_do);

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        firstControl = new MyControls.TextBox("field", "nazwa", db.rentComponents.FirstOrDefault(c => c.nr_skl == currentRentComponent.nr_skl).nazwa, MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, false);

                    form.Controls.Add(new MyControls.HtmlInputHidden("nr_skl", currentRentComponent.nr_skl.ToString()));
                    form.Controls.Add(new MyControls.HtmlInputHidden("id", id.ToString()));
                }

                placeOfNewComponent.Controls.Add(new MyControls.Label("label", "nr_skl", firstLabel, String.Empty));

                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    placeOfNewComponent.Controls.Add(firstControl);

                placeOfAmount.Controls.Add(new MyControls.Label("label", "dan_p", "Ilość: ", String.Empty));
                placeOfAmount.Controls.Add(new MyControls.TextBox("field", "dan_p", amount, MyControls.TextBox.TextBoxMode.Number, 15, 1, true));
                placeOfDate.Controls.Add(new MyControls.Label("label", "dat_od", "Zakres dat: ", String.Empty));
                placeOfDate.Controls.Add(new MyControls.TextBox("field", "dat_od", dat_od, MyControls.TextBox.TextBoxMode.Date, 10, 1, true));
                placeOfDate.Controls.Add(new MyControls.Label("label", "dat_do", " - ", String.Empty));
                placeOfDate.Controls.Add(new MyControls.TextBox("field", "dat_do", dat_do, MyControls.TextBox.TextBoxMode.Date, 10, 1, true));
                placeOfButtonsOfWindow.Controls.Add(new MyControls.Button("button", "saveChildAction", textOfSaveButton, postBackUrl));
                placeOfButtonsOfWindow.Controls.Add(new MyControls.Button("button", String.Empty, "Anuluj", postBackUrl));
            }

            placeOfTable.Controls.Add(new MyControls.Table("mainTable tabTable", rows, new string[] { "Lp.", "Nr", "Nazwa", "Stawka", "Ilość", "Wartość", "Początek zakresu dat", "Koniec zakresu dat" }, false, String.Empty, new List<int>() { 1, 2, 4, 5, 6 }, new List<int>() { 6 }));
        }
    }
}