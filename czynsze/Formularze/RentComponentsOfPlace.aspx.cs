using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class RentComponentsOfPlace : Strona
    {
        List<DostępDoBazy.SkładnikCzynszuLokalu> rentComponentsOfPlace
        {
            get { return (List<DostępDoBazy.SkładnikCzynszuLokalu>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                int kod_lok = PobierzWartośćParametru<int>("kod_lok");
                int nr_lok = PobierzWartośćParametru<int>("nr_lok");
                List<string[]> rows = new List<string[]>();
                string window = PobierzWartośćParametru<string>("ShowWindow");
                Enumeratory.Akcja parentAction = PobierzWartośćParametru<Enumeratory.Akcja>("parentAction");
                Enumeratory.Akcja childAction = PobierzWartośćParametru<Enumeratory.Akcja>("ChildAction");
                int id = PobierzWartośćParametru<int>("id");
                string postBackUrl = "RentComponentsOfPlace.aspx";
                List<int> allComponentsWithAmount;
                List<int> componentsWithAmount = new List<int>();
                DostępDoBazy.SkładnikCzynszuLokalu currentRentComponent = null;

                if (id != 0)
                    currentRentComponent = rentComponentsOfPlace.ElementAt(id - 1);

                allComponentsWithAmount = db.SkładnikiCzynszu.Where(c => c.s_zaplat == 2).Select(c => c.Id).ToList();

                if ((int)childAction != 0)
                {
                    string[] record = new string[]
                {
                    kod_lok.ToString(),
                    nr_lok.ToString(),
                    PobierzWartośćParametru<string>("nr_skl"),
                    PobierzWartośćParametru<string>("dan_p"),
                    PobierzWartośćParametru<string>("dat_od"),
                    PobierzWartośćParametru<string>("dat_do")
                };

                    switch (childAction)
                    {
                        case Enumeratory.Akcja.Dodaj:
                            if (DostępDoBazy.SkładnikCzynszuLokalu.Waliduj(record, childAction))
                            {
                                DostępDoBazy.SkładnikCzynszuLokalu rentComponentOfPlace = new DostępDoBazy.SkładnikCzynszuLokalu();

                                rentComponentOfPlace.Ustaw(record);
                                rentComponentsOfPlace.Add(rentComponentOfPlace);
                            }

                            break;

                        case Enumeratory.Akcja.Edytuj:
                            if (DostępDoBazy.SkładnikCzynszuLokalu.Waliduj(record, childAction))
                                currentRentComponent.Ustaw(record);

                            break;

                        case Enumeratory.Akcja.Usuń:
                            rentComponentsOfPlace.Remove(currentRentComponent);

                            break;
                    }
                }

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

                form.Controls.Add(new Kontrolki.HtmlInputHidden("parentAction", parentAction.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_lok", kod_lok.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_lok", nr_lok.ToString()));

                if (window == null)
                    switch (parentAction)
                    {
                        case Enumeratory.Akcja.Dodaj:
                        case Enumeratory.Akcja.Edytuj:
                            placeOfButtons.Controls.Add(new Kontrolki.Button("button", "addShowWindow", "Dodaj", postBackUrl));
                            placeOfButtons.Controls.Add(new Kontrolki.Button("button", "removeChildAction", "Usuń", postBackUrl));
                            placeOfButtons.Controls.Add(new Kontrolki.Button("button", "editShowWindow", "Edytuj", postBackUrl));

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
                        firstControl = new Kontrolki.DropDownList("field", "nr_skl", db.SkładnikiCzynszu.OrderBy(c => c.Id).ToList().Select(c => c.WażnePolaDoRozwijanejListy()).ToList(), String.Empty, true, false);
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

                        firstControl = new Kontrolki.TextBox("field", "nazwa", db.SkładnikiCzynszu.FirstOrDefault(c => c.Id == currentRentComponent.nr_skl).Nazwa, Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, false);

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_skl", currentRentComponent.nr_skl.ToString()));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));
                    }

                    placeOfNewComponent.Controls.Add(new Kontrolki.Label("label", "nr_skl", firstLabel, String.Empty));
                    placeOfNewComponent.Controls.Add(firstControl);

                    placeOfAmount.Controls.Add(new Kontrolki.Label("label", "dan_p", "Ilość: ", String.Empty));
                    placeOfAmount.Controls.Add(new Kontrolki.TextBox("field", "dan_p", amount, Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 15, 1, true));
                    placeOfDate.Controls.Add(new Kontrolki.Label("label", "dat_od", "Zakres dat: ", String.Empty));
                    placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "dat_od", dat_od, Kontrolki.TextBox.TextBoxMode.Data, 10, 1, true));
                    placeOfDate.Controls.Add(new Kontrolki.Label("label", "dat_do", " - ", String.Empty));
                    placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "dat_do", dat_do, Kontrolki.TextBox.TextBoxMode.Data, 10, 1, true));
                    placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", "saveChildAction", textOfSaveButton, postBackUrl));
                    placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", postBackUrl));
                }

                placeOfTable.Controls.Add(new Kontrolki.Table("mainTable tabTable", rows, new string[] { "Lp.", "Nr", "Nazwa", "Stawka", "Ilość", "Wartość", "Początek zakresu dat", "Koniec zakresu dat" }, false, String.Empty, new List<int>() { 1, 2, 4, 5, 6 }, new List<int>() { 6 }));
            }
        }
    }
}