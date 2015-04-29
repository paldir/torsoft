using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class CommunityBuildings : Strona
    {
        List<DostępDoBazy.BudynekWspólnoty> communityBuildings
        {
            get { return (List<DostępDoBazy.BudynekWspólnoty>)Session["communityBuildings"]; }
            set { Session["communityBuildings"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int kod = PobierzWartośćParametru<int>("kod");
            List<string[]> rows = new List<string[]>();
            string window = PobierzWartośćParametru<string>("ShowWindow");
            Enumeratory.Akcja parentAction = PobierzWartośćParametru<Enumeratory.Akcja>("parentAction");
            Enumeratory.Akcja childAction = PobierzWartośćParametru<Enumeratory.Akcja>("ChildAction");
            int id = PobierzWartośćParametru<int>("id");
            string postBackUrl = "CommunityBuildings.aspx";
            DostępDoBazy.BudynekWspólnoty currentCommunityBuilding = null;

            if (id != 0)
                currentCommunityBuilding = communityBuildings.ElementAt(id - 1);

            if ((int)childAction != 0)
            {
                string[] record = new string[]
                {
                    kod.ToString(),
                    PobierzWartośćParametru<string>("kod_1"),
                    PobierzWartośćParametru<string>("uwagi")
                };

                switch (childAction)
                {
                    case Enumeratory.Akcja.Dodaj:
                        DostępDoBazy.BudynekWspólnoty communityBuilding = new DostępDoBazy.BudynekWspólnoty();

                        communityBuilding.Ustaw(record);
                        communityBuildings.Add(communityBuilding);

                        break;

                    case Enumeratory.Akcja.Edytuj:
                        currentCommunityBuilding.Ustaw(record);

                        break;

                    case Enumeratory.Akcja.Usuń:
                        communityBuildings.Remove(currentCommunityBuilding);

                        break;
                }
            }

            for (int i = 0; i < communityBuildings.Count; i++)
            {
                string index = (i + 1).ToString();

                rows.Add(new string[] { index, index }.Concat(communityBuildings.ElementAt(i).WażnePola()).ToArray());
            }

            ViewState["id"] = id;

            form.Controls.Add(new Kontrolki.HtmlInputHidden("parentAction", parentAction.ToString()));
            form.Controls.Add(new Kontrolki.HtmlInputHidden("kod", kod.ToString()));

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
                Control firstControl;
                string comments;
                string textOfSaveButton;

                if (window == "Dodaj")
                {
                    firstLabel = "Nowy budynek: ";
                    textOfSaveButton = "Dodaj";
                    comments = String.Empty;

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        firstControl = new Kontrolki.DropDownList("field", "kod_1", db.Budynki.OrderBy(b => b.kod_1).ToList().Select(b => b.WażnePola()).ToList(), String.Empty, true, false);
                }
                else
                {
                    firstLabel = "Budynek: ";
                    textOfSaveButton = "Edytuj";
                    comments = currentCommunityBuilding.uwagi.Trim();

                    using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
                        firstControl = new Kontrolki.TextBox("field", "budynek", db.Budynki.FirstOrDefault(b => b.kod_1 == currentCommunityBuilding.kod_1).kod_1.ToString(), Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, false);

                    form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_1", currentCommunityBuilding.kod_1.ToString()));
                    form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));
                }

                placeOfNewBuilding.Controls.Add(new Kontrolki.Label("label", "kod_1", firstLabel, String.Empty));

                //using (DostępDoBazy.Czynsze_Entities db = new DostępDoBazy.Czynsze_Entities())
                placeOfNewBuilding.Controls.Add(firstControl);
                placeOfComments.Controls.Add(new Kontrolki.Label("field", "uwagi", "Uwagi: ", String.Empty));
                placeOfComments.Controls.Add(new Kontrolki.TextBox("field", "uwagi", comments, Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, true));
                placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", "saveChildAction", textOfSaveButton, postBackUrl));
                placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", postBackUrl));
            }

            placeOfTable.Controls.Add(new Kontrolki.Table("mainTable tabTable", rows, new string[] { "Lp.", "Nr budynku", "Adres", "Uwagi" }, false, String.Empty, new List<int>() { 1, 2 }, new List<int>()));
        }
    }
}