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
            EnumP.Action childAction = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("childAction"))]);
            int parentId = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("parentId"))]);

            List<string[]> rows = null;
            List<string[]> rowsOfDropDown = null;

            switch (childAction)
            {
                case EnumP.Action.Dodaj:
                    DataAccess.AttributeOfObject attributeOfObject = null;
                    int maxIdTmp = 0;
                    int maxId = 0;

                    string[] record = new string[]
                    {
                        String.Empty,
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("kod"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("wartosc"))],
                        parentId.ToString()
                    };

                    if (DataAccess.AttributeOfObject.Validate(record, attributesOfObject))
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
                case EnumP.Action.Usuń:
                    int id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);

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
                    placeOfDeletingButton.Controls.Add(new ControlsP.ButtonP("button", "deletechildAction", "Usuń", postBackUrl));
                    placeOfNewAttribute.Controls.Add(new LiteralControl("Nowa cecha: "));
                    placeOfNewAttribute.Controls.Add(new ControlsP.DropDownListP("field", "kod", rowsOfDropDown, String.Empty, true));
                    placeOfNewAttribute.Controls.Add(new LiteralControl("Wartość: "));
                    placeOfNewAttribute.Controls.Add(new ControlsP.TextBoxP("field", "wartosc", String.Empty, ControlsP.TextBoxP.TextBoxModeP.SingleLine, 25, 1, true));
                    placeOfNewAttribute.Controls.Add(new LiteralControl("<span id='unit'></span>"));
                    placeOfNewAttribute.Controls.Add(new ControlsP.ButtonP("button", "addchildAction", "Dodaj", postBackUrl));
                    break;
            }
        }
    }
}