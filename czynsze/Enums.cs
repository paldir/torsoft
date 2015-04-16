using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public static class Enums
    {
        public enum Action
        {
            Dodaj = 1,
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
            ReceivablesAndTurnoversOfTenant,
            TenantSaldo,
            TenantTurnovers
        };

        public enum Report
        {
            PlacesInEachBuilding,
            MonthlySumOfComponent,
            ReceivablesAndTurnoversOfTenant,
            MonthlyAnalysisOfReceivablesAndTurnovers,
            DetailedAnalysisOfReceivablesAndTurnovers,
            CurrentRentAmountOfPlaces,
            CurrentRentAmountOfBuildings,
            CurrentRentAmountOfCommunities
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

        public enum RentAmount
        {
            Current,
            ForMonth
        };

        public enum ContentsOfDescriptions
        {
            Payments,
            Book,
            Water
        }
    }
}