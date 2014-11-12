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

        bool sortable
        {
            get
            {
                if (ViewState["sortable"] == null)
                    return true;

                return (bool)ViewState["sortable"];
            }
            set { ViewState["sortable"] = value; }
        }

        List<int> indexesOfNumericColumns
        {
            get
            {
                if (ViewState["indexesOfNumericColumns"] == null)
                    return new List<int>();

                return (List<int>)ViewState["indexesOfNumericColumns"];
            }
            set { ViewState["indexesOfNumericColumns"] = value; }
        }

        List<int> indexesOfColumnsWithSummary
        {
            get
            {
                if (ViewState["indexesOfColumnsWithSummary"] == null)
                    return new List<int>();

                return (List<int>)ViewState["indexesOfColumnsWithSummary"];
            }
            set { ViewState["indexesOfColumnsWithSummary"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
            string postBackUrl = "Record.aspx";
            string heading = null;
            List<string[]> subMenu = null;
            sortable = true;
            int id = -1;

            if (Request.Params["id"] != null)
                id = Convert.ToInt16(Request.Params["id"]);

            switch (table)
            {
                case EnumP.Table.InactivePlaces:
                case EnumP.Table.InactiveTenants:
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));

                    break;

                case EnumP.Table.ReceivablesByTenants:
                case EnumP.Table.AllReceivablesOfTenant:
                case EnumP.Table.NotPastReceivablesOfTenant:
                case EnumP.Table.ReceivablesAndTurnoversOfTenant:

                    break;

                default:
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "addaction", "Dodaj", postBackUrl));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "editaction", "Edytuj", postBackUrl));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "deleteaction", "Usuń", postBackUrl));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "browseaction", "Przeglądaj", postBackUrl));

                    break;
            }

            DataAccess.Czynsze_Entities db;

            switch (table)
            {
                case EnumP.Table.Buildings:
                    heading = "Budynki";
                    headers = new string[] { "Kod", "Adres", "Adres cd." };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.buildings.OrderBy(b => b.kod_1).ToList().Select(b => b.ImportantFields()).ToList();

                    break;

                case EnumP.Table.Places:
                case EnumP.Table.InactivePlaces:
                    heading = "Lokale";
                    headers = new string[] { "Kod budynku", "Numer lokalu", "Typ lokalu", "Powierzchnia użytkowa", "Nazwisko", "Imię" };
                    indexesOfNumericColumns = new List<int>() { 1, 2, 4 };
                    List<DataAccess.Place> places = null;

                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "moveaction", "Przenieś", postBackUrl));

                    switch (table)
                    {
                        case EnumP.Table.Places:
                            heading += " (aktywne)";
                            subMenu = new List<string[]>()
                            {
                                new string[]
                                {
                                    "Wydruki",
                                    "<a href='ReportConfiguration.aspx?"+EnumP.Report.PlacesInEachBuilding+"report=#'>Lokale w budynkach</a>",
                                    "<a href='#'>Kolejny wydruk</a>",
                                    "<a href='#'>I jeszcze jeden</a>"
                                }
                            };

                            if (!IsPostBack)
                                using (db = new DataAccess.Czynsze_Entities())
                                    places = db.places.ToList().Cast<DataAccess.Place>().ToList();

                            break;

                        case EnumP.Table.InactivePlaces:
                            heading += " (nieaktywne)";

                            if (!IsPostBack)
                                using (db = new DataAccess.Czynsze_Entities())
                                    places = db.inactivePlaces.ToList().Cast<DataAccess.Place>().ToList();

                            break;
                    }

                    if (places != null)
                        rows = places.OrderBy(p => p.kod_lok).ThenBy(p => p.nr_lok).Select(p => p.ImportantFields()).ToList();

                    break;

                case EnumP.Table.Tenants:
                case EnumP.Table.InactiveTenants:
                    heading = "Najemcy";
                    headers = new string[] { "Numer kontrolny", "Nazwisko", "Imię", "Adres", "Adres cd." };
                    indexesOfNumericColumns = new List<int>() { 1 };
                    List<DataAccess.Tenant> tenants = null;

                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("mainTableButton", "moveaction", "Przenieś", postBackUrl));

                    switch (table)
                    {
                        case EnumP.Table.Tenants:
                            heading += " (aktywni)";

                            if (!IsPostBack)
                                using (db = new DataAccess.Czynsze_Entities())
                                    tenants = db.tenants.ToList().Cast<DataAccess.Tenant>().ToList();

                            break;

                        case EnumP.Table.InactiveTenants:
                            heading += " (nieaktywni)";

                            if (!IsPostBack)
                                using (db = new DataAccess.Czynsze_Entities())
                                    tenants = db.inactiveTenants.ToList().Cast<DataAccess.Tenant>().ToList();

                            break;
                    }

                    if (tenants != null)
                        rows = tenants.OrderBy(t => t.nazwisko).ThenBy(t => t.imie).Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.RentComponents:
                    heading = "Składniki opłat";
                    headers = new string[] { "Numer", "Nazwa", "Sposób naliczania", "Typ", "Stawka zł" };
                    indexesOfNumericColumns = new List<int>() { 1, 5 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.rentComponents.OrderBy(c => c.nr_skl).ToList().Select(c => c.ImportantFields()).ToList();

                    break;

                case EnumP.Table.Communities:
                    heading = "Wspólnoty";
                    headers = new string[] { "Kod", "Nazwa wspólnoty", "Il. bud.", "Il. miesz." };
                    indexesOfNumericColumns = new List<int>() { 1, 3, 4 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.communities.OrderBy(c => c.kod).ToList().Select(c => c.ImportantFields()).ToList();

                    break;

                case EnumP.Table.TypesOfPlace:
                    heading = "Typy lokali";
                    headers = new string[] { "Kod", "Typ lokalu" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.typesOfPlace.OrderBy(t => t.kod_typ).ToList().Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.TypesOfKitchen:
                    heading = "Rodzaje kuchni";
                    headers = new string[] { "Kod", "Rodzaj kuchni" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.typesOfKitchen.OrderBy(t => t.kod_kuch).ToList().Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.TypesOfTenant:
                    heading = "Rodzaje najemców";
                    headers = new string[] { "Kod", "Rodzaj najemcy" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.typesOfTenant.OrderBy(t => t.kod_najem).ToList().Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.Titles:
                    heading = "Tytuły prawne do lokali";
                    headers = new string[] { "Kod", "Tytuł prawny" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.titles.OrderBy(t => t.kod_praw).ToList().Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.TypesOfPayment:
                    heading = "Rodzaje wpłat i wypłat";
                    headers = new string[] { "Kod", "Rodzaj wpłaty lub wypłaty", "Sposób rozliczania", "Odsetki", "NO" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.typesOfPayment.OrderBy(t => t.kod_wplat).ToList().Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.GroupsOfRentComponents:
                    heading = "Grupy składników czynszu";
                    headers = new string[] { "Kod", "Nazwa grupy" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.groupsOfRentComponents.OrderBy(g => g.kod).ToList().Select(t => t.ImportantFields()).ToList();

                    break;

                case EnumP.Table.FinancialGroups:
                    heading = "Grupy finansowe";
                    headers = new string[] { "Kod", "Konto", "Nazwa grupy" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.financialGroups.OrderBy(g => g.kod).ToList().Select(g => g.ImportantFields()).ToList();

                    break;

                case EnumP.Table.VatRates:
                    heading = "Stawki VAT";
                    headers = new string[] { "Oznaczenie stawki", "Symbol fiskalny" };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.vatRates.OrderBy(r => r.symb_fisk).ToList().Select(r => r.ImportantFields()).ToList();

                    break;

                case EnumP.Table.Attributes:
                    heading = "Cechy obiektów";
                    headers = new string[] { "Kod", "Nazwa", "N/C", "L.", "N.", "B.", "Wsp." };
                    indexesOfNumericColumns = new List<int>() { 1 };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.attributes.OrderBy(a => a.kod).ToList().Select(a => a.ImportantFields()).ToList();

                    break;

                case EnumP.Table.Users:
                    heading = "Użytkownicy";
                    headers = new string[] { "Symbol", "Nazwisko", "Imię" };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.users.OrderBy(u => u.symbol).ToList().Select(u => u.ImportantFields()).ToList();

                    break;

                case EnumP.Table.ReceivablesByTenants:
                    heading = "Należności i obroty według najemców";
                    headers = new string[] { "Nazwisko", "Imię", "Kod", "Nr", "Adres najemcy", "Adres lokalu" };
                    sortable = false;
                    indexesOfNumericColumns = new List<int>() { 3, 4 };
                    subMenu = new List<string[]>()
                    {
                        new string[]
                        {
                            "Należności",
                            "<a href=\"javascript: Redirect('List.aspx?table=AllReceivablesOfTenant')\">Wszystkie</a>",
                            "<a href=\"javascript: Redirect('List.aspx?table=NotPastReceivablesOfTenant')\">Nieprzeterminowane</a>"
                        },
                        new string[]
                        {
                            "Rozliczenia",
                            "<a href=\"javascript: Redirect('List.aspx?table=ReceivablesAndTurnoversOfTenant')\">Należności i obroty</a>",
                            "<a href='#'>Zaległości płatnicze</a>",
                        }
                    };

                    if (!IsPostBack)
                        using (db = new DataAccess.Czynsze_Entities())
                            rows = db.tenants.OrderBy(t => t.nazwisko).ThenBy(t => t.imie).ToList().Select(t => t.WithPlace()).ToList();

                    ControlsP.RadioButtonListP list = new ControlsP.RadioButtonListP("list", "by", new List<string> { "wg nazwiska", "wg kodu lokalu" }, new List<string> { "nazwisko", "kod" }, "nazwisko", true, true);
                    list.SelectedIndexChanged += list_SelectedIndexChanged;

                    placeOfMainTableButtons.Controls.Add(list);
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("button", "saldo", "Saldo", "#"));

                    break;

                case EnumP.Table.AllReceivablesOfTenant:
                case EnumP.Table.NotPastReceivablesOfTenant:
                    headers = new string[] { "Kwota należności", "Termin zapłaty", "Uwagi", "Kod lokalu", "Nr lokalu" };
                    sortable = false;
                    indexesOfNumericColumns = new List<int>() { 1, 4, 5 };
                    indexesOfColumnsWithSummary = new List<int>() { 1 };

                    using (db = new DataAccess.Czynsze_Entities())
                    {
                        DataAccess.Tenant tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);
                        heading = "Należności najemcy " + tenant.nazwisko + " " + tenant.imie;
                        List<DataAccess.ReceivableFor14> receivables = db.receivablesFor14.Where(r => r.nr_kontr == id).OrderBy(r => r.data_nal).ToList();

                        switch (table)
                        {
                            case EnumP.Table.AllReceivablesOfTenant:
                                rows = receivables.Select(r => r.ImportantFields()).ToList();

                                break;

                            case EnumP.Table.NotPastReceivablesOfTenant:
                                heading += " (nieprzeterminowane)";
                                rows = receivables.Where(r => Convert.ToDateTime(r.data_nal) >= Hello.Date).Select(r => r.ImportantFields()).ToList();

                                break;
                        }
                    }

                    break;

                case EnumP.Table.ReceivablesAndTurnoversOfTenant:
                    headers = new string[] { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };
                    sortable = false;
                    indexesOfNumericColumns = new List<int>() { 1, 2 };
                    //indexesOfColumnsWithSummary = new List<int>() { 1, 2 };
                    float wnAmount, maAmount, pastReceivables;

                    using (db = new DataAccess.Czynsze_Entities())
                    {
                        DataAccess.Tenant tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);
                        List<DataAccess.Receivable> receivables = null;
                        List<DataAccess.Turnover> turnovers = null;
                        heading = "Należności  i obroty najemcy " + tenant.nazwisko + " " + tenant.imie;

                        switch (Hello.CurrentSet)
                        {
                            case EnumP.SettlementTable.Czynsze:
                                receivables = db.receivablesFor14.ToList().Cast<DataAccess.Receivable>().ToList();
                                turnovers = db.turnoversFor14.ToList().Cast<DataAccess.Turnover>().ToList();

                                break;

                            case EnumP.SettlementTable.SecondSet:
                                receivables = db.receivablesFor14From2ndSet.ToList().Cast<DataAccess.Receivable>().ToList();
                                turnovers = db.turnoversFor14From2ndSet.ToList().Cast<DataAccess.Turnover>().ToList();

                                break;

                            case EnumP.SettlementTable.ThirdSet:
                                receivables = db.receivablesFor14From3rdSet.ToList().Cast<DataAccess.Receivable>().ToList();
                                turnovers = db.turnoversFor14From3rdSet.ToList().Cast<DataAccess.Turnover>().ToList();

                                break;

                        }

                        rows = receivables.Where(r => r.nr_kontr == id).ToList().Select(r => r.ImportantFieldsForReceivablesAndTurnoversOfTenant()).ToList();

                        rows.AddRange(turnovers.Where(t => t.nr_kontr == id).ToList().Select(t => t.ImportantFields()).ToList());

                        rows = rows.OrderBy(r => DateTime.Parse(r[3])).ToList();
                        wnAmount = rows.Sum(r => (r[1] == String.Empty) ? 0 : Convert.ToSingle(r[1]));
                        maAmount = rows.Sum(r => (r[2] == String.Empty) ? 0 : Convert.ToSingle(r[2]));
                        pastReceivables = receivables.Where(r => r.nr_kontr == id && Convert.ToDateTime(r.data_nal) < Hello.Date).Sum(r => r.kwota_nal);
                    }

                    string summary = @"
                        <table class='additionalTable'>
                            <tr>
                                <td>Suma Wn: </td>
                                <td class='numericTableCell'>" + String.Format("{0:N2}", wnAmount) + @"</td>                                                                        
                            </tr>
                            <tr>
                                <td>Suma Ma: </td>
                                <td class='numericTableCell'>" + String.Format("{0:N2}", maAmount) + @"</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><hr /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class='numericTableCell'>" + String.Format("{0:N2}", maAmount - wnAmount) + @"</td>
                            </tr>
                        </table>
                        <table class='additionalTable'>
                            <tr>
                                <td>Należności przeterminowane: </td>
                                <td class='numericTableCell'>" + String.Format("{0:N2}", pastReceivables) + @"</td>                                                                        
                            </tr>
                            <tr>
                                <td>Suma Ma: </td>
                                <td class='numericTableCell'>" + String.Format("{0:N2}", maAmount) + @"</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><hr /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class='numericTableCell'>" + String.Format("{0:N2}", maAmount - pastReceivables) + @"</td>
                            </tr>
                        </table>";

                    placeUnderMainTable.Controls.Add(new LiteralControl(summary));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("button", EnumP.Report.MonthlySumOfComponent + "report", "Sumy miesięczne składnika", "ReportConfiguration.aspx"));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("button", EnumP.Report.ReceivablesAndTurnoversOfTenant + "report", "Wydruk", "ReportConfiguration.aspx"));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("button", EnumP.Report.MonthlyAnalysisOfReceivablesAndTurnovers + "report", "Analiza miesięczna", "ReportConfiguration.aspx"));
                    placeOfMainTableButtons.Controls.Add(new ControlsP.ButtonP("button", EnumP.Report.DetailedAnalysisOfReceivablesAndTurnovers + "report", "Analiza szczegółowa", "ReportConfiguration.aspx"));

                    break;
            }

            placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            placeOfMainTableButtons.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));

            if (subMenu != null)
            {
                ControlsP.HtmlGenericControlP superUl = new ControlsP.HtmlGenericControlP("ul", "superMenu");

                foreach (string[] items in subMenu)
                {
                    ControlsP.HtmlGenericControlP superLi = new ControlsP.HtmlGenericControlP("li", String.Empty);
                    ControlsP.HtmlGenericControlP subUl = new ControlsP.HtmlGenericControlP("ul", "subMenu");

                    superLi.Controls.Add(new LiteralControl(items[0]));

                    for (int i = 1; i < items.Length; i++)
                    {
                        ControlsP.HtmlGenericControlP subLi = new ControlsP.HtmlGenericControlP("li", String.Empty);

                        subLi.Controls.Add(new LiteralControl(items[i]));
                        subUl.Controls.Add(subLi);
                    }

                    superLi.Controls.Add(subUl);
                    superUl.Controls.Add(superLi);
                }

                placeOfMainTableButtons.Controls.Add(superUl);
            }

            Title = heading;
            Session["values"] = null;

            switch (table)
            {
                case EnumP.Table.Places:
                case EnumP.Table.InactivePlaces:
                    Hello.SiteMapPath = new List<string>() { "Kartoteki", "Lokale" };

                    break;

                case EnumP.Table.Tenants:
                case EnumP.Table.InactiveTenants:
                    Hello.SiteMapPath = new List<string>() { "Kartoteki", "Najemcy" };

                    break;

                case EnumP.Table.Buildings:
                case EnumP.Table.Communities:
                case EnumP.Table.RentComponents:
                    Hello.SiteMapPath = new List<string>() { "Kartoteki" };

                    break;

                case EnumP.Table.ReceivablesByTenants:
                    Hello.SiteMapPath = new List<string>() { "Rozliczenia finansowe", "Należności i obroty" };

                    break;

                case EnumP.Table.AllReceivablesOfTenant:
                case EnumP.Table.NotPastReceivablesOfTenant:
                case EnumP.Table.ReceivablesAndTurnoversOfTenant:
                    if (Hello.SiteMapPath.Count > 2)
                    {
                        Hello.SiteMapPath.RemoveRange(3, Hello.SiteMapPath.Count - 3);
                        
                        string node = Hello.SiteMapPath[2].Insert(0, "<a href=\"javascript: Load('List.aspx?table=" + EnumP.Table.ReceivablesByTenants + "')\">") + "</a>";

                        if (Hello.SiteMapPath.IndexOf(node) == -1)
                            Hello.SiteMapPath[2] = node;
                    }

                    break;

                case EnumP.Table.TypesOfPlace:
                case EnumP.Table.TypesOfKitchen:
                case EnumP.Table.TypesOfTenant:
                case EnumP.Table.Titles:
                case EnumP.Table.TypesOfPayment:
                case EnumP.Table.GroupsOfRentComponents:
                case EnumP.Table.FinancialGroups:
                case EnumP.Table.VatRates:
                case EnumP.Table.Attributes:
                    Hello.SiteMapPath = new List<string>() { "Słowniki" };

                    break;

                case EnumP.Table.Users:
                    Hello.SiteMapPath = new List<string>() { "Administracja" };

                    break;
            }

            if (Hello.SiteMapPath.IndexOf(heading) == -1)
                Hello.SiteMapPath.Add(heading);

            //
            //CXP PART
            //
            switch (table)
            {
                case EnumP.Table.Places:
                case EnumP.Table.InactivePlaces:
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
            ControlsP.TableP mainTable = new ControlsP.TableP("mainTable", rows, headers, sortable, String.Empty, indexesOfNumericColumns, indexesOfColumnsWithSummary);

            if (sortable)
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

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList list = (RadioButtonList)sender;

            switch (list.SelectedValue)
            {
                case "nazwisko":
                    rows = rows.OrderBy(r => r[1]).ThenBy(r => r[2]).ToList();

                    break;


                case "kod":
                    rows = rows.OrderBy(r => Convert.ToSingle(r[3])).ThenBy(r => Convert.ToSingle(r[4])).ToList();

                    break;
            }

            CreateMainTable();
        }
    }
}