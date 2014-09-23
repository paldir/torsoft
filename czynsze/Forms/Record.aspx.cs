using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
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
            List<int> columnSwitching = null;
            List<ControlsP.HtmlIframeP> tabs = null;
            List<ControlsP.HtmlInputRadioButtonP> tabButtons = null;
            List<ControlsP.LabelP> labelsOfTabButtons = null;
            List<Control> preview = null;

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

                    if (values == null)
                    {
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.buildings.FirstOrDefault(b => b.kod_1 == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new ControlsP.TextBoxP("field", "id", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 5, 1, idEnabled));
                    else
                    {
                        controls.Add(new ControlsP.TextBoxP("field", "idDisabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 5, 1, idEnabled));
                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", id.ToString()));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "il_miesz", values[1], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, globalEnabled));
                    controls.Add(new ControlsP.RadioButtonListP("field", "sp_rozl", new List<string>() { "budynek", "lokale" }, new List<string>() { "0", "1" }, values[2], globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres", values[3], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres_2", values[4], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "udzial_w_k", values[5], ControlsP.TextBoxP.TextBoxModeP.Float, 6, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "uwagi", values[6], ControlsP.TextBoxP.TextBoxModeP.MultiLine, 420, 6, globalEnabled));
                    break;
                case EnumP.Table.Places:
                case EnumP.Table.InactivePlaces:
                    
                    this.Title = "Lokal";
                    numberOfFields = 22;
                    heading += " lokalu";

                    if (table == EnumP.Table.InactivePlaces)
                    {
                        this.Title = "Lokal (nieaktywny)";
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
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            {
                                if (table == EnumP.Table.Places)
                                    values = db.places.FirstOrDefault(b => b.nr_system == id).AllFields();
                                else
                                    values = db.inactivePlaces.FirstOrDefault(b => b.nr_system == id).AllFields();
                            }
                        else
                        {
                            values = new string[numberOfFields];

                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values[0] = (db.places.Select(p => p.nr_system).ToList().Max() + 1).ToString();

                            values[1] = values[2] = "0";
                        }
                    }

                    tabButtons = new List<ControlsP.HtmlInputRadioButtonP>()
                    {
                        new ControlsP.HtmlInputRadioButtonP("tabRadio", "dane", "tabRadios", "dane", true),
                        new ControlsP.HtmlInputRadioButtonP("tabRadio", "skladnikiCzynszu", "tabRadios", "skladnikiCzynszu", false),
                        new ControlsP.HtmlInputRadioButtonP("tabRadio", "dokumenty", "tabRadios", "dokumenty", false)
                    };

                    labelsOfTabButtons = new List<ControlsP.LabelP>()
                    {
                        new ControlsP.LabelP("tabLabel", tabButtons.ElementAt(0).ID, "Dane", String.Empty),
                        new ControlsP.LabelP("tabLabel", tabButtons.ElementAt(1).ID, "Składniki czynszu", String.Empty),
                        new ControlsP.LabelP("tabLabel", tabButtons.ElementAt(2).ID, "Dokumenty", String.Empty)
                    };

                    preview = new List<Control>()
                    {
                        new LiteralControl("Nr budynku: "),
                        new ControlsP.LabelP("previewLabel", String.Empty, values[1], "kod_lok_preview"),
                        new LiteralControl("Nr lokalu: "),
                        new ControlsP.LabelP("previewLabel", String.Empty, values[2], "nr_lok_preview"),
                        new LiteralControl("Adres: "),
                        new ControlsP.LabelP("previewLabel", String.Empty, values[4], "adres_preview"),
                        new LiteralControl("Adres cd.: "),
                        new ControlsP.LabelP("previewLabel", String.Empty, values[5], "adres_2_preview")
                    };

                    //
                    //CXP PART
                    //
                    string parentAction;

                    switch (action)
                    {
                        case EnumP.Action.Dodaj:
                            parentAction = "add";
                            break;
                        case EnumP.Action.Edytuj:
                            parentAction = "edit";
                            break;
                        case EnumP.Action.Usuń:
                            parentAction = "delete";
                            break;
                        default:
                            parentAction = "browse";
                            break;
                    }
                    //
                    //TO DUMP BEHIND THE WALL
                    //

                    tabs = new List<ControlsP.HtmlIframeP>()
                    {
                        new ControlsP.HtmlIframeP("tab", "skladnikiCzynszu_tab", "/czynsze1/SkladnikiCzynszuLokalu.cxp?parentAction="+parentAction+"&kod_lok="+values[1]+"&nr_lok="+values[2], "hidden"),
                        new ControlsP.HtmlIframeP("tab", "dokumenty_tab", "/czynsze1/PlikiNajemcy.cxp?parentAction="+parentAction+"&nr_system="+values[0], "hidden")
                    };

                    //controls.Add(new ControlsP.TextBoxP("field", "Nr_system_disabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 14, 1, false));
                    form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));

                    if (idEnabled)
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            controls.Add(new ControlsP.DropDownListP("field", "kod_lok", db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList(), values[1], idEnabled));

                        controls.Add(new ControlsP.TextBoxP("field", "nr_lok", values[2], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                    }
                    else
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            controls.Add(new ControlsP.DropDownListP("field", "kod_lok_disabled", db.buildings.ToList().OrderBy(b => b.kod_1).Select(b => b.ImportantFields()).ToList(), values[1], idEnabled));

                        controls.Add(new ControlsP.TextBoxP("field", "nr_lok_disabled", values[2], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));

                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("kod_lok", values[1]));
                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("nr_lok", values[2]));
                    }

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new ControlsP.DropDownListP("field", "kod_typ", db.typesOfPlace.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[3], globalEnabled));

                    controls.Add(new ControlsP.TextBoxP("field", "adres", values[4], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres_2", values[5], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "pow_uzyt", values[6], ControlsP.TextBoxP.TextBoxModeP.Float, 8, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "pow_miesz", values[7], ControlsP.TextBoxP.TextBoxModeP.Float, 8, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "udzial", values[8], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "dat_od", values[9], ControlsP.TextBoxP.TextBoxModeP.Date, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "dat_do", values[10], ControlsP.TextBoxP.TextBoxModeP.Date, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_1", values[11], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_2", values[12], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_3", values[13], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_4", values[14], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_5", values[15], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "p_6", values[16], ControlsP.TextBoxP.TextBoxModeP.Float, 5, 1, globalEnabled));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                    {
                        controls.Add(new ControlsP.DropDownListP("field", "kod_kuch", db.typesOfKitchen.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[17], globalEnabled));
                        controls.Add(new ControlsP.DropDownListP("field", "nr_kontr", db.tenants.OrderBy(t => t.nazwisko).ToList().Select(t => t.ImportantFields().ToList().GetRange(1, 4).ToArray()).ToList(), values[18], globalEnabled));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "il_osob", values[19], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, globalEnabled));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new ControlsP.DropDownListP("field", "kod_praw", db.titles.ToList().Select(t => t.ImportantFields()).ToList(), values[20], globalEnabled));

                    controls.Add(new ControlsP.TextBoxP("field", "uwagi", values[21], ControlsP.TextBoxP.TextBoxModeP.MultiLine, 240, 4, globalEnabled));

                    //
                    //CXP PART
                    //
                    try
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE 1=2");
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE 1=2");
                                    break;
                                default:
                                    db.Database.ExecuteSqlCommand("CREATE TABLE skl_cz_tmp AS SELECT * FROM skl_cz WHERE kod_lok=" + values[1] + " AND nr_lok=" + values[2]);
                                    db.Database.ExecuteSqlCommand("CREATE TABLE pliki_tmp AS SELECT * FROM pliki WHERE nr_system=" + values[0]);
                                    break;
                            }
                        }
                    }
                    catch { }
                    //
                    //TO DUMP BEHIND THE WALL
                    //
                    break;
                case EnumP.Table.Tenants:
                    this.Title = "Najemca";
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

                    if (values == null)
                    {
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.tenants.FirstOrDefault(t => t.nr_kontr == id).AllFields();
                        else
                        {
                            values = new string[numberOfFields];

                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values[0] = (db.tenants.Select(t => t.nr_kontr).ToList().Max() + 1).ToString();
                        }
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "nr_kontr_disabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 6, 1, false));
                    placeOfButtons.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));

                    using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        controls.Add(new ControlsP.DropDownListP("field", "kod_najem", db.typesOfTenant.ToList().Select(t => t.ImportantFieldsForDropDown()).ToList(), values[1], globalEnabled));

                    controls.Add(new ControlsP.TextBoxP("field", "nazwisko", values[2], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 25, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "imie", values[3], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 25, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres_1", values[4], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "adres_2", values[5], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "nr_dow", values[6], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 9, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "pesel", values[7], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 11, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "nazwa_z", values[8], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 40, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "e_mail", values[9], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 40, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "l__has", values[10], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 15, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "uwagi", values[11], ControlsP.TextBoxP.TextBoxModeP.MultiLine, 120, 2, globalEnabled));
                    break;
                case EnumP.Table.RentComponents:
                    this.Title = "Składnik opłat";
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
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.rentComponents.FirstOrDefault(c => c.nr_skl == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new ControlsP.TextBoxP("field", "id", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new ControlsP.TextBoxP("field", "id_disabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "nazwa", values[1], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 30, 1, globalEnabled));
                    controls.Add(new ControlsP.DropDownListP("field", "rodz_e", new List<string[]> { new string[] { "1", "dziennik komornego" }, new string[] { "2", "wpłaty" }, new string[] { "3", "zmniejszenia" }, new string[] { "4", "zwiększenia" } }, values[2], globalEnabled));
                    controls.Add(new ControlsP.DropDownListP("field", "s_zaplat", new List<string[]> { new string[] { "1", "za m2 pow. użytkowej" }, new string[] { "2", "za określoną ilość" }, new string[] { "3", "za osobę" }, new string[] { "4", "za lokal" }, new string[] { "5", "za ilość dni w miesiącu" }, new string[] { "6", "za osobę - przedziały" } }, values[3], globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "stawka", values[4], ControlsP.TextBoxP.TextBoxModeP.Float, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "stawka_inf", values[5], ControlsP.TextBoxP.TextBoxModeP.Float, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.DropDownListP("field", "typ_skl", new List<string[]> { new string[] { "0", "stały" }, new string[] { "1", "zmienny" } }, values[6], globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "data_1", values[7], ControlsP.TextBoxP.TextBoxModeP.Date, 10, 1, globalEnabled));
                    controls.Add(new ControlsP.TextBoxP("field", "data_2", values[8], ControlsP.TextBoxP.TextBoxModeP.Date, 10, 1, globalEnabled));

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

                        tableCell.Controls.Add(new ControlsP.TextBoxP("field", "stawka_0" + i.ToString(), values[i + 9], ControlsP.TextBoxP.TextBoxModeP.Float, 10, 1, globalEnabled));
                        tableRow.Cells.Add(tableCell);
                        interval.Rows.Add(tableRow);
                    }

                    controls.Add(interval);
                    break;
                case EnumP.Table.TypesOfPlace:
                    this.Title = "Typ lokali";
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
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new ControlsP.TextBoxP("field", "id", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new ControlsP.TextBoxP("field", "id_disabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "typ_lok", values[1], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 6, 1, globalEnabled));
                    break;
                case EnumP.Table.TypesOfKitchen:
                    this.Title = "Rodzaj kuchni";
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
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new ControlsP.TextBoxP("field", "id", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new ControlsP.TextBoxP("field", "id_disabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "typ_kuch", values[1], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 15, 1, globalEnabled));
                    break;
                case EnumP.Table.TypesOfTenant:
                    this.Title = "Rodzaj najemców";
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
                        if (action != EnumP.Action.Dodaj)
                            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                                values = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id).AllFields();
                        else
                            values = new string[numberOfFields];
                    }

                    if (idEnabled)
                        controls.Add(new ControlsP.TextBoxP("field", "id", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                    else
                    {
                        controls.Add(new ControlsP.TextBoxP("field", "id_disabled", values[0], ControlsP.TextBoxP.TextBoxModeP.Number, 3, 1, idEnabled));
                        form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", values[0]));
                    }

                    controls.Add(new ControlsP.TextBoxP("field", "r_najemcy", values[1], ControlsP.TextBoxP.TextBoxModeP.SingleLine, 15, 1, globalEnabled));
                    break;
            }

            placeOfHeading.Controls.Add(new LiteralControl("<h2>" + heading + "</h2>"));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("action", action.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));

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
                cell.Controls.Add(new ControlsP.LabelP("fieldLabel", controls[i].ID, labels[i], String.Empty));
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

                foreach (ControlsP.HtmlIframeP tab in tabs)
                    placeOfTabs.Controls.Add(tab);
            }

            foreach (ControlsP.ButtonP button in buttons)
                placeOfButtons.Controls.Add(button);
        }
    }
}