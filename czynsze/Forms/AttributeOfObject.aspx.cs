using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class AttributeOfObject : Page
    {
        List<DataAccess.AttributeOfObject> attributesOfObject
        {
            get { return (List<DataAccess.AttributeOfObject>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //EnumP.AttributeOf attributeOf = (EnumP.AttributeOf)Enum.Parse(typeof(EnumP.AttributeOf), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("attributeOf"))]);
            Enums.AttributeOf attributeOf = GetParamValue<Enums.AttributeOf>("attributeOf");
            //EnumP.Action action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
            Enums.Action action = GetParamValue<Enums.Action>("action");
            Enums.Action childAction = Enums.Action.Przeglądaj;
            //int parentId = Convert.ToInt32(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("parentId"))]);
            int parentId = GetParamValue<int>("parentId");
            int id = GetParamValue<int>("id");
            string[] record;
            string childActionKey = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("childAction"));

            /*if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))] != null)
                id = Convert.ToInt32(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);*/

            if (childActionKey != null)
            {
                if (childActionKey.Contains("add"))
                    childAction = (Enums.Action)Enum.Parse(typeof(Enums.Action), Request.Params[childActionKey].Replace("Zapisz", "Dodaj"));
                else
                    if (childActionKey.Contains("edit"))
                        childAction = (Enums.Action)Enum.Parse(typeof(Enums.Action), Request.Params[childActionKey].Replace("Zapisz", "Edytuj"));
                    else
                        childAction = (Enums.Action)Enum.Parse(typeof(Enums.Action), Request.Params[childActionKey]);
            }

            List<string[]> rows = null;
            List<string[]> rowsOfDropDown = null;

            switch (childAction)
            {
                case Enums.Action.Dodaj:
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
                        if (attributesOfObject.Any())
                            maxIdTmp = attributesOfObject.Max(a => a.__record);

                        switch (attributeOf)
                        {
                            case Enums.AttributeOf.Building:
                                attributeOfObject = new DataAccess.AttributeOfBuilding();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfBuildings.Any())
                                        maxId = db.attributesOfBuildings.Max(a => a.__record);

                                break;

                            case Enums.AttributeOf.Community:
                                attributeOfObject = new DataAccess.AttributeOfCommunity();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfCommunities.Any())
                                        maxId = db.attributesOfCommunities.Max(a => a.__record);

                                break;

                            case Enums.AttributeOf.Place:
                                attributeOfObject = new DataAccess.AttributeOfPlace();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfPlaces.Any())
                                        maxId = db.attributesOfPlaces.Max(a => a.__record);

                                break;

                            case Enums.AttributeOf.Tenant:
                                attributeOfObject = new DataAccess.AttributeOfTenant();

                                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                    if (db.attributesOfTenants.Any())
                                        maxId = db.attributesOfTenants.Max(a => a.__record);

                                break;
                        }

                        record[0] = (Math.Max(maxId, maxIdTmp) + 1).ToString();

                        attributeOfObject.Set(record);
                        attributesOfObject.Add(attributeOfObject);
                    }

                    break;

                case Enums.Action.Edytuj:
                    string wartosc = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("wartosc_edit"))];
                    int id_edit = Convert.ToInt32(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id_edit"))]);
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

                case Enums.Action.Usuń:
                    attributesOfObject.Remove(attributesOfObject.FirstOrDefault(a => a.__record == id));

                    break;
            }

            rows = attributesOfObject.Select(a => a.ImportantFields()).ToList();

            switch (attributeOf)
            {
                case Enums.AttributeOf.Building:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_b == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();

                    break;

                case Enums.AttributeOf.Community:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_s == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();

                    break;

                case Enums.AttributeOf.Place:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_l == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();

                    break;

                case Enums.AttributeOf.Tenant:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        rowsOfDropDown = db.attributes.Where(a => a.zb_n == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();

                    break;
            }

            string postBackUrl = "AttributeOfObject.aspx";

            form.Controls.Add(new MyControls.HtmlInputHidden("parentId", parentId.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("attributeOf", attributeOf.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("action", action.ToString()));
            placeOfTable.Controls.Add(new MyControls.Table("mainTable tabTable", rows, new string[] { "Cecha", "Wartość" }, false, String.Empty, new List<int>(), new List<int>()));

            switch (action)
            {
                case Enums.Action.Dodaj:
                case Enums.Action.Edytuj:
                    if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showEditingWindow"))] != null)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            DataAccess.AttributeOfObject attributeOfObject = attributesOfObject.FirstOrDefault(a => a.__record == id);
                            DataAccess.Attribute attribute = db.attributes.FirstOrDefault(a => a.kod == attributeOfObject.kod);

                            placeOfEditingWindow.Controls.Add(new MyControls.HtmlInputHidden("id_edit", attributeOfObject.__record.ToString()));
                            placeOfEditingWindow.Controls.Add(new MyControls.Label("label", "nazwa", "Nazwa: ", String.Empty));
                            placeOfEditingWindow.Controls.Add(new MyControls.TextBox("field", "nazwa", attribute.nazwa, MyControls.TextBox.TextBoxMode.SingleLine, 20, 1, false));
                            placeOfEditingWindow.Controls.Add(new MyControls.Label("label", "wartosc_edit", "<br />Wartość: ", String.Empty));

                            switch (attribute.nr_str)
                            {
                                case "N":
                                    placeOfEditingWindow.Controls.Add(new MyControls.TextBox("field", "wartosc_edit", attributeOfObject.wartosc_n.ToString("F2"), MyControls.TextBox.TextBoxMode.Number, 16, 1, true));

                                    break;

                                case "C":
                                    placeOfEditingWindow.Controls.Add(new MyControls.TextBox("field", "wartosc_edit", attributeOfObject.wartosc_s.Trim(), MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, true));

                                    break;
                            }

                            AddNewLine(placeOfEditingWindow);
                            placeOfEditingWindow.Controls.Add(new MyControls.Button("button", "editchildAction", "Zapisz", postBackUrl));
                            placeOfEditingWindow.Controls.Add(new MyControls.Button("button", String.Empty, "Anuluj", postBackUrl));
                        }
                    else
                        if (Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("showAddingWindow"))] != null)
                        {
                            placeOfNewAttribute.Controls.Add(new MyControls.Label("label", "kod", "Nowa cecha: ", String.Empty));
                            placeOfNewAttribute.Controls.Add(new MyControls.DropDownList("field", "kod", rowsOfDropDown, String.Empty, true, false));
                            placeOfNewAttribute.Controls.Add(new MyControls.Label("label", "wartosc", "<br />Wartość: ", String.Empty));
                            placeOfNewAttribute.Controls.Add(new MyControls.TextBox("field", "wartosc", String.Empty, MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, true));
                            placeOfNewAttribute.Controls.Add(new LiteralControl("<span id='unit'></span>"));
                            placeOfEditingWindow.Controls.Add(new MyControls.Button("button", "addchildAction", "Zapisz", postBackUrl));
                            placeOfEditingWindow.Controls.Add(new MyControls.Button("button", String.Empty, "Anuluj", postBackUrl));
                        }
                        else
                        {
                            placeOfButtons.Controls.Add(new MyControls.Button("button", "showAddingWindow", "Dodaj", postBackUrl));
                            placeOfButtons.Controls.Add(new MyControls.Button("button", "deletechildAction", "Usuń", postBackUrl));
                            placeOfButtons.Controls.Add(new MyControls.Button("button", "showEditingWindow", "Edytuj", postBackUrl));
                        }

                    break;
            }
        }
    }
}