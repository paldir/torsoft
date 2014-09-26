using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class RecordValidation : System.Web.UI.Page
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
            table = (EnumP.Table)Enum.Parse(typeof(EnumP.Table), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("table"))]);
            action = (EnumP.Action)Enum.Parse(typeof(EnumP.Action), Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("action"))]);

            if (action != EnumP.Action.Dodaj)
            {
                id = Convert.ToInt16(Request.Params[Request.Params.AllKeys.FirstOrDefault(t => t.EndsWith("id"))]);

                form.Controls.Add(new ControlsP.HtmlInputHiddenP("id", id.ToString()));
            }

            form.Controls.Add(new ControlsP.HtmlInputHiddenP("table", table.ToString()));
            form.Controls.Add(new ControlsP.HtmlInputHiddenP("action", action.ToString()));

            switch (table)
            {
                case EnumP.Table.Buildings:
                    this.Title = "Edycja budynku";
                    DataAccess.Building building;

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
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        building = new DataAccess.Building();

                                        building.Set(record);
                                        db.buildings.Add(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                        {
                                            attributeOfBuilding.kod_powiaz = record[0];

                                            db.attributesOfBuildings.Add(attributeOfBuilding);
                                        }

                                        db.SaveChanges();

                                        dbWriteResult = "Budynek dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać budynku!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        building.Set(record);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in attributesOfObject)
                                            db.attributesOfBuildings.Add(attributeOfBuilding);

                                        db.SaveChanges();

                                        dbWriteResult = "Budynek zaktualizowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować budynku!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        building = db.buildings.FirstOrDefault(b => b.kod_1 == id);

                                        db.buildings.Remove(building);

                                        foreach (DataAccess.AttributeOfBuilding attributeOfBuilding in db.attributesOfBuildings.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfBuildings.Remove(attributeOfBuilding);

                                        db.SaveChanges();

                                        dbWriteResult = "Budynek usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć budynku!"; }
                                    break;
                            }
                        }
                    }
                    break;
                case EnumP.Table.Places:
                    this.Title = "Edycja lokalu";
                    DataAccess.Place place;

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

                    validationResult = DataAccess.Place.Validate(action, record);

                    if (validationResult == String.Empty)
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        place = new DataAccess.Place();

                                        place.Set(record);
                                        db.places.Add(place);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                        {
                                            attributeOfPlace.kod_powiaz = record[0];

                                            db.attributesOfPlaces.Add(attributeOfPlace);
                                        }

                                        db.SaveChanges();

                                        dbWriteResult = "Lokal dodany.";

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("INSERT INTO skl_cz(kod_lok, nr_lok, nr_skl, dan_p) SELECT " + record[1] + ", " + record[2] + ", nr_skl, dan_p FROM skl_cz_tmp");
                                        db.Database.ExecuteSqlCommand("INSERT INTO pliki(id, plik, nazwa_pliku, opis, nr_system) SELECT id, plik, nazwa_pliku, opis, nr_system FROM pliki_tmp");
                                        //
                                        //TO DUMP BEHIND THE WALL
                                        //
                                    }
                                    catch { dbWriteResult = "Nie można dodać lokalu!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        place = db.places.FirstOrDefault(p => p.nr_system == id);

                                        place.Set(record);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfPlaces.Remove(attributeOfPlace);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in attributesOfObject)
                                            db.attributesOfPlaces.Add(attributeOfPlace);

                                        db.SaveChanges();

                                        dbWriteResult = "Lokal wyedytowany.";

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
                                    }
                                    catch { dbWriteResult = "Nie można edytować lokalu!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        place = db.places.FirstOrDefault(p => p.nr_system == id);

                                        foreach (DataAccess.RentComponentOfPlace component in db.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                                            db.rentComponentsOfPlaces.Remove(component);

                                        foreach (DataAccess.AttributeOfPlace attributeOfPlace in db.attributesOfPlaces.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfPlaces.Remove(attributeOfPlace);

                                        db.places.Remove(place);
                                        db.SaveChanges();

                                        dbWriteResult = "Lokal usunięty.";

                                        //
                                        //CXP PART
                                        //
                                        db.Database.ExecuteSqlCommand("DELETE FROM skl_cz WHERE kod_lok=" + record[1] + " AND nr_lok=" + record[2]);
                                        db.Database.ExecuteSqlCommand("DELETE FROM pliki WHERE nr_system=" + record[0]);
                                        //
                                        //TO DUMP BEHIND THE WALL
                                        //
                                    }
                                    catch { dbWriteResult = "Nie można usunąć lokalu!"; }
                                    break;
                            }
                        }
                    }
                    break;
                case EnumP.Table.Tenants:
                    this.Title = "Edycja najemcy";
                    DataAccess.Tenant tenant;

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
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        tenant = new DataAccess.Tenant();

                                        tenant.Set(record);
                                        db.tenants.Add(tenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                        {
                                            attributeOfTenant.kod_powiaz = record[0];

                                            db.attributesOfTenants.Add(attributeOfTenant);
                                        }

                                        db.SaveChanges();

                                        dbWriteResult = "Najemca dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać najemcy!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        tenant.Set(record);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfTenants.Remove(attributeOfTenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in attributesOfObject)
                                            db.attributesOfTenants.Add(attributeOfTenant);

                                        db.SaveChanges();

                                        dbWriteResult = "Najemca wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować najemcy!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        tenant = db.tenants.FirstOrDefault(t => t.nr_kontr == id);

                                        db.tenants.Remove(tenant);

                                        foreach (DataAccess.AttributeOfTenant attributeOfTenant in db.attributesOfTenants.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfTenants.Remove(attributeOfTenant);

                                        db.SaveChanges();

                                        dbWriteResult = "Najemca usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć najemcy!"; }
                                    break;
                            }
                        }
                    }
                    break;
                case EnumP.Table.RentComponents:
                    this.Title = "Edycja składnika opłat";
                    DataAccess.RentComponent rentComponent;

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
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("data_2"))]/*,
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_00"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_01"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_02"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_03"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_04"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_05"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_06"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_07"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_08"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("stawka_09"))]*/
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
                    {
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                        {
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        rentComponent = new DataAccess.RentComponent();

                                        rentComponent.Set(record);
                                        db.rentComponents.Add(rentComponent);
                                        db.SaveChanges();

                                        dbWriteResult = "Składnik opłat dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać składnika opłat!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        rentComponent.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Składnik opłat wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować składnika opłat!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == id);

                                        db.rentComponents.Remove(rentComponent);
                                        db.SaveChanges();

                                        dbWriteResult = "Składnik opłat usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć składnika opłat!"; }
                                    break;
                            }
                        }
                    }
                    break;
                case EnumP.Table.Communities:
                    this.Title = "Edycja wspólnoty";
                    DataAccess.Community community;

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
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        community = new DataAccess.Community();

                                        community.Set(record);
                                        db.communities.Add(community);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                        {
                                            attributeOfCommunity.kod_powiaz = record[0];

                                            db.attributesOfCommunities.Add(attributeOfCommunity);
                                        }

                                        db.SaveChanges();

                                        dbWriteResult = "Wspólnota dodana.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać wspólnoty!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        community = db.communities.FirstOrDefault(c => c.kod == id);

                                        community.Set(record);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfCommunities.Remove(attributeOfCommunity);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in attributesOfObject)
                                            db.attributesOfCommunities.Add(attributeOfCommunity);

                                        db.SaveChanges();

                                        dbWriteResult = "Wspólnota wyedytowana.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować wspólnoty!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        community = db.communities.FirstOrDefault(c => c.kod == id);

                                        db.communities.Remove(community);

                                        foreach (DataAccess.AttributeOfCommunity attributeOfCommunity in db.attributesOfCommunities.ToList().Where(a => Convert.ToInt16(a.kod_powiaz) == Convert.ToInt16(record[0])))
                                            db.attributesOfCommunities.Remove(attributeOfCommunity);

                                        db.SaveChanges();

                                        dbWriteResult = "Wspólnota usunięta.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć wspólnoty!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.TypesOfPlace:
                    this.Title = "Edycja typu lokali";
                    DataAccess.TypeOfPlace typeOfPlace;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("typ_lok"))]
                    };

                    validationResult = DataAccess.TypeOfPlace.Validate(action, record);

                    if (validationResult == String.Empty)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        typeOfPlace = new DataAccess.TypeOfPlace();

                                        typeOfPlace.Set(record);
                                        db.typesOfPlace.Add(typeOfPlace);
                                        db.SaveChanges();

                                        dbWriteResult = "Typ lokali dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać typu lokali!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        typeOfPlace.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Typ lokalu wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować typu lokalu!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        typeOfPlace = db.typesOfPlace.FirstOrDefault(t => t.kod_typ == id);

                                        db.typesOfPlace.Remove(typeOfPlace);
                                        db.SaveChanges();

                                        dbWriteResult = "Typ lokalu usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć typu lokalu!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.TypesOfKitchen:
                    this.Title = "Edycja rodzaju kuchni";
                    DataAccess.TypeOfKitchen typeOfKitchen;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("typ_kuch"))]
                    };

                    validationResult = DataAccess.TypeOfKitchen.Validate(action, record);

                    if (validationResult == String.Empty)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        typeOfKitchen = new DataAccess.TypeOfKitchen();

                                        typeOfKitchen.Set(record);
                                        db.typesOfKitchen.Add(typeOfKitchen);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj kuchni dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać rodzaju kuchni!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        typeOfKitchen.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj kuchni wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować rodzaju kuchni!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        typeOfKitchen = db.typesOfKitchen.FirstOrDefault(t => t.kod_kuch == id);

                                        db.typesOfKitchen.Remove(typeOfKitchen);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj kuchni usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć rodzaju kuchni!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.TypesOfTenant:
                    this.Title = "Edycja rodzaju najemcy";
                    DataAccess.TypeOfTenant typeOfTenant;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("r_najemcy"))]
                    };

                    validationResult = DataAccess.TypeOfTenant.Validate(action, record);

                    if (validationResult == String.Empty)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        typeOfTenant = new DataAccess.TypeOfTenant();

                                        typeOfTenant.Set(record);
                                        db.typesOfTenant.Add(typeOfTenant);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj najemcy dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać rodzaju najemcy!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        typeOfTenant.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj najemcy wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować rodzaju najemcy!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        typeOfTenant = db.typesOfTenant.FirstOrDefault(t => t.kod_najem == id);

                                        db.typesOfTenant.Remove(typeOfTenant);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj najemcy usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć rodzaju najemcy!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.Titles:
                    this.Title = "Edycja tytułu prawnego do lokali";
                    DataAccess.Title title;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("tyt_prawny"))]
                    };

                    validationResult = DataAccess.Title.Validate(action, record);

                    if (validationResult == String.Empty)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        title = new DataAccess.Title();

                                        title.Set(record);
                                        db.titles.Add(title);
                                        db.SaveChanges();

                                        dbWriteResult = "Tytuł prawny do lokali dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać tytułu prawnego do lokali!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        title.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Tytuł prawny do lokali wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować tytułu prawnego do lokali!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        title = db.titles.FirstOrDefault(t => t.kod_praw == id);

                                        db.titles.Remove(title);
                                        db.SaveChanges();

                                        dbWriteResult = "Tytuł prawny do lokali usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć tytułu prawnego do lokali!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.TypesOfPayment:
                    this.Title = "Edycja rodzaju wpłaty lub wypłaty";
                    DataAccess.TypeOfPayment typeOfPayment;

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
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        typeOfPayment = new DataAccess.TypeOfPayment();

                                        typeOfPayment.Set(record);
                                        db.typesOfPayment.Add(typeOfPayment);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj wpłaty lub wypłaty dodany.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać rodzaju wpłaty lub wypłaty!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        typeOfPayment.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj wpłaty lub wypłaty wyedytowany.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować rodzaju wpłaty lub wypłaty!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        typeOfPayment = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == id);

                                        db.typesOfPayment.Remove(typeOfPayment);
                                        db.SaveChanges();

                                        dbWriteResult = "Rodzaj wpłaty lub wypłaty usunięty.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć rodzaju wpłaty lub wypłaty!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.VatRates:
                    this.Title = "Edycja stawki VAT";
                    DataAccess.VatRate vatRate;

                    record = new string[]
                    {
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("id"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("nazwa"))],
                        Request.Params[Request.Params.AllKeys.FirstOrDefault(k=>k.EndsWith("symb_fisk"))]
                    };

                    validationResult = DataAccess.VatRate.Validate(action, record);

                    if (validationResult == String.Empty)
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        vatRate = new DataAccess.VatRate();

                                        vatRate.Set(record);
                                        db.vatRates.Add(vatRate);
                                        db.SaveChanges();

                                        dbWriteResult = "Stawka VAT dodana.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać stawki VAT!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        vatRate.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Stawka VAT wyedytowana.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować stawki VAT!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        vatRate = db.vatRates.FirstOrDefault(r => r.__record == id);

                                        db.vatRates.Remove(vatRate);
                                        db.SaveChanges();

                                        dbWriteResult = "Stawka VAT usunięta.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć stawki VAT!"; }
                                    break;
                            }
                    break;
                case EnumP.Table.Attributes:
                    this.Title = "Edycja cechy obiektów";
                    DataAccess.Attribute attribute;

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
                        using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                            switch (action)
                            {
                                case EnumP.Action.Dodaj:
                                    try
                                    {
                                        attribute = new DataAccess.Attribute();

                                        attribute.Set(record);
                                        db.attributes.Add(attribute);
                                        db.SaveChanges();

                                        dbWriteResult = "Cecha obiektów dodana.";
                                    }
                                    catch { dbWriteResult = "Nie można dodać cechy obiektów!"; }
                                    break;
                                case EnumP.Action.Edytuj:
                                    try
                                    {
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        attribute.Set(record);
                                        db.SaveChanges();

                                        dbWriteResult = "Cecha obiektów wyedytowana.";
                                    }
                                    catch { dbWriteResult = "Nie można edytować cechy obiektów!"; }
                                    break;
                                case EnumP.Action.Usuń:
                                    try
                                    {
                                        attribute = db.attributes.FirstOrDefault(a => a.kod == id);

                                        db.attributes.Remove(attribute);
                                        db.SaveChanges();

                                        dbWriteResult = "Cecha obiektów usunięta.";
                                    }
                                    catch { dbWriteResult = "Nie można usunąć cechy obiektów!"; }
                                    break;
                            }
                    break;
            }

            form.Controls.Add(new LiteralControl(validationResult));

            if (dbWriteResult != null)
                form.Controls.Add(new LiteralControl(dbWriteResult + "<br />"));

            if (validationResult != String.Empty || (dbWriteResult != null && dbWriteResult.Last() == '!'))
            {
                form.Controls.Add(new ControlsP.ButtonP("button", "Repair", "Popraw", "Record.aspx"));
                form.Controls.Add(new ControlsP.ButtonP("button", "Cancel", "Anuluj", "List.aspx"));

                Session["values"] = record;
            }
            else
                form.Controls.Add(new ControlsP.ButtonP("button", "Back", "Powrót", "List.aspx"));
        }
    }
}