using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Record : System.Web.UI.Page
    {
        int id;
        EnumP.Action action;
        EnumP.Table table;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool globalEnabled = false;
            bool idEnabled = false;
            string[] values = (string[])Session["values"];
            int numberOfFields = 0;
            string[] labels = null;
            string heading;
            List<ControlsP.ButtonP> buttons = new List<ControlsP.ButtonP>();
            List<Control> controls = new List<Control>();

            id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);
            action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
            table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);

            switch (action)
            {
                case EnumP.Action.Dodaj:
                    globalEnabled = idEnabled = true;
                    heading = "Dodawanie";

                    buttons.Add(new ControlsP.ButtonP("buttons", "Save", "Zapisz", "RecordValidation.aspx"));
                    buttons.Add(new ControlsP.ButtonP("buttons", "Cancel", "Anuluj", "List.aspx"));
                    break;
                case EnumP.Action.Edytuj:
                    globalEnabled = true;
                    idEnabled = false;
                    heading = "Edycja";

                    buttons.Add(new ControlsP.ButtonP("buttons", "Save", "Zapisz", "RecordValidation.aspx"));
                    buttons.Add(new ControlsP.ButtonP("buttons", "Cancel", "Anuluj", "List.aspx"));
                    break;
                case EnumP.Action.Usuń:
                    globalEnabled = idEnabled = false;
                    heading = "Usuwanie";

                    buttons.Add(new ControlsP.ButtonP("buttons", "Delete", "Usuń", "RecordValidation.aspx"));
                    buttons.Add(new ControlsP.ButtonP("buttons", "Cancel", "Anuluj", "List.aspx"));
                    break;
                default:
                    globalEnabled = idEnabled = false;
                    heading = "Przeglądanie";

                    buttons.Add(new ControlsP.ButtonP("buttons", "Back", "Powrót", "List.aspx"));
                    break;
            }

            switch (table)
            {
                case EnumP.Table.Buildings:
                    this.Title = "Budynek";
                    numberOfFields = 7;
                    labels = new string[] { "Kod budynku: ", "Ilość lokali: ", "Sposób rozliczania: ", "Adres: ", "Adres cd.: ", "Udział w koszt.: ", "Uwagi: " };
                    heading += " budynku";

                    if (values == null)
                    {
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.buildings.Where(b => b.kod_1 == id).FirstOrDefault().AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new ControlsP.TextBoxP("field", "id", values[0], TextBoxMode.Number, 5, 1, idEnabled));
                    else
                    {
                        controls.Add(new ControlsP.TextBoxP("field", "idDisabled", values[0], TextBoxMode.Number, 5, 1, idEnabled));
                        placeOfFields.Controls.Add(new ControlsP.HtmlInputHiddenP("id", id.ToString()));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "il_miesz", values[1], TextBoxMode.Number, 3, 1, globalEnabled));
                    controls.Add(new ControlsP.RadioButtonListP("field", "sp_rozl", new List<string>() { "budynek", "lokale" }, new List<string>() { "0", "1" }, values[2], globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres", values[3], TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres_2", values[4], TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "udzial_w_k", values[5], TextBoxMode.Number, 6, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "uwagi", values[6], TextBoxMode.MultiLine, 420, 6, globalEnabled));
                    break;
                case EnumP.Table.Places:
                    this.Title = "Lokal";
                    numberOfFields = 19;
                    labels = new string[] { "Nr system: ", "Budynek: ", "Nr lokalu: ", "Typ: ", "Adres: ", "Adres cd.: ", "Powierzchnia użytkowa: ", "Powierzchnia mieszkalna: ", "Udział: ", "Początek zakresu dat: ", "Koniec zakresu dat: ", "Powierzchnia I pokoju: ", "Powierzchnia II pokoju: ", "Powierzchnia III pokoju: ", "Powierzchnia IV pokoju: ", "Powierzchnia V pokoju: ", "Powierzchnia VI pokoju: ", "Typ kuchni: ", "Najemca: " };
                    heading += " lokalu";

                    if (values == null)
                    {
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.places.Where(b => b.nr_system == id).FirstOrDefault().AllFields();
                        else
                        {
                            values = new string[numberOfFields];

                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values[0] = (db.places.Select(p => p.nr_system).ToList().Max() + 1).ToString();
                        }
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "Nr_system_disabled", values[0], TextBoxMode.Number, 14, 1, false));
                    placeOfFields.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));

                    if (idEnabled)
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            controls.Add(new ControlsP.DropDownListP("field", "kod_lok", db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList(), values[1], idEnabled));

                        controls.Add(new ControlsP.TextBoxP("field", "nr_lok", values[2], TextBoxMode.Number, 3, 1, idEnabled));
                    }
                    else
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            controls.Add(new ControlsP.DropDownListP("field", "kod_lok_disabled", db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList(), values[1], idEnabled));

                        controls.Add(new ControlsP.TextBoxP("field", "nr_lok_disabled", values[2], TextBoxMode.Number, 3, 1, idEnabled));

                        placeOfFields.Controls.Add(new ControlsP.HtmlInputHiddenP("kod_lok", values[1]));
                        placeOfFields.Controls.Add(new ControlsP.HtmlInputHiddenP("nr_lok", values[2]));
                    }

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new ControlsP.DropDownListP("field", "kod_typ", db.typesOfPlace.ToList().Select(t => t.ImportantFields()).ToList(), values[3], globalEnabled));

                    controls.Add(new ControlsP.TextBoxP("field", "adres", values[4], TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres_2", values[5], TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "pow_uzyt", values[6], TextBoxMode.SingleLine, 8, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "pow_miesz", values[7], TextBoxMode.SingleLine, 8, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "udzial", values[8], TextBoxMode.SingleLine, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "dat_od", values[9], TextBoxMode.Date, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "dat_do", values[10], TextBoxMode.Date, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_1", values[11], TextBoxMode.SingleLine, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_2", values[12], TextBoxMode.SingleLine, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_3", values[13], TextBoxMode.SingleLine, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_4", values[14], TextBoxMode.SingleLine, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_5", values[15], TextBoxMode.SingleLine, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_6", values[16], TextBoxMode.SingleLine, 5, 1, globalEnabled));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        controls.Add(new ControlsP.DropDownListP("field", "kod_kuch", db.typesOfKitchen.ToList().Select(t => t.ImportantFields()).ToList(), values[17], globalEnabled));
                        controls.Add(new ControlsP.DropDownListP("field", "nr_kontr", db.tenants.ToList().Select(t => t.ImportantFields()).ToList(), values[18], globalEnabled));
                    }
                    
                    break;
            }

            placeOfFields.Controls.Add(new LiteralControl("<h2>"+heading+"</h2>"));
            placeOfFields.Controls.Add(new ControlsP.HtmlInputHiddenP("action", action.ToString()));
            placeOfFields.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));
            
            for (int i = 0; i < controls.Count; i++)
            {
                placeOfFields.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                placeOfFields.Controls.Add(new ControlsP.LabelP("fieldLabel", controls[i].ID, labels[i]));
                placeOfFields.Controls.Add(new LiteralControl("<br />"));
                placeOfFields.Controls.Add(controls[i]);
                placeOfFields.Controls.Add(new LiteralControl("</div>"));
            }

            foreach (ControlsP.ButtonP button in buttons)
                placeOfButtons.Controls.Add(button);
        }
    }
}