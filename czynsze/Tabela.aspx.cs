using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

namespace czynsze
{
    public partial class Tabela : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            string postBackUrl="Rekord.aspx";
            placeOfMainTableButtons.Controls.Add(new Controls.ButtonP("mainTableButton", "addaction", "Dodaj", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new Controls.ButtonP("mainTableButton", "editaction", "Edytuj", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new Controls.ButtonP("mainTableButton", "deleteaction", "Usuń", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new Controls.ButtonP("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));
            
            using (var db = new DataAccess.Czynsze_Entities())
            {
                placeOfMainTable.Controls.Add(new Controls.TableP("mainTable", db.buildings.ToList().Select(b => b.ImportantFields()).ToList(), new string[] { "Kod", "Adres", "Adres cd." }));
            }

            placeOfMainTableButtons.Controls.Add(new Controls.HtmlInputHiddenP("table", "buildings"));
        }
    }
}