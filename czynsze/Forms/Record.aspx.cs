using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class Record : Page
    {
        int id;
        Enums.Action action;
        Enums.Table table;

        List<DataAccess.AttributeOfObject> attributesOfObject
        {
            get { return (List<DataAccess.AttributeOfObject>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool globalEnabled = false;
            bool idEnabled = false;
            string[] values = (string[])Session["values"];
            int numberOfFields = 0;
            string[] labels = null;
            string heading = null;
            List<MyControls.Button> buttons = new List<MyControls.Button>();
            List<Control> controls = new List<Control>();
            List<int> columnSwitching = null;
            List<MyControls.HtmlIframe> tabs = null;
            List<MyControls.HtmlInputRadioButton> tabButtons = null;
            List<MyControls.Label> labelsOfTabButtons = null;
            List<Control> preview = null;
            //id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))]);
            id = GetParamValue<int>("id");
            //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("action"))]);
            action = GetParamValue<Enums.Action>("action");
            //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("table"))]);
            table = GetParamValue<Enums.Table>("table");
            string backUrl = "javascript: Load('" + Request.UrlReferrer + "')";

            switch (action)
            {
                case Enums.Action.Dodaj:
                    globalEnabled = idEnabled = true;
                    heading = "Dodawanie";

                    buttons.Add(new MyControls.Button("buttons", "Save", "Zapisz", "RecordValidation.aspx"));
                    buttons.Add(new MyControls.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;

                case Enums.Action.Edytuj:
                    globalEnabled = true;
                    idEnabled = false;
                    heading = "Edycja";

                    buttons.Add(new MyControls.Button("buttons", "Save", "Zapisz", "RecordValidation.aspx"));
                    buttons.Add(new MyControls.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;

                case Enums.Action.Usuń:
                    globalEnabled = idEnabled = false;
                    heading = "Usuwanie";

                    buttons.Add(new MyControls.Button("buttons", "Delete", "Usuń", "RecordValidation.aspx"));
                    buttons.Add(new MyControls.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;

                case Enums.Action.Przeglądaj:
                    globalEnabled = idEnabled = false;
                    heading = "Przeglądanie";

                    buttons.Add(new MyControls.Button("buttons", "Back", "Powrót", backUrl));

                    break;

                case Enums.Action.Przenieś:
                    globalEnabled = idEnabled = false;
                    heading = "Przenoszenie";

                    buttons.Add(new MyControls.Button("buttons", "Move", "Przenieś", "RecordValidation.aspx"));
                    buttons.Add(new MyControls.Button("buttons", "Cancel", "Anuluj", backUrl));

                    break;
            }

            switch (table)
            {
                case Enums.Table.Buildings:
                    Title = "Budynek";
                    numberOfFields = 7;
                    heading += " budynku";
                    columnSwitching = new List<int>() { 0, 6 };
                    labels = new string[] 
                    { 
                        "Kod budynku: ", 
                        "Ilość lokali: ", 
                        "Sposób rozliczania: ", 
                        "Adres: ", 
                        "Adres cd.: ",
                        "Udział w koszt.: ",
                        "Uwagi: " 
                    };

                    tabButtons = new List<MyControls.HtmlInputRadioButton>()
                    {
                        new MyControls.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                        new MyControls.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                    };

                    labelsOfTabButtons = new List<MyControls.Label>()
                    {
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                    };

                    tabs = new List<MyControls.HtmlIframe>()
                    {
                        new MyControls.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enums.AttributeOf.Building+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.buildings.FirstOrDefault(b => b.kod_1 == id).AllFields();
                        else
                            values = new string[numberOfFields];

                        attributesOfObject = new List<DataAccess.AttributeOfObject>();

                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfBuilding);
                    }

                    preview = new List<Control>()
                    {
                        new LiteralControl("Kod budynku: "),
                        new MyControls.Label("previewLabel", String.Empty, values[0], "id_preview"),
                        new LiteralControl("Adres: "),
                        new MyControls.Label("previewLabel", String.Empty, values[3], "adres_preview"),
                        new LiteralControl("Adres cd.: "),
                        new MyControls.Label("previewLabel", String.Empty, values[4], "adres_2_preview")
                    };

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 5, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "idDisabled", values[0], MyControls.TextBox.TextBoxMode.Number, 5, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", id.ToString()));
                    }

                    controls.Add(new MyControls.TextBox("field", "il_miesz", values[1], MyControls.TextBox.TextBoxMode.Number, 3, 1, globalEnabled));
                    controls.Add(new MyControls.RadioButtonList("field", "sp_rozl", new List<string>() { "budynek", "lokale" }, new List<string>() { "0", "1" }, values[2], globalEnabled, false));
                    controls.Add(new MyControls.TextBox("field", "adres", values[3], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "adres_2", values[4], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "udzial_w_k", values[5], MyControls.TextBox.TextBoxMode.Float, 6, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "uwagi", values[6], MyControls.TextBox.TextBoxMode.MultiLine, 420, 6, globalEnabled));

                    break;

                case Enums.Table.Places:
                case Enums.Table.InactivePlaces:

                    Title = "Lokal";
                    numberOfFields = 22;
                    heading += " lokalu";

                    if (table == Enums.Table.InactivePlaces)
                    {
                        Title = "Lokal (nieaktywny)";
                        heading += " (nieaktywnego)";
                    }

                    columnSwitching = new List<int> { 0, 5, 10, 17 };
                    labels = new string[] 
                    { 
                        //"Nr system: ", 
                        "Budynek: ", 
                        "Nr lokalu: ",
                        "Typ: ", 
                        "Adres: ",
                        "Adres cd.: ",
                        "Powierzchnia użytkowa: ", 
                        "Powierzchnia mieszkalna: ", 
                        "Udział: ", 
                        "Początek zakresu dat: ",
                        "Koniec zakresu dat: ", 
                        "Powierzchnia I pokoju: ",
                        "Powierzchnia II pokoju: ", 
                        "Powierzchnia III pokoju: ", 
                        "Powierzchnia IV pokoju: ", 
                        "Powierzchnia V pokoju: ",
                        "Powierzchnia VI pokoju: ", 
                        "Typ kuchni: ", 
                        "Najemca: ", 
                        "Ilość osób: ", 
                        "Tytuł prawny do lokalu: ", 
                        "Uwagi: " 
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            {
                                if (table == Enums.Table.Places)
                                    values = db.places.FirstOrDefault(b => b.nr_system == id).AllFields();
                                else
                                    values = db.inactivePlaces.FirstOrDefault(b => b.nr_system == id).AllFields();
                            }
                        else
                        {
                            values = new string[numberOfFields];

                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            {
                                List<DataAccess.Place> places = db.places.ToList().Cast<DataAccess.Place>().ToList().Concat(db.inactivePlaces.ToList().Cast<DataAccess.InactivePlace>().ToList()).ToList();

                                if (places.Count > 0)
                                    values[0] = (places.Max(p => p.nr_system) + 1).ToString();
                                else
                                    values[0] = "0";
                            }

                            values[1] = values[2] = "0";
                        }

                        attributesOfObject = new List<DataAccess.AttributeOfObject>();

                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfPlace);
                    }

                    tabButtons = new List<MyControls.HtmlInputRadioButton>()
                    {
                        new MyControls.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                        new MyControls.HtmlInputRadioButton("tabRadio", "skladnikiCzynszu", "tabRadios", "skladnikiCzynszu", false),
                        new MyControls.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                        new MyControls.HtmlInputRadioButton("tabRadio", "dokumenty", "tabRadios", "dokumenty", false)
                    };

                    labelsOfTabButtons = new List<MyControls.Label>()
                    {
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(1).ID, "Składniki czynszu", String.Empty),
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(2).ID, "Cechy", String.Empty),
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(3).ID, "Dokumenty", String.Empty)
                    };

                    preview = new List<Control>()
                    {
                        new LiteralControl("Nr budynku: "),
                        new MyControls.Label("previewLabel", String.Empty, values[1], "kod_lok_preview"),
                        new LiteralControl("Nr lokalu: "),
                        new MyControls.Label("previewLabel", String.Empty, values[2], "nr_lok_preview"),
                        new LiteralControl("Adres: "),
                        new MyControls.Label("previewLabel", String.Empty, values[4], "adres_preview"),
                        new LiteralControl("Adres cd.: "),
                        new MyControls.Label("previewLabel", String.Empty, values[5], "adres_2_preview")
                    };

                    //
                    //CXP PART
                    //
                    string parentAction;

                    switch (action)
                    {
                        case Enums.Action.Dodaj:
                            parentAction = "add";

                            break;

                        case Enums.Action.Edytuj:
                            parentAction = "edit";

                            break;

                        case Enums.Action.Usuń:
                            parentAction = "delete";

                            break;

                        default:
                            parentAction = "browse";

                            break;
                    }
                    //
                    //TO DUMP BEHIND THE WALL
                    //

                    tabs = new List<MyControls.HtmlIframe>()
                    {
                        new MyControls.HtmlIframe("tab", "skladnikiCzynszu_tab", "/czynsze1/SkladnikiCzynszuLokalu.cxp?parentAction="+parentAction+"&kod_lok="+values[1]+"&nr_lok="+values[2], "hidden"),
                        new MyControls.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enums.AttributeOf.Place+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden"),
                        new MyControls.HtmlIframe("tab", "dokumenty_tab", "/czynsze1/PlikiNajemcy.cxp?parentAction="+parentAction+"&nr_system="+values[0], "hidden")
                    };

                    //controls.Add(new MyControls.TextBoxP("field", "Nr_system_disabled", values[0], MyControls.TextBoxP.TextBoxMode.Number, 14, 1, false));
                    form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));

                    if (idEnabled)
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            controls.Add(new MyControls.DropDownList("field", "kod_lok", db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList(), values[1], idEnabled, false));

                        controls.Add(new MyControls.TextBox("field", "nr_lok", values[2], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    }
                    else
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            controls.Add(new MyControls.DropDownList("field", "kod_lok_disabled", db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList(), values[1], idEnabled, false));

                        controls.Add(new MyControls.TextBox("field", "nr_lok_disabled", values[2], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));

                        form.Controls.Add(new MyControls.HtmlInputHidden("kod_lok", values[1]));
                        form.Controls.Add(new MyControls.HtmlInputHidden("nr_lok", values[2]));
                    }

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new MyControls.DropDownList("field", "kod_typ", db.typesOfPlace.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[3], globalEnabled, false));

                    controls.Add(new MyControls.TextBox("field", "adres", values[4], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "adres_2", values[5], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "pow_uzyt", values[6], MyControls.TextBox.TextBoxMode.Float, 8, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "pow_miesz", values[7], MyControls.TextBox.TextBoxMode.Float, 8, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "udzial", values[8], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "dat_od", values[9], MyControls.TextBox.TextBoxMode.Date, 10, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "dat_do", values[10], MyControls.TextBox.TextBoxMode.Date, 10, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "p_1", values[11], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "p_2", values[12], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "p_3", values[13], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "p_4", values[14], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "p_5", values[15], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "p_6", values[16], MyControls.TextBox.TextBoxMode.Float, 5, 1, globalEnabled));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        controls.Add(new MyControls.DropDownList("field", "kod_kuch", db.typesOfKitchen.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[17], globalEnabled, false));
                        controls.Add(new MyControls.DropDownList("field", "nr_kontr", db.tenants.OrderBy(t => t.nazwisko).ToList().Select(t => t.ImportantFields().ToList().GetRange(1, 4).ToArray()).ToList(), values[18], globalEnabled, true));
                    }

                    controls.Add(new MyControls.TextBox("field", "il_osob", values[19], MyControls.TextBox.TextBoxMode.Number, 3, 1, globalEnabled));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new MyControls.DropDownList("field", "kod_praw", db.titles.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[20], globalEnabled, false));

                    controls.Add(new MyControls.TextBox("field", "uwagi", values[21], MyControls.TextBox.TextBoxMode.MultiLine, 240, 4, globalEnabled));

                    //
                    //CXP PART
                    //
                    try
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case Enums.Action.Dodaj:
                                    db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE 1=2");
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE 1=2");

                                    break;

                                default:
                                    db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE kod_lok=" + values[1] + " AND nr_lok=" + values[2]);
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE nr_system=" + values[0]);

                                    break;
                            }
                    }
                    catch { }
                    //
                    //TO DUMP BEHIND THE WALL
                    //

                    break;

                case Enums.Table.Tenants:
                case Enums.Table.InactiveTenants:
                    Title = "Najemca";
                    numberOfFields = 12;
                    heading += " najemcy";
                    columnSwitching = new List<int> { 0, 6 };
                    labels = new string[] 
                    { 
                        "Nr kontrolny: ", 
                        "Najemca: ", 
                        "Nazwisko: ",
                        "Imię: ", 
                        "Adres: ", 
                        "Adres cd.: ", 
                        "Numer dowodu osobistego: ", 
                        "Pesel: ", 
                        "Zakład pracy: ",
                        "Login/e-mail: ", 
                        "Hasło: ", 
                        "Uwagi: " 
                    };

                    if (table == Enums.Table.InactiveTenants)
                    {
                        Title = "Najemca (nieaktywny)";
                        heading += " (nieaktywnego)";
                    }

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                switch (table)
                                {
                                    case Enums.Table.Tenants:
                                        values = db.tenants.FirstOrDefault(t => t.nr_kontr == id).AllFields();

                                        break;

                                    case Enums.Table.InactiveTenants:
                                        values = db.inactiveTenants.FirstOrDefault(t => t.nr_kontr == id).AllFields();

                                        break;
                                }
                        else
                        {
                            values = new string[numberOfFields];

                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            {
                                List<DataAccess.Tenant> tenants = db.tenants.ToList().Cast<DataAccess.Tenant>().ToList().Concat(db.inactiveTenants.ToList().Cast<DataAccess.Tenant>().ToList()).ToList();

                                if (tenants.Count > 0)
                                    values[0] = (tenants.Max(t => t.nr_kontr) + 1).ToString();
                                else
                                    values[0] = "1";
                            }
                        }

                        attributesOfObject = new List<DataAccess.AttributeOfObject>();

                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfTenant);
                    }

                    preview = new List<Control>()
                    {
                        new LiteralControl("Numer kontrolny: "),
                        new MyControls.Label("previewLabel", String.Empty, values[0], "id_preview"),
                        new LiteralControl("Nazwisko: "),
                        new MyControls.Label("previewLabel", String.Empty, values[2], "nazwisko_preview"),
                        new LiteralControl("Imię: "),
                        new MyControls.Label("previewLabel", String.Empty, values[3], "imie_preview"),
                        new LiteralControl("Adres: "),
                        new MyControls.Label("previewLabel", String.Empty, values[4], "adres_1_preview"),
                        new LiteralControl("Adres cd.: "),
                        new MyControls.Label("previewLabel", String.Empty, values[5], "adres_2_preview")
                    };

                    tabButtons = new List<MyControls.HtmlInputRadioButton>()
                    {
                        new MyControls.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                        new MyControls.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                    };

                    labelsOfTabButtons = new List<MyControls.Label>()
                    {
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                    };

                    tabs = new List<MyControls.HtmlIframe>()
                    {
                        new MyControls.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enums.AttributeOf.Tenant+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                    };

                    controls.Add(new MyControls.TextBox("field", "nr_kontr_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 6, 1, false));
                    placeOfButtons.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new MyControls.DropDownList("field", "kod_najem", db.typesOfTenant.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[1], globalEnabled, false));

                    controls.Add(new MyControls.TextBox("field", "nazwisko", values[2], MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "imie", values[3], MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "adres_1", values[4], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "adres_2", values[5], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nr_dow", values[6], MyControls.TextBox.TextBoxMode.SingleLine, 9, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "pesel", values[7], MyControls.TextBox.TextBoxMode.SingleLine, 11, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nazwa_z", values[8], MyControls.TextBox.TextBoxMode.SingleLine, 40, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "e_mail", values[9], MyControls.TextBox.TextBoxMode.SingleLine, 40, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "l__has", values[10], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "uwagi", values[11], MyControls.TextBox.TextBoxMode.MultiLine, 120, 2, globalEnabled));

                    break;

                case Enums.Table.RentComponents:
                    Title = "Składnik opłat";
                    numberOfFields = 19;
                    heading += " składnika opłat";
                    columnSwitching = new List<int> { 0, 6, 9 };
                    labels = new string[]
                    {
                        "Nr składnika: ",
                        "Nazwa: ",
                        "Rodzaj ewidencji: ",
                        "Sposób naliczania: ",
                        "Stawka: ",
                        "Stawka do korespondencji: ",
                        "Typ składnika: ",
                        "Początek okresu naliczania: ",
                        "Koniec okresu naliczania: ",
                        "Przedziały za osobę (dotyczy sposoby naliczania &quot;za osobę - przedziały&quot): "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.rentComponents.FirstOrDefault(c => c.nr_skl == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "nazwa", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.DropDownList("field", "rodz_e", new List<string[]> { new string[] { "1", "dziennik komornego" }, new string[] { "2", "wpłaty" }, new string[] { "3", "zmniejszenia" }, new string[] { "4", "zwiększenia" } }, values[2], globalEnabled, false));
                    controls.Add(new MyControls.DropDownList("field", "s_zaplat", new List<string[]> { new string[] { "1", "za m2 pow. użytkowej" }, new string[] { "2", "za określoną ilość" }, new string[] { "3", "za osobę" }, new string[] { "4", "za lokal" }, new string[] { "5", "za ilość dni w miesiącu" }, new string[] { "6", "za osobę - przedziały" } }, values[3], globalEnabled, false));
                    controls.Add(new MyControls.TextBox("field", "stawka", values[4], MyControls.TextBox.TextBoxMode.Float, 10, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "stawka_inf", values[5], MyControls.TextBox.TextBoxMode.Float, 10, 1, globalEnabled));
                    controls.Add(new MyControls.DropDownList("field", "typ_skl", new List<string[]> { new string[] { "0", "stały" }, new string[] { "1", "zmienny" } }, values[6], globalEnabled, false));
                    controls.Add(new MyControls.TextBox("field", "data_1", values[7], MyControls.TextBox.TextBoxMode.Date, 10, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "data_2", values[8], MyControls.TextBox.TextBoxMode.Date, 10, 1, globalEnabled));

                    Table interval = new Table();
                    TableHeaderRow headerRow = new TableHeaderRow();
                    TableHeaderCell headerCell = new TableHeaderCell();

                    headerCell.Controls.Add(new LiteralControl("Os."));
                    headerRow.Cells.Add(headerCell);

                    headerCell = new TableHeaderCell();

                    headerCell.Controls.Add(new LiteralControl("Cena"));
                    headerRow.Cells.Add(headerCell);
                    interval.Rows.Add(headerRow);

                    for (int i = 0; i < 10; i++)
                    {
                        TableRow tableRow = new TableRow();
                        TableCell tableCell = new TableCell();

                        tableCell.Controls.Add(new LiteralControl(i.ToString()));
                        tableRow.Controls.Add(tableCell);

                        tableCell = new TableCell();

                        tableCell.Controls.Add(new MyControls.TextBox("field", "stawka_0" + i.ToString(), values[i + 9], MyControls.TextBox.TextBoxMode.Float, 10, 1, globalEnabled));
                        tableRow.Cells.Add(tableCell);
                        interval.Rows.Add(tableRow);
                    }

                    controls.Add(interval);

                    break;

                case Enums.Table.Communities:
                    Title = "Wspólnota";
                    numberOfFields = 12;
                    heading += " wspólnoty";
                    columnSwitching = new List<int>() { 0, 7 };
                    labels = new string[]
                    {
                        "Kod wspólnoty: ",
                        "Ilość budynków: ",
                        "Ilość lokali: ",
                        "Nazwa pełna wspólnoty: ",
                        "Nazwa skrócona: ",
                        "Adres wspólnoty: ",
                        "Adres cd.: ",
                        "Nr konta 1: ",
                        "Nr konta 2: ",
                        "Nr konta 3: ",
                        "Ścieżka do F-K: ",
                        "Uwagi: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.communities.FirstOrDefault(c => c.kod == id).AllFields();
                        else
                            values = new string[numberOfFields];

                        attributesOfObject = new List<DataAccess.AttributeOfObject>();

                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == id))
                                attributesOfObject.Add(attributeOfCommunity);
                    }

                    preview = new List<Control>()
                    {
                        new LiteralControl("Kod: "),
                        new MyControls.Label("previewLabel", String.Empty, values[0], "id_preview"),
                        new LiteralControl("Nazwa: "),
                        new MyControls.Label("previewLabel", String.Empty, values[4], "nazwa_skr_preview"),
                        new LiteralControl("Ilość budynków: "),
                        new MyControls.Label("previewLabel", String.Empty, values[1], "il_bud_preview"),
                        new LiteralControl("Ilość lokali: "),
                        new MyControls.Label("previewLabel", String.Empty, values[2], "il_miesz_preview")
                    };

                    tabButtons = new List<MyControls.HtmlInputRadioButton>()
                    {
                        new MyControls.HtmlInputRadioButton("tabRadio", "dane", "tabRadios", "dane", true),
                        new MyControls.HtmlInputRadioButton("tabRadio", "cechy", "tabRadios", "cechy", false),
                    };

                    labelsOfTabButtons = new List<MyControls.Label>()
                    {
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                        new MyControls.Label("tabLabel", tabButtons.ElementAt(1).ID, "Cechy", String.Empty),
                    };

                    tabs = new List<MyControls.HtmlIframe>()
                    {
                        new MyControls.HtmlIframe("tab", "cechy_tab", "AttributeOfObject.aspx?attributeOf="+Enums.AttributeOf.Community+"&parentId="+id.ToString()+"&action="+action.ToString()+"&childAction=Przeglądaj", "hidden")
                    };

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 5, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 5, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "il_bud", values[1], MyControls.TextBox.TextBoxMode.Number, 3, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "il_miesz", values[2], MyControls.TextBox.TextBoxMode.Number, 4, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nazwa_pel", values[3], MyControls.TextBox.TextBoxMode.SingleLine, 50, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nazwa_skr", values[4], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "adres", values[5], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "adres_2", values[6], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nr1_konta", values[7], MyControls.TextBox.TextBoxMode.SingleLine, 32, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nr2_konta", values[8], MyControls.TextBox.TextBoxMode.SingleLine, 32, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nr3_konta", values[9], MyControls.TextBox.TextBoxMode.SingleLine, 32, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "sciezka_fk", values[10], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "uwagi", values[11], MyControls.TextBox.TextBoxMode.MultiLine, 420, 6, globalEnabled));

                    break;

                case Enums.Table.TypesOfPlace:
                    Title = "Typ lokali";
                    numberOfFields = 2;
                    heading += " typu lokalu";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Typ lokalu: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "typ_lok", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 6, 1, globalEnabled));

                    break;

                case Enums.Table.TypesOfKitchen:
                    Title = "Rodzaj kuchni";
                    numberOfFields = 2;
                    heading += " rodzaju kuchni";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Rodzaj kuchni: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "typ_kuch", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, globalEnabled));

                    break;

                case Enums.Table.TypesOfTenant:
                    Title = "Rodzaj najemców";
                    numberOfFields = 2;
                    heading += " rodzaju najemców";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Rodzaj najemcy: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "r_najemcy", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, globalEnabled));

                    break;

                case Enums.Table.Titles:
                    Title = "Tytuł prawny do lokali";
                    numberOfFields = 2;
                    heading += " tytułu prawnego do lokali";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Tytuł prawny: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.titles.FirstOrDefault(t => t.kod_praw == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "tyt_prawny", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, globalEnabled));

                    break;

                case Enums.Table.TypesOfPayment:
                    Title = "Rodzaj wpłaty lub wypłaty";
                    numberOfFields = 8;
                    heading += " rodzaju wpłaty lub wypłaty";
                    columnSwitching = new List<int>() { 0, 4 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Rodzaj wpłaty lub wypłaty: ",
                        "Rodzaj ewidencji: ",
                        "Sposób rozliczenia: ",
                        "Czy naliczać odsetki? ",
                        "Czy liczyć odsetki na nocie? ",
                        "VAT: ",
                        "SWW: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, globalEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, globalEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "typ_wplat", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, globalEnabled));

                    controls.Add(new MyControls.DropDownList("field", "rodz_e", new List<string[]>()
                    {
                        new string[] {"0", String.Empty},
                        new string[] {"1", "dziennik komornego"},
                        new string[] {"2", "wpłaty"},
                        new string[] {"3", "zmniejszenia"},
                        new string[] {"4", "zwiększenia"}
                    }, values[2], globalEnabled, false));

                    controls.Add(new MyControls.DropDownList("field", "s_rozli", new List<string[]>()
                    {
                        new string[] {"1", "Zmniejszenie"},
                        new string[] {"2", "Zwiększenie"},
                        new string[] {"3", "Zwrot"}
                    }, values[3], globalEnabled, false));

                    controls.Add(new MyControls.RadioButtonList("field", "tn_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, values[4], globalEnabled, false));
                    controls.Add(new MyControls.RadioButtonList("field", "nota_odset", new List<string>() { "Nie", "Tak" }, new List<string>() { "0", "1" }, values[5], globalEnabled, false));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new MyControls.DropDownList("field", "vat", db.vatRates.ToList().Select(r => r.ImportantFieldsForDropDown()).ToList(), values[6], globalEnabled, false));

                    controls.Add(new MyControls.TextBox("field", "sww", values[7], MyControls.TextBox.TextBoxMode.SingleLine, 10, 1, globalEnabled));

                    break;

                case Enums.Table.GroupsOfRentComponents:
                    Title = "Grupa składników czynszu";
                    numberOfFields = 2;
                    heading += " grupy składników czynszu";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Nazwa grupy składników czynszu: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.groupsOfRentComponents.FirstOrDefault(g => g.kod == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "nazwa", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, globalEnabled));

                    break;

                case Enums.Table.FinancialGroups:
                    Title = "Grupa finansowa";
                    numberOfFields = 3;
                    heading += " grupy finansowej";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Konto FK: ",
                        "Nazwa grupy finansowej: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.financialGroups.FirstOrDefault(g => g.kod == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "k_syn", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 3, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "nazwa", values[2], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));

                    break;

                case Enums.Table.VatRates:
                    Title = "Stawka VAT";
                    numberOfFields = 3;
                    heading += " stawki VAT";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Oznaczenie stawki: ",
                        "Symbol fiskalny: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.vatRates.FirstOrDefault(r => r.__record == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "nazwa", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 2, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "nazwa_disabled", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 2, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("nazwa", values[1]));
                    }

                    controls.Add(new MyControls.TextBox("field", "symb_fisk", values[2], MyControls.TextBox.TextBoxMode.SingleLine, 2, 1, globalEnabled));

                    break;

                case Enums.Table.Attributes:
                    Title = "Cecha obiektów";
                    numberOfFields = 10;
                    heading += " cechy obiektów";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Kod: ",
                        "Nazwa: ",
                        "Numeryczna/charakter: ",
                        "Jednostka miary: ",
                        "Wartość domyślna: ",
                        "Uwagi: ",
                        "Dotyczy: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.attributes.FirstOrDefault(a => a.kod == id).AllFields();
                        else
                        {
                            values = new string[numberOfFields];
                            values[2] = "N";
                            values[6] = values[7] = values[8] = values[9] = "X";
                        }
                    }

                    if (idEnabled)
                        controls.Add(new MyControls.TextBox("field", "id", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "id_disabled", values[0], MyControls.TextBox.TextBoxMode.Number, 3, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    }

                    controls.Add(new MyControls.TextBox("field", "nazwa", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 20, 1, globalEnabled));
                    controls.Add(new MyControls.RadioButtonList("field", "nr_str", new List<string>() { "numeryczna", "charakter" }, new List<string>() { "N", "C" }, values[2], globalEnabled, false));
                    controls.Add(new MyControls.TextBox("field", "jedn", values[3], MyControls.TextBox.TextBoxMode.SingleLine, 6, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "wartosc", values[4], MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "uwagi", values[5], MyControls.TextBox.TextBoxMode.SingleLine, 30, 1, globalEnabled));

                    List<string> selectedValues = new List<string>();

                    if (values[6] == "X")
                        selectedValues.Add("l");

                    if (values[7] == "X")
                        selectedValues.Add("n");

                    if (values[8] == "X")
                        selectedValues.Add("b");

                    if (values[9] == "X")
                        selectedValues.Add("s");

                    controls.Add(new MyControls.CheckBoxList("field", "zb", new List<string>() { "lokale", "najemcy", "budynki", "wspólnoty" }, new List<string>() { "l", "n", "b", "s" }, selectedValues, globalEnabled));

                    break;

                case Enums.Table.Users:
                    Title = "Użytkownik";
                    numberOfFields = 6;
                    heading += " użytkownika";
                    columnSwitching = new List<int>() { 0 };
                    labels = new string[]
                    {
                        "Symbol: ",
                        "Nazwisko: ",
                        "Imię: ",
                        "Użytkownik: ",
                        "Hasło: ",
                        "Potwierdź hasło: "
                    };

                    if (values == null)
                    {
                        if (action != Enums.Action.Dodaj)
                        {
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.users.FirstOrDefault(u => u.__record == id).AllFields();

                            values[5] = String.Empty;
                        }
                        else
                            values = new string[numberOfFields];
                    }

                    form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    controls.Add(new MyControls.TextBox("field", "symbol", values[1], MyControls.TextBox.TextBoxMode.SingleLine, 2, 1, globalEnabled));

                    if (idEnabled)
                    {
                        controls.Add(new MyControls.TextBox("field", "nazwisko", values[2], MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, idEnabled));
                        controls.Add(new MyControls.TextBox("field", "imie", values[3], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, idEnabled));
                    }
                    else
                    {
                        controls.Add(new MyControls.TextBox("field", "nazwisko_disabled", values[2], MyControls.TextBox.TextBoxMode.SingleLine, 25, 1, idEnabled));
                        controls.Add(new MyControls.TextBox("field", "imie_disabled", values[3], MyControls.TextBox.TextBoxMode.SingleLine, 15, 1, idEnabled));
                        form.Controls.Add(new MyControls.HtmlInputHidden("nazwisko", values[2]));
                        form.Controls.Add(new MyControls.HtmlInputHidden("imie", values[3]));
                    }

                    controls.Add(new MyControls.TextBox("field", "uzytkownik", values[4], MyControls.TextBox.TextBoxMode.SingleLine, 40, 1, false));
                    controls.Add(new MyControls.TextBox("field", "haslo", values[5], MyControls.TextBox.TextBoxMode.SingleLine, 8, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "haslo2", String.Empty, MyControls.TextBox.TextBoxMode.SingleLine, 8, 1, globalEnabled));

                    break;

                case Enums.Table.TenantTurnovers:
                    Title = "Obrót najemcy";
                    numberOfFields = 9;
                    heading += " obrotu najemcy";
                    columnSwitching = new List<int>() { 0, 3, 4 };
                    labels = new string[]
                    {
                        "Kwota: ",
                        "Data: ",
                        "Data NO: ",
                        "Rodzaj obrotu: ",
                        "Nr dowodu: ",
                        "Pozycja",
                        "Uwagi"
                    };

                    if (values == null)
                    {
                        List<DataAccess.Turnover> turnOvers;

                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            turnOvers = db.turnoversFor14.ToList().Cast<DataAccess.Turnover>().ToList();

                        if (action != Enums.Action.Dodaj)
                            values = turnOvers.FirstOrDefault(t => t.__record == id).AllFields();
                        else
                        {
                            values = new string[numberOfFields];
                            values[8] = GetParamValue<string>("additionalId");

                            if (turnOvers.Count == 0)
                                values[0] = "1";
                            else
                                values[0] = (turnOvers.Max(t => t.__record) + 1).ToString();
                        }
                    }

                    form.Controls.Add(new MyControls.HtmlInputHidden("id", values[0]));
                    controls.Add(new MyControls.TextBox("field", "suma", values[1], MyControls.TextBox.TextBoxMode.Float, 14, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "data_obr", values[2], MyControls.TextBox.TextBoxMode.Date, 10, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "?", values[3], MyControls.TextBox.TextBoxMode.Date, 10, 1, globalEnabled));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        List<DataAccess.TypeOfPayment> typesOfPayment = db.typesOfPayment.ToList();

                        controls.Add(new MyControls.RadioButtonList("field", "kod_wplat", typesOfPayment.Select(t => t.typ_wplat).ToList(), typesOfPayment.Select(t => t.kod_wplat.ToString()).ToList(), values[4], globalEnabled, false));
                    }

                    controls.Add(new MyControls.TextBox("field", "nr_dowodu", values[5], MyControls.TextBox.TextBoxMode.SingleLine, 11, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "pozycja_d", values[6], MyControls.TextBox.TextBoxMode.Number, 2, 1, globalEnabled));
                    controls.Add(new MyControls.TextBox("field", "uwagi", values[7], MyControls.TextBox.TextBoxMode.SingleLine, 40, 1, globalEnabled));
                    form.Controls.Add(new MyControls.HtmlInputHidden("nr_kontr", values[8]));

                    break;
            }

            placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            form.Controls.Add(new MyControls.HtmlInputHidden("action", action.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("table", table.ToString()));

            Control cell = null;
            int columnIndex = -1;

            for (int i = 0; i < controls.Count; i++)
            {
                if (columnSwitching.IndexOf(i) != -1)
                {
                    columnIndex++;
                    cell = formRow.FindControl("column" + columnIndex.ToString());
                }

                cell.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                cell.Controls.Add(new MyControls.Label("fieldLabel", controls[i].ID, labels[i], String.Empty));
                cell.Controls.Add(new LiteralControl("<br />"));
                cell.Controls.Add(controls[i]);
                cell.Controls.Add(new LiteralControl("</div>"));
            }

            if (preview != null)
            {
                placeOfPreview.Controls.Add(new LiteralControl("<h3>"));

                for (int i = 0; i < preview.Count; i += 2)
                {
                    placeOfPreview.Controls.Add(preview[i]);
                    placeOfPreview.Controls.Add(preview[i + 1]);
                    placeOfPreview.Controls.Add(new LiteralControl("<br />"));
                }

                placeOfPreview.Controls.Add(new LiteralControl("</h3>"));
            }

            if (tabButtons != null)
            {
                for (int i = 0; i < tabButtons.Count; i++)
                {
                    placeOfTabButtons.Controls.Add(tabButtons[i]);
                    placeOfTabButtons.Controls.Add(labelsOfTabButtons[i]);
                }

                foreach (MyControls.HtmlIframe tab in tabs)
                    placeOfTabs.Controls.Add(tab);
            }

            foreach (MyControls.Button button in buttons)
                placeOfButtons.Controls.Add(button);

            if (Hello.SiteMapPath.Count > 0)
            {
                if (Hello.SiteMapPath.IndexOf(heading) == -1)
                {
                    Hello.SiteMapPath[Hello.SiteMapPath.Count - 1] = String.Concat("<a href=\"" + backUrl + "\">", Hello.SiteMapPath[Hello.SiteMapPath.Count - 1]) + "</a>";

                    Hello.SiteMapPath.Add(heading);
                }
            }
        }
    }
}