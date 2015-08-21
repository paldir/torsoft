using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class SkladnikiCzynszuLokalu : Strona
    {
        List<DostępDoBazy.SkładnikCzynszuLokalu> _składnikiCzynszuLokalu
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
                List<string[]> wiersze = new List<string[]>();
                string okno = PobierzWartośćParametru<string>("ShowWindow");
                Enumeratory.Akcja akcjaRodzica = PobierzWartośćParametru<Enumeratory.Akcja>("parentAction");
                Enumeratory.Akcja akcjaDziecka = PobierzWartośćParametru<Enumeratory.Akcja>("ChildAction");
                int id = PobierzWartośćParametru<int>("id");
                string url = "SkladnikiCzynszuLokalu.aspx";
                List<int> wszystkieSkładnikiZWartością;
                List<int> składnikiZWartością = new List<int>();
                DostępDoBazy.SkładnikCzynszuLokalu obecnySkładnik = null;

                if (id != 0)
                    obecnySkładnik = _składnikiCzynszuLokalu.ElementAt(id - 1);

                wszystkieSkładnikiZWartością = db.SkładnikiCzynszu.Where(c => c.s_zaplat == 2).Select(c => c.nr_skl).ToList();

                if ((int)akcjaDziecka != 0)
                {
                    string[] rekord = new string[]
                {
                    kod_lok.ToString(),
                    nr_lok.ToString(),
                    PobierzWartośćParametru<string>("nr_skl"),
                    PobierzWartośćParametru<string>("dan_p"),
                    PobierzWartośćParametru<string>("dat_od"),
                    PobierzWartośćParametru<string>("dat_do")
                };

                    switch (akcjaDziecka)
                    {
                        case Enumeratory.Akcja.Dodaj:
                            if (DostępDoBazy.SkładnikCzynszuLokalu.Waliduj(rekord, akcjaDziecka))
                            {
                                DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu = new DostępDoBazy.SkładnikCzynszuLokalu();

                                składnikCzynszuLokalu.Ustaw(rekord);
                                _składnikiCzynszuLokalu.Add(składnikCzynszuLokalu);
                            }

                            break;

                        case Enumeratory.Akcja.Edytuj:
                            if (DostępDoBazy.SkładnikCzynszuLokalu.Waliduj(rekord, akcjaDziecka))
                                obecnySkładnik.Ustaw(rekord);

                            break;

                        case Enumeratory.Akcja.Usuń:
                            _składnikiCzynszuLokalu.Remove(obecnySkładnik);

                            break;
                    }
                }

                {
                    DostępDoBazy.SkładnikCzynszuLokalu.Lokale = db.AktywneLokale.ToList();
                    DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = db.SkładnikiCzynszu.ToList();
                }

                for (int i = 0; i < _składnikiCzynszuLokalu.Count; i++)
                {
                    string indeks = (i + 1).ToString();

                    wiersze.Add(new string[] { indeks, indeks }.Concat(_składnikiCzynszuLokalu.ElementAt(i).PolaDoTabeli()).ToArray());
                }

                DostępDoBazy.SkładnikCzynszuLokalu.Lokale = null;
                DostępDoBazy.SkładnikCzynszuLokalu.SkładnikiCzynszu = null;

                ViewState["componentsWithAmount"] = wszystkieSkładnikiZWartością;
                ViewState["id"] = id;

                form.Controls.Add(new Kontrolki.HtmlInputHidden("parentAction", akcjaRodzica.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("kod_lok", kod_lok.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_lok", nr_lok.ToString()));

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
                    string wartość;
                    Control pierwszaKontrolka;
                    string tekstPrzyciskuZapisu;
                    string dat_od = String.Empty;
                    string dat_do = String.Empty;

                    if (String.Equals(okno, "Dodaj"))
                    {
                        pierwszaEtykieta = "Nowy składnik: ";
                        wartość = String.Empty;
                        tekstPrzyciskuZapisu = "Dodaj";
                        pierwszaKontrolka = new Kontrolki.DropDownList("field", "nr_skl", db.SkładnikiCzynszu.AsEnumerable<DostępDoBazy.SkładnikCzynszu>().OrderBy(c => c.nr_skl).Select(c => c.WażnePolaDoRozwijanejListy()).ToList(), true, false);
                    }
                    else
                    {
                        pierwszaEtykieta = "Nazwa składnika: ";
                        wartość = obecnySkładnik.dan_p.ToString("F2");
                        tekstPrzyciskuZapisu = "Edytuj";

                        if (obecnySkładnik.dat_od != null)
                            dat_od = String.Format("{0:yyyy-MM-dd}", obecnySkładnik.dat_od);

                        if (obecnySkładnik.dat_do != null)
                            dat_do = String.Format("{0:yyyy-MM-dd}", obecnySkładnik.dat_do);

                        pierwszaKontrolka = new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 30, 1, false, db.SkładnikiCzynszu.FirstOrDefault(c => c.nr_skl == obecnySkładnik.nr_skl).nazwa);

                        form.Controls.Add(new Kontrolki.HtmlInputHidden("nr_skl", obecnySkładnik.nr_skl.ToString()));
                        form.Controls.Add(new Kontrolki.HtmlInputHidden("id", id.ToString()));
                    }

                    placeOfNewComponent.Controls.Add(new Kontrolki.Label("label", "nr_skl", pierwszaEtykieta, String.Empty));
                    placeOfNewComponent.Controls.Add(pierwszaKontrolka);

                    placeOfAmount.Controls.Add(new Kontrolki.Label("label", "dan_p", "Ilość: ", String.Empty));
                    placeOfAmount.Controls.Add(new Kontrolki.TextBox("field", "dan_p", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 15, 1, true, wartość));
                    placeOfDate.Controls.Add(new Kontrolki.Label("label", "dat_od", "Zakres dat: ", String.Empty));
                    placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "dat_od", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, true, dat_od));
                    placeOfDate.Controls.Add(new Kontrolki.Label("label", "dat_do", " - ", String.Empty));
                    placeOfDate.Controls.Add(new Kontrolki.TextBox("field", "dat_do", Kontrolki.TextBox.TextBoxMode.Data, 10, 1, true, dat_do));
                    placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", "saveChildAction", tekstPrzyciskuZapisu, url));
                    placeOfButtonsOfWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", url));
                }

                placeOfTable.Controls.Add(new Kontrolki.Table("mainTable tabTable", wiersze, new string[] { "Lp.", "Nr", "Nazwa", "Stawka", "Ilość", "Wartość", "Początek zakresu dat", "Koniec zakresu dat" }, false, String.Empty, new List<int>() { 1, 2, 4, 5, 6 }, new List<int>() { 6 }));
            }
        }
    }
}