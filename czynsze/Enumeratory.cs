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
            NaleznosciIObrotyNajemcy,
            SaldoNajemcy,
            ObrotyNajemcy
        };

        public enum Raport
        {
            LokaleWBudynkach,
            MiesieczneSumySkladnikow,
            NaleznosciIObrotyNajemcy,
            MiesiecznaAnalizaNaleznosciIObrotow,
            SzczegolowaAnalizaNaleznosciIObrotow,
            NaleznosciZaDanyMiesiacLokale,
            NaleznosciZaDanyMiesiacBudynki,
            NaleznosciZaDanyMiesiacWspolnoty,
            SkladnikiCzynszuStawkaZwykla,
            SkladnikiCzynszuStawkaInformacyjna,
            WykazWgSkladnika,
            NaleznosciSzczegolowoMiesiacLokale,
            NaleznosciSzczegolowoMiesiacBudynki,
            NaleznosciSzczegolowoMiesiacWspolnoty,
            NaleznosciWgEwidencjiLokale,
            NaleznosciWgEwidencjiBudynki,
            NaleznosciWgEwidencjiWspolnoty,
            NaleznosciWgGrupSkladnikiLokale,
            NaleznosciWgGrupSkladnikiBudynki,
            NaleznosciWgGrupSkladnikiWspolnoty,
            NaleznosciWgGrupSumyLokale,
            NaleznosciWgGrupSumyBudynki,
            NaleznosciWgGrupSumyWspolnoty,
            ObrotyZaDanyMiesiacLokale,
            ObrotyZaDanyMiesiacBudynki,
            ObrotyZaDanyMiesiacWspolnoty,
            ObrotySzczegolowoMiesiacLokale,
            ObrotySzczegolowoMiesiacBudynki,
            ObrotySzczegolowoMiesiacWspolnoty,
            ObrotyWgEwidencjiLokale,
            ObrotyWgEwidencjiBudynki,
            ObrotyWgEwidencjiWspolnoty,
            ObrotyWgGrupSkladnikiLokale,
            ObrotyWgGrupSkladnikiBudynki,
            ObrotyWgGrupSkladnikiWspolnoty,
            ObrotyWgGrupSumyLokale,
            ObrotyWgGrupSumyBudynki,
            ObrotyWgGrupSumyWspolnoty
        };

        public enum FormatRaportu
        {
            Pdf,
            Csv
        };

        public enum PorządekSortowania
        {
            Rosnaco,
            Malejaco
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

        /*public enum KwotaCzynszu
        {
            Biezaca,
            ZaDanyMiesiac
        };*/

        public enum TreściOpisów
        {
            Oplaty,
            Ksiazka,
            Woda
        }

        public enum WykazWedługSkładnika
        {
            Obecny,
            HistoriaOgolem,
            HistoriaSpecyfikacja
        }

        public enum Analiza
        {
            NaleznosciBiezace,
            NaleznosciZaDanyMiesiac,
            NaleznosciSzczegolowoMiesiac,
            NaleznosciWgEwidencji,
            NaleznosciWgGrupSkladniki,
            NaleznosciWgGrupSumy,
            ObrotyBiezace,
            ObrotyZaDanyMiesiac,
            ObrotySzczegolowoMiesiac,
            ObrotyWgEwidencji,
            ObrotyWgGrupSkladniki,
            ObrotyWgGrupSumy
        }

        public enum ObiektAnalizy
        {
            Lokale,
            Budynki,
            Wspolnoty
        }
    }
}