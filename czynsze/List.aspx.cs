using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

namespace czynsze
{
    public partial class List : System.Web.UI.Page
    {
        EnumP.Table table;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<string[]> rows = null;
            string[] headers = null;

            table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
            string postBackUrl = "Record.aspx";

            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "addaction", "Dodaj", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "editaction", "Edytuj", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "deleteaction", "Usuń", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));


            using (var db = new DataAccess.Czynsze_Entities())
            {
                switch (table)
                {
                    case EnumP.Table.Buildings:
                        this.Title = "Budynki";
                        headers = new string[] { "Kod", "Adres", "Adres cd." };
                        rows = db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList();
                        break;
                    case EnumP.Table.Places:
                        this.Title = "Lokale";
                        headers = new string[] { "Kod budynku", "Numer lokalu", "Typ lokalu", "Powierzchnia użytkowa", "Nazwisko", "Imię" };
                        rows = db.places.ToList().OrderBy(p => p.kod_lok).ThenBy(p => p.nr_lok).Select(p => p.ImportantFields()).ToList();
                        break;
                }
            }

            placeOfMainTable.Controls.Add(new ControlsP.TableP("mainTable", rows, headers));
            placeOfMainTableButtons.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));

            Session["values"] = null;
        }
    }
}