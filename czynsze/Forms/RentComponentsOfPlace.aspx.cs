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
            //int kod_lok = GetParamValue<int>("kod_lok");
            //int nr_lok = GetParamValue<int>("nr_lok");
            List<string[]> rows = new List<string[]>();
            string window = GetParamValue<string>("ShowWindow");
            Enums.Action parentAction = GetParamValue<Enums.Action>("parentAction");
            Enums.Action childAction = GetParamValue<Enums.Action>("ChildAction");
            int id = GetParamValue<int>("id");
            string postBackUrl = "RentComponentsOfPlace.aspx";
            List<int> allComponentsWithAmount;
            List<int> componentsWithAmount = new List<int>();

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                allComponentsWithAmount = db.rentComponents.Where(c => c.s_zaplat == 2).Select(c => c.nr_skl).ToList();

            switch (childAction)
            {
                case Enums.Action.Dodaj:

                    break;

                case Enums.Action.Edytuj:

                    break;

                case Enums.Action.Usuń:
                    rentComponentsOfPlace.RemoveAt(id - 1);

                    break;
            }

            for (int i = 0; i < rentComponentsOfPlace.Count; i++)
            {
                string index = (i + 1).ToString();
                DataAccess.RentComponentOfPlace rentComponentOfPlace = rentComponentsOfPlace.ElementAt(i);

                rows.Add(new string[] { index, index }.Concat(rentComponentOfPlace.ImportantFields()).ToArray());
            }

            ViewState["componentsWithAmount"] = allComponentsWithAmount; 

            form.Controls.Add(new MyControls.HtmlInputHidden("parentAction", parentAction.ToString()));

            if (window == "-1")
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
                if (window == "Dodaj")
                {
                    placeOfNewComponent.Controls.Add(new MyControls.Label("label", "nr_skl", "Nowy składnik: ", String.Empty));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        placeOfNewComponent.Controls.Add(new MyControls.DropDownList("field", "nr_skl", db.rentComponents.OrderBy(c => c.nr_skl).ToList().Select(c => c.ImportantFieldsForDropdown()).ToList(), String.Empty, true, false));

                    placeOfAmount.Controls.Add(new MyControls.Label("label", "dan_p", "Ilość: ", String.Empty));
                    placeOfAmount.Controls.Add(new MyControls.TextBox("field", "dan_p", String.Empty, MyControls.TextBox.TextBoxMode.Number, 15, 1, true));
                }

            placeOfTable.Controls.Add(new MyControls.Table("mainTable tabTable", rows, new string[] { "Lp.", "Nr", "Nazwa", "Stawka", "Ilość", "Wartość" }, false, String.Empty, new List<int>() { 1, 2, 4, 5, 6 }, new List<int>() { 6 }));
        }
    }
}