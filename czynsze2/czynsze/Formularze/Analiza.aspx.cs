﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class Analiza : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Enumeratory.Analiza rodzaj = PobierzWartośćParametru<Enumeratory.Analiza>("rodzaj");
                //Enumeratory.KwotaCzynszu tryb = PobierzWartośćParametru<Enumeratory.KwotaCzynszu>("tryb");
                string zakres = Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith("Wybór"));
                Start.ŚcieżkaStrony = new czynsze.ŚcieżkaStrony("Raporty");
                string ogólnyRodzaj = null;
                string konkretnyRodzaj = null;
                string rodzajNapis = rodzaj.ToString();

                if (rodzajNapis.StartsWith("Naleznosci"))
                    ogólnyRodzaj = "Analizy należności";
                else if (rodzajNapis.StartsWith("Obroty"))
                    ogólnyRodzaj = "Analizy obrotów";
                else if (rodzajNapis.StartsWith("Ogolem"))
                    ogólnyRodzaj = "Analizy ogółem";

                switch (rodzaj)
                {
                    case Enumeratory.Analiza.NaleznosciBiezace:
                        konkretnyRodzaj = "Bieżące";

                        break;

                    case Enumeratory.Analiza.NaleznosciZaDanyMiesiac:
                    case Enumeratory.Analiza.ObrotyZaDanyMiesiac:
                    case Enumeratory.Analiza.OgolemZaDanyMiesiac:
                        konkretnyRodzaj = "Za dany miesiąc";

                        break;

                    case Enumeratory.Analiza.NaleznosciSzczegolowoMiesiac:
                    case Enumeratory.Analiza.ObrotySzczegolowoMiesiac:
                    case Enumeratory.Analiza.OgolemSzczegolowoMiesiac:
                        konkretnyRodzaj = "Szczegółowo miesiąc";

                        break;

                    case Enumeratory.Analiza.NaleznosciWgEwidencji:
                    case Enumeratory.Analiza.ObrotyWgEwidencji:
                    case Enumeratory.Analiza.OgolemWgEwidencji:
                        konkretnyRodzaj = "Wg ewidencji";

                        break;

                    case Enumeratory.Analiza.NaleznosciWgGrupSkladniki:
                    case Enumeratory.Analiza.NaleznosciWgGrupSumy:
                    case Enumeratory.Analiza.ObrotyWgGrupSkladniki:
                    case Enumeratory.Analiza.ObrotyWgGrupSumy:
                    case Enumeratory.Analiza.OgolemWgGrupSkladniki:
                    case Enumeratory.Analiza.OgolemWgGrupSumy:
                        List<string> id = new List<string>();
                        List<string> nazwy = new List<string>();

                        placeOfOthers.Controls.Add(new Kontrolki.Label("kontrolka", "grupyCzynszu", "Grupy składników czynszu: ", String.Empty));

                        foreach (DostępDoBazy.GrupaSkładnikówCzynszu grupa in db.GrupySkładnikówCzynszu.OrderBy(g => g.kod))
                        {
                            id.Add(grupa.kod.ToString());
                            nazwy.Add(grupa.nazwa);
                        }

                        placeOfOthers.Controls.Add(new Kontrolki.CheckBoxList("kontrolka", "grupyCzynszu", nazwy, id, true, id));

                        switch (rodzaj)
                        {
                            case Enumeratory.Analiza.NaleznosciWgGrupSkladniki:
                            case Enumeratory.Analiza.ObrotyWgGrupSkladniki:
                            case Enumeratory.Analiza.OgolemWgGrupSkladniki:
                                konkretnyRodzaj = "Wg grup - składniki";

                                break;

                            case Enumeratory.Analiza.NaleznosciWgGrupSumy:
                            case Enumeratory.Analiza.ObrotyWgGrupSumy:
                            case Enumeratory.Analiza.OgolemWgGrupSumy:
                                konkretnyRodzaj = "Wg grup - sumy";

                                break;
                        }

                        break;
                }

                Start.ŚcieżkaStrony.Dodaj(ogólnyRodzaj);
                Start.ŚcieżkaStrony.Dodaj(konkretnyRodzaj, ŚcieżkaIQuery);

                Title = String.Format("{0} - {1}", ogólnyRodzaj, konkretnyRodzaj);

                IEnumerable<DostępDoBazy.AktywnyLokal> wszystkieLokale = db.AktywneLokale.OrderBy(l => l.kod_lok).ThenBy(l => l.nr_lok);
                int minimalnyBudynek;
                int minimalnyLokal;
                int maksymalnyBudynek;
                int maksymalnyLokal;
                int minimalnaWspólnota;
                int maksymalnaWspólnota;

                if (String.IsNullOrEmpty(zakres))
                {
                    form.Controls.Add(new Kontrolki.HtmlInputHidden("rodzaj", rodzaj.ToString()));

                    DodajWybórLokaliBudynkówIWspólnot(placeOfPlaces, placeOfBuildings, placeOfCommunities, out minimalnyBudynek, out minimalnyLokal, out maksymalnyBudynek, out maksymalnyLokal, out minimalnaWspólnota, out maksymalnaWspólnota);
                }
                else
                {
                    zakres = zakres.Substring(zakres.LastIndexOf('$') + 1).Replace("Wybór", String.Empty);
                    int kod_1_1, kod_1_2, nr1, nr2, kod1, kod2;
                    nr1 = nr2 = kod1 = kod2 = 0;
                    Enumeratory.Raport raport = (Enumeratory.Raport)(-1);
                    Enumeratory.ObiektAnalizy obiektAnalizy = (Enumeratory.ObiektAnalizy)(-1);
                    kod_1_1 = PobierzWartośćParametru<int>("minimalnyBudynek");
                    kod_1_2 = PobierzWartośćParametru<int>("maksymalnyBudynek");

                    switch (zakres)
                    {
                        case "wszystkieLokale":
                            nr1 = PobierzWartośćParametru<int>("minimalnyLokal");
                            nr2 = PobierzWartośćParametru<int>("maksymalnyLokal");
                            obiektAnalizy = Enumeratory.ObiektAnalizy.Lokale;

                            break;

                        case "odDoLokalu":
                            string[] odLokalu = PobierzWartośćParametru("odLokalu").Split('-');
                            string[] doLokalu = PobierzWartośćParametru("doLokalu").Split('-');
                            kod_1_1 = Int32.Parse(odLokalu[0]);
                            nr1 = Int32.Parse(odLokalu[1]);
                            kod_1_2 = Int32.Parse(doLokalu[0]);
                            nr2 = Int32.Parse(doLokalu[1]);
                            obiektAnalizy = Enumeratory.ObiektAnalizy.Lokale;

                            break;

                        case "wszystkieBudynki":
                            obiektAnalizy = Enumeratory.ObiektAnalizy.Budynki;

                            break;

                        case "odDoBudynku":
                            kod_1_1 = PobierzWartośćParametru<int>("odBudynku");
                            kod_1_2 = PobierzWartośćParametru<int>("doBudynku");
                            obiektAnalizy = Enumeratory.ObiektAnalizy.Budynki;

                            break;

                        case "wszystkieWspólnoty":
                            kod1 = PobierzWartośćParametru<int>("minimalnaWspólnota");
                            kod2 = PobierzWartośćParametru<int>("maksymalnaWspólnota");
                            obiektAnalizy = Enumeratory.ObiektAnalizy.Wspolnoty;

                            break;

                        case "odDoWspólnoty":
                            kod1 = PobierzWartośćParametru<int>("odWspólnoty");
                            kod2 = PobierzWartośćParametru<int>("doWspólnoty");
                            obiektAnalizy = Enumeratory.ObiektAnalizy.Wspolnoty;

                            break;
                    }

                    WartościSesji.TrybAnalizy = rodzaj;

                    switch (rodzaj)
                    {
                        case Enumeratory.Analiza.NaleznosciBiezace:
                            rodzaj = Enumeratory.Analiza.NaleznosciZaDanyMiesiac;

                            break;

                        case Enumeratory.Analiza.NaleznosciWgGrupSkladniki:
                        case Enumeratory.Analiza.NaleznosciWgGrupSumy:
                        case Enumeratory.Analiza.ObrotyWgGrupSkladniki:
                        case Enumeratory.Analiza.ObrotyWgGrupSumy:
                        case Enumeratory.Analiza.OgolemWgGrupSkladniki:
                        case Enumeratory.Analiza.OgolemWgGrupSumy:
                            List<int> wybraneGrupy = PobierzWartośćParametru("grupyCzynszu").Split(',').Select(p => Int32.Parse(p)).ToList();
                            WartościSesji.NumeryGrupWybranychSkładnikówCzynszu = wybraneGrupy;

                            break;
                    }

                    raport = (Enumeratory.Raport)Enum.Parse(typeof(Enumeratory.Raport), String.Concat(rodzaj, obiektAnalizy));

                    Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?raport={0}&odBudynku={1}&odLokalu={2}&doBudynku={3}&doLokalu={4}&odWspólnoty={5}&doWspólnoty={6}", raport, kod_1_1, nr1, kod_1_2, nr2, kod1, kod2));
                }
            }
        }
    }
}