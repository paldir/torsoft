using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public static class EnumP
    {
        public enum Action { Dodaj, Edytuj, Usuń, Przeglądaj };
        public enum Table { Buildings, Places, Tenants, RentComponents };
        public enum Report { PlacesInEachBuilding };
    }
}