using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class AttributeOfObject : System.Web.UI.Page
    {
        /*List<DataAccess.AttributeOfBuilding> attributesOfObject
        {
            get { return (List<DataAccess.AttributeOfBuilding>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }*/

        List<DataAccess.AttributeOfObject> attributesOfObject
        {
            get { return (List<DataAccess.AttributeOfObject>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EnumP.AttributeOf attributeOf = (EnumP.AttributeOf)Enum.Parse(typeof(EnumP.AttributeOf), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("attributeOf"))]);
            EnumP.Action action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
            EnumP.Action childAction = EnumP.Action.Przeglądaj;
            int parentId = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("parentId"))]);
            int id = -1;
            string[] record;
            string childActionKey = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("childAction"));

            if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))] != null)
                id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);

            if (childActionKey != null)
            {
                if (childActionKey.IndexOf("add") != -1)
                    childAction = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[childActionKey].Replace("Zapisz", "Dodaj"));
                else
                    if (childActionKey.IndexOf("edit") != -1)
                        childAction = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[childActionKey].Replace("Zapisz", "Edytuj"));
                    else
                        childAction = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[childActionKey]);
            }

            List<string[]> rows = null;
            List<string[]> rowsOfDropDown = null;

            switch (childAction)
            {
                case EnumP.Action.Dodaj:
                    DataAccess.AttributeOfObject attributeOfObject = null;
                    int maxIdTmp = 0;
                    int maxId = 0;

                    record = new string[]
                    {
                        String.Empty,
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("kod"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("wartosc"))],
                        parentId.ToString()
                    };

                    if (DataAccess.AttributeOfObject.Validate(childAction, record, attributesOfObject))
                    {
                        if (attributesOfObject.Count > 0)
                            maxIdTmp = attributesOfObject.Max(a => a.__record);

                        switch (attributeOf)
                        {
                            case EnumP.AttributeOf.Building:
                                attributeOfObject = new DataAccess.AttributeOfBuilding();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfBuildings.Count() > 0)
                                        maxId = db.attributesOfBuildings.Max(a => a.__record);
                                break;
                            case EnumP.AttributeOf.Community:
                                attributeOfObject = new DataAccess.AttributeOfCommunity();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfCommunities.Count() > 0)
                                        maxId = db.attributesOfCommunities.Max(a => a.__record);
                                break;
                            case EnumP.AttributeOf.Place:
                                attributeOfObject = new DataAccess.AttributeOfPlace();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfPlaces.Count() > 0)
                                        maxId = db.attributesOfPlaces.Max(a => a.__record);
                                break;
                            case EnumP.AttributeOf.Tenant:
                                attributeOfObject = new DataAccess.AttributeOfTenant();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfTenants.Count() > 0)
                                        maxId = db.attributesOfTenants.Max(a => a.__record);
                                break;
                        }

                        record[0] = (Math.Max(maxId, maxIdTmp) + 1).ToString();

                        attributeOfObject.Set(record);
                        attributesOfObject.Add(attributeOfObject);
                    }
                    break;
                case EnumP.Action.Edytuj:
                    string wartosc = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("wartosc_edit"))];
                    int id_edit = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id_edit"))]);
                    DataAccess.AttributeOfObject attribute = attributesOfObject.FirstOrDefault(a => a.__record == id_edit);

                    record = new string[]
                    {
                        attribute.__record.ToString(),
                        attribute.kod.ToString(),
                        wartosc,
                        attribute.kod_powiaz
                    };

                    if (DataAccess.AttributeOfObject.Validate(childAction, record, attributesOfObject))
                        attribute.Set(record);
                    break;
                case EnumP.Action.Usuń:
                    attributesOfObject.Remove(attributesOfObject.FirstOrDefault(a => a.__record == id));
                    break;
            }

            rows = attributesOfObject.Select(a => a.ImportantFields()).ToList();

            switch (attributeOf)
            {
                case EnumP.AttributeOf.Building:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_b == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();
                    break;
                case EnumP.AttributeOf.Community:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_s == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();
                    break;
                case EnumP.AttributeOf.Place:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_l == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();
                    break;
                case EnumP.AttributeOf.Tenant:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_n == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();
                    break;
            }

            string postBackUrl = "AttributeOfObject.aspx";

            form.Controls.Add(new ControlsP.HtmlInputHiddenP("parentId", parentId.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("attributeOf", attributeOf.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("action", action.ToString()));
            placeOfTable.Controls.Add(new ControlsP.TableP("mainTable tabTable", rows, new string[] { "Cecha", "Wartość" }, false, String.Empty));

            switch (action)
            {
                case EnumP.Action.Dodaj:
                case EnumP.Action.Edytuj:
                    if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showEditingWindow"))] != null)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            DataAccess.AttributeOfObject attributeOfObject = attributesOfObject.FirstOrDefault(a => a.__record == id);
                            DataAccess.Attribute attribute = db.attributes.FirstOrDefault(a => a.kod == attributeOfObject.kod);

                            placeOfEditingWindow.Controls.Add(new ControlsP.HtmlInputHiddenP("id_edit", attributeOfObject.__record.ToString()));
                            placeOfEditingWindow.Controls.Add(new ControlsP.LabelP("label", "nazwa", "Nazwa: ", String.Empty));
                            placeOfEditingWindow.Controls.Add(new ControlsP.TextBoxP("field", "nazwa", attribute.nazwa, ControlsP.TextBoxP.TextBoxModeP.SingleLine, 20, 1, false));
                            placeOfEditingWindow.Controls.Add(new ControlsP.LabelP("label", "wartosc_edit", "<br />Wartość: ", String.Empty));

                            switch (attribute.nr_str)
                            {
                                case "N":
                                    placeOfEditingWindow.Controls.Add(new ControlsP.TextBoxP("field", "wartosc_edit", attributeOfObject.wartosc_n.ToString("F2"), ControlsP.TextBoxP.TextBoxModeP.Number, 16, 1, true));
                                    break;
                                case "C":
                                    placeOfEditingWindow.Controls.Add(new ControlsP.TextBoxP("field", "wartosc_edit", attributeOfObject.wartosc_s.Trim(), ControlsP.TextBoxP.TextBoxModeP.SingleLine, 25, 1, true));
                                    break;
                            }

                            placeOfEditingWindow.Controls.Add(new LiteralControl("<br />"));
                            placeOfEditingWindow.Controls.Add(new ControlsP.ButtonP("button", "editchildAction", "Zapisz", postBackUrl));
                            placeOfEditingWindow.Controls.Add(new ControlsP.ButtonP("button", String.Empty, "Anuluj", postBackUrl));
                        }
                    else
                        if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showAddingWindow"))] != null)
                        {
                            placeOfNewAttribute.Controls.Add(new ControlsP.LabelP("label", "kod", "Nowa cecha: ", String.Empty));
                            placeOfNewAttribute.Controls.Add(new ControlsP.DropDownListP("field", "kod", rowsOfDropDown, String.Empty, true, false));
                            placeOfNewAttribute.Controls.Add(new ControlsP.LabelP("label", "wartosc", "<br />Wartość: ", String.Empty));
                            placeOfNewAttribute.Controls.Add(new ControlsP.TextBoxP("field", "wartosc", String.Empty, ControlsP.TextBoxP.TextBoxModeP.SingleLine, 25, 1, true));
                            placeOfNewAttribute.Controls.Add(new LiteralControl("<span id='unit'></span>"));
                            placeOfEditingWindow.Controls.Add(new ControlsP.ButtonP("button", "addchildAction", "Zapisz", postBackUrl));
                            placeOfEditingWindow.Controls.Add(new ControlsP.ButtonP("button", String.Empty, "Anuluj", postBackUrl));
                        }
                        else
                        {
                            placeOfButtons.Controls.Add(new ControlsP.ButtonP("button", "showAddingWindow", "Dodaj", postBackUrl));
                            placeOfButtons.Controls.Add(new ControlsP.ButtonP("button", "deletechildAction", "Usuń", postBackUrl));
                            placeOfButtons.Controls.Add(new ControlsP.ButtonP("button", "showEditingWindow", "Edytuj", postBackUrl));
                        }
                    break;
            }
        }
    }
}