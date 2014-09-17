using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

namespace czynsze.Forms
{
    public partial class List : System.Web.UI.Page
    {
        EnumP.Table table;

        List<string[]> rows
        {
            get
            {
                if (ViewState["rows"] == null)
                    return new List<string[]>();
                else
                    return (List<string[]>)ViewState["rows"];
            }
            set { ViewState["rows"] = value; }
        }

        string[] headers
        {
            get
            {
                if (ViewState["headers"] == null)
                    return new string[0];
                else
                    return (string[])ViewState["headers"];
            }
            set { ViewState["headers"] = value; }
        }

        EnumP.SortOrder sortOrder
        {
            get
            {
                if (ViewState["sortOrder"] == null)
                    return EnumP.SortOrder.Asc;
                else
                    return (EnumP.SortOrder)Enum.Parse(typeof(EnumP.SortOrder), ViewState["sortOrder"].ToString());
            }
            set { ViewState["sortOrder"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
            string postBackUrl = "Record.aspx";
            string heading = null;
            List<string> printingsSubMenu=null;

            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "addaction", "Dodaj", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "editaction", "Edytuj", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "deleteaction", "Usuń", postBackUrl));
            placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));

            DataAccess.Czynsze_Entities db;
            switch (table)
            {
                case EnumP.Table.Buildings:
                    heading = "Budynki";
                    headers = new string[] { "Kod", "Adres", "Adres cd." };

                    //placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("superMenuButton", "report", "Wydruk lokali wg budynków", "ReportConfiguration.aspx?report=PlacesInEachBuilding"));
                    printingsSubMenu = new List<string>()
                    {
                        "<a href='ReportConfiguration.aspx?report="+EnumP.Report.PlacesInEachBuilding.ToString()+"'>Lokale w budynkach</a>",
                        "<a href='#'>Kolejny wydruk</a>",
                        "<a href='#'>I jeszcze jeden</a>"
                    };

                    //placeOfMainTableButtons.Controls.Add(new LiteralControl("<ul class='superMenu'><li>Wydruki<ul class='subMenu'><li><a href='ReportConfiguration.aspx?report=PlacesInEachBuilding'>Lokale w budynkach</a></li><li><a href='#'>Kolejny wydruk</a></li><li><a href='#'>I jeszcze jeden</a></li></ul></li></ul>"));
                    
                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.buildings.OrderBy(b => b.kod_1).ToList().Select(b => b.ImportantFields()).ToList();
                    break;
                case EnumP.Table.Places:
                    heading = "Lokale";
                    headers = new string[] { "Kod budynku", "Numer lokalu", "Typ lokalu", "Powierzchnia użytkowa", "Nazwisko", "Imię" };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.places.OrderBy(p => p.kod_lok).ThenBy(p => p.nr_lok).ToList().Select(p => p.ImportantFields()).ToList();
                    break;
                case EnumP.Table.Tenants:
                    heading = "Najemcy";
                    headers = new string[] { "Numer kontrolny", "Nazwisko", "Imię", "Adres", "Adres cd." };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.tenants.OrderBy(t => t.nazwisko).ThenBy(t => t.imie).ToList().Select(t => t.ImportantFields()).ToList();
                    break;
                case EnumP.Table.RentComponents:
                    heading = "Składniki czynszu";
                    headers = new string[] { "Numer", "Nazwa", "Sposób naliczania", "Typ", "Stawka zł" };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.rentComponents.OrderBy(c => c.nr_skl).ToList().Select(c => c.ImportantFields()).ToList();
                    break;
            }

            placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            placeOfMainTableButtons.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));

            if (printingsSubMenu != null)
            {
                ControlsP.HtmlGenericControlP superUl = new ControlsP.HtmlGenericControlP("ul", "superMenu");
                ControlsP.HtmlGenericControlP superLi = new ControlsP.HtmlGenericControlP("li", String.Empty);
                ControlsP.HtmlGenericControlP subUl = new ControlsP.HtmlGenericControlP("ul", "subMenu");

                superLi.Controls.Add(new LiteralControl("Wydruki"));

                foreach (string item in printingsSubMenu)
                {
                    ControlsP.HtmlGenericControlP subLi = new ControlsP.HtmlGenericControlP("li", String.Empty);

                    subLi.Controls.Add(new LiteralControl(item));
                    subUl.Controls.Add(subLi);
                }

                superLi.Controls.Add(subUl);
                superUl.Controls.Add(superLi);
                placeOfMainTableButtons.Controls.Add(superUl);
            }

            this.Title = heading;
            Session["values"] = null;

            //
            //CXP PART
            //
            switch (table)
            {
                case EnumP.Table.Places:
                    using (db = new DataAccess.Czynsze_Entities())
                    {
                        try
                        {
                            db.Database.ExecuteSqlCommand("DROP TABLE skl_cz_tmp");
                            db.Database.ExecuteSqlCommand("DROP TABLE pliki_tmp");
                        }
                        catch { }
                    }
                    break;
            }
            //
            //TO DUMP BEHIND THE WALL
            //
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateMainTable();
        }

        void CreateMainTable()
        {
            ControlsP.TableP mainTable = new ControlsP.TableP("mainTable", rows, headers, true);

            foreach (TableCell cell in mainTable.Rows[0].Cells)
                ((ControlsP.LinkButtonP)cell.Controls[0]).Click += LinkButtonOfColumn_Click;

            placeOfMainTable.Controls.Clear();
            placeOfMainTable.Controls.Add(mainTable);
        }

        void LinkButtonOfColumn_Click(object sender, EventArgs e)
        {
            int columnNumber = Convert.ToInt16(((ControlsP.LinkButtonP)sender).ID.Replace("column", String.Empty)) + 1;

            switch (sortOrder)
            {
                case EnumP.SortOrder.Asc:
                    try { rows = rows.OrderBy(r => Convert.ToSingle(r[columnNumber])).ToList(); }
                    catch { rows = rows.OrderBy(r => r[columnNumber]).ToList(); }

                    sortOrder = EnumP.SortOrder.Desc;
                    break;
                case EnumP.SortOrder.Desc:
                    try { rows = rows.OrderByDescending(r => Convert.ToSingle(r[columnNumber])).ToList(); }
                    catch { rows = rows.OrderByDescending(r => r[columnNumber]).ToList(); }

                    sortOrder = EnumP.SortOrder.Asc;
                    break;
            }

            CreateMainTable();
        }
    }

}