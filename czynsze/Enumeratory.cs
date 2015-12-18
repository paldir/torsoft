﻿using System;
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
            ObrotyNajemcy
        };

        public enum Raport
        {
            LokaleWBudynkach,
            SumyMiesięczneSkładnika,
            WydrukNależnościIObrotów,
            AnalizaMiesięczna,
            AnalizaSzczegółowa,
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
            ObrotyWgGrupSumyWspolnoty,
            OgolemZaDanyMiesiacLokale,
            OgolemZaDanyMiesiacBudynki,
            OgolemZaDanyMiesiacWspolnoty,
            OgolemSzczegolowoMiesiacLokale,
            OgolemSzczegolowoMiesiacBudynki,
            OgolemSzczegolowoMiesiacWspolnoty,
            OgolemWgEwidencjiLokale,
            OgolemWgEwidencjiBudynki,
            OgolemWgEwidencjiWspolnoty,
            OgolemWgGrupSkladnikiLokale,
            OgolemWgGrupSkladnikiBudynki,
            OgolemWgGrupSkladnikiWspolnoty,
            OgolemWgGrupSumyLokale,
            OgolemWgGrupSumyBudynki,
            OgolemWgGrupSumyWspolnoty
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
            ObrotyZaDanyMiesiac,
            ObrotySzczegolowoMiesiac,
            ObrotyWgEwidencji,
            ObrotyWgGrupSkladniki,
            ObrotyWgGrupSumy,
            OgolemZaDanyMiesiac,
            OgolemSzczegolowoMiesiac,
            OgolemWgEwidencji,
            OgolemWgGrupSkladniki,
            OgolemWgGrupSumy
        }

        public enum ObiektAnalizy
        {
            Lokale,
            Budynki,
            Wspolnoty
        }
    }
}