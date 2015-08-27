using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class WykazWgSkladnika : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Start.ŚcieżkaStrony = new czynsze.ŚcieżkaStrony("Raporty", "Wykaz wg składnika");
                string przycisk = PobierzWartośćParametru<string>("przycisk");
                Enumeratory.WykazWedługSkładnika tryb = PobierzWartośćParametru<Enumeratory.WykazWedługSkładnika>("tryb");
                string trybTekstowo = null;

                switch (tryb)
                {
                    case Enumeratory.WykazWedługSkładnika.Obecny:
                        trybTekstowo = "Obecny";

                        break;

                    case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                        trybTekstowo = "Historia - ogółem";

                        break;

                    case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                        trybTekstowo = "Historia - specyfikacja";

                        break;
                }

                Start.ŚcieżkaStrony.Dodaj(trybTekstowo, ŚcieżkaIQuery);

                Title += String.Format(" ({0})", trybTekstowo);

                {
                    if (String.IsNullOrEmpty(przycisk))
                    {
                        int minimalnyBudynek;
                        int minimalnyLokal;
                        int maksymalnyBudynek;
                        int maksymalnyLokal;
                        List<DostępDoBazy.SkładnikCzynszu> składnikiCzynszu = db.SkładnikiCzynszu.OrderBy(s => s.nr_skl).ToList();

                        pojemnikSkladnika.Controls.Add(new Kontrolki.Label("label", "składnik", "Składnik: ", String.Empty));
                        pojemnikSkladnika.Controls.Add(new Kontrolki.DropDownList("field", "składnik", składnikiCzynszu.Select(s => s.WażnePolaDoRozwijanejListy()).ToList(), true, false, składnikiCzynszu.First().nr_skl.ToString()));
                        DodajNowąLinię(pojemnikReszty);

                        switch (tryb)
                        {
                            case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                            case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:

                                pojemnikReszty.Controls.Add(new LiteralControl("Podaj początek okresu rozliczeniowego: "));
                                DodajNowąLinię(pojemnikReszty);
                                pojemnikReszty.Controls.Add(new Kontrolki.Label("label", "rok", "Rok: ", String.Empty));
                                pojemnikReszty.Controls.Add(new Kontrolki.TextBox("field", "rok", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, true));
                                pojemnikReszty.Controls.Add(new Kontrolki.Label("field", "miesiąc", " Miesiąc: ", String.Empty));
                                pojemnikReszty.Controls.Add(new Kontrolki.TextBox("field", "miesiąc", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, true));
                                DodajNowąLinię(pojemnikReszty);
                                DodajNowąLinię(pojemnikReszty);

                                break;
                        }

                        DodajWybórLokali(pojemnikReszty, out minimalnyBudynek, out minimalnyLokal, out maksymalnyBudynek, out maksymalnyLokal);
                        DodajNowąLinię(pojemnikReszty);
                        pojemnikReszty.Controls.Add(new Kontrolki.Button("button", "przycisk", "Wybierz", String.Empty));
                    }
                    else
                    {
                        string[] pierwszyLokal = PobierzWartośćParametru<string>("odLokalu").Split('-');
                        string[] ostatniLokal = PobierzWartośćParametru<string>("doLokalu").Split('-');
                        int nrSkładnika = PobierzWartośćParametru<int>("składnik");
                        WartościSesji.TrybWykazuWgSkładnika = tryb;

                        switch (tryb)
                        {
                            case Enumeratory.WykazWedługSkładnika.HistoriaOgolem:
                            case Enumeratory.WykazWedługSkładnika.HistoriaSpecyfikacja:
                                DateTime data;

                                try
                                {
                                    data = new DateTime(PobierzWartośćParametru<int>("rok"), PobierzWartośćParametru<int>("miesiąc"), 1);
                                }
                                catch
                                {
                                    DateTime dziś = Start.Data;
                                    data = new DateTime(dziś.Year, dziś.Month, 1);
                                }

                                WartościSesji.DataWykazuWgSkładnika = data;

                                break;
                        }

                        Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?raport={0}&odBudynku={1}&odLokalu={2}&doBudynku={3}&doLokalu={4}&nrSkladnika={5}", Enumeratory.Raport.WykazWgSkladnika, pierwszyLokal[0], pierwszyLokal[1], ostatniLokal[0], ostatniLokal[1], nrSkładnika));
                    }
                }
            }
        }
    }
}