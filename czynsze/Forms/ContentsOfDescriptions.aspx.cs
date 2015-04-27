using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class ContentsOfDescriptions : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            const string format = "{0}_{1}";
            Enums.ContentsOfDescriptions mode = GetParamValue<Enums.ContentsOfDescriptions>("which");
            Hello.SiteMapPath = new List<string>() { "Administracja", "Treści opisów" };
            int numberOfFields;
            int fieldLength;
            string prefix;
            string heading;

            switch (mode)
            {
                case Enums.ContentsOfDescriptions.Payments:
                    numberOfFields = 15;
                    fieldLength = 40;
                    prefix = "op";
                    heading = "Opłaty";

                    break;

                case Enums.ContentsOfDescriptions.Book:
                    numberOfFields = 10;
                    fieldLength = 40;
                    prefix = "pu";
                    heading = "Książeczka";

                    break;

                case Enums.ContentsOfDescriptions.Water:
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

            Hello.SiteMapPath.Add(heading);

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                DostępDoBazy.Treść contents = db.Treści.FirstOrDefault();
                string[] fields = new string[numberOfFields];

                if (String.IsNullOrEmpty(GetParamValue<string>("Save")))
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

                    placeOfFields.Controls.Add(new MyControls.HtmlInputHidden("which", mode.ToString()));

                    for (int i = 0; i < fields.Length; i++)
                    {
                        placeOfFields.Controls.Add(new MyControls.TextBox("field", String.Format(format, prefix, i + 1), fields[i], MyControls.TextBox.TextBoxMode.SingleLine, fieldLength, 1, true));
                        AddNewLine(placeOfFields);
                    }

                    placeOfButtons.Controls.Add(new MyControls.Button("button", "Save", "Zapisz", String.Empty));
                    placeOfButtons.Controls.Add(new MyControls.Button("button", "Anuluj", "Anuluj", "Hello.aspx"));
                }
                else
                {
                    for (int i = 0; i < numberOfFields; i++)
                    {
                        string property = String.Format(format, prefix, i + 1);

                        contents.GetType().GetProperty(property).SetValue(contents, GetParamValue<string>(property));
                    }

                    db.SaveChanges();
                    Response.Redirect("Hello.aspx");
                }
            }
        }
    }
}