using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class AtrybutyObiektu : Strona
    {
        List<DostępDoBazy.AtrybutObiektu> _atrybutyObiektu
        {
            get { return (List<DostępDoBazy.AtrybutObiektu>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                //EnumP.AttributeOf attributeOf = (EnumP.AttributeOf)Enum.Parse(typeof(EnumP.AttributeOf), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("attributeOf"))]);
                Enumeratory.Atrybut atrybutDotyczy = PobierzWartośćParametru<Enumeratory.Atrybut>("attributeOf");
                //EnumP.Action action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
                Enumeratory.Akcja akcja = PobierzWartośćParametru<Enumeratory.Akcja>("action");
                Enumeratory.Akcja akcjaDziecka = Enumeratory.Akcja.Przeglądaj;
                //int parentId = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("parentId"))]);
                int idRodzica = PobierzWartośćParametru<int>("parentId");
                int id = PobierzWartośćParametru<int>("id");
                string[] rekord;
                string kluczAkcjiDziecka = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("childAction"));

                /*if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))] != null)
                    id = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);*/

                if (kluczAkcjiDziecka != null)
                {
                    if (kluczAkcjiDziecka.Contains("add"))
                        akcjaDziecka = (Enumeratory.Akcja)Enum.Parse(typeof(Enumeratory.Akcja), Request.Params[kluczAkcjiDziecka].Replace("Zapisz", "Dodaj"));
                    else
                        if (kluczAkcjiDziecka.Contains("edit"))
                            akcjaDziecka = (Enumeratory.Akcja)Enum.Parse(typeof(Enumeratory.Akcja), Request.Params[kluczAkcjiDziecka].Replace("Zapisz", "Edytuj"));
                        else
                            akcjaDziecka = (Enumeratory.Akcja)Enum.Parse(typeof(Enumeratory.Akcja), Request.Params[kluczAkcjiDziecka]);
                }

                List<string[]> wiersze = null;
                List<string[]> wierszeRozwijanejListy = null;

                switch (akcjaDziecka)
                {
                    case Enumeratory.Akcja.Dodaj:
                        DostępDoBazy.AtrybutObiektu atrybutObiektu = null;
                        int maksymalneIdTmp = 0;
                        int maksymalneId = 0;

                        rekord = new string[]
                    {
                        String.Empty,
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("kod"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("wartosc"))],
                        idRodzica.ToString()
                    };

                        if (DostępDoBazy.AtrybutObiektu.Waliduj(akcjaDziecka, rekord, _atrybutyObiektu))
                        {
                            if (_atrybutyObiektu.Any())
                                maksymalneIdTmp = _atrybutyObiektu.Max(a => a.__record);

                            switch (atrybutDotyczy)
                            {
                                case Enumeratory.Atrybut.Budynku:
                                    atrybutObiektu = new DostępDoBazy.AtrybutBudynku();

                                    if (db.AtrybutyBudynków.Any())
                                        maksymalneId = db.AtrybutyBudynków.Max(a => a.__record);

                                    break;

                                case Enumeratory.Atrybut.Wspólnoty:
                                    atrybutObiektu = new DostępDoBazy.AtrybutWspólnoty();

                                    if (db.AtrybutyWspólnot.Any())
                                        maksymalneId = db.AtrybutyWspólnot.Max(a => a.__record);

                                    break;

                                case Enumeratory.Atrybut.Lokalu:
                                    atrybutObiektu = new DostępDoBazy.AtrybutLokalu();

                                    if (db.AtrybutyLokali.Any())
                                        maksymalneId = db.AtrybutyLokali.Max(a => a.__record);

                                    break;

                                case Enumeratory.Atrybut.Najemcy:
                                    atrybutObiektu = new DostępDoBazy.AtrybutNajemcy();

                                    if (db.AtrybutyNajemców.Any())
                                        maksymalneId = db.AtrybutyNajemców.Max(a => a.__record);

                                    break;
                            }

                            rekord[0] = (Math.Max(maksymalneId, maksymalneIdTmp) + 1).ToString();

                            atrybutObiektu.Ustaw(rekord);
                            _atrybutyObiektu.Add(atrybutObiektu);
                        }

                        break;

                    case Enumeratory.Akcja.Edytuj:
                        string wartosc = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("wartosc_edit"))];
                        int id_edycja = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id_edit"))]);
                        DostępDoBazy.AtrybutObiektu atrybut = _atrybutyObiektu.FirstOrDefault(a => a.__record == id_edycja);

                        rekord = new string[]
                    {
                        atrybut.__record.ToString(),
                        atrybut.kod.ToString(),
                        wartosc,
                        atrybut.kod_powiaz
                    };

                        if (DostępDoBazy.AtrybutObiektu.Waliduj(akcjaDziecka, rekord, _atrybutyObiektu))
                            atrybut.Ustaw(rekord);

                        break;

                    case Enumeratory.Akcja.Usuń:
                        _atrybutyObiektu.Remove(_atrybutyObiektu.FirstOrDefault(a => a.__record == id));

                        break;
                }

                wiersze = _atrybutyObiektu.Select(a => a.PolaDoTabeli()).ToList();
                IEnumerable<DostępDoBazy.Atrybut> atrybuty = db.Atrybuty.AsEnumerable<DostępDoBazy.Atrybut>();

                switch (atrybutDotyczy)
                {
                    case Enumeratory.Atrybut.Budynku:
                        wierszeRozwijanejListy = atrybuty.Where(a => String.Equals(a.zb_b, "X")).Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;

                    case Enumeratory.Atrybut.Wspólnoty:
                        wierszeRozwijanejListy = atrybuty.Where(a => String.Equals(a.zb_s, "X")).Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;

                    case Enumeratory.Atrybut.Lokalu:
                        wierszeRozwijanejListy = atrybuty.Where(a => String.Equals(a.zb_l, "X")).Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;

                    case Enumeratory.Atrybut.Najemcy:
                        wierszeRozwijanejListy = atrybuty.Where(a => String.Equals(a.zb_n, "X")).Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;
                }

                string url = "AtrybutyObiektu.aspx";

                form.Controls.Add(new Kontrolki.HtmlInputHidden("parentId", idRodzica.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("attributeOf", atrybutDotyczy.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("action", akcja.ToString()));
                placeOfTable.Controls.Add(new Kontrolki.Table("mainTable tabTable", wiersze, new string[] { "Cecha", "Wartość" }, false, String.Empty, new List<int>(), new List<int>()));

                switch (akcja)
                {
                    case Enumeratory.Akcja.Dodaj:
                    case Enumeratory.Akcja.Edytuj:
                        if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showEditingWindow"))] != null)
                        {
                            DostępDoBazy.AtrybutObiektu atrybutyObiektu = _atrybutyObiektu.FirstOrDefault(a => a.__record == id);
                            DostępDoBazy.Atrybut atrybut = db.Atrybuty.FirstOrDefault(a => a.kod == atrybutyObiektu.kod);

                            placeOfEditingWindow.Controls.Add(new Kontrolki.HtmlInputHidden("id_edit", atrybutyObiektu.__record.ToString()));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Label("label", "nazwa", "Nazwa: ", String.Empty));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.TextBox("field", "nazwa", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 20, 1, false, atrybut.nazwa));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Label("label", "wartosc_edit", "<br />Wartość: ", String.Empty));

                            switch (atrybut.nr_str)
                            {
                                case "N":
                                    placeOfEditingWindow.Controls.Add(new Kontrolki.TextBox("field", "wartosc_edit", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 16, 1, true, atrybutyObiektu.wartosc_n.ToString("F2")));

                                    break;

                                case "C":
                                    placeOfEditingWindow.Controls.Add(new Kontrolki.TextBox("field", "wartosc_edit", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, true, atrybutyObiektu.wartosc_s.Trim()));

                                    break;
                            }

                            DodajNowąLinię(placeOfEditingWindow);
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", "editchildAction", "Zapisz", url));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", url));
                        }
                        else
                            if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showAddingWindow"))] != null)
                            {
                                placeOfNewAttribute.Controls.Add(new Kontrolki.Label("label", "kod", "Nowa cecha: ", String.Empty));
                                placeOfNewAttribute.Controls.Add(new Kontrolki.DropDownList("field", "kod", wierszeRozwijanejListy, true, false));
                                placeOfNewAttribute.Controls.Add(new Kontrolki.Label("label", "wartosc", "<br />Wartość: ", String.Empty));
                                placeOfNewAttribute.Controls.Add(new Kontrolki.TextBox("field", "wartosc", Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, true));
                                placeOfNewAttribute.Controls.Add(new LiteralControl("<span id='unit'></span>"));
                                placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", "addchildAction", "Zapisz", url));
                                placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", url));
                            }
                            else
                            {
                                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "showAddingWindow", "Dodaj", url));
                                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "deletechildAction", "Usuń", url));
                                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "showEditingWindow", "Edytuj", url));
                            }

                        break;
                }
            }
        }
    }
}