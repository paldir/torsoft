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
            SkladnikiCzynszu,
            TypyLokali,
            TypyKuchni,
            RodzajeNajemcy,
            TytulyPrawne,
            Wspolnoty,
            TypyWplat,
            StawkiVat,
            Atrybuty,
            GrupySkładnikowCzynszu,
            GrupyFinansowe,
            Uzytkownicy,
            NaleznosciWedlugNajemcow,
            WszystkieNaleznosciNajemcy,
            NieprzeterminowaneNaleznosciNajemcy,
            NaleznoscIObrotyNajemcy,
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
            SkładnikiCzynszuStawkaZwykła,
            SkładnikiCzynszuStawkaInformacyjna
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
            NiepoprawneDaneUwierzytelniajace,
            NiezalogowanyLubSesjaWygasla
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
            Oplaty,
            Ksiazka,
            Woda
        }
    }
}