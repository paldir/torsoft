using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class BudynkiWspolnoty : Strona
    {
        List<DostępDoBazy.BudynekWspólnoty> _budynkiWspólnoty
        {
            get { return (List<DostępDoBazy.BudynekWspólnoty>)Session["communityBuildings"]; }
            set { Session["communityBuildings"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                int kod = PobierzWartośćParametru<int>("kod");
                List<string[]> wiersze = new List<string[]>();
                string okno = PobierzWartośćParametru<string>("ShowWindow");
                Enumeratory.Akcja akcjaRodzica = PobierzWartośćParametru<Enumeratory.Akcja>("parentAction");
                Enumeratory.Akcja akcjaDziecka = PobierzWartośćParametru<Enumeratory.Akcja>("ChildAction");
                int id = PobierzWartośćParametru<int>("id");
                string url = "BudynkiWspolnoty.aspx";
                DostępDoBazy.BudynekWspólnoty obecnyBudynekWspólnoty = null;

                if (id != 0)
                    obecnyBudynekWspólnoty = _budynkiWspólnoty.ElementAt(id - 1);

                if ((int)akcjaDziecka != 0)
                {
                    string[] rekord = new string[]
                {
                    kod.ToString(),
                    PobierzWartośćParametru<string>("kod_1"),
                    PobierzWartośćParametru<string>("uwagi")
                };

                    switch (akcjaDziecka)
                    {
                        case Enumeratory.Akcja.Dodaj:
                            DostępDoBazy.BudynekWspólnoty budynekWspólnoty = new DostępDoBazy.BudynekWspólnoty();

                            budynekWspólnoty.Ustaw(rekord);
                            _budynkiWspólnoty.Add(budynekWspólnoty);

                            break;

                        case Enumeratory.Akcja.Edytuj:
                            obecnyBudynekWspólnoty.Ustaw(rekord);

                            break;

                        case Enumeratory.Akcja.Usuń:
                            _budynkiWspólnoty.Remove(obecnyBudynekWspólnoty);

                            break;
                    }
                }

                for (int i = 0; i < _budynkiWspólnoty.Count; i++)
                {
                    string indeks = (i + 1).ToString();

                    wiersze.Add(new string[] { indeks, indeks }.Concat(_budynkiWspólnoty.ElementAt(i).PolaDoTabeli()).ToArray());
                }

                ViewState["id"] = id;

                form.Controls.Add(new Kontrolki.HtmlInputHidden("parentAction", akcjaRodzica.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("kod", kod.ToString()));

                if (okno == null)
                    switch (akcjaRodzica)
                    {
                        case Enumeratory.Akcja.Dodaj:
                        case Enumeratory.Akcja.Edytuj:
                            placeOfButtons.Controls.Add(new Kontrolki.Button("button", "addShowWindow", "Dodaj", url));
                            placeOfButtons.Controls.Add(new Kontrolki.Button("button", "removeChildAction", "Usuń", url));
                            placeOfButtons.Controls.Add(new Kontrolki.Button("button", "editShowWindow", "Edytuj", url));

                            break;
                    }
                else
                {
                    string pierwszaEtykieta;
                    Control pierwszaKontrolka;
                    string komentarze;
                    string tekstPrzyciskuZapisywania;

                    if (okno == "Dodaj")
                    {
                        pierwszaEtykieta = "Nowy budynek: ";
                        tekstPrzyciskuZapisywania = "Dodaj";
                        komentarze = String.Empty;
                        pierwszaKontrolka = new Kontrolki.DropDownList("field", "kod_1", db.Budynki.AsEnumerable<DostępDoBazy.Budynek>().OrderBy(b => b.kod_1).Select(b => b.PolaDoTabeli()).ToList(), true, false);
                    }
                    else
                    {
                        pierwszaEtykieta = "Budynek: ";
                        tekstPrzyciskuZapisywania = "Edytuj";
                        komentarze = obecnyBudynekWspólnoty.uwagi.Trim();
                        pierwszaKontrolka = new Kontrolki.TextBox("field", "budynek", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, false, db.Budynki.FirstOrDefault(b => b.kod_1 == obecnyBudynekWspólnoty.kod_1).kod_1.ToString());

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_1", obecnyBudynekWspólnoty.kod_1.ToString()));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));
                    }

                    placeOfNewBuilding.Controls.Add(new Kontrolki.Label("label", "kod_1", pierwszaEtykieta, String.Empty));

                    //using (DostępDoBazy.Czynsze_Entities db = new DostępDoBazy.Czynsze_Entities())
                    placeOfNewBuilding.Controls.Add(pierwszaKontrolka);
                    placeOfComments.Controls.Add(new Kontrolki.Label("field", "uwagi", "Uwagi: ", String.Empty));
                    placeOfComments.Controls.Add(new Kontrolki.TextBox("field", "uwagi", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, true, komentarze));
                    placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", "saveChildAction", tekstPrzyciskuZapisywania, url));
                    placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", url));
                }

                placeOfTable.Controls.Add(new Kontrolki.Table("mainTable tabTable", wiersze, new string[] { "Lp.", "Nr budynku", "Adres", "Uwagi" }, false, String.Empty, new List<int>() { 1, 2 }, new List<int>()));
            }
        }
    }
}