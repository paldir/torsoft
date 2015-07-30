using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class TresciOpisow : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                const string format = "{0}_{1}";
                Enumeratory.TreściOpisów tryb = PobierzWartośćParametru<Enumeratory.TreściOpisów>("which");
                Start.ŚcieżkaStrony = new czynsze.ŚcieżkaStrony("Administracja", "Treści opisów");
                int liczbaPól;
                int długośćPola;
                string prefiks;
                string nagłówek;

                switch (tryb)
                {
                    case Enumeratory.TreściOpisów.Oplaty:
                        liczbaPól = 15;
                        długośćPola = 40;
                        prefiks = "op";
                        nagłówek = "Opłaty";

                        break;

                    case Enumeratory.TreściOpisów.Ksiazka:
                        liczbaPól = 10;
                        długośćPola = 40;
                        prefiks = "pu";
                        nagłówek = "Książeczka";

                        break;

                    case Enumeratory.TreściOpisów.Woda:
                        liczbaPól = 10;
                        długośćPola = 76;
                        prefiks = "pw";
                        nagłówek = "Woda";

                        break;

                    default:
                        liczbaPól = 0;
                        długośćPola = 0;
                        prefiks = String.Empty;
                        nagłówek = String.Empty;

                        break;
                }

                Start.ŚcieżkaStrony.Dodaj(nagłówek);

                {
                    DostępDoBazy.Treść treści = db.Treści.FirstOrDefault();
                    string[] pola = new string[liczbaPól];

                    if (String.IsNullOrEmpty(PobierzWartośćParametru<string>("Save")))
                    {
                        if (treści == null)
                        {
                            treści = new DostępDoBazy.Treść();

                            db.Treści.Add(treści);
                            db.SaveChanges();
                        }
                        else
                            for (int i = 0; i < liczbaPól; i++)
                            {
                                object wartość = treści.GetType().GetProperty(String.Format(format, prefiks, i + 1)).GetValue(treści);

                                if (wartość != null)
                                    pola[i] = wartość.ToString().Trim();
                            }

                        placeOfFields.Controls.Add(new Kontrolki.HtmlInputHidden("which", tryb.ToString()));

                        for (int i = 0; i < pola.Length; i++)
                        {
                            placeOfFields.Controls.Add(new Kontrolki.TextBox("field", String.Format(format, prefiks, i + 1), Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, długośćPola, 1, true, pola[i]));
                            DodajNowąLinię(placeOfFields);
                        }

                        placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Save", "Zapisz", String.Empty));
                        placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Anuluj", "Anuluj", "Start.aspx"));
                    }
                    else
                    {
                        for (int i = 0; i < liczbaPól; i++)
                        {
                            string właściwość = String.Format(format, prefiks, i + 1);

                            treści.GetType().GetProperty(właściwość).SetValue(treści, PobierzWartośćParametru<string>(właściwość));
                        }

                        db.SaveChanges();
                        Response.Redirect("Start.aspx");
                    }
                }
            }
        }
    }
}