using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class ContentsOfDescriptions : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                const string format = "{0}_{1}";
                Enumeratory.TreściOpisów mode = PobierzWartośćParametru<Enumeratory.TreściOpisów>("which");
                Start.ŚcieżkaStrony = new List<string>() { "Administracja", "Treści opisów" };
                int numberOfFields;
                int fieldLength;
                string prefix;
                string heading;

                switch (mode)
                {
                    case Enumeratory.TreściOpisów.Oplaty:
                        numberOfFields = 15;
                        fieldLength = 40;
                        prefix = "op";
                        heading = "Opłaty";

                        break;

                    case Enumeratory.TreściOpisów.Ksiazka:
                        numberOfFields = 10;
                        fieldLength = 40;
                        prefix = "pu";
                        heading = "Książeczka";

                        break;

                    case Enumeratory.TreściOpisów.Woda:
                        numberOfFields = 10;
                        fieldLength = 76;
                        prefix = "pw";
                        heading = "Woda";

                        break;

                    default:
                        numberOfFields = 0;
                        fieldLength = 0;
                        prefix = String.Empty;
                        heading = String.Empty;

                        break;
                }

                Start.ŚcieżkaStrony.Add(heading);

                {
                    DostępDoBazy.Treść contents = db.Treści.FirstOrDefault();
                    string[] fields = new string[numberOfFields];

                    if (String.IsNullOrEmpty(PobierzWartośćParametru<string>("Save")))
                    {
                        if (contents == null)
                        {
                            contents = new DostępDoBazy.Treść();

                            db.Treści.Add(contents);
                            db.SaveChanges();
                        }
                        else
                            for (int i = 0; i < numberOfFields; i++)
                            {
                                object value = contents.GetType().GetProperty(String.Format(format, prefix, i + 1)).GetValue(contents);

                                if (value != null)
                                    fields[i] = value.ToString().Trim();
                            }

                        placeOfFields.Controls.Add(new Kontrolki.HtmlInputHidden("which", mode.ToString()));

                        for (int i = 0; i < fields.Length; i++)
                        {
                            placeOfFields.Controls.Add(new Kontrolki.TextBox("field", String.Format(format, prefix, i + 1), fields[i], Kontrolki.TextBox.TextBoxMode.PojedynczaLinia, fieldLength, 1, true));
                            DodajNowąLinię(placeOfFields);
                        }

                        placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Save", "Zapisz", String.Empty));
                        placeOfButtons.Controls.Add(new Kontrolki.Button("button", "Anuluj", "Anuluj", "Start.aspx"));
                    }
                    else
                    {
                        for (int i = 0; i < numberOfFields; i++)
                        {
                            string property = String.Format(format, prefix, i + 1);

                            contents.GetType().GetProperty(property).SetValue(contents, PobierzWartośćParametru<string>(property));
                        }

                        db.SaveChanges();
                        Response.Redirect("Start.aspx");
                    }
                }
            }
        }
    }
}