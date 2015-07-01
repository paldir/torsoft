using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class AttributeOfObject : Strona
    {
        List<DostępDoBazy.AtrybutObiektu> attributesOfObject
        {
            get { return (List<DostępDoBazy.AtrybutObiektu>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //EnumP.AttributeOf attributeOf = (EnumP.AttributeOf)Enum.Parse(typeof(EnumP.AttributeOf), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("attributeOf"))]);
            Enumeratory.Atrybut attributeOf = PobierzWartośćParametru<Enumeratory.Atrybut>("attributeOf");
            //EnumP.Action action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
            Enumeratory.Akcja action = PobierzWartośćParametru<Enumeratory.Akcja>("action");
            Enumeratory.Akcja childAction = Enumeratory.Akcja.Przeglądaj;
            //int parentId = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("parentId"))]);
            int parentId = PobierzWartośćParametru<int>("parentId");
            int id = PobierzWartośćParametru<int>("id");
            string[] record;
            string childActionKey = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("childAction"));

            /*if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))] != null)
                id = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);*/

            if (childActionKey != null)
            {
                if (childActionKey.Contains("add"))
                    childAction = (Enumeratory.Akcja)Enum.Parse(typeof(Enumeratory.Akcja), Request.Params[childActionKey].Replace("Zapisz", "Dodaj"));
                else
                    if (childActionKey.Contains("edit"))
                        childAction = (Enumeratory.Akcja)Enum.Parse(typeof(Enumeratory.Akcja), Request.Params[childActionKey].Replace("Zapisz", "Edytuj"));
                    else
                        childAction = (Enumeratory.Akcja)Enum.Parse(typeof(Enumeratory.Akcja), Request.Params[childActionKey]);
            }

            List<string[]> rows = null;
            List<string[]> rowsOfDropDown = null;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                switch (childAction)
                {
                    case Enumeratory.Akcja.Dodaj:
                        DostępDoBazy.AtrybutObiektu attributeOfObject = null;
                        int maxIdTmp = 0;
                        int maxId = 0;

                        record = new string[]
                    {
                        String.Empty,
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("kod"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("wartosc"))],
                        parentId.ToString()
                    };

                        if (DostępDoBazy.AtrybutObiektu.Waliduj(childAction, record, attributesOfObject))
                        {
                            if (attributesOfObject.Any())
                                maxIdTmp = attributesOfObject.Max(a => a.__record);

                            switch (attributeOf)
                            {
                                case Enumeratory.Atrybut.Budynku:
                                    attributeOfObject = new DostępDoBazy.AtrybutBudynku();

                                    if (db.AtrybutyBudynków.Any())
                                        maxId = db.AtrybutyBudynków.Max(a => a.__record);

                                    break;

                                case Enumeratory.Atrybut.Wspólnoty:
                                    attributeOfObject = new DostępDoBazy.AtrybutWspólnoty();

                                    if (db.AtrybutyWspólnot.Any())
                                        maxId = db.AtrybutyWspólnot.Max(a => a.__record);

                                    break;

                                case Enumeratory.Atrybut.Lokalu:
                                    attributeOfObject = new DostępDoBazy.AtrybutLokalu();

                                    if (db.AtrybutyLokali.Any())
                                        maxId = db.AtrybutyLokali.Max(a => a.__record);

                                    break;

                                case Enumeratory.Atrybut.Najemcy:
                                    attributeOfObject = new DostępDoBazy.AtrybutNajemcy();

                                    if (db.AtrybutyNajemców.Any())
                                        maxId = db.AtrybutyNajemców.Max(a => a.__record);

                                    break;
                            }

                            record[0] = (Math.Max(maxId, maxIdTmp) + 1).ToString();

                            attributeOfObject.Ustaw(record);
                            attributesOfObject.Add(attributeOfObject);
                        }

                        break;

                    case Enumeratory.Akcja.Edytuj:
                        string wartosc = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("wartosc_edit"))];
                        int id_edit = Int32.Parse(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id_edit"))]);
                        DostępDoBazy.AtrybutObiektu attribute = attributesOfObject.FirstOrDefault(a => a.__record == id_edit);

                        record = new string[]
                    {
                        attribute.__record.ToString(),
                        attribute.kod.ToString(),
                        wartosc,
                        attribute.kod_powiaz
                    };

                        if (DostępDoBazy.AtrybutObiektu.Waliduj(childAction, record, attributesOfObject))
                            attribute.Ustaw(record);

                        break;

                    case Enumeratory.Akcja.Usuń:
                        attributesOfObject.Remove(attributesOfObject.FirstOrDefault(a => a.__record == id));

                        break;
                }

                rows = attributesOfObject.Select(a => a.WażnePola()).ToList();

                switch (attributeOf)
                {
                    case Enumeratory.Atrybut.Budynku:
                        rowsOfDropDown = db.Atrybuty.Where(a => a.zb_b == "X").ToList().Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;

                    case Enumeratory.Atrybut.Wspólnoty:
                        rowsOfDropDown = db.Atrybuty.Where(a => a.zb_s == "X").ToList().Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;

                    case Enumeratory.Atrybut.Lokalu:
                        rowsOfDropDown = db.Atrybuty.Where(a => a.zb_l == "X").ToList().Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;

                    case Enumeratory.Atrybut.Najemcy:
                        rowsOfDropDown = db.Atrybuty.Where(a => a.zb_n == "X").ToList().Select(a => a.WażnePolaDoRozwijanejListy()).ToList();

                        break;
                }

                string postBackUrl = "AttributeOfObject.aspx";

                form.Controls.Add(new Kontrolki.HtmlInputHidden("parentId", parentId.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("attributeOf", attributeOf.ToString()));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("action", action.ToString()));
                placeOfTable.Controls.Add(new Kontrolki.Table("mainTable tabTable", rows, new string[] { "Cecha", "Wartość" }, false, String.Empty, new List<int>(), new List<int>()));

                switch (action)
                {
                    case Enumeratory.Akcja.Dodaj:
                    case Enumeratory.Akcja.Edytuj:
                        if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showEditingWindow"))] != null)
                        {
                            DostępDoBazy.AtrybutObiektu attributeOfObject = attributesOfObject.FirstOrDefault(a => a.__record == id);
                            DostępDoBazy.Atrybut attribute = db.Atrybuty.FirstOrDefault(a => a.kod == attributeOfObject.kod);

                            placeOfEditingWindow.Controls.Add(new Kontrolki.HtmlInputHidden("id_edit", attributeOfObject.__record.ToString()));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Label("label", "nazwa", "Nazwa: ", String.Empty));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.TextBox("field", "nazwa", attribute.nazwa, Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 20, 1, false));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Label("label", "wartosc_edit", "<br />Wartość: ", String.Empty));

                            switch (attribute.nr_str)
                            {
                                case "N":
                                    placeOfEditingWindow.Controls.Add(new Kontrolki.TextBox("field", "wartosc_edit", attributeOfObject.wartosc_n.ToString("F2"), Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 16, 1, true));

                                    break;

                                case "C":
                                    placeOfEditingWindow.Controls.Add(new Kontrolki.TextBox("field", "wartosc_edit", attributeOfObject.wartosc_s.Trim(), Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, true));

                                    break;
                            }

                            DodajNowąLinię(placeOfEditingWindow);
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", "editchildAction", "Zapisz", postBackUrl));
                            placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", postBackUrl));
                        }
                        else
                            if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showAddingWindow"))] != null)
                            {
                                placeOfNewAttribute.Controls.Add(new Kontrolki.Label("label", "kod", "Nowa cecha: ", String.Empty));
                                placeOfNewAttribute.Controls.Add(new Kontrolki.DropDownList("field", "kod", rowsOfDropDown, String.Empty, true, false));
                                placeOfNewAttribute.Controls.Add(new Kontrolki.Label("label", "wartosc", "<br />Wartość: ", String.Empty));
                                placeOfNewAttribute.Controls.Add(new Kontrolki.TextBox("field", "wartosc", String.Empty, Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, 25, 1, true));
                                placeOfNewAttribute.Controls.Add(new LiteralControl("<span id='unit'></span>"));
                                placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", "addchildAction", "Zapisz", postBackUrl));
                                placeOfEditingWindow.Controls.Add(new Kontrolki.Button("button", String.Empty, "Anuluj", postBackUrl));
                            }
                            else
                            {
                                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "showAddingWindow", "Dodaj", postBackUrl));
                                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "deletechildAction", "Usuń", postBackUrl));
                                placeOfButtons.Controls.Add(new Kontrolki.Button("button", "showEditingWindow", "Edytuj", postBackUrl));
                            }

                        break;
                }
            }
        }
    }
}