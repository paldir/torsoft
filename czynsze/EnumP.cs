using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public static class EnumP
    {
        public enum Action
        {
            Dodaj,
            Edytuj,
            Usuń,
            Przeglądaj,
            Przenieś
        };

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
            Attributes,
            GroupsOfRentComponents,
            FinancialGroups,
            Users,
            ReceivablesByTenants,
            AllReceivablesOfTenant,
            NotPastReceivablesOfTenant,
            ReceivablesAndTurnoversOfTenant
        };

        public enum Report
        {
            PlacesInEachBuilding,
            MonthlySumOfComponent,
            ReceivablesAndTurnoversOfTenant,
            MonthlyAnalysisOfReceivablesAndTurnovers,
            DetailedAnalysisOfReceivablesAndTurnovers
        };

        public enum ReportFormat
        {
            Pdf,
            Csv
        };

        public enum SortOrder
        {
            Asc,
            Desc
        };

        public enum AttributeOf
        {
            Place,
            Tenant,
            Building,
            Community
        };

        public enum ReasonOfRedirectToLoginPage
        {
            IncorrectCredentials,
            NotLoggedInOrSessionExpired
        };

        public enum SettlementTable
        {
            Czynsze,
            SecondSet,
            ThirdSet
        };
    }
}