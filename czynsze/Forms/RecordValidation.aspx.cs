using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;

namespace czynsze.Forms
{
    public partial class RecordValidation : Page
    {
        Enums.Table table;
        Enums.Action action;
        int id;

        List<DataAccess.AttributeOfObject> attributesOfObject
        {
            get { return (List<DataAccess.AttributeOfObject>)Session["attributesOfObject"]; }
            set { Session["attributesOfObject"] = value; }
        }

        List<DataAccess.RentComponentOfPlace> rentComponentsOfPlace
        {
            get { return (List<DataAccess.RentComponentOfPlace>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] recordFields = null;
            string validationResult = null;
            string dbWriteResult = null;
            //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("table"))]);
            table = GetParamValue<Enums.Table>("table");
            //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("action"))]);
            action = GetParamValue<Enums.Action>("action");
            string backUrl = "javascript: Load('List.aspx?table=" + table + "')";
            string nominativeCase = String.Empty;
            string genitiveCase = String.Empty;
            DataAccess.IRecord record = null;
            Type attributeType = null;

            Dictionary<Enums.Action, string> dictionaryOfActionInfinitives = new Dictionary<Enums.Action, string>()
            {
                { Enums.Action.Dodaj, "dodać" },
                { Enums.Action.Edytuj, "edytować" },
                { Enums.Action.Przenieś, "przenieść" },
                { Enums.Action.Usuń, "usunąć" }
            };

            Dictionary<Enums.Action, string> dictionaryOfActionParticiples = new Dictionary<Enums.Action, string>()
            {
                { Enums.Action.Dodaj, "dodany" },
                { Enums.Action.Edytuj, "wyedytowany" },
                { Enums.Action.Przenieś, "przeniesiony" },
                { Enums.Action.Usuń, "usunięty" }
            };

            if (action != Enums.Action.Dodaj)
            {
                //id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("id"))]);
                id = GetParamValue<int>("id");

                form.Controls.Add(new MyControls.HtmlInputHidden("id", id.ToString()));
            }

            form.Controls.Add(new MyControls.HtmlInputHidden("table", table.ToString()));
            form.Controls.Add(new MyControls.HtmlInputHidden("action", action.ToString()));

            try
            {
                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                {
                    switch (table)
                    {
                        case Enums.Table.Buildings:
                        case Enums.Table.RentComponents:
                        case Enums.Table.TypesOfPlace:
                        case Enums.Table.TypesOfKitchen:
                        case Enums.Table.TypesOfTenant:
                        case Enums.Table.Titles:
                        case Enums.Table.TypesOfPayment:
                        case Enums.Table.GroupsOfRentComponents:
                        case Enums.Table.FinancialGroups:
                        case Enums.Table.VatRates:
                        case Enums.Table.Attributes:
                        case Enums.Table.Users:
                        case Enums.Table.TenantTurnovers:
                            switch (table)
                            {
                                case Enums.Table.Buildings:
                                    record = new DataAccess.Building();
                                    attributeType = typeof(DataAccess.AttributeOfBuilding);
                                    nominativeCase = "budynek";
                                    genitiveCase = "budynku";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("il_miesz"),
                                        GetParamValue<string>("sp_rozl"),
                                        GetParamValue<string>("adres"),
                                        GetParamValue<string>("adres_2"),
                                        GetParamValue<string>("udzial_w_k"),
                                        GetParamValue<string>("uwagi")
                                    };

                                    break;

                                case Enums.Table.RentComponents:
                                    record = new DataAccess.RentComponent();
                                    nominativeCase = "składnik opłat";
                                    genitiveCase = "składnika opłat";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("nazwa"),
                                        GetParamValue<string>("rodz_e"),
                                        GetParamValue<string>("s_zaplat"),
                                        GetParamValue<string>("stawka"),
                                        GetParamValue<string>("stawka_inf"),
                                        GetParamValue<string>("typ_skl"),
                                        GetParamValue<string>("data_1"),
                                        GetParamValue<string>("data_2")
                                    };

                                    if (recordFields[3] == "6")
                                        recordFields = recordFields.ToList().Concat(new string[] 
                                        {
                                            GetParamValue<string>("stawka_00"),
                                            GetParamValue<string>("stawka_01"),
                                            GetParamValue<string>("stawka_02"),
                                            GetParamValue<string>("stawka_03"),
                                            GetParamValue<string>("stawka_04"),
                                            GetParamValue<string>("stawka_05"),
                                            GetParamValue<string>("stawka_06"),
                                            GetParamValue<string>("stawka_07"),
                                            GetParamValue<string>("stawka_08"),
                                            GetParamValue<string>("stawka_09")
                                        }).ToArray();
                                    else
                                        recordFields = recordFields.ToList().Concat(new string[] { "", "", "", "", "", "", "", "", "", "" }).ToArray();

                                    break;

                                case Enums.Table.TypesOfPlace:
                                    record = new DataAccess.TypeOfPlace();
                                    nominativeCase = "typ lokali";
                                    genitiveCase = "typu lokali";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("typ_lok")
                                    };

                                    break;

                                case Enums.Table.TypesOfKitchen:
                                    record = new DataAccess.TypeOfKitchen();
                                    nominativeCase = "typ kuchni";
                                    genitiveCase = "typu kuchni";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("typ_kuch")
                                    };

                                    break;

                                case Enums.Table.TypesOfTenant:
                                    record = new DataAccess.TypeOfTenant();
                                    nominativeCase = "rodzaj najemcy";
                                    genitiveCase = "rodzaju najemcy";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("r_najemcy")
                                    };

                                    break;

                                case Enums.Table.Titles:
                                    record = new DataAccess.Title();
                                    nominativeCase = "tytuł prawny do lokali";
                                    genitiveCase = "tytułu prawnego do lokali";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("tyt_prawny")
                                    };

                                    break;

                                case Enums.Table.TypesOfPayment:
                                    record = new DataAccess.TypeOfPayment();
                                    nominativeCase = "rodzaj wpłaty lub wypłaty";
                                    genitiveCase = "rodzaju wpłaty lub wypłaty";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("typ_wplat"),
                                        GetParamValue<string>("rodz_e"),
                                        GetParamValue<string>("s_rozli"),
                                        GetParamValue<string>("tn_odset"),
                                        GetParamValue<string>("nota_odset"),
                                        GetParamValue<string>("vat"),
                                        GetParamValue<string>("sww")
                                    };

                                    break;

                                case Enums.Table.GroupsOfRentComponents:
                                    record = new DataAccess.GroupOfRentComponents();
                                    nominativeCase = "grupa składników czynszu";
                                    genitiveCase = "grupy składników czynszu";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("nazwa")
                                    };

                                    break;

                                case Enums.Table.FinancialGroups:
                                    record = new DataAccess.FinancialGroup();
                                    nominativeCase = "grupa finansowa";
                                    genitiveCase = "grupy finansowej";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("k_syn"),
                                        GetParamValue<string>("nazwa")
                                    };

                                    break;

                                case Enums.Table.VatRates:
                                    record = new DataAccess.VatRate();
                                    nominativeCase = "stawka VAT";
                                    genitiveCase = "stawki VAt";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("nazwa"),
                                        GetParamValue<string>("symb_fisk")
                                    };

                                    break;

                                case Enums.Table.Attributes:
                                    record = new DataAccess.Attribute();
                                    nominativeCase = "cecha obiektów";
                                    genitiveCase = "cechy obiektów";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("nazwa"),
                                        GetParamValue<string>("nr_str"),
                                        GetParamValue<string>("jedn"),
                                        GetParamValue<string>("wartosc"),
                                        GetParamValue<string>("uwagi"),
                                        GetParamValue<string>("zb_0"),
                                        GetParamValue<string>("zb_1"),
                                        GetParamValue<string>("zb_2"),
                                        GetParamValue<string>("zb_3"),
                                    };

                                    break;

                                case Enums.Table.Users:
                                    record = new DataAccess.User();
                                    nominativeCase = "użytkownik";
                                    genitiveCase = "użytkownika";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("symbol"),
                                        GetParamValue<string>("nazwisko"),
                                        GetParamValue<string>("imie"),
                                        GetParamValue<string>("haslo"),
                                        GetParamValue<string>("haslo2")
                                    };

                                    break;

                                case Enums.Table.TenantTurnovers:
                                    nominativeCase = "obrót najemcy";
                                    genitiveCase = "obrotu najemcy";

                                    recordFields = new string[]
                                    {
                                        GetParamValue<string>("id"),
                                        GetParamValue<string>("suma"),
                                        GetParamValue<string>("data_obr"),
                                        GetParamValue<string>("?"),
                                        GetParamValue<string>("kod_wplat"),
                                        GetParamValue<string>("nr_dowodu"),
                                        GetParamValue<string>("pozycja_d"),
                                        GetParamValue<string>("uwagi"),
                                        GetParamValue<string>("nr_kontr")
                                    };

                                    switch (Hello.CurrentSet)
                                    {
                                        case Enums.SettlementTable.Czynsze:
                                            record = new DataAccess.TurnoverFrom1stSet();

                                            break;

                                        case Enums.SettlementTable.SecondSet:
                                            record = new DataAccess.TurnoverFrom2ndSet();

                                            break;

                                        case Enums.SettlementTable.ThirdSet:
                                            record = new DataAccess.TurnoverFrom3rdSet();

                                            break;
                                    }

                                    backUrl = backUrl.Insert(backUrl.LastIndexOf('\''), "&id=" + recordFields[8]);

                                    break;
                            }

                            validationResult = record.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                            {
                                DbSet dbSet = db.Set(record.GetType());
                                DbSet dbSetOfAttributes = null;

                                if (attributeType != null)
                                    dbSetOfAttributes = db.Set(attributeType);

                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        record.Set(recordFields);
                                        dbSet.Add(record);

                                        if (dbSetOfAttributes != null)
                                            foreach (DataAccess.AttributeOfObject attribute in attributesOfObject)
                                            {
                                                attribute.kod_powiaz = recordFields[0];

                                                dbSetOfAttributes.Add(attribute);
                                            }

                                        break;

                                    case Enums.Action.Edytuj:
                                        record = (DataAccess.IRecord)dbSet.Find(id);

                                        record.Set(recordFields);

                                        if (dbSetOfAttributes != null)
                                        {
                                            foreach (DataAccess.AttributeOfObject attributeOfObject in dbSetOfAttributes.ToListAsync().Result.Cast<DataAccess.AttributeOfObject>().Where(a => a.kod_powiaz.Trim() == id.ToString()))
                                                dbSetOfAttributes.Remove(attributeOfObject);

                                            foreach (DataAccess.AttributeOfObject attributeOfObject in attributesOfObject)
                                                dbSetOfAttributes.Add(attributeOfObject);
                                        }

                                        break;

                                    case Enums.Action.Usuń:
                                        record = (DataAccess.IRecord)dbSet.Find(id);

                                        dbSet.Remove(record);

                                        if (dbSetOfAttributes != null)
                                            foreach (DataAccess.AttributeOfObject attributeOfObject in dbSetOfAttributes.ToListAsync().Result.Cast<DataAccess.AttributeOfObject>().Where(a => a.kod_powiaz.Trim() == id.ToString()))
                                                dbSetOfAttributes.Remove(attributeOfObject);

                                        break;
                                }
                            }

                            break;

                        default:
                            switch (table)
                            {
                                case Enums.Table.Places:
                                    DataAccess.ActivePlace place;
                                    nominativeCase = "lokal";
                                    genitiveCase = "lokalu";

                                    recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("kod_lok"),
                                GetParamValue<string>("nr_lok"),
                                GetParamValue<string>("kod_typ"),
                                GetParamValue<string>("adres"),
                                GetParamValue<string>("adres_2"),
                                GetParamValue<string>("pow_uzyt"),
                                GetParamValue<string>("pow_miesz"),
                                GetParamValue<string>("udzial"),
                                GetParamValue<string>("dat_od"),
                                GetParamValue<string>("dat_do"),
                                GetParamValue<string>("p_1"),
                                GetParamValue<string>("p_2"),
                                GetParamValue<string>("p_3"),
                                GetParamValue<string>("p_4"),
                                GetParamValue<string>("p_5"),
                                GetParamValue<string>("p_6"),
                                GetParamValue<string>("kod_kuch"),
                                GetParamValue<string>("nr_kontr"),
                                GetParamValue<string>("il_osob"),
                                GetParamValue<string>("kod_praw"),
                                GetParamValue<string>("uwagi")
                            };

                                    validationResult = DataAccess.ActivePlace.Validate(action, recordFields);

                                    if (String.IsNullOrEmpty(validationResult))
                                        switch (action)
                                        {
                                            case Enums.Action.Dodaj:
                                                place = new DataAccess.ActivePlace();

                                                place.Set(recordFields);
                                                db.places.Add(place);

                                                foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                                {
                                                    attributeOfPlace.kod_powiaz = recordFields[0];

                                                    db.attributesOfPlaces.Add(attributeOfPlace);
                                                }

                                                foreach (DataAccess.RentComponentOfPlace rentComponentOfPlace in rentComponentsOfPlace)
                                                {
                                                    rentComponentOfPlace.kod_lok = Convert.ToInt16(recordFields[1]);
                                                    rentComponentOfPlace.nr_lok = Convert.ToInt16(recordFields[2]);

                                                    db.rentComponentsOfPlaces.Add(rentComponentOfPlace);
                                                }

                                                //
                                                //CXP PART
                                                //
                                                //db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT " + recordFields[1] + ", " + recordFields[2] + ", nr_skl, dan_p FROM skl_cz_tmp");
                                                db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                                //
                                                //TO DUMP BEHIND THE WALL
                                                //

                                                break;

                                            case Enums.Action.Edytuj:
                                                place = db.places.FirstOrDefault(p => p.nr_system == id);

                                                place.Set(recordFields);

                                                foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == place.nr_system))
                                                    db.attributesOfPlaces.Remove(attributeOfPlace);

                                                foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                                    db.attributesOfPlaces.Add(attributeOfPlace);

                                                foreach (DataAccess.RentComponentOfPlace rentComponentOfPlace in db.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                                    db.rentComponentsOfPlaces.Remove(rentComponentOfPlace);

                                                foreach (DataAccess.RentComponentOfPlace rentComponentOfPlace in rentComponentsOfPlace)
                                                    db.rentComponentsOfPlaces.Add(rentComponentOfPlace);

                                                //
                                                //CXP PART
                                                //
                                                //db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + recordFields[1] + " AND nr_lok=" + recordFields[2]);
                                                //db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT kod_lok, nr_lok, nr_skl, dan_p FROM skl_cz_tmp");
                                                db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + recordFields[0]);
                                                db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                                //
                                                //TO DUMP BEHIND THE WALL
                                                //

                                                break;

                                            case Enums.Action.Usuń:
                                                place = db.places.FirstOrDefault(p => p.nr_system == id);

                                                foreach (DataAccess.RentComponentOfPlace component in db.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                                    db.rentComponentsOfPlaces.Remove(component);

                                                foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                                    db.attributesOfPlaces.Remove(attributeOfPlace);

                                                db.places.Remove(place);

                                                //
                                                //CXP PART
                                                //
                                                //db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + recordFields[1] + " AND nr_lok=" + recordFields[2]);
                                                db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + recordFields[0]);
                                                //
                                                //TO DUMP BEHIND THE WALL
                                                //

                                                break;

                                            case Enums.Action.Przenieś:
                                                DataAccess.InactivePlace inactivePlace = new DataAccess.InactivePlace();
                                                place = db.places.FirstOrDefault(p => p.nr_system == id);

                                                db.places.Remove(place);

                                                recordFields = place.AllFields();

                                                DataAccess.Place.Validate(action, recordFields);
                                                inactivePlace.Set(recordFields);
                                                db.inactivePlaces.Add(inactivePlace);

                                                break;
                                        }

                                    break;

                                case Enums.Table.InactivePlaces:
                                    validationResult = String.Empty;
                                    nominativeCase = "lokal (nieaktywny)";
                                    genitiveCase = "lokalu (nieaktywnego)";

                                    switch (action)
                                    {
                                        case Enums.Action.Przenieś:
                                            DataAccess.ActivePlace activePlace = new DataAccess.ActivePlace();
                                            DataAccess.InactivePlace inactivePlace = db.inactivePlaces.FirstOrDefault(p => p.nr_system == id);

                                            db.inactivePlaces.Remove(inactivePlace);

                                            recordFields = inactivePlace.AllFields();

                                            DataAccess.Place.Validate(action, recordFields);
                                            activePlace.Set(recordFields);
                                            db.places.Add(activePlace);

                                            break;
                                    }

                                    break;

                                case Enums.Table.Tenants:
                                    DataAccess.ActiveTenant tenant;
                                    nominativeCase = "najemca";
                                    genitiveCase = "najemcy";

                                    recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("kod_najem"),
                                GetParamValue<string>("nazwisko"),
                                GetParamValue<string>("imie"),
                                GetParamValue<string>("adres_1"),
                                GetParamValue<string>("adres_2"),
                                GetParamValue<string>("nr_dow"),
                                GetParamValue<string>("pesel"),
                                GetParamValue<string>("nazwa_z"),
                                GetParamValue<string>("e_mail"),
                                GetParamValue<string>("l__has"),
                                GetParamValue<string>("uwagi")
                            };

                                    validationResult = "";

                                    if (String.IsNullOrEmpty(validationResult))
                                        switch (action)
                                        {
                                            case Enums.Action.Dodaj:
                                                tenant = new DataAccess.ActiveTenant();

                                                tenant.Set(recordFields);
                                                db.tenants.Add(tenant);

                                                foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                                {
                                                    attributeOfTenant.kod_powiaz = recordFields[0];

                                                    db.attributesOfTenants.Add(attributeOfTenant);
                                                }

                                                break;

                                            case Enums.Action.Edytuj:
                                                tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                                tenant.Set(recordFields);

                                                foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                                    db.attributesOfTenants.Remove(attributeOfTenant);

                                                foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                                    db.attributesOfTenants.Add(attributeOfTenant);

                                                break;

                                            case Enums.Action.Usuń:
                                                tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                                db.tenants.Remove(tenant);

                                                foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                                    db.attributesOfTenants.Remove(attributeOfTenant);

                                                break;

                                            case Enums.Action.Przenieś:
                                                DataAccess.InactiveTenant inactiveTenant = new DataAccess.InactiveTenant();
                                                tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                                db.tenants.Remove(tenant);

                                                recordFields = tenant.AllFields();

                                                inactiveTenant.Set(recordFields);
                                                db.inactiveTenants.Add(inactiveTenant);

                                                break;
                                        }

                                    break;

                                case Enums.Table.InactiveTenants:
                                    validationResult = String.Empty;
                                    nominativeCase = "najemca (nieaktywny)";
                                    genitiveCase = "najemcy (nieaktywnego)";

                                    switch (action)
                                    {
                                        case Enums.Action.Przenieś:
                                            DataAccess.ActiveTenant activeTenant = new DataAccess.ActiveTenant();
                                            DataAccess.InactiveTenant inactiveTenant = db.inactiveTenants.FirstOrDefault(t => t.nr_kontr == id);

                                            db.inactiveTenants.Remove(inactiveTenant);

                                            recordFields = inactiveTenant.AllFields();

                                            activeTenant.Set(recordFields);
                                            db.tenants.Add(activeTenant);

                                            break;
                                    }

                                    break;

                                case Enums.Table.Communities:
                                    DataAccess.Community community;
                                    nominativeCase = "wspólnota";
                                    genitiveCase = "wspólnoty";

                                    recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("il_bud"),
                                GetParamValue<string>("il_miesz"),
                                GetParamValue<string>("nazwa_pel"),
                                GetParamValue<string>("nazwa_skr"),
                                GetParamValue<string>("adres"),
                                GetParamValue<string>("adres_2"),
                                GetParamValue<string>("nr1_konta"),
                                GetParamValue<string>("nr2_konta"),
                                GetParamValue<string>("nr3_konta"),
                                GetParamValue<string>("sciezka_fk"),
                                GetParamValue<string>("uwagi")
                            };

                                    validationResult = DataAccess.Community.Validate(action, recordFields);

                                    if (String.IsNullOrEmpty(validationResult))
                                        switch (action)
                                        {
                                            case Enums.Action.Dodaj:
                                                community = new DataAccess.Community();

                                                community.Set(recordFields);
                                                db.communities.Add(community);

                                                foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                                {
                                                    attributeOfCommunity.kod_powiaz = recordFields[0];

                                                    db.attributesOfCommunities.Add(attributeOfCommunity);
                                                }

                                                break;

                                            case Enums.Action.Edytuj:
                                                community = db.communities.FirstOrDefault(c => c.kod == id);

                                                community.Set(recordFields);

                                                foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                                    db.attributesOfCommunities.Remove(attributeOfCommunity);

                                                foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                                    db.attributesOfCommunities.Add(attributeOfCommunity);

                                                break;

                                            case Enums.Action.Usuń:
                                                community = db.communities.FirstOrDefault(c => c.kod == id);

                                                db.communities.Remove(community);

                                                foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                                    db.attributesOfCommunities.Remove(attributeOfCommunity);

                                                break;
                                        }

                                    break;
                            }

                            break;
                    }

                    if (String.IsNullOrEmpty(validationResult))
                    {
                        db.SaveChanges();

                        if (!String.IsNullOrEmpty(nominativeCase))
                            dbWriteResult = Char.ToUpper(nominativeCase[0]) + nominativeCase.Substring(1) + " " + dictionaryOfActionParticiples[action] + ".";
                    }
                }
            }
            catch (Exception exception) { dbWriteResult = "Nie można " + dictionaryOfActionInfinitives[action] + " " + genitiveCase + "! " + exception.Message; }

            Title = "Edycja " + genitiveCase;

            placeOfMessage.Controls.Add(new LiteralControl(validationResult));

            if (!String.IsNullOrEmpty(dbWriteResult))
                placeOfMessage.Controls.Add(new LiteralControl(dbWriteResult + "<br />"));

            if (!String.IsNullOrEmpty(validationResult) || (!String.IsNullOrEmpty(dbWriteResult) && dbWriteResult.Contains("!")))
            {
                placeOfButtons.Controls.Add(new MyControls.Button("button", "Repair", "Popraw", "Record.aspx"));
                placeOfButtons.Controls.Add(new MyControls.Button("button", "Cancel", "Anuluj", backUrl));

                Session["values"] = recordFields;
            }
            else
                placeOfButtons.Controls.Add(new MyControls.Button("button", "Back", "Powrót", backUrl));
        }
    }
}