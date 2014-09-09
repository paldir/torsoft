using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class RecordValidation : System.Web.UI.Page
    {
        EnumP.Table table;
        EnumP.Action action;
        int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] record = null;
            string validationResult = null;
            string dbWriteResult = null;
            table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("table"))]);
            action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("action"))]);

            if (action != EnumP.Action.Dodaj)
            {
                id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("id"))]);

                form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", id.ToString()));
            }

            form.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("action", action.ToString()));

            switch (table)
            {
                case EnumP.Table.Buildings:
                    this.Title = "Edycja budynku";
                    DataAccess.Building building;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("il_miesz"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("sp_rozl"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("adres"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("adres_2"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("udzial_w_k"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("uwagi"))]
                    };

                    validationResult = DataAccess.Building.Validate(action, record);

                    if (validationResult == String.Empty)
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        building = new DataAccess.Building();

                                        building.Set(record);
                                        db.buildings.Add(building);
                                        db.SaveChanges();

                                        dbWriteResult = "Budynek dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać budynku!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        building = db.buildings.Where(b => b.kod_1 == id).FirstOrDefault();

                                        building.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Budynek zaktualizowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować budynku!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        building = db.buildings.Where(b => b.kod_1 == id).FirstOrDefault();

                                        db.buildings.Remove(building);
                                        db.SaveChanges();

                                        dbWriteResult = "Budynek usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć budynku!"; }
                                    break;
                            }
                        }
                    }
                    break;
                case EnumP.Table.Places:
                    this.Title = "Edycja lokalu";
                    DataAccess.Place place;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("kod_lok"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("nr_lok"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("kod_typ"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("adres"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("adres_2"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("pow_uzyt"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("pow_miesz"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("udzial"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("dat_od"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("dat_do"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("p_1"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("p_2"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("p_3"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("p_4"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("p_5"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("p_6"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("kod_kuch"))]
                    };

                    validationResult = DataAccess.Place.Validate(action, record);

                    if (validationResult == String.Empty)
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        place = new DataAccess.Place();

                                        place.Set(record);
                                        db.places.Add(place);
                                        db.SaveChanges();

                                        dbWriteResult = "Lokal dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać lokalu!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        place = db.places.Where(p => p.nr_system == id).FirstOrDefault();

                                        place.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Lokal wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować lokalu!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        place = db.places.Where(p => p.nr_system == id).FirstOrDefault();

                                        db.places.Remove(place);
                                        db.SaveChanges();

                                        dbWriteResult = "Lokal usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć lokalu!"; }
                                    break;
                            }
                        }
                    }
                    break;
            }

            form.Controls.Add(new LiteralControl(validationResult));

            if (dbWriteResult != null)
                form.Controls.Add(new LiteralControl(dbWriteResult + "<br />"));

            if (validationResult != String.Empty || (dbWriteResult != null && dbWriteResult.Last() == '!'))
            {
                form.Controls.Add(new ControlsP.ButtonP("button", "Repair", "Popraw", "Record.aspx"));
                form.Controls.Add(new ControlsP.ButtonP("button", "Cancel", "Anuluj", "List.aspx"));

                Session["values"] = record;
            }
            else
                form.Controls.Add(new ControlsP.ButtonP("button", "Back", "Powrót", "List.aspx"));
        }
    }
}