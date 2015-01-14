using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] record = null;
            string validationResult = null;
            string dbWriteResult = null;
            //table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("table"))]);
            table = GetParamValue<Enums.Table>("table");
            //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("action"))]);
            action = GetParamValue<Enums.Action>("action");
            string backUrl = "javascript: Load('List.aspx?table=" + table + "')";
            string nominativeCase = String.Empty;
            string genitiveCase = String.Empty;

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
                            DataAccess.Building building;
                            nominativeCase = "budynek";
                            genitiveCase = "budynku";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("il_miesz"),
                                GetParamValue<string>("sp_rozl"),
                                GetParamValue<string>("adres"),
                                GetParamValue<string>("adres_2"),
                                GetParamValue<string>("udzial_w_k"),
                                GetParamValue<string>("uwagi")
                            };

                            validationResult = DataAccess.Building.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        building = new DataAccess.Building();

                                        building.Set(record);
                                        db.buildings.Add(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                        {
                                            attributeOfBuilding.kod_powiaz = record[0];

                                            db.attributesOfBuildings.Add(attributeOfBuilding);
                                        }

                                        break;

                                    case Enums.Action.Edytuj:
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        building.Set(record);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                            db.attributesOfBuildings.Add(attributeOfBuilding);

                                        break;

                                    case Enums.Action.Usuń:
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        db.buildings.Remove(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        break;
                                }

                            break;

                        case Enums.Table.Places:
                            DataAccess.ActivePlace place;
                            nominativeCase = "lokal";
                            genitiveCase = "lokalu";

                            record = new string[]
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

                            validationResult = DataAccess.ActivePlace.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        place = new DataAccess.ActivePlace();

                                        place.Set(record);
                                        db.places.Add(place);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                        {
                                            attributeOfPlace.kod_powiaz = record[0];

                                            db.attributesOfPlaces.Add(attributeOfPlace);
                                        }

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT " + record[1] + ", " + record[2] + ", nr_skl, dan_p FROM skl_cz_tmp");
                                        db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                        //
                                        //TO DUMP BEHIND THE WALL
                                        //

                                        break;

                                    case Enums.Action.Edytuj:
                                        place = db.places.FirstOrDefault(p => p.nr_system == id);

                                        place.Set(record);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfPlaces.Remove(attributeOfPlace);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                            db.attributesOfPlaces.Add(attributeOfPlace);

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + record[1] + " AND nr_lok=" + record[2]);
                                        db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT kod_lok, nr_lok, nr_skl, dan_p FROM skl_cz_tmp");
                                        db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + record[0]);
                                        db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                        //
                                        //TO DUMP BEHIND THE WALL
                                        //

                                        break;

                                    case Enums.Action.Usuń:
                                        place = db.places.FirstOrDefault(p => p.nr_system == id);

                                        foreach (DataAccess.RentComponentOfPlace component in db.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                            db.rentComponentsOfPlaces.Remove(component);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfPlaces.Remove(attributeOfPlace);

                                        db.places.Remove(place);

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + record[1] + " AND nr_lok=" + record[2]);
                                        db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + record[0]);
                                        //
                                        //TO DUMP BEHIND THE WALL
                                        //

                                        break;

                                    case Enums.Action.Przenieś:
                                        DataAccess.InactivePlace inactivePlace = new DataAccess.InactivePlace();
                                        place = db.places.FirstOrDefault(p => p.nr_system == id);

                                        db.places.Remove(place);

                                        record = place.AllFields();

                                        DataAccess.Place.Validate(action, record);
                                        inactivePlace.Set(record);
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

                                    record = inactivePlace.AllFields();

                                    DataAccess.Place.Validate(action, record);
                                    activePlace.Set(record);
                                    db.places.Add(activePlace);

                                    break;
                            }

                            break;

                        case Enums.Table.Tenants:
                            DataAccess.ActiveTenant tenant;
                            nominativeCase = "najemca";
                            genitiveCase = "najemcy";

                            record = new string[]
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

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        tenant = new DataAccess.ActiveTenant();

                                        tenant.Set(record);
                                        db.tenants.Add(tenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                        {
                                            attributeOfTenant.kod_powiaz = record[0];

                                            db.attributesOfTenants.Add(attributeOfTenant);
                                        }

                                        break;

                                    case Enums.Action.Edytuj:
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        tenant.Set(record);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfTenants.Remove(attributeOfTenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                            db.attributesOfTenants.Add(attributeOfTenant);

                                        break;

                                    case Enums.Action.Usuń:
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        db.tenants.Remove(tenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfTenants.Remove(attributeOfTenant);

                                        break;

                                    case Enums.Action.Przenieś:
                                        DataAccess.InactiveTenant inactiveTenant = new DataAccess.InactiveTenant();
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        db.tenants.Remove(tenant);

                                        record = tenant.AllFields();

                                        inactiveTenant.Set(record);
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

                                    record = inactiveTenant.AllFields();

                                    activeTenant.Set(record);
                                    db.tenants.Add(activeTenant);

                                    break;
                            }

                            break;

                        case Enums.Table.RentComponents:
                            DataAccess.RentComponent rentComponent;
                            nominativeCase = "składnik opłat";
                            genitiveCase = "składnika opłat";

                            record = new string[]
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

                            if (record[3] == "6")
                                record = record.ToList().Concat(new string[] 
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
                                record = record.ToList().Concat(new string[] { "", "", "", "", "", "", "", "", "", "" }).ToArray();

                            validationResult = DataAccess.RentComponent.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        rentComponent = new DataAccess.RentComponent();

                                        rentComponent.Set(record);
                                        db.rentComponents.Add(rentComponent);

                                        break;

                                    case Enums.Action.Edytuj:
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        rentComponent.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        db.rentComponents.Remove(rentComponent);

                                        break;
                                }

                            break;

                        case Enums.Table.Communities:
                            DataAccess.Community community;
                            nominativeCase = "wspólnota";
                            genitiveCase = "wspólnoty";

                            record = new string[]
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

                            validationResult = DataAccess.Community.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        community = new DataAccess.Community();

                                        community.Set(record);
                                        db.communities.Add(community);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                        {
                                            attributeOfCommunity.kod_powiaz = record[0];

                                            db.attributesOfCommunities.Add(attributeOfCommunity);
                                        }

                                        break;

                                    case Enums.Action.Edytuj:
                                        community = db.communities.FirstOrDefault(c => c.kod == id);

                                        community.Set(record);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfCommunities.Remove(attributeOfCommunity);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                            db.attributesOfCommunities.Add(attributeOfCommunity);

                                        break;

                                    case Enums.Action.Usuń:
                                        community = db.communities.FirstOrDefault(c => c.kod == id);

                                        db.communities.Remove(community);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfCommunities.Remove(attributeOfCommunity);

                                        break;
                                }

                            break;

                        case Enums.Table.TypesOfPlace:
                            DataAccess.TypeOfPlace typeOfPlace;
                            nominativeCase = "typ lokali";
                            genitiveCase = "typu lokali";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("typ_lok")
                            };

                            validationResult = DataAccess.TypeOfPlace.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfPlace = new DataAccess.TypeOfPlace();

                                        typeOfPlace.Set(record);
                                        db.typesOfPlace.Add(typeOfPlace);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        typeOfPlace.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        db.typesOfPlace.Remove(typeOfPlace);

                                        break;
                                }

                            break;

                        case Enums.Table.TypesOfKitchen:
                            DataAccess.TypeOfKitchen typeOfKitchen;
                            nominativeCase = "typ kuchni";
                            genitiveCase = "typu kuchni";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("typ_kuch")
                            };

                            validationResult = DataAccess.TypeOfKitchen.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfKitchen = new DataAccess.TypeOfKitchen();

                                        typeOfKitchen.Set(record);
                                        db.typesOfKitchen.Add(typeOfKitchen);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        typeOfKitchen.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        db.typesOfKitchen.Remove(typeOfKitchen);

                                        break;
                                }

                            break;

                        case Enums.Table.TypesOfTenant:
                            DataAccess.TypeOfTenant typeOfTenant;
                            nominativeCase = "rodzaj najemcy";
                            genitiveCase = "rodzaju najemcy";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("r_najemcy")
                            };

                            validationResult = DataAccess.TypeOfTenant.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfTenant = new DataAccess.TypeOfTenant();

                                        typeOfTenant.Set(record);
                                        db.typesOfTenant.Add(typeOfTenant);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        typeOfTenant.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        db.typesOfTenant.Remove(typeOfTenant);

                                        break;

                                }

                            break;

                        case Enums.Table.Titles:
                            DataAccess.Title title;
                            nominativeCase = "tytuł prawny do lokali";
                            genitiveCase = "tytułu prawnego do lokali";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("tyt_prawny")
                            };

                            validationResult = DataAccess.Title.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        title = new DataAccess.Title();

                                        title.Set(record);
                                        db.titles.Add(title);

                                        break;

                                    case Enums.Action.Edytuj:
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        title.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        db.titles.Remove(title);

                                        break;
                                }

                            break;

                        case Enums.Table.TypesOfPayment:
                            DataAccess.TypeOfPayment typeOfPayment;
                            nominativeCase = "rodzaj wpłaty lub wypłaty";
                            genitiveCase = "rodzaju wpłaty lub wypłaty";

                            record = new string[]
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

                            validationResult = DataAccess.TypeOfPayment.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfPayment = new DataAccess.TypeOfPayment();

                                        typeOfPayment.Set(record);
                                        db.typesOfPayment.Add(typeOfPayment);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        typeOfPayment.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        db.typesOfPayment.Remove(typeOfPayment);

                                        break;
                                }

                            break;

                        case Enums.Table.GroupsOfRentComponents:
                            DataAccess.GroupOfRentComponents groupOfRentComponents;
                            nominativeCase = "grupa składników czynszu";
                            genitiveCase = "grupy składników czynszu";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("nazwa")
                            };

                            validationResult = DataAccess.GroupOfRentComponents.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        groupOfRentComponents = new DataAccess.GroupOfRentComponents();

                                        groupOfRentComponents.Set(record);
                                        db.groupsOfRentComponents.Add(groupOfRentComponents);

                                        break;

                                    case Enums.Action.Edytuj:
                                        groupOfRentComponents = db.groupsOfRentComponents.FirstOrDefault(g => g.kod == id);

                                        groupOfRentComponents.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        groupOfRentComponents = db.groupsOfRentComponents.FirstOrDefault(g => g.kod == id);

                                        db.groupsOfRentComponents.Remove(groupOfRentComponents);

                                        break;
                                }

                            break;

                        case Enums.Table.FinancialGroups:
                            DataAccess.FinancialGroup financialGroup;
                            nominativeCase = "grupa finansowa";
                            genitiveCase = "grupy finansowej";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("k_syn"),
                                GetParamValue<string>("nazwa")
                            };

                            validationResult = DataAccess.FinancialGroup.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        financialGroup = new DataAccess.FinancialGroup();

                                        financialGroup.Set(record);
                                        db.financialGroups.Add(financialGroup);

                                        break;

                                    case Enums.Action.Edytuj:
                                        financialGroup = db.financialGroups.FirstOrDefault(r => r.kod == id);

                                        financialGroup.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        financialGroup = db.financialGroups.FirstOrDefault(r => r.kod == id);

                                        db.financialGroups.Remove(financialGroup);

                                        break;
                                }

                            break;

                        case Enums.Table.VatRates:
                            DataAccess.VatRate vatRate;
                            nominativeCase = "stawka VAT";
                            genitiveCase = "stawki VAt";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("nazwa"),
                                GetParamValue<string>("symb_fisk")
                            };

                            validationResult = DataAccess.VatRate.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        vatRate = new DataAccess.VatRate();

                                        vatRate.Set(record);
                                        db.vatRates.Add(vatRate);

                                        break;

                                    case Enums.Action.Edytuj:
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        vatRate.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        db.vatRates.Remove(vatRate);

                                        break;
                                }

                            break;

                        case Enums.Table.Attributes:
                            DataAccess.Attribute attribute;
                            nominativeCase = "cecha obiektów";
                            genitiveCase = "cechy obiektów";

                            record = new string[]
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

                            validationResult = DataAccess.Attribute.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        attribute = new DataAccess.Attribute();

                                        attribute.Set(record);
                                        db.attributes.Add(attribute);

                                        break;

                                    case Enums.Action.Edytuj:
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        attribute.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        db.attributes.Remove(attribute);

                                        break;
                                }

                            break;

                        case Enums.Table.Users:
                            DataAccess.User user;
                            nominativeCase = "użytkownik";
                            genitiveCase = "użytkownika";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("symbol"),
                                GetParamValue<string>("nazwisko"),
                                GetParamValue<string>("imie"),
                                GetParamValue<string>("haslo"),
                                GetParamValue<string>("haslo2")
                            };

                            validationResult = DataAccess.User.Validate(action, ref record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        user = new DataAccess.User();

                                        user.Set(record);
                                        db.users.Add(user);

                                        break;

                                    case Enums.Action.Edytuj:
                                        user = db.users.FirstOrDefault(u => u.__record == id);

                                        user.Set(record);

                                        break;

                                    case Enums.Action.Usuń:
                                        user = db.users.FirstOrDefault(u => u.__record == id);

                                        db.users.Remove(user);

                                        break;
                                }

                            break;

                        case Enums.Table.TenantTurnovers:
                            DataAccess.Turnover turnOver;
                            nominativeCase = "obrót najemcy";
                            genitiveCase = "obrotu najemcy";

                            record = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("suma"),
                                GetParamValue<string>("data_obr"),
                                GetParamValue<string>("kod_wplat"),
                                GetParamValue<string>("nr_dowodu"),
                                GetParamValue<string>("pozycja_d"),
                                GetParamValue<string>("uwagi")
                            };

                            validationResult = DataAccess.Turnover.Validate(record);

                            break;
                    }

                    if (validationResult == String.Empty)
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

                Session["values"] = record;
            }
            else
                placeOfButtons.Controls.Add(new MyControls.Button("button", "Back", "Powrót", backUrl));
        }
    }
}