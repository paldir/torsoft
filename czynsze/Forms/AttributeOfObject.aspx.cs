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
        protected void Page_Load(object sender, EventArgs e)
        {
            EnumP.AttributeOf attributeOf = (EnumP.AttributeOf)Enum.Parse(typeof(EnumP.AttributeOf), Request.Params["attributeOf"]);
            int id = Convert.ToInt16(Request.Params["id"]);

            List<string[]> rows = null;
            List<string[]> rowsOfDropDown=null;

            switch (attributeOf)
            {
                case EnumP.AttributeOf.Building:
                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        rows = db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == id).Select(a => a.ImportantFields()).ToList();
                        rowsOfDropDown = db.attributes.Where(a => a.zb_b == "X").ToList().Select(a => a.ImportantFieldsForDropDown()).ToList();
                    }
                    break;
            }

            form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", id.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("attributeOf", attributeOf.ToString()));
            placeOfTable.Controls.Add(new ControlsP.TableP("", rows, new string[] { "Cecha", "Wartość" }, false, String.Empty));
            placeOfDropDown.Controls.Add(new ControlsP.DropDownListP("field", "kod", rowsOfDropDown, String.Empty, true));
            placeOfValue.Controls.Add(new ControlsP.TextBoxP("field", "wartosc", String.Empty, ControlsP.TextBoxP.TextBoxModeP.SingleLine, 25, 1, true));
        }
    }
}