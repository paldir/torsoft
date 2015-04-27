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
        List<DostępDoBazy.SkładnikCzynszuLokalu> rentComponentsOfPlace
        {
            get { return (List<DostępDoBazy.SkładnikCzynszuLokalu>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int kod_lok = GetParamValue<int>("kod_lok");
            int nr_lok = GetParamValue<int>("nr_lok");
            List<string[]> rows = new List<string[]>();
            string window = GetParamValue<string>("ShowWindow");
            Enums.Akcja parentAction = GetParamValue<Enums.Akcja>("parentAction");
            Enums.Akcja childAction = GetParamValue<Enums.Akcja>("ChildAction");
            int id = GetParamValue<int>("id");
            string postBackUrl = "RentComponentsOfPlace.aspx";
            List<int> allComponentsWithAmount;
            List<int> componentsWithAmount = new List<int>();
            DostępDoBazy.SkładnikCzynszuLokalu currentRentComponent = null;

            if (id != 0)
                currentRentComponent = rentComponentsOfPlace.ElementAt(id - 1);

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                allComponentsWithAmount = db.SkładnikiCzynszu.Where(c => c.s_zaplat == 2).Select(c => c.nr_skl).ToList();

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
                    case Enums.Akcja.Dodaj:
                        if (DostępDoBazy.SkładnikCzynszuLokalu.Waliduj(record, childAction))
                        {
                            DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace = new DostępDoBazy.SkładnikCzynszuLokalu();

                            rentComponentOfPlace.Ustaw(record);
                            rentComponentsOfPlace.Add(rentComponentOfPlace);
                        }

                        break;

                    case Enums.Akcja.Edytuj:
                        if (DostępDoBazy.SkładnikCzynszuLokalu.Waliduj(record, childAction))
                            currentRentComponent.Ustaw(record);

                        break;

                    case Enums.Akcja.Usuń:
                        rentComponentsOfPlace.Remove(currentRentComponent);

                        break;
                }
            }

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                DostępDoBazy.SkładnikCzynszuLokalu.Lokale = db.AktywneLokale.ToList();
                DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
            }

            for (int i = 0; i < rentComponentsOfPlace.Count; i++)
            {
                string index = (i + 1).ToString();

                rows.Add(new string[] { index, index }.Concat(rentComponentsOfPlace.ElementAt(i).WażnePola()).ToArray());
            }

            DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
            DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

            ViewState["componentsWithAmount"] = allComponentsWithAmount;
            ViewState["id"] = id;

            form.Controls.Add(new MyControls.HtmlInputHidden("parentAction", parentAction.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("kod_lok", kod_lok.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("nr_lok", nr_lok.ToString()));

            if (window == null)
                switch (parentAction)
                {
                    case Enums.Akcja.Dodaj:
                    case Enums.Akcja.Edytuj:
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

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        firstControl = new MyControls.DropDownList("field", "nr_skl", db.SkładnikiCzynszu.OrderBy(c => c.nr_skl).ToList().Select(c => c.WażnePolaDoRozwijanejListy()).ToList(), String.Empty, true, false);
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

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        firstControl = new MyControls.TextBox("field", "nazwa", db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == currentRentComponent.nr_skl).nazwa, MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, false);

                    form.Controls.Add(new MyControls.HtmlInputHidden("nr_skl", currentRentComponent.nr_skl.ToString()));
                    form.Controls.Add(new MyControls.HtmlInputHidden("id", id.ToString()));
                }

                placeOfNewComponent.Controls.Add(new MyControls.Label("label", "nr_skl", firstLabel, String.Empty));

                using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
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