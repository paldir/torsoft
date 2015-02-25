﻿using System;
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

                            validationResult = DataAccess.Building.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        building = new DataAccess.Building();

                                        building.Set(recordFields);
                                        db.buildings.Add(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                        {
                                            attributeOfBuilding.kod_powiaz = recordFields[0];

                                            db.attributesOfBuildings.Add(attributeOfBuilding);
                                        }

                                        break;

                                    case Enums.Action.Edytuj:
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        building.Set(recordFields);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                            db.attributesOfBuildings.Add(attributeOfBuilding);

                                        break;

                                    case Enums.Action.Usuń:
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        db.buildings.Remove(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        break;
                                }

                            break;

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

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT " + recordFields[1] + ", " + recordFields[2] + ", nr_skl, dan_p FROM skl_cz_tmp");
                                        db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                        //
                                        //TO DUMP BEHIND THE WALL
                                        //

                                        break;

                                    case Enums.Action.Edytuj:
                                        place = db.places.FirstOrDefault(p => p.nr_system == id);

                                        place.Set(recordFields);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(recordFields[0])))
                                            db.attributesOfPlaces.Remove(attributeOfPlace);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                            db.attributesOfPlaces.Add(attributeOfPlace);

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + recordFields[1] + " AND nr_lok=" + recordFields[2]);
                                        db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT kod_lok, nr_lok, nr_skl, dan_p FROM skl_cz_tmp");
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
                                        db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + recordFields[1] + " AND nr_lok=" + recordFields[2]);
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

                        case Enums.Table.RentComponents:
                            DataAccess.RentComponent rentComponent;
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

                            validationResult = DataAccess.RentComponent.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        rentComponent = new DataAccess.RentComponent();

                                        rentComponent.Set(recordFields);
                                        db.rentComponents.Add(rentComponent);

                                        break;

                                    case Enums.Action.Edytuj:
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        rentComponent.Set(recordFields);

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

                        case Enums.Table.TypesOfPlace:
                            //DataAccess.TypeOfPlace typeOfPlace;
                            DataAccess.IRecord record = new DataAccess.TypeOfPlace();
                            nominativeCase = "typ lokali";
                            genitiveCase = "typu lokali";

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("typ_lok")
                            };

                            validationResult = record.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        /*typeOfPlace = new DataAccess.TypeOfPlace();

                                        typeOfPlace.Set(record);
                                        db.typesOfPlace.Add(typeOfPlace);*/

                                        record.Set(recordFields);
                                        record.Add(db);

                                        break;

                                    case Enums.Action.Edytuj:
                                        /*typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        typeOfPlace.Set(record);*/

                                        record = record.Find(db, id);

                                        record.Set(recordFields);

                                        break;

                                    case Enums.Action.Usuń:
                                        /*typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        db.typesOfPlace.Remove(typeOfPlace);*/

                                        record = record.Find(db, id);

                                        record.Remove(db);

                                        break;
                                }

                            break;

                        case Enums.Table.TypesOfKitchen:
                            DataAccess.TypeOfKitchen typeOfKitchen;
                            nominativeCase = "typ kuchni";
                            genitiveCase = "typu kuchni";

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("typ_kuch")
                            };

                            validationResult = DataAccess.TypeOfKitchen.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfKitchen = new DataAccess.TypeOfKitchen();

                                        typeOfKitchen.Set(recordFields);
                                        db.typesOfKitchen.Add(typeOfKitchen);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        typeOfKitchen.Set(recordFields);

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

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("r_najemcy")
                            };

                            validationResult = DataAccess.TypeOfTenant.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfTenant = new DataAccess.TypeOfTenant();

                                        typeOfTenant.Set(recordFields);
                                        db.typesOfTenant.Add(typeOfTenant);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        typeOfTenant.Set(recordFields);

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

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("tyt_prawny")
                            };

                            validationResult = DataAccess.Title.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        title = new DataAccess.Title();

                                        title.Set(recordFields);
                                        db.titles.Add(title);

                                        break;

                                    case Enums.Action.Edytuj:
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        title.Set(recordFields);

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

                            validationResult = DataAccess.TypeOfPayment.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        typeOfPayment = new DataAccess.TypeOfPayment();

                                        typeOfPayment.Set(recordFields);
                                        db.typesOfPayment.Add(typeOfPayment);

                                        break;

                                    case Enums.Action.Edytuj:
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        typeOfPayment.Set(recordFields);

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

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("nazwa")
                            };

                            validationResult = DataAccess.GroupOfRentComponents.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        groupOfRentComponents = new DataAccess.GroupOfRentComponents();

                                        groupOfRentComponents.Set(recordFields);
                                        db.groupsOfRentComponents.Add(groupOfRentComponents);

                                        break;

                                    case Enums.Action.Edytuj:
                                        groupOfRentComponents = db.groupsOfRentComponents.FirstOrDefault(g => g.kod == id);

                                        groupOfRentComponents.Set(recordFields);

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

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("k_syn"),
                                GetParamValue<string>("nazwa")
                            };

                            validationResult = DataAccess.FinancialGroup.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        financialGroup = new DataAccess.FinancialGroup();

                                        financialGroup.Set(recordFields);
                                        db.financialGroups.Add(financialGroup);

                                        break;

                                    case Enums.Action.Edytuj:
                                        financialGroup = db.financialGroups.FirstOrDefault(r => r.kod == id);

                                        financialGroup.Set(recordFields);

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

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("nazwa"),
                                GetParamValue<string>("symb_fisk")
                            };

                            validationResult = DataAccess.VatRate.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        vatRate = new DataAccess.VatRate();

                                        vatRate.Set(recordFields);
                                        db.vatRates.Add(vatRate);

                                        break;

                                    case Enums.Action.Edytuj:
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        vatRate.Set(recordFields);

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

                            validationResult = DataAccess.Attribute.Validate(action, recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        attribute = new DataAccess.Attribute();

                                        attribute.Set(recordFields);
                                        db.attributes.Add(attribute);

                                        break;

                                    case Enums.Action.Edytuj:
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        attribute.Set(recordFields);

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

                            recordFields = new string[]
                            {
                                GetParamValue<string>("id"),
                                GetParamValue<string>("symbol"),
                                GetParamValue<string>("nazwisko"),
                                GetParamValue<string>("imie"),
                                GetParamValue<string>("haslo"),
                                GetParamValue<string>("haslo2")
                            };

                            validationResult = DataAccess.User.Validate(action, ref recordFields);

                            if (String.IsNullOrEmpty(validationResult))
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        user = new DataAccess.User();

                                        user.Set(recordFields);
                                        db.users.Add(user);

                                        break;

                                    case Enums.Action.Edytuj:
                                        user = db.users.FirstOrDefault(u => u.__record == id);

                                        user.Set(recordFields);

                                        break;

                                    case Enums.Action.Usuń:
                                        user = db.users.FirstOrDefault(u => u.__record == id);

                                        db.users.Remove(user);

                                        break;
                                }

                            break;

                        case Enums.Table.TenantTurnovers:
                            DataAccess.Turnover turnOver = null;
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

                            validationResult = DataAccess.Turnover.Validate(recordFields, action);

                            if (String.IsNullOrEmpty(validationResult))
                            {
                                switch (action)
                                {
                                    case Enums.Action.Dodaj:
                                        switch (Hello.CurrentSet)
                                        {
                                            case Enums.SettlementTable.Czynsze:
                                                turnOver = new DataAccess.TurnoverFrom1stSet();

                                                break;

                                            case Enums.SettlementTable.SecondSet:
                                                turnOver = new DataAccess.TurnoverFrom2ndSet();

                                                break;

                                            case Enums.SettlementTable.ThirdSet:
                                                turnOver = new DataAccess.TurnoverFrom3rdSet();

                                                break;
                                        }

                                        turnOver.Set(recordFields);

                                        switch (Hello.CurrentSet)
                                        {
                                            case Enums.SettlementTable.Czynsze:
                                                db.turnoversFor14.Add((DataAccess.TurnoverFrom1stSet)turnOver);

                                                break;

                                            case Enums.SettlementTable.SecondSet:
                                                db.turnoversFor14From2ndSet.Add((DataAccess.TurnoverFrom2ndSet)turnOver);

                                                break;

                                            case Enums.SettlementTable.ThirdSet:
                                                db.turnoversFor14From3rdSet.Add((DataAccess.TurnoverFrom3rdSet)turnOver);

                                                break;
                                        }

                                        break;

                                    case Enums.Action.Edytuj:
                                        switch (Hello.CurrentSet)
                                        {
                                            case Enums.SettlementTable.Czynsze:
                                                turnOver = db.turnoversFor14.FirstOrDefault(t => t.__record == id);

                                                break;

                                            case Enums.SettlementTable.SecondSet:
                                                turnOver = db.turnoversFor14From2ndSet.FirstOrDefault(t => t.__record == id);

                                                break;

                                            case Enums.SettlementTable.ThirdSet:
                                                turnOver = db.turnoversFor14From3rdSet.FirstOrDefault(t => t.__record == id);

                                                break;
                                        }

                                        turnOver.Set(recordFields);

                                        break;

                                    case Enums.Action.Usuń:
                                        switch (Hello.CurrentSet)
                                        {
                                            case Enums.SettlementTable.Czynsze:
                                                turnOver = db.turnoversFor14.FirstOrDefault(t => t.__record == id);

                                                db.turnoversFor14.Remove((DataAccess.TurnoverFrom1stSet)turnOver);

                                                break;

                                            case Enums.SettlementTable.SecondSet:
                                                turnOver = db.turnoversFor14From2ndSet.FirstOrDefault(t => t.__record == id);

                                                db.turnoversFor14From2ndSet.Remove((DataAccess.TurnoverFrom2ndSet)turnOver);

                                                break;

                                            case Enums.SettlementTable.ThirdSet:
                                                turnOver = db.turnoversFor14From3rdSet.FirstOrDefault(t => t.__record == id);

                                                db.turnoversFor14From3rdSet.Remove((DataAccess.TurnoverFrom3rdSet)turnOver);

                                                break;
                                        }

                                        break;
                                }
                            }

                            backUrl = backUrl.Insert(backUrl.LastIndexOf('\''), "&id=" + recordFields[8]);

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