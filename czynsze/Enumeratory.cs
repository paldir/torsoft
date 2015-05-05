using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze
{
    public static class Enumeratory
    {
        public enum Akcja
        {
            Dodaj = 1,
            Edytuj,
            Usuń,
            Przeglądaj,
            Przenieś
        };

        public enum Tabela
        {
            Budynki,
            AktywneLokale,
            NieaktywneLokale,
            AktywniNajemcy,
            NieaktywniNajemcy,
            SkładnikiCzynszu,
            TypyLokali,
            TypyKuchni,
            RodzajeNajemcy,
            TytułyPrawne,
            Wspólnoty,
            TypyWpłat,
            StawkiVat,
            Atrybuty,
            GrupySkładnikówCzynszu,
            GrupyFinansowe,
            Użytkownicy,
            NależnościWedługNajemców,
            WszystkieNależnościNajemcy,
            NieprzeterminowaneNależnościNajemcy,
            NależnoścIObrotyNajemcy,
            SaldoNajemcy,
            ObrotyNajemcy
        };

        public enum Raport
        {
            LokaleWBudynkach,
            MiesięczneSumySkładników,
            NależnościIObrotyNajemcy,
            MiesięcznaAnalizaNależnościIObrotów,
            SzczegółowaAnalizaNależnościIObrotów,
            KwotaCzynszuLokali,
            KwotaCzynszuBudynków,
            KwotaCzynszuWspólnot,
        };

        public enum FormatRaportu
        {
            Pdf,
            Csv
        };

        public enum PorządekSortowania
        {
            Rosnąco,
            Malejąco
        };

        public enum Atrybut
        {
            Lokalu,
            Najemcy,
            Budynku,
            Wspólnoty
        };

        public enum PowódPrzeniesieniaNaStronęLogowania
        {
            NiepoprawneDaneUwierzytelniające,
            NiezalogowanyLubSesjaWygasła
        };

        public enum Zbiór
        {
            Czynsze,
            Drugi,
            Trzeci
        };

        public enum KwotaCzynszu
        {
            Biezaca,
            ZaDanyMiesiac
        };

        public enum TreściOpisów
        {
            Wpłaty,
            Książka,
            Woda
        }
    }
}