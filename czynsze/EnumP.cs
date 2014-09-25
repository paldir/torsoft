using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public static class EnumP
    {
        public enum Action { Dodaj, Edytuj, Usuń, Przeglądaj };
        public enum Table
        {
            Buildings,
            Places,
            InactivePlaces,
            Tenants,
            InactiveTenants,
            RentComponents,
            TypesOfPlace,
            TypesOfKitchen,
            TypesOfTenant,
            Titles,
            Communities,
            TypesOfPayment,
            VatRates,
            Attributes
        };

        public enum Report { PlacesInEachBuilding };
        public enum ReportFormat { Pdf, Csv };
        public enum SortOrder { Asc, Desc };
        public enum AttributeOf { Place, Tenant, Building, Community };
    }
}