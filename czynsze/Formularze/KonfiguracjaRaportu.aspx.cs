﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using System.Xml;

namespace czynsze.Formularze
{
    public partial class KonfiguracjaRaportu : Strona
    {
        Enumeratory.Raport _raport;
        bool _analiza;
        enum ObiektRaportu { NALEZNOSCI, OBROTY, OGOLEM }

        int[] identyfikatory
        {
            get
            {
                if (ViewState["identyfikatory"] == null)
                    ViewState["identyfikatory"] = new int[6];

                return (int[])ViewState["identyfikatory"];
            }

            set { ViewState["identyfikatory"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                List<Control> kontrolki = new List<Control>();
                List<string> etykiety = new List<string>();
                string nagłówek = "Konfiguracja wydruku ";
                string klucz = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("raport"));
                //_raport = (Enumeratory.Raport)Enum.Parse(typeof(Enumeratory.Raport), klucz.Replace("raport", String.Empty).Substring(klucz.LastIndexOf('$') + 1));
                _raport = PobierzWartośćParametru<Enumeratory.Raport>("raport");
                klucz = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("id"));
                int indeks = Request.UrlReferrer.Query.IndexOf("id=");

                if (!String.IsNullOrEmpty(klucz))
                    identyfikatory[0] = PobierzWartośćParametru<int>(klucz);

                if (indeks != -1)
                    identyfikatory[1] = Int32.Parse(Request.UrlReferrer.Query.Substring(indeks + 3));

                //placeOfConfigurationFields.Controls.Add(new Kontrolki.HtmlInputHidden(_raport + "raport", "#"));

                switch (_raport)
                {
                    case Enumeratory.Raport.LokaleWBudynkach:
                        {
                            nagłówek += "(Lokale w budynkach)";
                            int numerPierwszegoBudynku, numerOstatniegoBudynku;
                            Kontrolki.HtmlGenericControl pierwszyBudynek = new Kontrolki.HtmlGenericControl("div", "control");
                            Kontrolki.HtmlGenericControl drugiBudynek = new Kontrolki.HtmlGenericControl("div", "control");
                            List<string[]> budynki = db.Budynki.AsEnumerable().OrderBy(b => b.kod_1).Select(b => b.PolaDoTabeli().ToArray()).ToList();

                            if (db.Budynki.Any())
                            {
                                numerPierwszegoBudynku = db.Budynki.Min(b => b.kod_1);
                                numerOstatniegoBudynku = db.Budynki.Max(b => b.kod_1);
                            }
                            else
                                numerPierwszegoBudynku = numerOstatniegoBudynku = 0;

                            pierwszyBudynek.Controls.Add(new Kontrolki.TextBox("field", "kod_1_start", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true, numerPierwszegoBudynku.ToString()));
                            pierwszyBudynek.Controls.Add(new Kontrolki.DropDownList("field", "kod_1_start_dropdown", budynki, true, false, numerPierwszegoBudynku.ToString()));
                            drugiBudynek.Controls.Add(new Kontrolki.TextBox("field", "kod_1_end", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 5, 1, true, numerOstatniegoBudynku.ToString()));
                            drugiBudynek.Controls.Add(new Kontrolki.DropDownList("field", "kod_1_end_dropdown", budynki, true, false, numerOstatniegoBudynku.ToString()));

                            kontrolki = new List<Control>()
                            {
                                pierwszyBudynek,
                                drugiBudynek,
                                new Kontrolki.CheckBoxList("field", "kod_typ", db.TypyLokali.Select(t=>t.typ_lok).ToList(), db.TypyLokali.Select(t=>t.kod_typ.ToString()).ToList(), true, db.TypyLokali.Select(t=>t.kod_typ.ToString()).ToList())
                            };
                        }

                        etykiety = new List<string>()
                    {
                        "Numer pierwszego budynku: ",
                        "Numer ostatniego budynku:",
                        "Typy lokali: "
                    };

                        break;

                    case Enumeratory.Raport.SumyMiesięczneSkładnika:
                        nagłówek += "(Sumy miesięczne składnika)";

                        break;

                    case Enumeratory.Raport.WydrukNależnościIObrotów:
                        nagłówek += "(Należności i obroty najemcy)";

                        break;

                    case Enumeratory.Raport.AnalizaMiesięczna:
                        nagłówek += "(Analiza miesięczna)";

                        break;

                    case Enumeratory.Raport.AnalizaSzczegółowa:
                        nagłówek += "(Analiza szczegółowa)";

                        break;

                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                    case Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla:
                    case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                    case Enumeratory.Raport.WykazWgSkladnika:
                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                    case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                    case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                    case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                    case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                    case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                    case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                    case Enumeratory.Raport.ObrotyZaDanyMiesiacLokale:
                    case Enumeratory.Raport.ObrotyZaDanyMiesiacBudynki:
                    case Enumeratory.Raport.ObrotyZaDanyMiesiacWspolnoty:
                    case Enumeratory.Raport.ObrotySzczegolowoMiesiacLokale:
                    case Enumeratory.Raport.ObrotySzczegolowoMiesiacBudynki:
                    case Enumeratory.Raport.ObrotySzczegolowoMiesiacWspolnoty:
                    case Enumeratory.Raport.ObrotyWgEwidencjiLokale:
                    case Enumeratory.Raport.ObrotyWgEwidencjiBudynki:
                    case Enumeratory.Raport.ObrotyWgEwidencjiWspolnoty:
                    case Enumeratory.Raport.ObrotyWgGrupSkladnikiLokale:
                    case Enumeratory.Raport.ObrotyWgGrupSkladnikiBudynki:
                    case Enumeratory.Raport.ObrotyWgGrupSkladnikiWspolnoty:
                    case Enumeratory.Raport.ObrotyWgGrupSumyLokale:
                    case Enumeratory.Raport.ObrotyWgGrupSumyBudynki:
                    case Enumeratory.Raport.ObrotyWgGrupSumyWspolnoty:
                    case Enumeratory.Raport.OgolemZaDanyMiesiacLokale:
                    case Enumeratory.Raport.OgolemZaDanyMiesiacBudynki:
                    case Enumeratory.Raport.OgolemZaDanyMiesiacWspolnoty:
                    case Enumeratory.Raport.OgolemSzczegolowoMiesiacLokale:
                    case Enumeratory.Raport.OgolemSzczegolowoMiesiacBudynki:
                    case Enumeratory.Raport.OgolemSzczegolowoMiesiacWspolnoty:
                    case Enumeratory.Raport.OgolemWgEwidencjiLokale:
                    case Enumeratory.Raport.OgolemWgEwidencjiBudynki:
                    case Enumeratory.Raport.OgolemWgEwidencjiWspolnoty:
                    case Enumeratory.Raport.OgolemWgGrupSkladnikiLokale:
                    case Enumeratory.Raport.OgolemWgGrupSkladnikiBudynki:
                    case Enumeratory.Raport.OgolemWgGrupSkladnikiWspolnoty:
                    case Enumeratory.Raport.OgolemWgGrupSumyLokale:
                    case Enumeratory.Raport.OgolemWgGrupSumyBudynki:
                    case Enumeratory.Raport.OgolemWgGrupSumyWspolnoty:
                        identyfikatory[0] = PobierzWartośćParametru<int>("odBudynku");
                        identyfikatory[1] = PobierzWartośćParametru<int>("odLokalu");
                        identyfikatory[2] = PobierzWartośćParametru<int>("doBudynku");
                        identyfikatory[3] = PobierzWartośćParametru<int>("doLokalu");

                        switch (_raport)
                        {
                            case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                            case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                            case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                            case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                            case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                            case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                            case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                            case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                            case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                            case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                            case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                            case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                            case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                            case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                            case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                            case Enumeratory.Raport.ObrotyZaDanyMiesiacLokale:
                            case Enumeratory.Raport.ObrotyZaDanyMiesiacBudynki:
                            case Enumeratory.Raport.ObrotyZaDanyMiesiacWspolnoty:
                            case Enumeratory.Raport.ObrotySzczegolowoMiesiacLokale:
                            case Enumeratory.Raport.ObrotySzczegolowoMiesiacBudynki:
                            case Enumeratory.Raport.ObrotySzczegolowoMiesiacWspolnoty:
                            case Enumeratory.Raport.ObrotyWgEwidencjiLokale:
                            case Enumeratory.Raport.ObrotyWgEwidencjiBudynki:
                            case Enumeratory.Raport.ObrotyWgEwidencjiWspolnoty:
                            case Enumeratory.Raport.ObrotyWgGrupSkladnikiLokale:
                            case Enumeratory.Raport.ObrotyWgGrupSkladnikiBudynki:
                            case Enumeratory.Raport.ObrotyWgGrupSkladnikiWspolnoty:
                            case Enumeratory.Raport.ObrotyWgGrupSumyLokale:
                            case Enumeratory.Raport.ObrotyWgGrupSumyBudynki:
                            case Enumeratory.Raport.ObrotyWgGrupSumyWspolnoty:
                            case Enumeratory.Raport.OgolemZaDanyMiesiacLokale:
                            case Enumeratory.Raport.OgolemZaDanyMiesiacBudynki:
                            case Enumeratory.Raport.OgolemZaDanyMiesiacWspolnoty:
                            case Enumeratory.Raport.OgolemSzczegolowoMiesiacLokale:
                            case Enumeratory.Raport.OgolemSzczegolowoMiesiacBudynki:
                            case Enumeratory.Raport.OgolemSzczegolowoMiesiacWspolnoty:
                            case Enumeratory.Raport.OgolemWgEwidencjiLokale:
                            case Enumeratory.Raport.OgolemWgEwidencjiBudynki:
                            case Enumeratory.Raport.OgolemWgEwidencjiWspolnoty:
                            case Enumeratory.Raport.OgolemWgGrupSkladnikiLokale:
                            case Enumeratory.Raport.OgolemWgGrupSkladnikiBudynki:
                            case Enumeratory.Raport.OgolemWgGrupSkladnikiWspolnoty:
                            case Enumeratory.Raport.OgolemWgGrupSumyLokale:
                            case Enumeratory.Raport.OgolemWgGrupSumyBudynki:
                            case Enumeratory.Raport.OgolemWgGrupSumyWspolnoty:
                                identyfikatory[4] = PobierzWartośćParametru<int>("odWspólnoty");
                                identyfikatory[5] = PobierzWartośćParametru<int>("doWspólnoty");
                                _analiza = true;

                                switch (WartościSesji.TrybAnalizy)
                                {
                                    case Enumeratory.Analiza.NaleznosciBiezace:
                                        nagłówek += "(Bieżące należności)";

                                        break;

                                    case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                        nagłówek += "(Należności za dany miesiąc)";

                                        break;

                                    case Enumeratory.Analiza.NaleznosciSzczegolowoMiesiac:
                                        nagłówek += "(Należności za dany miesiąc szczegółowo)";

                                        break;

                                    case Enumeratory.Analiza.NaleznosciWgEwidencji:
                                        nagłówek += "(Należności wg ewidencji)";

                                        break;

                                    case Enumeratory.Analiza.NaleznosciWgGrupSkladniki:
                                        nagłówek += "(Należności wg grup - składniki)";

                                        break;

                                    case Enumeratory.Analiza.NaleznosciWgGrupSumy:
                                        nagłówek += "(Należności wg grup - sumy)";

                                        break;

                                    case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                        nagłówek += "(Obroty za dany miesiąc)";

                                        break;

                                    case Enumeratory.Analiza.ObrotySzczegolowoMiesiac:
                                        nagłówek += "(Obroty za dany miesiąc szczegółowo)";

                                        break;

                                    case Enumeratory.Analiza.ObrotyWgEwidencji:
                                        nagłówek += "(Obroty wg ewidencji)";

                                        break;

                                    case Enumeratory.Analiza.ObrotyWgGrupSkladniki:
                                        nagłówek += "(Obroty wg grup - składniki)";

                                        break;

                                    case Enumeratory.Analiza.ObrotyWgGrupSumy:
                                        nagłówek += "(Obroty wg grup - sumy)";

                                        break;

                                    case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                                        nagłówek += "(Analiza ogółem za dany miesiąc)";

                                        break;

                                    case Enumeratory.Analiza.OgolemSzczegolowoMiesiac:
                                        nagłówek += "(Analiza ogółem za dany miesiąc szczegółowo)";

                                        break;

                                    case Enumeratory.Analiza.OgolemWgEwidencji:
                                        nagłówek += "(Analiza ogółem wg ewidencji)";

                                        break;

                                    case Enumeratory.Analiza.OgolemWgGrupSkladniki:
                                        nagłówek += "(Analiza ogółem wg grup - składniki)";

                                        break;

                                    case Enumeratory.Analiza.OgolemWgGrupSumy:
                                        nagłówek += "(Analiza ogółem wg grup - sumy)";

                                        break;
                                }

                                break;

                            case Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla:
                                nagłówek += "(Składniki czynszu - stawka zwykła)";

                                break;
                            case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                                nagłówek += "(Składniki czynszu - stawka informacyjna)";

                                break;
                            case Enumeratory.Raport.WykazWgSkladnika:
                                identyfikatory[4] = PobierzWartośćParametru<int>("nrSkladnika");

                                switch (WartościSesji.TrybWykazuWgSkładnika)
                                {
                                    case Enumeratory.WykazWedługSkładnika.Obecny:
                                        nagłówek += "(Wykaz wg składnika - obecny)";

                                        break;

                                    case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                        nagłówek += "(Wykaz wg składnika - historia ogółem)";

                                        break;
                                }

                                break;
                        }

                        break;
                }

                placeOfConfigurationFields.Controls.Add(new LiteralControl("<h2>" + nagłówek + "</h2>"));
                kontrolki.Add(new Kontrolki.RadioButtonList("list", "format", new List<string>() { "PDF", "CSV" }, new List<string>() { Enumeratory.FormatRaportu.Pdf.ToString(), Enumeratory.FormatRaportu.Csv.ToString() }, true, false, Enumeratory.FormatRaportu.Pdf.ToString()));
                etykiety.Add("Format: ");

                for (int i = 0; i < kontrolki.Count; i++)
                {
                    placeOfConfigurationFields.Controls.Add(new LiteralControl("<div class='fieldWithLabel'>"));
                    placeOfConfigurationFields.Controls.Add(new Kontrolki.Label("label", kontrolki[i].ID, etykiety[i], String.Empty));
                    DodajNowąLinię(placeOfConfigurationFields);
                    placeOfConfigurationFields.Controls.Add(kontrolki[i]);
                    placeOfConfigurationFields.Controls.Add(new LiteralControl("</div>"));
                }

                generationButton.Click += generationButton_Click;
                Title = nagłówek;

                Start.ŚcieżkaStrony.Dodaj(nagłówek);
            }
        }

        void generationButton_Click(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                List<List<string[]>> tabele = new List<List<string[]>>();
                List<string> nagłówki = null;
                List<string> podpisy = new List<string>();
                string tytuł = null;
                List<string> gotowaDefinicjaHtml = null;
                DateTime data = Start.Data;
                MagazynRekordów magazyn = WartościSesji.MagazynRekordów;

                {
                    IEnumerable<DostępDoBazy.Należność> należności = null;
                    IEnumerable<DostępDoBazy.Obrót> obroty = null;
                    List<DostępDoBazy.PozycjaDoAnalizy> pozycje = null;
                    List<DostępDoBazy.IInformacjeOPozycji> informacjeOPozycjach = null;
                    string napisRaportu = _raport.ToString();
                    ObiektRaportu obiektRaportu = (ObiektRaportu)(-1);

                    if (napisRaportu.StartsWith("Naleznosci"))
                        obiektRaportu = ObiektRaportu.NALEZNOSCI;
                    else if (napisRaportu.StartsWith("Obroty"))
                        obiektRaportu = ObiektRaportu.OBROTY;
                    else if (napisRaportu.StartsWith("Ogolem"))
                        obiektRaportu = ObiektRaportu.OGOLEM;

                    switch (obiektRaportu)
                    {
                        case ObiektRaportu.NALEZNOSCI:
                            informacjeOPozycjach = db.SkładnikiCzynszu.AsEnumerable<DostępDoBazy.IInformacjeOPozycji>().ToList();

                            break;

                        case ObiektRaportu.OBROTY:
                            informacjeOPozycjach = db.RodzajePłatności.AsEnumerable<DostępDoBazy.IInformacjeOPozycji>().ToList();

                            break;

                        case ObiektRaportu.OGOLEM:
                            informacjeOPozycjach = Enumerable.Concat<DostępDoBazy.IInformacjeOPozycji>(db.SkładnikiCzynszu, db.RodzajePłatności).ToList();

                            break;

                    }

                    switch (Start.AktywnyZbiór)
                    {
                        case Enumeratory.Zbiór.Czynsze:
                            if (_analiza)
                                switch (obiektRaportu)
                                {
                                    case ObiektRaportu.NALEZNOSCI:
                                        pozycje = db.Należności1.AsEnumerable<DostępDoBazy.PozycjaDoAnalizy>().ToList();

                                        break;

                                    case ObiektRaportu.OBROTY:
                                        pozycje = db.Obroty1.AsEnumerable<DostępDoBazy.PozycjaDoAnalizy>().ToList();

                                        break;

                                    case ObiektRaportu.OGOLEM:
                                        pozycje = Enumerable.Concat<DostępDoBazy.PozycjaDoAnalizy>(db.Należności1, db.Obroty1).ToList();

                                        break;
                                }
                            else
                            {
                                należności = db.Należności1;
                                obroty = db.Obroty1;
                            }

                            break;

                        case Enumeratory.Zbiór.Drugi:
                            if (_analiza)
                                switch (obiektRaportu)
                                {
                                    case ObiektRaportu.NALEZNOSCI:
                                        pozycje = db.Należności2.AsEnumerable<DostępDoBazy.PozycjaDoAnalizy>().ToList();

                                        break;

                                    case ObiektRaportu.OBROTY:
                                        pozycje = db.Obroty2.AsEnumerable<DostępDoBazy.PozycjaDoAnalizy>().ToList();

                                        break;

                                    case ObiektRaportu.OGOLEM:
                                        pozycje = Enumerable.Concat<DostępDoBazy.PozycjaDoAnalizy>(db.Należności2, db.Obroty2).ToList();

                                        break;
                                }
                            else
                            {
                                należności = db.Należności2;
                                obroty = db.Obroty2;
                            }

                            break;

                        case Enumeratory.Zbiór.Trzeci:
                            if (_analiza)
                                switch (obiektRaportu)
                                {
                                    case ObiektRaportu.NALEZNOSCI:
                                        pozycje = db.Należności3.AsEnumerable<DostępDoBazy.PozycjaDoAnalizy>().ToList();

                                        break;

                                    case ObiektRaportu.OBROTY:
                                        pozycje = db.Obroty3.AsEnumerable<DostępDoBazy.PozycjaDoAnalizy>().ToList();

                                        break;

                                    case ObiektRaportu.OGOLEM:
                                        pozycje = Enumerable.Concat<DostępDoBazy.PozycjaDoAnalizy>(db.Należności3, db.Obroty3).ToList();

                                        break;
                                }
                            else
                            {
                                należności = db.Należności3;
                                obroty = db.Obroty3;
                            }

                            break;
                    }

                    if (pozycje != null)
                    {
                        List<DostępDoBazy.PozycjaDoAnalizy> pozycjeDoUsunięcia = new List<DostępDoBazy.PozycjaDoAnalizy>();

                        foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycje)
                        {
                            DostępDoBazy.IInformacjeOPozycji informacja = informacjeOPozycjach.SingleOrDefault(i => i.IdInformacji == pozycja.IdInformacji);

                            if (informacja == null)
                                pozycjeDoUsunięcia.Add(pozycja);
                            else
                                pozycja.Informacje = informacja;
                        }

                        foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeDoUsunięcia)
                            pozycje.Remove(pozycja);
                    }

                    switch (_raport)
                    {
                        case Enumeratory.Raport.LokaleWBudynkach:
                            {
                                int kod_1_start;
                                int kod_1_koniec;
                                List<int> wybraneTypyLokali = new List<int>();
                                tytuł = "LOKALE W BUDYNKACH";
                                nagłówki = new List<string>()
                            {
                                "Numer lokalu",
                                "Typ lokalu",
                                "Powierzchnia użytkowa",
                                "Nazwisko",
                                "Imię"
                            };

                                try { kod_1_start = Int32.Parse(((TextBox)placeOfConfigurationFields.FindControl("kod_1_start")).Text); }
                                catch { kod_1_start = 0; }

                                try { kod_1_koniec = Int32.Parse(((TextBox)placeOfConfigurationFields.FindControl("kod_1_end")).Text); }
                                catch { kod_1_koniec = 0; }

                                try
                                {
                                    foreach (ListItem pozycja in ((CheckBoxList)placeOfConfigurationFields.FindControl("kod_typ")).Items)
                                        if (pozycja.Selected)
                                            wybraneTypyLokali.Add(Int32.Parse(pozycja.Value));
                                }
                                catch { }

                                {
                                    for (int i = kod_1_start; i <= kod_1_koniec; i++)
                                    {
                                        DostępDoBazy.Budynek budynek = db.Budynki.Where(b => b.kod_1 == i).FirstOrDefault();

                                        if (budynek != null)
                                        {
                                            podpisy.Add("Budynek nr " + budynek.kod_1.ToString() + ", " + budynek.adres + ", " + budynek.adres_2);
                                            tabele.Add(db.AktywneLokale.Where(p => p.kod_lok == i && wybraneTypyLokali.Contains(p.kod_typ)).OrderBy(p => p.nr_lok).Select(p => p.PolaDoTabeli().ToList().GetRange(2, p.PolaDoTabeli().Count() - 2).ToArray()).ToList());
                                        }
                                    }
                                }
                            }

                            break;

                        case Enumeratory.Raport.SumyMiesięczneSkładnika:
                        case Enumeratory.Raport.WydrukNależnościIObrotów:
                        case Enumeratory.Raport.AnalizaMiesięczna:
                        case Enumeratory.Raport.AnalizaSzczegółowa:
                            {
                                tabele = new List<List<string[]>> { new List<string[]>() };
                                decimal sumaWn, sumaMa;
                                List<decimal> salda = new List<decimal>();
                                int nr_kontr = identyfikatory[1];

                                //DostępDoBazy.CzynszeKontekst db = DostępDoBazy.CzynszeKontekst.BazaDanych;
                                {
                                    DostępDoBazy.Najemca najemca = magazyn.NrKontrNaNajemcę[nr_kontr];
                                    podpisy = new List<string>() { najemca.nazwisko + " " + najemca.imie + "<br />" + najemca.adres_1 + " " + najemca.adres_2 + "<br />" };
                                    List<DostępDoBazy.Należność> należnościNajemcy = należności.Where(r => r.nr_kontr == nr_kontr).ToList();
                                    List<DostępDoBazy.Obrót> obrotyNajemcy = obroty.Where(r => r.nr_kontr == nr_kontr).ToList();

                                    switch (_raport)
                                    {
                                        case Enumeratory.Raport.SumyMiesięczneSkładnika:
                                            tytuł = String.Format("ZESTAWIENIE ROZLICZEN MIESIECZNYCH ZA ROK {0}", data.Year);
                                            nagłówki = new List<string>() { "m-c", "Wartość" };

                                            if (identyfikatory[0] < 0)
                                            {

                                                int nr_skl = należnościNajemcy.FirstOrDefault(r => r.__record == -1 * identyfikatory[0]).nr_skl;
                                                podpisy[0] += magazyn.NumerSkładnikaNaSkładnikCzynszu[nr_skl].nazwa;

                                                for (int i = 1; i <= 12; i++)
                                                    tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N}", należnościNajemcy.Where(r => r.nr_skl == nr_skl).ToList().Where(r => r.data_nal.Year == data.Year && r.data_nal.Month == i).Sum(r => r.kwota_nal)) });
                                            }
                                            else
                                            {
                                                int kod_wplat = obrotyNajemcy.FirstOrDefault(t => t.__record == identyfikatory[0]).kod_wplat;
                                                podpisy[0] += db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == kod_wplat).typ_wplat;

                                                for (int i = 1; i <= 12; i++)
                                                    tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N}", obrotyNajemcy.Where(t => t.kod_wplat == kod_wplat).ToList().Where(t => t.data_obr.Year == data.Year && t.data_obr.Month == i).Sum(t => t.suma)) });
                                            }

                                            tabele[0].Add(new string[] { "Razem", String.Format("{0:N}", tabele[0].Sum(r => Single.Parse(r[1]))) });

                                            break;

                                        case Enumeratory.Raport.WydrukNależnościIObrotów:
                                            tytuł = "ZESTAWIENIE NALEZNOSCI I WPLAT";
                                            nagłówki = new List<string> { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };

                                            foreach (DostępDoBazy.Należność receivable in należnościNajemcy)
                                            {
                                                List<string> pola = receivable.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                                pola.RemoveAt(0);

                                                tabele[0].Add(pola.ToArray());
                                            }

                                            foreach (DostępDoBazy.Obrót turnover in obrotyNajemcy)
                                            {
                                                List<string> pola = turnover.WażnePolaDoNależnościIObrotówNajemcy().ToList();

                                                pola.RemoveAt(0);

                                                tabele[0].Add(pola.ToArray());
                                            }

                                            tabele[0] = tabele[0].OrderBy(r => DateTime.Parse(r[2])).ToList();
                                            sumaWn = tabele[0].Sum(r => String.IsNullOrEmpty(r[0]) ? 0 : Decimal.Parse(r[0]));
                                            sumaMa = tabele[0].Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));

                                            tabele[0].Add(new string[] { String.Format("{0:N}", sumaWn), String.Format("{0:N}", sumaMa), String.Empty, String.Empty });
                                            tabele[0].Add(new string[] { "SALDO", String.Format("{0:N}", sumaMa - sumaWn), String.Empty, String.Empty });

                                            break;

                                        case Enumeratory.Raport.AnalizaMiesięczna:
                                            tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                            nagłówki = new List<string>() { "m-c", "suma WN w miesiącu", "suma MA w miesiącu", "saldo w miesiącu", "suma WN narastająco", "suma MA narastająco", "saldo narastająco" };
                                            List<decimal> kwotyWn = new List<decimal>();
                                            List<decimal> kwotyMa = new List<decimal>();

                                            for (int i = 1; i <= 12; i++)
                                            {
                                                List<string[]> miesięczneNależności = należnościNajemcy.Where(r => r.data_nal.Month == i).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                                List<string[]> miesięczneObroty = obrotyNajemcy.Where(t => t.data_obr.Month == i).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                                                sumaWn = miesięczneNależności.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1])) + miesięczneObroty.Sum(t => String.IsNullOrEmpty(t[1]) ? 0 : Decimal.Parse(t[1]));
                                                sumaMa = miesięczneObroty.Sum(t => String.IsNullOrEmpty(t[2]) ? 0 : Decimal.Parse(t[2]));

                                                kwotyWn.Add(sumaWn);
                                                kwotyMa.Add(sumaMa);
                                                salda.Add(sumaMa - sumaWn);
                                                tabele[0].Add(new string[] { i.ToString(), String.Format("{0:N}", sumaWn), String.Format("{0:N}", sumaMa), String.Format("{0:N}", salda.Last()), String.Format("{0:N}", kwotyWn.Sum()), String.Format("{0:N}", kwotyMa.Sum()), String.Format("{0:N}", salda.Sum()) });
                                            }

                                            break;

                                        case Enumeratory.Raport.AnalizaSzczegółowa:
                                            tytuł = "ZESTAWIENIE ROZLICZEN MIESIECZNYCH";
                                            nagłówki = new List<string>() { "m-c", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Saldo miesiąca", "Saldo narastająco" };
                                            string[] nowyWiersz;

                                            for (int i = 1; i <= 12; i++)
                                            {
                                                decimal[] suma = new decimal[] { 0, 0, 0, 0 };
                                                sumaWn = sumaMa = 0;

                                                foreach (DostępDoBazy.Należność należność in należnościNajemcy.Where(r => r.data_nal.Month == i))
                                                {
                                                    string[] wiersz = należność.WażnePolaDoNależnościIObrotówNajemcy();
                                                    int indeks = magazyn.NumerSkładnikaNaSkładnikCzynszu[należność.nr_skl].rodz_e - 1;

                                                    if (!String.IsNullOrEmpty(wiersz[1]))
                                                    {
                                                        decimal kwota = Decimal.Parse(wiersz[1]);
                                                        suma[indeks] += kwota;
                                                        sumaWn += kwota;
                                                    }
                                                }

                                                foreach (DostępDoBazy.Obrót obrót in obrotyNajemcy.Where(t => t.data_obr.Month == i))
                                                {
                                                    string[] wiersz = obrót.WażnePolaDoNależnościIObrotówNajemcy();
                                                    int indeks = db.RodzajePłatności.FirstOrDefault(t => t.kod_wplat == obrót.kod_wplat).rodz_e - 1;

                                                    if (indeks >= 0)
                                                    {
                                                        decimal kwota;

                                                        if (!String.IsNullOrEmpty(wiersz[1]))
                                                        {
                                                            kwota = Decimal.Parse(wiersz[1]);
                                                            suma[indeks] += kwota;
                                                            sumaWn += kwota;
                                                        }

                                                        if (!String.IsNullOrEmpty(wiersz[2]))
                                                        {
                                                            kwota = Decimal.Parse(wiersz[2]);
                                                            suma[indeks] += kwota;
                                                            sumaMa += kwota;
                                                        }
                                                    }
                                                }

                                                salda.Add(sumaMa - sumaWn);

                                                nowyWiersz = new string[7];
                                                nowyWiersz[0] = i.ToString();

                                                for (int j = 1; j <= 4; j++)
                                                    nowyWiersz[j] = String.Format("{0:N}", suma[j - 1]);

                                                nowyWiersz[5] = String.Format("{0:N}", salda.Last());
                                                nowyWiersz[6] = String.Format("{0:N}", salda.Sum());

                                                tabele[0].Add(nowyWiersz);
                                            }

                                            nowyWiersz = new string[7];
                                            nowyWiersz[0] = "Razem";

                                            for (int i = 1; i < nowyWiersz.Length - 1; i++)
                                                nowyWiersz[i] = String.Format("{0:N}", tabele[0].Sum(r => Single.Parse(r[i])));

                                            nowyWiersz[6] = nowyWiersz[5];

                                            tabele[0].Add(nowyWiersz);

                                            break;
                                    }
                                }
                            }

                            break;

                        case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                        case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                        case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                        case Enumeratory.Raport.ObrotyZaDanyMiesiacLokale:
                        case Enumeratory.Raport.ObrotyZaDanyMiesiacBudynki:
                        case Enumeratory.Raport.ObrotyZaDanyMiesiacWspolnoty:
                        case Enumeratory.Raport.OgolemZaDanyMiesiacLokale:
                        case Enumeratory.Raport.OgolemZaDanyMiesiacBudynki:
                        case Enumeratory.Raport.OgolemZaDanyMiesiacWspolnoty:
                            {
                                Enumeratory.Analiza rodzajAnalizy = WartościSesji.TrybAnalizy;
                                DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                                DateTime koniecMiesiąca = początekMiesiąca.AddDays(DateTime.DaysInMonth(początekMiesiąca.Year, początekMiesiąca.Month)).AddSeconds(-1);
                                List<DostępDoBazy.PozycjaDoAnalizy> pozycjeZaDanyMiesiąc = null;

                                switch (rodzajAnalizy)
                                {
                                    case Enumeratory.Analiza.NaleznosciBiezace:
                                        tytuł = "BIEZACE NALEZNOSCI";

                                        break;

                                    case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                    case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                    case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                                        tytuł = String.Format("{0} ZA {1} {2}", obiektRaportu, DostępDoBazy.CzynszeKontekst.NumerMiesiącaNaNazwęBezPolskichZnaków[data.Month].ToUpper(), data.Year);
                                        pozycjeZaDanyMiesiąc = pozycje.Where(p => p.Data >= początekMiesiąca && p.Data <= koniecMiesiąca).ToList();

                                        break;
                                }

                                switch (_raport)
                                {
                                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacLokale:
                                    case Enumeratory.Raport.ObrotyZaDanyMiesiacLokale:
                                    case Enumeratory.Raport.OgolemZaDanyMiesiacLokale:
                                        nagłówki = new List<string>() { "Lp.", "Kod budynku", "Nr lokalu", "Typ lokalu", "Nazwisko", "Imię", "Adres", "Kwota" };

                                        //DostępDoBazy.CzynszeKontekst db = DostępDoBazy.CzynszeKontekst.BazaDanych;
                                        {
                                            int kod1 = identyfikatory[0];
                                            int nr1 = identyfikatory[1];
                                            int kod2 = identyfikatory[2];
                                            int nr2 = identyfikatory[3];
                                            int indeks = 1;
                                            decimal ogólnaSuma = 0;
                                            SortedDictionary<int, SortedDictionary<int, DostępDoBazy.Lokal>> kodINumerNaLokal = magazyn.KodINumerNaLokal;

                                            for (int kodBudynku = kod1; kodBudynku <= kod2; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (magazyn.KodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    SortedDictionary<int, DostępDoBazy.Lokal> numerNaLokal = kodINumerNaLokal[kodBudynku];
                                                    List<string[]> tabela = new List<string[]>();
                                                    List<DostępDoBazy.PozycjaDoAnalizy> pozycjeBudynku = null;
                                                    decimal sumaBudynku = 0;

                                                    switch (rodzajAnalizy)
                                                    {
                                                        case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                        case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                                        case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                                                            pozycjeBudynku = pozycjeZaDanyMiesiąc.Where(p => p.KodBudynku == kodBudynku).ToList();

                                                            break;
                                                    }

                                                    int numerPierwszego = numerNaLokal.First().Key;
                                                    int numerOstatniego = numerNaLokal.Last().Key;

                                                    if (kodBudynku == kod1)
                                                        numerPierwszego = nr1;

                                                    if (kodBudynku == kod2)
                                                        numerOstatniego = nr2;

                                                    for (int numerLokalu = numerPierwszego; numerLokalu <= numerOstatniego; numerLokalu++)
                                                    {
                                                        DostępDoBazy.Lokal lokal;

                                                        if (numerNaLokal.TryGetValue(numerLokalu, out lokal))
                                                        {
                                                            decimal suma = 0;
                                                            int kodLokalu = lokal.kod_lok;
                                                            int nrLokalu = lokal.nr_lok;

                                                            switch (rodzajAnalizy)
                                                            {
                                                                case Enumeratory.Analiza.NaleznosciBiezace:
                                                                    foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == kodLokalu && c.nr_lok == nrLokalu))
                                                                    {
                                                                        float ilosc;
                                                                        decimal stawka;

                                                                        składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                                        suma += Decimal.Round(Convert.ToDecimal(ilosc) * stawka, 2, MidpointRounding.AwayFromZero);
                                                                    }

                                                                    break;

                                                                case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                                case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                                                case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                                                                    foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeBudynku.Where(p => p.NrLokalu == lokal.nr_lok))
                                                                        suma += pozycja.Kwota;

                                                                    break;
                                                            }

                                                            tabela.Add(new string[] { indeks.ToString(), kodLokalu.ToString(), nrLokalu.ToString(), lokal.Rozpoznaj_kod_typ(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Format("{0:N}", suma) });

                                                            indeks++;
                                                            sumaBudynku += suma;
                                                        }
                                                    }

                                                    tabela.Add(new string[] { String.Empty, kodBudynku.ToString(), String.Empty, String.Empty, "<b>RAZEM</b>", "<b>BUDYNEK</b>", String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N}", sumaBudynku) });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Empty);

                                                    ogólnaSuma += sumaBudynku;
                                                }
                                            }

                                            if (tabele.Any())
                                                tabele.Last().Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", "<b>WSZYSTKIE</b>", "<b>BUDYNKI</b>", String.Format("{0:N}", ogólnaSuma) });
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacBudynki:
                                    case Enumeratory.Raport.ObrotyZaDanyMiesiacBudynki:
                                    case Enumeratory.Raport.OgolemZaDanyMiesiacBudynki:
                                        nagłówki = new List<string>() { "Lp.", "Kod budynku", "Adres", "Kwota" };

                                        //DostępDoBazy.CzynszeKontekst db = DostępDoBazy.CzynszeKontekst.BazaDanych;
                                        {
                                            int kod1 = identyfikatory[0];
                                            int kod2 = identyfikatory[2];
                                            decimal sumaGłówna = 0;
                                            List<string[]> tabela = new List<string[]>();
                                            SortedDictionary<int, DostępDoBazy.Budynek> kodNaBudynek = magazyn.KodNaBudynek;

                                            for (int kodBudynku = kod1; kodBudynku <= kod2; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (kodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    decimal suma = 0;

                                                    switch (rodzajAnalizy)
                                                    {
                                                        case Enumeratory.Analiza.NaleznosciBiezace:
                                                            foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == kodBudynku))
                                                            {
                                                                float ilosc;
                                                                decimal stawka;

                                                                składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                                suma += Decimal.Round(Convert.ToDecimal(ilosc) * stawka, 2, MidpointRounding.AwayFromZero);
                                                            }

                                                            break;

                                                        case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                        case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                                        case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                                                            foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(p => p.KodBudynku == kodBudynku))
                                                                suma += pozycja.Kwota;

                                                            break;
                                                    }

                                                    sumaGłówna += suma;

                                                    tabela.Add(new string[] { String.Format("{0}", kodBudynku - kod1 + 1), budynek.kod_1.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N}", suma) });
                                                }
                                            }

                                            tabela.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N}", sumaGłówna) });
                                            podpisy.Add(String.Empty);
                                            tabele.Add(tabela);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciZaDanyMiesiacWspolnoty:
                                    case Enumeratory.Raport.ObrotyZaDanyMiesiacWspolnoty:
                                    case Enumeratory.Raport.OgolemZaDanyMiesiacWspolnoty:
                                        nagłówki = new List<string>() { "Lp.", "Kod budynku", "Adres", "Kwota" };

                                        //DostępDoBazy.CzynszeKontekst db = DostępDoBazy.CzynszeKontekst.BazaDanych;
                                        {
                                            int kod1 = identyfikatory[4];
                                            int kod2 = identyfikatory[5];
                                            decimal sumaOgólna = 0;
                                            SortedDictionary<int, DostępDoBazy.Wspólnota> kodNaWspólnotę = magazyn.KodNaWspólnotę;

                                            for (int kodWspólnoty = kod1; kodWspólnoty <= kod2; kodWspólnoty++)
                                            {
                                                DostępDoBazy.Wspólnota wspólnota;

                                                if (kodNaWspólnotę.TryGetValue(kodWspólnoty, out wspólnota))
                                                {
                                                    List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == kodWspólnoty).ToList();
                                                    decimal sumaWspólnoty = 0;
                                                    List<string[]> tabela = new List<string[]>();

                                                    foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                    {
                                                        DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[budynekWspólnoty.kod_1];
                                                        int kodBudynku = budynek.kod_1;
                                                        decimal suma = 0;

                                                        switch (rodzajAnalizy)
                                                        {
                                                            case Enumeratory.Analiza.NaleznosciBiezace:
                                                                foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(c => c.kod_lok == kodBudynku))
                                                                {
                                                                    float ilosc;
                                                                    decimal stawka;

                                                                    składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilosc, out stawka);

                                                                    suma += Decimal.Round(Convert.ToDecimal(ilosc) * stawka, 2, MidpointRounding.AwayFromZero);
                                                                }

                                                                break;

                                                            case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                                                            case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                                                            case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                                                                foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(p => p.KodBudynku == kodBudynku))
                                                                    suma += pozycja.Kwota;

                                                                break;
                                                        }

                                                        sumaWspólnoty += suma;

                                                        tabela.Add(new string[] { String.Format("{0}", kodWspólnoty - kod1 + 1), kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Format("{0:N}", suma) });
                                                    }

                                                    sumaOgólna += sumaWspólnoty;

                                                    tabela.Add(new string[] { String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("{0:N}", sumaWspólnoty) });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                                }
                                            }
                                        }

                                        break;
                                }
                            }

                            break;

                        case Enumeratory.Raport.SkladnikiCzynszuStawkaZwykla:
                        case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                            {
                                XmlDocument dokument = new XmlDocument();
                                tytuł = "SKLADNIKI CZYNSZU I OPLAT";

                                dokument.Load(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "Formularze", "Szablon.html"));

                                XmlNode druk = dokument.SelectSingleNode(XPathZnajdźElementPoId("druk"));
                                gotowaDefinicjaHtml = new List<string>();
                                int kod_1_1 = identyfikatory[0];
                                int nr1 = identyfikatory[1];
                                int kod_1_2 = identyfikatory[2];
                                int nr2 = identyfikatory[3];
                                /*List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == kod_1_1 && l.nr_lok == nr1);
                                int indeksDrugiego = wszystkieLokale.FindLastIndex(l => l.kod_lok == kod_1_2 && l.nr_lok == nr2);*/
                                SortedDictionary<int, SortedDictionary<int, DostępDoBazy.Lokal>> kodINumerNaLokal = magazyn.KodINumerNaLokal;

                                for (int kodLokalu = kod_1_1; kodLokalu <= kod_1_2; kodLokalu++)
                                {
                                    SortedDictionary<int, DostępDoBazy.Lokal> numerNaLokal;

                                    if (kodINumerNaLokal.TryGetValue(kodLokalu, out numerNaLokal))
                                    {
                                        int numerPierwszego = numerNaLokal.First().Key;
                                        int numerOstatniego = numerNaLokal.Last().Key;

                                        if (kodLokalu == kod_1_1)
                                            numerPierwszego = nr1;

                                        if (kodLokalu == kod_1_2)
                                            numerOstatniego = nr2;

                                        for (int numerLokalu = numerPierwszego; numerLokalu <= numerOstatniego; numerLokalu++)
                                        {
                                            DostępDoBazy.Lokal lokal;

                                            if (numerNaLokal.TryGetValue(numerLokalu, out lokal))
                                            {
                                                XmlNode nowyDruk = druk.CloneNode(true);
                                                XmlNode razem = nowyDruk.SelectSingleNode(XPathZnajdźElementPoId("razem"));
                                                XmlNode składnikOpłat = nowyDruk.SelectSingleNode(XPathZnajdźElementPoId("składnikOpłat"));
                                                decimal suma = 0;

                                                WypełnijTagXml(nowyDruk, "nazwiskoImię", String.Format("{0} {1}", lokal.nazwisko, lokal.imie));
                                                WypełnijTagXml(nowyDruk, "adres1", lokal.adres);
                                                WypełnijTagXml(nowyDruk, "adres2", lokal.adres_2);
                                                WypełnijTagXml(nowyDruk, "kodLokalu", String.Format("{0} - {1}", lokal.kod_lok, lokal.nr_lok));
                                                WypełnijTagXml(nowyDruk, "powierzchnia", lokal.pow_uzyt);
                                                WypełnijTagXml(nowyDruk, "ilośćOsób", lokal.il_osob);
                                                WypełnijTagXml(nowyDruk, "data", DateTime.Now.ToShortDateString());

                                                if (db.Treści.Any())
                                                {
                                                    DostępDoBazy.Treść treść = db.Treści.FirstOrDefault();

                                                    for (int i = 1; i <= 15; i++)
                                                    {
                                                        string op = typeof(DostępDoBazy.Treść).GetProperty(String.Format("op_{0}", i)).GetValue(treść).ToString();

                                                        WypełnijTagXml(nowyDruk, String.Format("op{0}", i), op);
                                                    }
                                                }

                                                foreach (DostępDoBazy.SkładnikCzynszuLokalu składnikCzynszuLokalu in db.SkładnikiCzynszuLokalu.Where(s => s.kod_lok == lokal.kod_lok && s.nr_lok == lokal.nr_lok).ToList())
                                                {
                                                    decimal stawka;
                                                    float ilość;
                                                    XmlNode nowySkładnikOpłat = składnikOpłat.CloneNode(true);
                                                    DostępDoBazy.SkładnikCzynszu składnikCzynszu = db.SkładnikiCzynszu.FirstOrDefault(s => s.nr_skl == składnikCzynszuLokalu.nr_skl);

                                                    składnikCzynszuLokalu.Rozpoznaj_ilosc_i_stawka(out ilość, out stawka);

                                                    switch (_raport)
                                                    {
                                                        case Enumeratory.Raport.SkladnikiCzynszuStawkaInformacyjna:
                                                            stawka = składnikCzynszu.stawka_inf;

                                                            break;
                                                    }

                                                    decimal wartość = Decimal.Round(Convert.ToDecimal(ilość) * stawka, 2, MidpointRounding.AwayFromZero);
                                                    suma += wartość;

                                                    WypełnijTagXml(nowySkładnikOpłat, "nazwa", składnikCzynszu.nazwa);
                                                    WypełnijTagXml(nowySkładnikOpłat, "stawka", stawka.ToString("N"));
                                                    WypełnijTagXml(nowySkładnikOpłat, "ilość", ilość.ToString("N"));
                                                    WypełnijTagXml(nowySkładnikOpłat, "wartość", wartość.ToString("N"));
                                                    składnikOpłat.ParentNode.InsertBefore(nowySkładnikOpłat, składnikOpłat);
                                                }

                                                WypełnijTagXml(nowyDruk, "razem", suma.ToString("N"));
                                                składnikOpłat.ParentNode.RemoveChild(składnikOpłat);
                                                gotowaDefinicjaHtml.Add(nowyDruk.OuterXml);
                                            }
                                        }
                                    }
                                }
                            }

                            break;

                        case Enumeratory.Raport.WykazWgSkladnika:
                            {
                                List<DostępDoBazy.AktywnyLokal> wszystkieLokale = magazyn.Lokale;
                                int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                int indeksOstatniego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                wszystkieLokale = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                int nrSkładnika = identyfikatory[4];
                                DostępDoBazy.SkładnikCzynszu składnikCzynszu = magazyn.SkładnikiCzynszu[nrSkładnika];
                                DateTime początekMiesiąca = DateTime.MaxValue;
                                DateTime koniecMiesiąca = new DateTime(data.Year, data.Month, 1).AddMonths(1).AddSeconds(-1);
                                IEnumerable<DostępDoBazy.Należność> należnościDotycząceDanegoSkładnika = należności.Where(n => n.nr_skl == nrSkładnika && n.data_nal <= koniecMiesiąca);
                                IEnumerable<DostępDoBazy.Należność> należnościDoAnalizy = null;
                                nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Nazwisko", "Imię", "Adres" };
                                tytuł = "Wykaz wedlug skladnika ";
                                Enumeratory.WykazWedługSkładnika tryb = WartościSesji.TrybWykazuWgSkładnika;
                                int[] array = { 1, 2, 3, 4, 5 };

                                switch (tryb)
                                {
                                    case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                    case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                        string rodzajHistorii = null;
                                        początekMiesiąca = WartościSesji.DataWykazuWgSkładnika;
                                        należnościDoAnalizy = należnościDotycząceDanegoSkładnika.Where(n => n.data_nal >= początekMiesiąca);

                                        switch (tryb)
                                        {
                                            case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                                rodzajHistorii = "ogolem";

                                                break;

                                            case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                                rodzajHistorii = "specyfikacja";

                                                break;
                                        }

                                        tytuł += String.Format("({0} od {1:yyyy - MM} do {2:yyyy - MM})", rodzajHistorii, początekMiesiąca, koniecMiesiąca);

                                        break;

                                    case Enumeratory.WykazWedługSkładnika.Obecny:
                                        tytuł += "(Biezacy miesiac)";
                                        początekMiesiąca = data.AddDays(-data.Day + 1);
                                        należnościDoAnalizy = należnościDotycząceDanegoSkładnika.Where(n => n.data_nal >= początekMiesiąca);

                                        break;
                                }

                                switch (tryb)
                                {
                                    case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                    case Enumeratory.WykazWedługSkładnika.Obecny:
                                        nagłówki.AddRange(new string[] { "Stawka", "Ilość" });

                                        break;
                                }

                                nagłówki.Add("Wartość");

                                switch (tryb)
                                {
                                    case Enumeratory.WykazWedługSkładnika.Obecny:
                                    case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                        {
                                            List<string[]> tabela = new List<string[]>();

                                            for (int i = 1; i <= wszystkieLokale.Count; i++)
                                            {
                                                DostępDoBazy.Lokal lokal = wszystkieLokale[i - 1];
                                                IEnumerable<DostępDoBazy.Należność> należnościLokalu = należnościDoAnalizy.Where(n => n.kod_lok == lokal.kod_lok && n.nr_lok == lokal.nr_lok);
                                                List<string> opcjonalnyKawałekTablicy = null;
                                                decimal wartość = 0;

                                                switch (tryb)
                                                {
                                                    case Enumeratory.WykazWedługSkładnika.Obecny:
                                                        decimal stawka;
                                                        float ilość;

                                                        if (należnościLokalu.Any())
                                                        {
                                                            DostępDoBazy.Należność należność = należnościLokalu.Single();
                                                            stawka = należność.stawka;
                                                            ilość = należność.ilosc;
                                                            wartość = należność.kwota_nal;
                                                        }
                                                        else
                                                        {
                                                            stawka = 0;
                                                            ilość = 0;
                                                        }

                                                        opcjonalnyKawałekTablicy = new List<string>() { stawka.ToString("N"), ilość.ToString("N") };

                                                        break;

                                                    case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                                                        opcjonalnyKawałekTablicy = new List<string>();

                                                        if (należnościLokalu.Any())
                                                            wartość = należnościLokalu.Sum(n => n.kwota_nal);

                                                        break;
                                                }

                                                tabela.Add(new List<string>() { i.ToString(), lokal.kod_lok.ToString(), lokal.nr_lok.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2) }.Concat(opcjonalnyKawałekTablicy).Concat(new string[] { wartość.ToString("N") }).ToArray());
                                            }

                                            tabele.Add(tabela);
                                            podpisy.Add(składnikCzynszu.nazwa);
                                        }

                                        break;

                                    case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                        {
                                            List<DateTime> początkiMiesięcy = new List<DateTime>();
                                            List<DateTime> końceMiesięcy = new List<DateTime>();
                                            nagłówki = new List<string>() { "L.p.", "Nazwisko", "Imię", "Adres", "Miesiąc", "Stawka", "Ilość", "Wartość" };
                                            List<string[]> tabela = new List<string[]>();

                                            for (DateTime i = początekMiesiąca; i <= koniecMiesiąca; i = i.AddMonths(1))
                                            {
                                                początkiMiesięcy.Add(i);
                                                końceMiesięcy.Add(i.AddMonths(1).AddSeconds(-1));
                                            }

                                            for (int i = 1; i <= wszystkieLokale.Count; i++)
                                            {
                                                DostępDoBazy.AktywnyLokal lokal = wszystkieLokale[i - 1];
                                                int kodLokalu = lokal.kod_lok;
                                                int nrLokalu = lokal.nr_lok;
                                                IEnumerable<DostępDoBazy.Należność> należnościLokalu = należnościDoAnalizy.Where(n => n.kod_lok == kodLokalu && n.nr_lok == nrLokalu);

                                                tabela.Add(new string[] { i.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, String.Empty, String.Empty });

                                                for (int j = 0; j < początkiMiesięcy.Count; j++)
                                                {
                                                    decimal stawka = 0;
                                                    float ilość = 0;
                                                    decimal wartość = 0;
                                                    DateTime początek = początkiMiesięcy[j];

                                                    if (należnościLokalu.Any())
                                                    {
                                                        DostępDoBazy.Należność należność = należnościLokalu.SingleOrDefault(n => n.data_nal >= początek && n.data_nal <= końceMiesięcy[j]);

                                                        if (należność != null)
                                                        {
                                                            stawka = należność.stawka;
                                                            ilość = należność.ilosc;
                                                            wartość = należność.kwota_nal;
                                                        }
                                                    }

                                                    tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Format("{0:yyyy - MM}", początek), stawka.ToString("N"), ilość.ToString("N"), wartość.ToString("N") });
                                                }
                                            }

                                            tabele.Add(tabela);
                                            podpisy.Add(składnikCzynszu.nazwa);
                                        }

                                        break;
                                }
                            }

                            break;

                        case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                        case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                        case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                        case Enumeratory.Raport.ObrotySzczegolowoMiesiacLokale:
                        case Enumeratory.Raport.ObrotySzczegolowoMiesiacBudynki:
                        case Enumeratory.Raport.ObrotySzczegolowoMiesiacWspolnoty:
                        case Enumeratory.Raport.OgolemSzczegolowoMiesiacLokale:
                        case Enumeratory.Raport.OgolemSzczegolowoMiesiacBudynki:
                        case Enumeratory.Raport.OgolemSzczegolowoMiesiacWspolnoty:
                            {
                                int miesiąc = data.Month;
                                int rok = data.Year;
                                tytuł = String.Format("{0} - SZCZEGOLOWA ANALIZA ZA M-C {1:D2} - {2}", obiektRaportu, miesiąc, rok);
                                DateTime początekMiesiąca = new DateTime(rok, miesiąc, 1);
                                DateTime koniecMiesiąca = początekMiesiąca.AddMonths(1).AddSeconds(-1);
                                List<DostępDoBazy.PozycjaDoAnalizy> pozycjeZaDanyMiesiąc = pozycje.Where(p => p.Data >= początekMiesiąca && p.Data <= koniecMiesiąca).ToList();

                                switch (_raport)
                                {
                                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacLokale:
                                    case Enumeratory.Raport.ObrotySzczegolowoMiesiacLokale:
                                    case Enumeratory.Raport.OgolemSzczegolowoMiesiacLokale:
                                        {
                                            int kodPierwszegoBudynku = identyfikatory[0];
                                            int kodOstatniegoBudynku = identyfikatory[2];
                                            int numerPierwszegoLokalu = identyfikatory[1];
                                            int numerOstatniegoLokalu = identyfikatory[3];
                                            List<string[]> tabela = new List<string[]>();
                                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Nazwisko<br /><br />Typ Lokalu", "Imię", "Adres<br /><br />Nazwa składnika", "Stawka", "Ilość", "Kwota" };
                                            decimal sumaOgólna = 0;
                                            int liczbaPorządkowa = 1;
                                            int liczbaKolumn = nagłówki.Count;
                                            SortedDictionary<int, SortedDictionary<int, DostępDoBazy.Lokal>> kodINumerNaLokal = magazyn.KodINumerNaLokal;

                                            for (int kodBudynku = kodPierwszegoBudynku; kodBudynku <= kodOstatniegoBudynku; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (magazyn.KodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    decimal sumaBudynku = 0;
                                                    SortedDictionary<int, DostępDoBazy.Lokal> numerNaLokal = kodINumerNaLokal[kodBudynku];
                                                    int numerPierwszego = numerNaLokal.First().Key;
                                                    int numerOstatniego = numerNaLokal.Last().Key;

                                                    if (kodBudynku == kodPierwszegoBudynku)
                                                        numerPierwszego = numerPierwszegoLokalu;

                                                    if (kodBudynku == kodOstatniegoBudynku)
                                                        numerOstatniego = numerOstatniegoLokalu;

                                                    for (int numerLokalu = numerPierwszego; numerLokalu <= numerOstatniego; numerLokalu++)
                                                    {
                                                        DostępDoBazy.Lokal lokal;

                                                        if (numerNaLokal.TryGetValue(numerLokalu, out lokal))
                                                        {

                                                            int kodLok = lokal.kod_lok;
                                                            int nrLok = lokal.nr_lok;
                                                            IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeLokalu = pozycjeZaDanyMiesiąc.Where(p => p.KodBudynku == kodLok && p.NrLokalu == nrLok);
                                                            decimal suma = 0;
                                                            DostępDoBazy.TypLokalu typLokalu = magazyn.TypyLokali[lokal.kod_typ];
                                                            string nazwaTypuLokalu;

                                                            if (typLokalu == null)
                                                                nazwaTypuLokalu = String.Empty;
                                                            else
                                                                nazwaTypuLokalu = typLokalu.typ_lok;

                                                            tabela.Add(new string[] { kodBudynku.ToString(), kodLok.ToString(), nrLok.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, String.Empty });
                                                            tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, nazwaTypuLokalu, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty });

                                                            foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeLokalu)
                                                            {
                                                                DostępDoBazy.IInformacjeOPozycji informacje = pozycja.Informacje;
                                                                decimal kwota = pozycja.Kwota;
                                                                suma += kwota;

                                                                tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, informacje.Nazwa, pozycja.Stawka.ToString("N"), pozycja.Ilość.ToString("N"), kwota.ToString("N") });
                                                            }

                                                            sumaBudynku += suma;

                                                            tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", String.Empty, String.Empty, suma.ToString("N") });
                                                            tabela.Add(PustaLinia(liczbaKolumn));

                                                            liczbaPorządkowa++;
                                                        }
                                                    }

                                                    tabela.Add(new string[] { String.Empty, kodBudynku.ToString(), String.Empty, "<b>RAZEM</b>", "<b>BUDYNEK</b>", String.Format("<b>{0} {1}</b>", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });

                                                    sumaOgólna += sumaBudynku;
                                                }
                                            }

                                            tabela.Add(PustaLinia(liczbaKolumn));
                                            tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", "<b>zakres lokali</b>", String.Format("<b>{0} - {1}</b>", kodPierwszegoBudynku, numerPierwszegoLokalu), String.Format("<b>{0} - {1}</b>", kodOstatniegoBudynku, numerOstatniegoLokalu), sumaOgólna.ToString("N") });
                                            podpisy.Add(String.Empty);
                                            tabele.Add(tabela);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacBudynki:
                                    case Enumeratory.Raport.ObrotySzczegolowoMiesiacBudynki:
                                    case Enumeratory.Raport.OgolemSzczegolowoMiesiacBudynki:
                                        {
                                            List<string[]> tabela = new List<string[]>();
                                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Adres", "Nazwa składnika", "Kwota" };
                                            int kodPierwszego = identyfikatory[0];
                                            int kodOstatniego = identyfikatory[2];

                                            for (int kodBudynku = kodPierwszego; kodBudynku <= kodOstatniego; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (magazyn.KodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    Dictionary<int, decimal> nrSkładnikaNaSumę = new Dictionary<int, decimal>();
                                                    decimal sumaBudynku = 0;

                                                    tabela.Add(new string[] { kodBudynku.ToString(), kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty });

                                                    foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(p => p.KodBudynku == kodBudynku))
                                                    {
                                                        int nrSkładnika = pozycja.IdInformacji;
                                                        decimal kwota = pozycja.Kwota;

                                                        if (nrSkładnikaNaSumę.ContainsKey(nrSkładnika))
                                                            nrSkładnikaNaSumę[nrSkładnika] += kwota;
                                                        else
                                                            nrSkładnikaNaSumę.Add(nrSkładnika, kwota);
                                                    }

                                                    foreach (KeyValuePair<int, decimal> składnik in nrSkładnikaNaSumę)
                                                    {
                                                        DostępDoBazy.IInformacjeOPozycji informacja = informacjeOPozycjach.Single(n => n.IdInformacji == składnik.Key);
                                                        decimal kwota = składnik.Value;
                                                        sumaBudynku += kwota;

                                                        tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, informacja.Nazwa, kwota.ToString("N") });
                                                    }

                                                    tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", sumaBudynku.ToString("N") });
                                                    tabela.Add(PustaLinia(nagłówki.Count));
                                                }
                                            }

                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciSzczegolowoMiesiacWspolnoty:
                                    case Enumeratory.Raport.ObrotySzczegolowoMiesiacWspolnoty:
                                    case Enumeratory.Raport.OgolemSzczegolowoMiesiacWspolnoty:
                                        {
                                            int kodPierwszejWspólnoty = identyfikatory[4];
                                            int kodOstatniejWspólnoty = identyfikatory[5];
                                            nagłówki = new List<string>() { "Kod budynku", "Adres budynku", "Nazwa składnika", "Kwota" };

                                            for (int kodWspólnoty = kodPierwszejWspólnoty; kodWspólnoty <= kodOstatniejWspólnoty; kodWspólnoty++)
                                            {
                                                DostępDoBazy.Wspólnota wspólnota;

                                                if (magazyn.KodNaWspólnotę.TryGetValue(kodWspólnoty, out wspólnota))
                                                {
                                                    List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == wspólnota.kod).OrderBy(b => b.kod_1).ToList();
                                                    List<string[]> tabela = new List<string[]>();
                                                    decimal sumaWspólnoty = 0;

                                                    foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                    {
                                                        DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[budynekWspólnoty.kod_1];
                                                        Dictionary<int, decimal> nrSkładnikaNaSumę = new Dictionary<int, decimal>();
                                                        int kodBudynku = budynek.kod_1;
                                                        decimal sumaBudynku = 0;

                                                        tabela.Add(new string[] { kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty });

                                                        foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(p => p.KodBudynku == kodBudynku))
                                                        {
                                                            int nrSkładnika = pozycja.IdInformacji;
                                                            decimal kwota = pozycja.Kwota;

                                                            if (nrSkładnikaNaSumę.ContainsKey(nrSkładnika))
                                                                nrSkładnikaNaSumę[nrSkładnika] += kwota;
                                                            else
                                                                nrSkładnikaNaSumę.Add(nrSkładnika, kwota);
                                                        }

                                                        foreach (KeyValuePair<int, decimal> składnik in nrSkładnikaNaSumę)
                                                        {
                                                            DostępDoBazy.IInformacjeOPozycji informacje = informacjeOPozycjach.Single(s => s.IdInformacji == składnik.Key);
                                                            decimal kwota = składnik.Value;
                                                            sumaBudynku += kwota;

                                                            tabela.Add(new string[] { String.Empty, String.Empty, informacje.Nazwa, kwota.ToString("N") });
                                                        }

                                                        sumaWspólnoty += sumaBudynku;

                                                        tabela.Add(new string[] { String.Empty, String.Empty, "RAZEM", sumaBudynku.ToString("N") });
                                                        tabela.Add(PustaLinia(nagłówki.Count));
                                                    }

                                                    tabela.Add(new string[] { String.Empty, String.Empty, "RAZEM WSPÓLNOTA", sumaWspólnoty.ToString("N") });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(wspólnota.nazwa_pel);
                                                }
                                            }
                                        }

                                        break;
                                }
                            }

                            break;

                        case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                        case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                        case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                        case Enumeratory.Raport.ObrotyWgEwidencjiLokale:
                        case Enumeratory.Raport.ObrotyWgEwidencjiBudynki:
                        case Enumeratory.Raport.ObrotyWgEwidencjiWspolnoty:
                        case Enumeratory.Raport.OgolemWgEwidencjiLokale:
                        case Enumeratory.Raport.OgolemWgEwidencjiBudynki:
                        case Enumeratory.Raport.OgolemWgEwidencjiWspolnoty:
                            {
                                DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                                DateTime koniecMiesiąca = początekMiesiąca.AddMonths(1).AddSeconds(-1);
                                List<DostępDoBazy.PozycjaDoAnalizy> pozycjeZaDanyMiesiąc = pozycje.Where(n => n.Data >= początekMiesiąca && n.Data <= koniecMiesiąca).ToList();
                                tytuł = String.Format("{0} - KWOTA WG RODZAJU EWIDENCJI ZA M-C {1:D2} - {2}", obiektRaportu, data.Month, data.Year);
                                Dictionary<int, decimal> słownikWzorcowy = new Dictionary<int, decimal>() { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 } };

                                switch (_raport)
                                {
                                    case Enumeratory.Raport.NaleznosciWgEwidencjiLokale:
                                    case Enumeratory.Raport.ObrotyWgEwidencjiLokale:
                                    case Enumeratory.Raport.OgolemWgEwidencjiLokale:
                                        {
                                            int kodPierwszegoBudynku = identyfikatory[0];
                                            int kodOstatniegoBudynku = identyfikatory[2];
                                            int numerPierwszegoLokalu = identyfikatory[1];
                                            int numerOstatniegoLokalu = identyfikatory[3];

                                            /*List<DostępDoBazy.AktywnyLokal> lokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                            int indeksPierwszego = lokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                            int indeksOstatniego = lokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                            List<DostępDoBazy.AktywnyLokal> lokaleDoAnalizy = lokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                            DostępDoBazy.AktywnyLokal pierwszyLokal = lokaleDoAnalizy.First();
                                            DostępDoBazy.AktywnyLokal ostatniLokal = lokaleDoAnalizy.Last();*/
                                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr budynku", "Nazwisko", "Imię", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Ogólna kwota" };
                                            int liczbaPorządkowa = 1;
                                            List<string[]> tabela = new List<string[]>();
                                            Dictionary<int, decimal> rodzajEwidencjiNaSumęOgólną = new Dictionary<int, decimal>(słownikWzorcowy);

                                            for (int kodBudynku = kodPierwszegoBudynku; kodBudynku <= kodOstatniegoBudynku; kodBudynku++)
                                            {
                                                SortedDictionary<int, DostępDoBazy.Lokal> numerNaLokal;

                                                if (magazyn.KodINumerNaLokal.TryGetValue(kodBudynku, out numerNaLokal))
                                                {
                                                    DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[kodBudynku];
                                                    Dictionary<int, decimal> rodzajEwidencjiNaSumęBudynku = new Dictionary<int, decimal>(słownikWzorcowy);
                                                    int numerPierwszego = numerNaLokal.First().Value.nr_lok;
                                                    int numerOstatniego = numerNaLokal.Last().Value.nr_lok;

                                                    if (kodBudynku == kodPierwszegoBudynku)
                                                        numerPierwszego = numerPierwszegoLokalu;

                                                    if (kodBudynku == kodOstatniegoBudynku)
                                                        numerOstatniego = numerOstatniegoLokalu;

                                                    for (int numerLokalu = numerPierwszego; numerLokalu <= numerOstatniego; numerLokalu++)
                                                    {
                                                        DostępDoBazy.Lokal lokal;

                                                        if (numerNaLokal.TryGetValue(numerLokalu, out lokal))
                                                        {
                                                            Dictionary<int, decimal> rodzajEwidencjiNaSumę = new Dictionary<int, decimal>(słownikWzorcowy);

                                                            foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(n => n.KodBudynku == kodBudynku && n.NrLokalu == numerLokalu))
                                                            {
                                                                decimal kwota = pozycja.Kwota;
                                                                DostępDoBazy.IInformacjeOPozycji składnik = pozycja.Informacje;
                                                                rodzajEwidencjiNaSumę[składnik.RodzajEwidencji] += kwota;
                                                            }

                                                            foreach (int klucz in rodzajEwidencjiNaSumę.Keys)
                                                                rodzajEwidencjiNaSumęBudynku[klucz] += rodzajEwidencjiNaSumę[klucz];

                                                            tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodBudynku.ToString(), numerLokalu.ToString(), lokal.nazwisko, lokal.imie, rodzajEwidencjiNaSumę[1].ToString("N"), rodzajEwidencjiNaSumę[2].ToString("N"), rodzajEwidencjiNaSumę[3].ToString("N"), rodzajEwidencjiNaSumę[4].ToString("N"), rodzajEwidencjiNaSumę.Values.Sum().ToString("N") });

                                                            liczbaPorządkowa++;
                                                        }
                                                    }

                                                    tabela.Add(new string[] { String.Empty, kodBudynku.ToString(), String.Empty, "<b>RAZEM</b>", "BUDYNEK", rodzajEwidencjiNaSumęBudynku[1].ToString("N"), rodzajEwidencjiNaSumęBudynku[2].ToString("N"), rodzajEwidencjiNaSumęBudynku[3].ToString("N"), rodzajEwidencjiNaSumęBudynku[4].ToString("N"), rodzajEwidencjiNaSumęBudynku.Values.Sum().ToString("N") });
                                                    tabela.Add(PustaLinia(nagłówki.Count));

                                                    foreach (int klucz in rodzajEwidencjiNaSumęBudynku.Keys)
                                                        rodzajEwidencjiNaSumęOgólną[klucz] += rodzajEwidencjiNaSumęBudynku[klucz];
                                                }
                                            }

                                            tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "<b>RAZEM ZAKRES LOKALI</b>", String.Format("{0}-{1} - {2}-{3}", kodOstatniegoBudynku, numerPierwszegoLokalu, kodOstatniegoBudynku, numerOstatniegoLokalu), rodzajEwidencjiNaSumęOgólną[1].ToString("N"), rodzajEwidencjiNaSumęOgólną[2].ToString("N"), rodzajEwidencjiNaSumęOgólną[3].ToString("N"), rodzajEwidencjiNaSumęOgólną[4].ToString("N"), rodzajEwidencjiNaSumęOgólną.Values.Sum().ToString("N") });
                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgEwidencjiBudynki:
                                    case Enumeratory.Raport.ObrotyWgEwidencjiBudynki:
                                    case Enumeratory.Raport.OgolemWgEwidencjiBudynki:
                                        {
                                            int kodPierwszego = identyfikatory[0];
                                            int kodOstatniego = identyfikatory[2];
                                            int liczbaPorządkowa = 1;
                                            List<string[]> tabela = new List<string[]>();
                                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Adres", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Ogólna kwota" };
                                            Dictionary<int, decimal> rodzajEwidencjiNaSumęOgólną = new Dictionary<int, decimal>(słownikWzorcowy);

                                            for (int kodBudynku = kodPierwszego; kodBudynku <= kodOstatniego; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (magazyn.KodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    Dictionary<int, decimal> rodzajEwidencjiNaSumęBudynku = new Dictionary<int, decimal>(słownikWzorcowy);

                                                    foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(n => n.KodBudynku == kodBudynku))
                                                    {
                                                        decimal kwota = pozycja.Kwota;
                                                        DostępDoBazy.IInformacjeOPozycji składnik = pozycja.Informacje;
                                                        rodzajEwidencjiNaSumęBudynku[składnik.RodzajEwidencji] += kwota;
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), rodzajEwidencjiNaSumęBudynku[1].ToString("N"), rodzajEwidencjiNaSumęBudynku[2].ToString("N"), rodzajEwidencjiNaSumęBudynku[3].ToString("N"), rodzajEwidencjiNaSumęBudynku[4].ToString("N"), rodzajEwidencjiNaSumęBudynku.Values.Sum().ToString("N") });

                                                    foreach (int klucz in rodzajEwidencjiNaSumęBudynku.Keys)
                                                        rodzajEwidencjiNaSumęOgólną[klucz] += rodzajEwidencjiNaSumęBudynku[klucz];

                                                    liczbaPorządkowa++;
                                                }
                                            }

                                            tabela.Add(PustaLinia(nagłówki.Count));
                                            tabela.Add(new string[] { String.Empty, String.Empty, String.Format("RAZEM BUDYNKI {0} - {1}", kodPierwszego, kodOstatniego), rodzajEwidencjiNaSumęOgólną[1].ToString("N"), rodzajEwidencjiNaSumęOgólną[2].ToString("N"), rodzajEwidencjiNaSumęOgólną[3].ToString("N"), rodzajEwidencjiNaSumęOgólną[4].ToString("N"), rodzajEwidencjiNaSumęOgólną.Values.Sum().ToString("N") });
                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgEwidencjiWspolnoty:
                                    case Enumeratory.Raport.ObrotyWgEwidencjiWspolnoty:
                                    case Enumeratory.Raport.OgolemWgEwidencjiWspolnoty:
                                        {
                                            int numerPierwszej = identyfikatory[4];
                                            int numerOstatniej = identyfikatory[5];
                                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Adres", "Dziennik komornego", "Wpłaty", "Zmniejszenia", "Zwiększenia", "Ogólna kwota" };
                                            Dictionary<int, decimal> rodzajEwidencjiNaSumęOgólną = new Dictionary<int, decimal>(słownikWzorcowy);

                                            for (int kodWspólnoty = numerPierwszej; kodWspólnoty <= numerOstatniej; kodWspólnoty++)
                                            {
                                                DostępDoBazy.Wspólnota wspólnota;

                                                if (magazyn.KodNaWspólnotę.TryGetValue(kodWspólnoty, out wspólnota))
                                                {
                                                    List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == kodWspólnoty).OrderBy(b => b.kod_1).ToList();
                                                    List<string[]> tabela = new List<string[]>();
                                                    int liczbaPorządkowa = 1;
                                                    Dictionary<int, decimal> rodzajEwidencjiNaSumęWspólnoty = new Dictionary<int, decimal>(słownikWzorcowy);

                                                    foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                    {
                                                        int kodBudynku = budynekWspólnoty.kod_1;
                                                        DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[kodBudynku];
                                                        Dictionary<int, decimal> rodzajEwidencjiNaSumęBudynku = new Dictionary<int, decimal>(słownikWzorcowy);

                                                        foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc.Where(n => n.KodBudynku == kodWspólnoty))
                                                        {
                                                            decimal kwota = pozycja.Kwota;
                                                            DostępDoBazy.IInformacjeOPozycji składnik = pozycja.Informacje;
                                                            rodzajEwidencjiNaSumęBudynku[składnik.RodzajEwidencji] += kwota;
                                                        }

                                                        foreach (int klucz in rodzajEwidencjiNaSumęBudynku.Keys)
                                                            rodzajEwidencjiNaSumęWspólnoty[klucz] += rodzajEwidencjiNaSumęBudynku[klucz];

                                                        tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodWspólnoty.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), rodzajEwidencjiNaSumęBudynku[1].ToString("N"), rodzajEwidencjiNaSumęBudynku[2].ToString("N"), rodzajEwidencjiNaSumęBudynku[3].ToString("N"), rodzajEwidencjiNaSumęBudynku[4].ToString("N"), rodzajEwidencjiNaSumęBudynku.Values.Sum().ToString("N") });
                                                    }

                                                    tabela.Add(new string[] { String.Empty, String.Empty, "RAZEM", rodzajEwidencjiNaSumęWspólnoty[1].ToString("N"), rodzajEwidencjiNaSumęWspólnoty[2].ToString("N"), rodzajEwidencjiNaSumęWspólnoty[3].ToString("N"), rodzajEwidencjiNaSumęWspólnoty[4].ToString("N"), rodzajEwidencjiNaSumęWspólnoty.Values.Sum().ToString("N") });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                                }
                                            }

                                            tabele.Last().Add(new string[] { String.Empty, String.Empty, String.Format("RAZEM WSPÓLNOTY {0} - {1}", numerPierwszej, numerOstatniej), rodzajEwidencjiNaSumęOgólną[1].ToString("N"), rodzajEwidencjiNaSumęOgólną[2].ToString("N"), rodzajEwidencjiNaSumęOgólną[3].ToString("N"), rodzajEwidencjiNaSumęOgólną[4].ToString("N"), rodzajEwidencjiNaSumęOgólną.Values.Sum().ToString("N") });
                                        }

                                        break;
                                }
                            }

                            break;

                        case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                        case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                        case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                        case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                        case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                        case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                        case Enumeratory.Raport.ObrotyWgGrupSkladnikiLokale:
                        case Enumeratory.Raport.ObrotyWgGrupSkladnikiBudynki:
                        case Enumeratory.Raport.ObrotyWgGrupSkladnikiWspolnoty:
                        case Enumeratory.Raport.ObrotyWgGrupSumyLokale:
                        case Enumeratory.Raport.ObrotyWgGrupSumyBudynki:
                        case Enumeratory.Raport.ObrotyWgGrupSumyWspolnoty:
                        case Enumeratory.Raport.OgolemWgGrupSkladnikiLokale:
                        case Enumeratory.Raport.OgolemWgGrupSkladnikiBudynki:
                        case Enumeratory.Raport.OgolemWgGrupSkladnikiWspolnoty:
                        case Enumeratory.Raport.OgolemWgGrupSumyLokale:
                        case Enumeratory.Raport.OgolemWgGrupSumyBudynki:
                        case Enumeratory.Raport.OgolemWgGrupSumyWspolnoty:
                            {
                                DateTime początekMiesiąca = new DateTime(data.Year, data.Month, 1);
                                DateTime koniecMiesiąca = początekMiesiąca.AddMonths(1).AddSeconds(-1);
                                tytuł = String.Format("{0} - ANALIZA WG GRUP CZYNSZOWYCH ZA M-C {1:D2} - {2}", obiektRaportu, data.Month, data.Year);
                                List<int> grupySkładnikówCzynszu = WartościSesji.NumeryGrupWybranychSkładnikówCzynszu;

                                grupySkładnikówCzynszu.Sort();

                                List<DostępDoBazy.PozycjaDoAnalizy> pozycjeZaDanyMiesiąc = pozycje.Where(n => n.Data >= początekMiesiąca && n.Data <= koniecMiesiąca).ToList();
                                Dictionary<int, List<DostępDoBazy.PozycjaDoAnalizy>> numerGrupyNaPozycje = new Dictionary<int, List<DostępDoBazy.PozycjaDoAnalizy>>();
                                List<DostępDoBazy.GrupaSkładnikówCzynszu> wybraneGrupy = new List<DostępDoBazy.GrupaSkładnikówCzynszu>();

                                foreach (int grupa in grupySkładnikówCzynszu)
                                {
                                    numerGrupyNaPozycje.Add(grupa, new List<DostępDoBazy.PozycjaDoAnalizy>());
                                    wybraneGrupy.Add(db.GrupySkładnikówCzynszu.Single(g => g.kod == grupa));
                                }

                                foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeZaDanyMiesiąc)
                                {
                                    int numerGrupy = pozycja.Informacje.Grupa;

                                    if (numerGrupyNaPozycje.ContainsKey(numerGrupy))
                                        numerGrupyNaPozycje[numerGrupy].Add(pozycja);
                                }

                                switch (_raport)
                                {
                                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiLokale:
                                    case Enumeratory.Raport.ObrotyWgGrupSkladnikiLokale:
                                    case Enumeratory.Raport.OgolemWgGrupSkladnikiLokale:
                                        {
                                            int kodPierwszegoBudynku = identyfikatory[0];
                                            int numerPierwszegoLokalu = identyfikatory[1];
                                            int kodOstatniegoBudynku = identyfikatory[2];
                                            int numerOstatniegoLokalu = identyfikatory[3];
                                            nagłówki = new List<string>() { "L.p.", "Kod budynku", "Nr lokalu", "Nazwisko", "Imię<br /><br />Nazwa składnika", "Adres<br /><br />Grupa składnika", "Ilość", "Stawka", "Kwota" };
                                            int liczbaKolumn = nagłówki.Count;
                                            int liczbaPorządkowa = 1;
                                            decimal sumaOgólna = 0;

                                            for (int kodBudynku = kodPierwszegoBudynku; kodBudynku <= kodOstatniegoBudynku; kodBudynku++)
                                            {
                                                SortedDictionary<int, DostępDoBazy.Lokal> numerNaLokal;

                                                if (magazyn.KodINumerNaLokal.TryGetValue(kodBudynku, out numerNaLokal))
                                                {
                                                    DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[kodBudynku];
                                                    List<string[]> tabela = new List<string[]>();
                                                    decimal sumaBudynku = 0;
                                                    int numerPierwszego = numerNaLokal.First().Value.nr_lok;
                                                    int numerOstatniego = numerNaLokal.Last().Value.nr_lok;

                                                    if (kodBudynku == kodPierwszegoBudynku)
                                                        numerPierwszego = numerPierwszegoLokalu;

                                                    if (kodBudynku == kodOstatniegoBudynku)
                                                        numerOstatniego = numerOstatniegoLokalu;

                                                    for (int numerLokalu = numerPierwszego; numerLokalu <= numerOstatniego; numerLokalu++)
                                                    {
                                                        DostępDoBazy.Lokal lokal;

                                                        if (numerNaLokal.TryGetValue(numerLokalu, out lokal))
                                                        {
                                                            decimal sumaLokalu = 0;
                                                            List<string[]> podTabela = new List<string[]>();

                                                            for (int j = 0; j < numerGrupyNaPozycje.Count; j++)
                                                            {
                                                                KeyValuePair<int, List<DostępDoBazy.PozycjaDoAnalizy>> para = numerGrupyNaPozycje.ElementAt(j);
                                                                IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeLokalu = para.Value.Where(n => n.KodBudynku == kodBudynku && n.NrLokalu == numerLokalu);
                                                                DostępDoBazy.GrupaSkładnikówCzynszu informacjeOGrupie = wybraneGrupy[j];
                                                                decimal sumaGrupy = 0;
                                                                decimal sumaStawek = 0;

                                                                foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeLokalu)
                                                                {
                                                                    DostępDoBazy.IInformacjeOPozycji informacje = pozycja.Informacje;
                                                                    decimal kwota = pozycja.Kwota;
                                                                    decimal stawka = pozycja.Stawka;
                                                                    sumaGrupy += kwota;
                                                                    sumaStawek += stawka;

                                                                    podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, informacje.Nazwa, informacjeOGrupie.nazwa, pozycja.Ilość.ToString("N"), stawka.ToString("N"), kwota.ToString("N") });
                                                                }

                                                                sumaLokalu += sumaGrupy;

                                                                podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "RAZEM", informacjeOGrupie.nazwa, String.Empty, sumaStawek.ToString("N"), sumaGrupy.ToString("N") });
                                                                podTabela.Add(PustaLinia(liczbaKolumn));
                                                            }

                                                            tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodBudynku.ToString(), numerLokalu.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, sumaLokalu.ToString("N") });
                                                            tabela.Add(PustaLinia(liczbaKolumn));
                                                            tabela.AddRange(podTabela);

                                                            liczbaPorządkowa++;
                                                            sumaBudynku += sumaLokalu;
                                                        }
                                                    }

                                                    tabela.Add(new string[] { String.Empty, kodBudynku.ToString(), String.Empty, String.Empty, String.Empty, "RAZEM BUDYNEK", String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Format("Budynek nr {0}, {1} {2}", kodBudynku, budynek.adres, budynek.adres_2));

                                                    sumaOgólna += sumaBudynku;
                                                }
                                            }

                                            List<string[]> ostatniaTabela = tabele.Last();

                                            ostatniaTabela.Add(PustaLinia(liczbaKolumn));
                                            ostatniaTabela.Add(PustaLinia(liczbaKolumn));
                                            ostatniaTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, "<b>RAZEM</b>", String.Format("ZAKRES LOKALI {0}-{1} - {2}-{3}", kodPierwszegoBudynku, numerPierwszegoLokalu, kodOstatniegoBudynku, numerOstatniegoLokalu), String.Empty, String.Empty, sumaOgólna.ToString("N") });
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiBudynki:
                                    case Enumeratory.Raport.ObrotyWgGrupSkladnikiBudynki:
                                    case Enumeratory.Raport.OgolemWgGrupSkladnikiBudynki:
                                        {
                                            int numerPierwszego = identyfikatory[0];
                                            int numerOstatniego = identyfikatory[2];
                                            nagłówki = new List<string>() { "L.p", "Kod budynku", "Adres", "Składnik", "Grupa", "Kwota" };
                                            List<string[]> tabela = new List<string[]>();
                                            int liczbaPorządkowa = 1;
                                            int liczbaKolumn = nagłówki.Count;

                                            for (int kodBudynku = numerPierwszego; kodBudynku <= numerOstatniego; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (magazyn.KodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    decimal sumaBudynku = 0;
                                                    List<string[]> podTabela = new List<string[]>();

                                                    for (int j = 0; j < numerGrupyNaPozycje.Count; j++)
                                                    {
                                                        KeyValuePair<int, List<DostępDoBazy.PozycjaDoAnalizy>> paraGrupaPozycje = numerGrupyNaPozycje.ElementAt(j);
                                                        string nazwaGrupy = wybraneGrupy[j].nazwa;
                                                        IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeBudynku = paraGrupaPozycje.Value.Where(n => n.KodBudynku == kodBudynku);
                                                        Dictionary<int, decimal> numerSkładnikaNaSumę = new Dictionary<int, decimal>();

                                                        foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeBudynku)
                                                        {
                                                            int numerSkładnika = pozycja.Informacje.IdInformacji;
                                                            decimal kwota = pozycja.Kwota;

                                                            if (numerSkładnikaNaSumę.ContainsKey(numerSkładnika))
                                                                numerSkładnikaNaSumę[numerSkładnika] += kwota;
                                                            else
                                                                numerSkładnikaNaSumę.Add(numerSkładnika, kwota);
                                                        }

                                                        foreach (KeyValuePair<int, decimal> paraSkładnikSuma in numerSkładnikaNaSumę)
                                                        {
                                                            DostępDoBazy.IInformacjeOPozycji składnik = informacjeOPozycjach.Single(s => s.IdInformacji == paraSkładnikSuma.Key);

                                                            podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, składnik.Nazwa, nazwaGrupy, paraSkładnikSuma.Value.ToString("N") });
                                                        }

                                                        decimal sumaGrupy = numerSkładnikaNaSumę.Values.Sum();
                                                        sumaBudynku += sumaGrupy;

                                                        podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM", nazwaGrupy, sumaGrupy.ToString("N") });
                                                        podTabela.Add(PustaLinia(liczbaKolumn));
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                    tabela.Add(PustaLinia(liczbaKolumn));
                                                    tabela.AddRange(podTabela);

                                                    liczbaPorządkowa++;
                                                }
                                            }

                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgGrupSkladnikiWspolnoty:
                                    case Enumeratory.Raport.ObrotyWgGrupSkladnikiWspolnoty:
                                    case Enumeratory.Raport.OgolemWgGrupSkladnikiWspolnoty:
                                        {
                                            int kodPierwszej = identyfikatory[4];
                                            int kodOstatniej = identyfikatory[5];
                                            int liczbaPorządkowa = 1;
                                            nagłówki = new List<string>() { "L.p", "Kod budynku", "Adres", "Składnik", "Grupa", "Kwota" };
                                            int liczbaKolumn = nagłówki.Count;

                                            for (int kodWspólnoty = kodPierwszej; kodWspólnoty <= kodOstatniej; kodWspólnoty++)
                                            {
                                                DostępDoBazy.Wspólnota wspólnota;

                                                if (magazyn.KodNaWspólnotę.TryGetValue(kodWspólnoty, out wspólnota))
                                                {
                                                    List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == kodWspólnoty).ToList();
                                                    List<string[]> tabela = new List<string[]>();

                                                    foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                    {
                                                        DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[budynekWspólnoty.kod_1];
                                                        decimal sumaBudynku = 0;
                                                        List<string[]> podTabela = new List<string[]>();

                                                        for (int j = 0; j < numerGrupyNaPozycje.Count; j++)
                                                        {
                                                            KeyValuePair<int, List<DostępDoBazy.PozycjaDoAnalizy>> paraGrupaNaPozycje = numerGrupyNaPozycje.ElementAt(j);
                                                            string nazwaGrupy = wybraneGrupy[j].nazwa;
                                                            IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeBudynku = paraGrupaNaPozycje.Value.Where(n => n.KodBudynku == kodWspólnoty);
                                                            Dictionary<int, decimal> numerSkładnikaNaSumę = new Dictionary<int, decimal>();

                                                            foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeBudynku)
                                                            {
                                                                int numerSkładnika = pozycja.Informacje.IdInformacji;
                                                                decimal kwota = pozycja.Kwota;

                                                                if (numerSkładnikaNaSumę.ContainsKey(numerSkładnika))
                                                                    numerSkładnikaNaSumę[numerSkładnika] += kwota;
                                                                else
                                                                    numerSkładnikaNaSumę.Add(numerSkładnika, kwota);
                                                            }

                                                            foreach (KeyValuePair<int, decimal> paraSkładnikSuma in numerSkładnikaNaSumę)
                                                            {
                                                                DostępDoBazy.IInformacjeOPozycji informacja = informacjeOPozycjach.Single(s => s.IdInformacji == paraSkładnikSuma.Key);

                                                                podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, informacja.Nazwa, nazwaGrupy, paraSkładnikSuma.Value.ToString("N") });
                                                            }

                                                            decimal sumaGrupy = numerSkładnikaNaSumę.Values.Sum();
                                                            sumaBudynku += sumaGrupy;

                                                            podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM", nazwaGrupy, sumaGrupy.ToString("N") });
                                                            podTabela.Add(PustaLinia(liczbaKolumn));
                                                        }

                                                        tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodWspólnoty.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                        tabela.Add(PustaLinia(liczbaKolumn));
                                                        tabela.AddRange(podTabela);
                                                        tabele.Add(tabela);
                                                        podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));

                                                        liczbaPorządkowa++;
                                                    }
                                                }
                                            }
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgGrupSumyLokale:
                                    case Enumeratory.Raport.ObrotyWgGrupSumyLokale:
                                    case Enumeratory.Raport.OgolemWgGrupSumyLokale:
                                        {
                                            int kodPierwszegoBudynku = identyfikatory[0];
                                            int numerPierwszegoLokalu = identyfikatory[1];
                                            int kodOstatniegoBudynku = identyfikatory[2];
                                            int numerOstatniegoLokalu = identyfikatory[3];

                                            /*List<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok).ToList();
                                            int indeksPierwszego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[0] && l.nr_lok == identyfikatory[1]);
                                            int indeksOstatniego = wszystkieLokale.FindIndex(l => l.kod_lok == identyfikatory[2] && l.nr_lok == identyfikatory[3]);
                                            List<DostępDoBazy.AktywnyLokal> lokaleDoAnalizy = wszystkieLokale.GetRange(indeksPierwszego, indeksOstatniego - indeksPierwszego + 1);
                                            DostępDoBazy.AktywnyLokal pierwszyLokal = lokaleDoAnalizy.First();
                                            DostępDoBazy.AktywnyLokal ostatniLokal = lokaleDoAnalizy.Last();*/
                                            int liczbaPorządkowa = 1;
                                            nagłówki = new List<string>() { "Lp.", "Kod budynku", "Nr lokalu", "Nazwisko", "Imię", "Adres", "Grupa", "Ilość", "Kwota" };
                                            decimal sumaOgólna = 0;
                                            int liczbaKolumn = nagłówki.Count;

                                            for (int kodBudynku = kodPierwszegoBudynku; kodBudynku <= kodOstatniegoBudynku; kodBudynku++)
                                            {
                                                SortedDictionary<int, DostępDoBazy.Lokal> numerNaLokal;

                                                if (magazyn.KodINumerNaLokal.TryGetValue(kodBudynku, out numerNaLokal))
                                                {
                                                    List<string[]> tabela = new List<string[]>();
                                                    decimal sumaBudynku = 0;
                                                    DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[kodBudynku];
                                                    int numerPierwszego = numerNaLokal.First().Value.nr_lok;
                                                    int numerOstatniego = numerNaLokal.Last().Value.nr_lok;

                                                    if (kodBudynku == kodPierwszegoBudynku)
                                                        numerPierwszego = numerPierwszegoLokalu;

                                                    if (kodBudynku == kodOstatniegoBudynku)
                                                        numerOstatniego = numerOstatniegoLokalu;

                                                    for (int numerLokalu = numerPierwszego; numerLokalu <= numerOstatniego; numerLokalu++)
                                                    {
                                                        decimal sumaLokalu = 0;
                                                        List<string[]> podTabela = new List<string[]>();
                                                        DostępDoBazy.Lokal lokal;

                                                        if (numerNaLokal.TryGetValue(numerLokalu, out lokal))
                                                        {

                                                            for (int j = 0; j < numerGrupyNaPozycje.Count; j++)
                                                            {
                                                                KeyValuePair<int, List<DostępDoBazy.PozycjaDoAnalizy>> para = numerGrupyNaPozycje.ElementAt(j);
                                                                IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeLokalu = para.Value.Where(n => n.KodBudynku == kodBudynku && n.NrLokalu == numerLokalu);
                                                                DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupy[j];
                                                                decimal sumaKwot = 0;
                                                                float sumaIlości = 0;

                                                                foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeLokalu)
                                                                {
                                                                    sumaIlości += pozycja.Ilość;
                                                                    sumaKwot += pozycja.Kwota;
                                                                }

                                                                sumaLokalu += sumaKwot;

                                                                podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, grupa.nazwa, sumaIlości.ToString("N"), sumaKwot.ToString("N") });
                                                            }

                                                            tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodBudynku.ToString(), numerLokalu.ToString(), lokal.nazwisko, lokal.imie, String.Format("{0} {1}", lokal.adres, lokal.adres_2), String.Empty, String.Empty, sumaLokalu.ToString("N") });
                                                            tabela.AddRange(podTabela);
                                                            tabela.Add(PustaLinia(liczbaKolumn));

                                                            liczbaPorządkowa++;
                                                            sumaBudynku += sumaLokalu;
                                                        }
                                                    }

                                                    sumaOgólna += sumaBudynku;

                                                    tabela.Add(new string[] { String.Empty, kodBudynku.ToString(), String.Empty, "RAZEM", "BUDYNEK", String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Empty);
                                                }
                                            }

                                            List<string[]> ostatniaTabela = tabele.Last();

                                            ostatniaTabela.Add(PustaLinia(liczbaKolumn));
                                            ostatniaTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM", "ZAKRES", String.Format("LOKALI {0}-{1} - {2}-{3}", kodPierwszegoBudynku, numerPierwszegoLokalu, kodOstatniegoBudynku, numerOstatniegoLokalu), String.Empty, String.Empty, sumaOgólna.ToString("N") });
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgGrupSumyBudynki:
                                    case Enumeratory.Raport.ObrotyWgGrupSumyBudynki:
                                    case Enumeratory.Raport.OgolemWgGrupSumyBudynki:
                                        {
                                            nagłówki = new List<string>() { "Liczba porządkowa", "Kod budynku", "Adres", "Grupa", "Ilość", "Kwota" };
                                            int kodPierwszego = identyfikatory[0];
                                            int kodOstatniego = identyfikatory[2];
                                            List<string[]> tabela = new List<string[]>();
                                            decimal liczbaPorządkowa = 1;

                                            for (int kodBudynku = kodPierwszego; kodBudynku <= kodOstatniego; kodBudynku++)
                                            {
                                                DostępDoBazy.Budynek budynek;

                                                if (magazyn.KodNaBudynek.TryGetValue(kodBudynku, out budynek))
                                                {
                                                    List<string[]> podTabela = new List<string[]>();
                                                    decimal sumaBudynku = 0;

                                                    for (int j = 0; j < numerGrupyNaPozycje.Count; j++)
                                                    {
                                                        KeyValuePair<int, List<DostępDoBazy.PozycjaDoAnalizy>> para = numerGrupyNaPozycje.ElementAt(j);
                                                        IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeBudynku = para.Value.Where(n => n.KodBudynku == kodBudynku);
                                                        DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupy[j];
                                                        decimal sumaKwot = 0;
                                                        float sumaIlości = 0;

                                                        foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeBudynku)
                                                        {
                                                            sumaIlości += pozycja.Ilość;
                                                            sumaKwot += pozycja.Kwota;
                                                        }

                                                        sumaBudynku += sumaKwot;

                                                        podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, grupa.nazwa, sumaIlości.ToString("N"), sumaKwot.ToString("N") });
                                                    }

                                                    tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodBudynku.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                    tabela.AddRange(podTabela);
                                                    tabela.Add(PustaLinia(nagłówki.Count));
                                                }
                                            }

                                            tabele.Add(tabela);
                                            podpisy.Add(String.Empty);
                                        }

                                        break;

                                    case Enumeratory.Raport.NaleznosciWgGrupSumyWspolnoty:
                                    case Enumeratory.Raport.ObrotyWgGrupSumyWspolnoty:
                                    case Enumeratory.Raport.OgolemWgGrupSumyWspolnoty:
                                        {
                                            nagłówki = new List<string>() { "Liczba porządkowa", "Kod budynku", "Adres", "Grupa", "Ilość", "Kwota" };
                                            int kodPierwszej = identyfikatory[4];
                                            int kodOstatniej = identyfikatory[5];
                                            decimal liczbaPorządkowa = 1;

                                            for (int kodWspólnoty = kodPierwszej; kodWspólnoty <= kodOstatniej; kodWspólnoty++)
                                            {
                                                DostępDoBazy.Wspólnota wspólnota;

                                                if (magazyn.KodNaWspólnotę.TryGetValue(kodWspólnoty, out wspólnota))
                                                {
                                                    List<DostępDoBazy.BudynekWspólnoty> budynkiWspólnoty = db.BudynkiWspólnot.Where(b => b.kod == kodWspólnoty).ToList();
                                                    List<string[]> tabela = new List<string[]>();
                                                    decimal sumaWspólnoty = 0;

                                                    foreach (DostępDoBazy.BudynekWspólnoty budynekWspólnoty in budynkiWspólnoty)
                                                    {
                                                        DostępDoBazy.Budynek budynek = magazyn.KodNaBudynek[budynekWspólnoty.kod_1];
                                                        List<string[]> podTabela = new List<string[]>();
                                                        decimal sumaBudynku = 0;

                                                        for (int j = 0; j < numerGrupyNaPozycje.Count; j++)
                                                        {
                                                            KeyValuePair<int, List<DostępDoBazy.PozycjaDoAnalizy>> para = numerGrupyNaPozycje.ElementAt(j);
                                                            IEnumerable<DostępDoBazy.PozycjaDoAnalizy> pozycjeBudynku = para.Value.Where(n => n.KodBudynku == kodWspólnoty);
                                                            DostępDoBazy.GrupaSkładnikówCzynszu grupa = wybraneGrupy[j];
                                                            decimal sumaKwot = 0;
                                                            float sumaIlości = 0;

                                                            foreach (DostępDoBazy.PozycjaDoAnalizy pozycja in pozycjeBudynku)
                                                            {
                                                                sumaIlości += pozycja.Ilość;
                                                                sumaKwot += pozycja.Kwota;
                                                            }

                                                            sumaBudynku += sumaKwot;

                                                            podTabela.Add(new string[] { String.Empty, String.Empty, String.Empty, grupa.nazwa, sumaIlości.ToString("N"), sumaKwot.ToString("N") });
                                                        }

                                                        tabela.Add(new string[] { liczbaPorządkowa.ToString(), kodWspólnoty.ToString(), String.Format("{0} {1}", budynek.adres, budynek.adres_2), String.Empty, String.Empty, sumaBudynku.ToString("N") });
                                                        tabela.AddRange(podTabela);
                                                        tabela.Add(PustaLinia(nagłówki.Count));

                                                        sumaWspólnoty += sumaBudynku;
                                                        liczbaPorządkowa++;
                                                    }

                                                    tabela.Add(new string[] { String.Empty, String.Empty, String.Empty, "RAZEM WSPÓLNOTA", String.Empty, sumaWspólnoty.ToString("N") });
                                                    tabele.Add(tabela);
                                                    podpisy.Add(String.Format("{0} {1} {2}", wspólnota.nazwa_pel, wspólnota.adres, wspólnota.adres_2));
                                                }
                                            }
                                        }

                                        break;
                                }
                            }

                            break;
                    }
                }

                WartościSesji.NagłówkiRaportu = nagłówki;
                WartościSesji.TabeleRaportu = tabele;
                WartościSesji.PodpisyRaportu = podpisy;
                WartościSesji.FormatRaportu = ((RadioButtonList)placeOfConfigurationFields.FindControl("format")).SelectedValue;
                WartościSesji.TytułRaportu = tytuł;
                WartościSesji.GotowaDefinicjaHtmlRaportu = gotowaDefinicjaHtml;

                Response.Redirect("Raport.aspx");
            }
        }

        static void WypełnijTagXml(XmlNode rodzic, string id, object wartość)
        {
            if (wartość != null)
                rodzic.SelectSingleNode(XPathZnajdźElementPoId(id)).InnerText = wartość.ToString();
        }

        static string XPathZnajdźElementPoId(string id)
        {
            return String.Format("//*[@id='{0}']", id);
        }

        static string[] PustaLinia(int ilośćKolumn)
        {
            string[] linia = new string[ilośćKolumn];

            linia[0] = ".";

            for (int i = 1; i < ilośćKolumn; i++)
                linia[i] = String.Empty;

            return linia;
        }
    }
}