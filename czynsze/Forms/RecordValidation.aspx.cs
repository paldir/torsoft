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
        EnumP.Table table;
        EnumP.Action action;
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
            table = GetParamValue<EnumP.Table>("table");
            //action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("action"))]);
            action = GetParamValue<EnumP.Action>("action");
            string backUrl = "javascript: Load('List.aspx?table=" + table + "')";
            string nominativeCase = String.Empty;
            string genitiveCase = String.Empty;

            Dictionary<EnumP.Action, string> dictionaryOfActionInfinitives = new Dictionary<EnumP.Action, string>()
            {
                { EnumP.Action.Dodaj, "dodać" },
                { EnumP.Action.Edytuj, "edytować" },
                { EnumP.Action.Przenieś, "przenieść" },
                { EnumP.Action.Usuń, "usunąć" }
            };

            Dictionary<EnumP.Action, string> dictionaryOfActionParticiples = new Dictionary<EnumP.Action, string>()
            {
                { EnumP.Action.Dodaj, "dodany" },
                { EnumP.Action.Edytuj, "wyedytowany" },
                { EnumP.Action.Przenieś, "przeniesiony" },
                { EnumP.Action.Usuń, "usunięty" }
            };

            if (action != EnumP.Action.Dodaj)
            {
                //id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("id"))]);
                id = GetParamValue<int>("id");

                form.Controls.Add(new ControlsP.HtmlInputHidden("id", id.ToString()));
            }

            form.Controls.Add(new ControlsP.HtmlInputHidden("table", table.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHidden("action", action.ToString()));

            try
            {
                using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                {
                    switch (table)
                    {
                        case EnumP.Table.Buildings:
                            DataAccess.Building building;
                            nominativeCase = "budynek";
                            genitiveCase = "budynku";

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
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        building = new DataAccess.Building();

                                        building.Set(record);
                                        db.buildings.Add(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                        {
                                            attributeOfBuilding.kod_powiaz = record[0];

                                            db.attributesOfBuildings.Add(attributeOfBuilding);
                                        }

                                        break;

                                    case EnumP.Action.Edytuj:
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        building.Set(record);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                            db.attributesOfBuildings.Add(attributeOfBuilding);

                                        break;

                                    case EnumP.Action.Usuń:
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        db.buildings.Remove(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        break;
                                }

                            break;

                        case EnumP.Table.Places:
                            DataAccess.ActivePlace place;
                            nominativeCase = "lokal";
                            genitiveCase = "lokalu";

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
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("kod_kuch"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("nr_kontr"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("il_osob"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("kod_praw"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("uwagi"))]
                        };

                            validationResult = DataAccess.ActivePlace.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
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

                                    case EnumP.Action.Edytuj:
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

                                    case EnumP.Action.Usuń:
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

                                    case EnumP.Action.Przenieś:
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

                        case EnumP.Table.InactivePlaces:
                            validationResult = String.Empty;
                            nominativeCase = "lokal (nieaktywny)";
                            genitiveCase = "lokalu (nieaktywnego)";

                            switch (action)
                            {
                                case EnumP.Action.Przenieś:
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

                        case EnumP.Table.Tenants:
                            DataAccess.ActiveTenant tenant;
                            nominativeCase = "najemca";
                            genitiveCase = "najemcy";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("kod_najem"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("nazwisko"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("imie"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("adres_1"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("adres_2"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("nr_dow"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("pesel"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("nazwa_z"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("e_mail"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("l__has"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("uwagi"))]
                        };

                            validationResult = "";

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        tenant = new DataAccess.ActiveTenant();

                                        tenant.Set(record);
                                        db.tenants.Add(tenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                        {
                                            attributeOfTenant.kod_powiaz = record[0];

                                            db.attributesOfTenants.Add(attributeOfTenant);
                                        }

                                        break;

                                    case EnumP.Action.Edytuj:
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        tenant.Set(record);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfTenants.Remove(attributeOfTenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                            db.attributesOfTenants.Add(attributeOfTenant);

                                        break;

                                    case EnumP.Action.Usuń:
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        db.tenants.Remove(tenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfTenants.Remove(attributeOfTenant);

                                        break;

                                    case EnumP.Action.Przenieś:
                                        DataAccess.InactiveTenant inactiveTenant = new DataAccess.InactiveTenant();
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        db.tenants.Remove(tenant);

                                        record = tenant.AllFields();

                                        inactiveTenant.Set(record);
                                        db.inactiveTenants.Add(inactiveTenant);

                                        break;
                                }

                            break;

                        case EnumP.Table.InactiveTenants:
                            validationResult = String.Empty;
                            nominativeCase = "najemca (nieaktywny)";
                            genitiveCase = "najemcy (nieaktywnego)";

                            switch (action)
                            {
                                case EnumP.Action.Przenieś:
                                    DataAccess.ActiveTenant activeTenant = new DataAccess.ActiveTenant();
                                    DataAccess.InactiveTenant inactiveTenant = db.inactiveTenants.FirstOrDefault(t => t.nr_kontr == id);

                                    db.inactiveTenants.Remove(inactiveTenant);

                                    record = inactiveTenant.AllFields();

                                    activeTenant.Set(record);
                                    db.tenants.Add(activeTenant);

                                    break;
                            }

                            break;

                        case EnumP.Table.RentComponents:
                            DataAccess.RentComponent rentComponent;
                            nominativeCase = "składnik opłat";
                            genitiveCase = "składnika opłat";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("nazwa"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("rodz_e"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("s_zaplat"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_inf"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("typ_skl"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("data_1"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("data_2"))]
                        };

                            if (record[3] == "6")
                                record = record.ToList().Concat(new string[] 
                            {
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_00"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_01"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_02"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_03"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_04"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_05"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_06"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_07"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_08"))],
                                Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_09"))]
                            }).ToArray();
                            else
                                record = record.ToList().Concat(new string[] { "", "", "", "", "", "", "", "", "", "" }).ToArray();

                            validationResult = DataAccess.RentComponent.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        rentComponent = new DataAccess.RentComponent();

                                        rentComponent.Set(record);
                                        db.rentComponents.Add(rentComponent);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        rentComponent.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        db.rentComponents.Remove(rentComponent);

                                        break;
                                }

                            break;

                        case EnumP.Table.Communities:
                            DataAccess.Community community;
                            nominativeCase = "wspólnota";
                            genitiveCase = "wspólnoty";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("il_bud"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("il_miesz"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa_pel"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa_skr"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("adres"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("adres_2"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nr1_konta"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nr2_konta"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nr3_konta"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("sciezka_fk"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("uwagi"))]
                        };

                            validationResult = DataAccess.Community.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        community = new DataAccess.Community();

                                        community.Set(record);
                                        db.communities.Add(community);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                        {
                                            attributeOfCommunity.kod_powiaz = record[0];

                                            db.attributesOfCommunities.Add(attributeOfCommunity);
                                        }

                                        break;

                                    case EnumP.Action.Edytuj:
                                        community = db.communities.FirstOrDefault(c => c.kod == id);

                                        community.Set(record);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfCommunities.Remove(attributeOfCommunity);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                            db.attributesOfCommunities.Add(attributeOfCommunity);

                                        break;

                                    case EnumP.Action.Usuń:
                                        community = db.communities.FirstOrDefault(c => c.kod == id);

                                        db.communities.Remove(community);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfCommunities.Remove(attributeOfCommunity);

                                        break;
                                }

                            break;

                        case EnumP.Table.TypesOfPlace:
                            DataAccess.TypeOfPlace typeOfPlace;
                            nominativeCase = "typ lokali";
                            genitiveCase = "typu lokali";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("typ_lok"))]
                        };

                            validationResult = DataAccess.TypeOfPlace.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        typeOfPlace = new DataAccess.TypeOfPlace();

                                        typeOfPlace.Set(record);
                                        db.typesOfPlace.Add(typeOfPlace);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        typeOfPlace.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        db.typesOfPlace.Remove(typeOfPlace);

                                        break;
                                }

                            break;

                        case EnumP.Table.TypesOfKitchen:
                            DataAccess.TypeOfKitchen typeOfKitchen;
                            nominativeCase = "typ kuchni";
                            genitiveCase = "typu kuchni";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("typ_kuch"))]
                        };

                            validationResult = DataAccess.TypeOfKitchen.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        typeOfKitchen = new DataAccess.TypeOfKitchen();

                                        typeOfKitchen.Set(record);
                                        db.typesOfKitchen.Add(typeOfKitchen);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        typeOfKitchen.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        db.typesOfKitchen.Remove(typeOfKitchen);

                                        break;
                                }

                            break;

                        case EnumP.Table.TypesOfTenant:
                            DataAccess.TypeOfTenant typeOfTenant;
                            nominativeCase = "rodzaj najemcy";
                            genitiveCase = "rodzaju najemcy";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("r_najemcy"))]
                        };

                            validationResult = DataAccess.TypeOfTenant.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        typeOfTenant = new DataAccess.TypeOfTenant();

                                        typeOfTenant.Set(record);
                                        db.typesOfTenant.Add(typeOfTenant);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        typeOfTenant.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        db.typesOfTenant.Remove(typeOfTenant);

                                        break;

                                }

                            break;

                        case EnumP.Table.Titles:
                            DataAccess.Title title;
                            nominativeCase = "tytuł prawny do lokali";
                            genitiveCase = "tytułu prawnego do lokali";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("tyt_prawny"))]
                        };

                            validationResult = DataAccess.Title.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        title = new DataAccess.Title();

                                        title.Set(record);
                                        db.titles.Add(title);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        title.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        db.titles.Remove(title);

                                        break;
                                }

                            break;

                        case EnumP.Table.TypesOfPayment:
                            DataAccess.TypeOfPayment typeOfPayment;
                            nominativeCase = "rodzaj wpłaty lub wypłaty";
                            genitiveCase = "rodzaju wpłaty lub wypłaty";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("typ_wplat"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("rodz_e"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("s_rozli"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("tn_odset"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nota_odset"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("vat"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("sww"))]
                        };

                            validationResult = DataAccess.TypeOfPayment.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        typeOfPayment = new DataAccess.TypeOfPayment();

                                        typeOfPayment.Set(record);
                                        db.typesOfPayment.Add(typeOfPayment);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        typeOfPayment.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        db.typesOfPayment.Remove(typeOfPayment);

                                        break;
                                }

                            break;

                        case EnumP.Table.GroupsOfRentComponents:
                            DataAccess.GroupOfRentComponents groupOfRentComponents;
                            nominativeCase = "grupa składników czynszu";
                            genitiveCase = "grupy składników czynszu";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa"))]
                        };

                            validationResult = DataAccess.GroupOfRentComponents.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        groupOfRentComponents = new DataAccess.GroupOfRentComponents();

                                        groupOfRentComponents.Set(record);
                                        db.groupsOfRentComponents.Add(groupOfRentComponents);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        groupOfRentComponents = db.groupsOfRentComponents.FirstOrDefault(g => g.kod == id);

                                        groupOfRentComponents.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        groupOfRentComponents = db.groupsOfRentComponents.FirstOrDefault(g => g.kod == id);

                                        db.groupsOfRentComponents.Remove(groupOfRentComponents);

                                        break;
                                }

                            break;

                        case EnumP.Table.FinancialGroups:
                            DataAccess.FinancialGroup financialGroup;
                            nominativeCase = "grupa finansowa";
                            genitiveCase = "grupy finansowej";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("k_syn"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa"))]
                        };

                            validationResult = DataAccess.FinancialGroup.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        financialGroup = new DataAccess.FinancialGroup();

                                        financialGroup.Set(record);
                                        db.financialGroups.Add(financialGroup);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        financialGroup = db.financialGroups.FirstOrDefault(r => r.kod == id);

                                        financialGroup.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        financialGroup = db.financialGroups.FirstOrDefault(r => r.kod == id);

                                        db.financialGroups.Remove(financialGroup);

                                        break;
                                }

                            break;

                        case EnumP.Table.VatRates:
                            DataAccess.VatRate vatRate;
                            nominativeCase = "stawka VAT";
                            genitiveCase = "stawki VAt";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("symb_fisk"))]
                        };

                            validationResult = DataAccess.VatRate.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        vatRate = new DataAccess.VatRate();

                                        vatRate.Set(record);
                                        db.vatRates.Add(vatRate);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        vatRate.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        db.vatRates.Remove(vatRate);

                                        break;
                                }

                            break;

                        case EnumP.Table.Attributes:
                            DataAccess.Attribute attribute;
                            nominativeCase = "cecha obiektów";
                            genitiveCase = "cechy obiektów";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nr_str"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("jedn"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("wartosc"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("uwagi"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("zb_0"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("zb_1"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("zb_2"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("zb_3"))],
                        };

                            validationResult = DataAccess.Attribute.Validate(action, record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        attribute = new DataAccess.Attribute();

                                        attribute.Set(record);
                                        db.attributes.Add(attribute);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        attribute.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        db.attributes.Remove(attribute);

                                        break;
                                }

                            break;

                        case EnumP.Table.Users:
                            DataAccess.User user;
                            nominativeCase = "użytkownik";
                            genitiveCase = "użytkownika";

                            record = new string[]
                        {
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("symbol"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwisko"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("imie"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("haslo"))],
                            Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("haslo2"))]
                        };

                            validationResult = DataAccess.User.Validate(action, ref record);

                            if (validationResult == String.Empty)
                                switch (action)
                                {
                                    case EnumP.Action.Dodaj:
                                        user = new DataAccess.User();

                                        user.Set(record);
                                        db.users.Add(user);

                                        break;

                                    case EnumP.Action.Edytuj:
                                        user = db.users.FirstOrDefault(u => u.__record == id);

                                        user.Set(record);

                                        break;

                                    case EnumP.Action.Usuń:
                                        user = db.users.FirstOrDefault(u => u.__record == id);

                                        db.users.Remove(user);

                                        break;
                                }

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
                placeOfButtons.Controls.Add(new ControlsP.Button("button", "Repair", "Popraw", "Record.aspx"));
                placeOfButtons.Controls.Add(new ControlsP.Button("button", "Cancel", "Anuluj", backUrl));

                Session["values"] = record;
            }
            else
                placeOfButtons.Controls.Add(new ControlsP.Button("button", "Back", "Powrót", backUrl));
        }
    }
}