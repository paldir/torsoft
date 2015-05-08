using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;

namespace czynsze.Formularze
{
    public partial class SkladnikiCzynszu : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string stawka = PobierzWartośćParametru<string>("stawka");

            form.Controls.Add(new Kontrolki.Label("label", "stawka", "Wybór stawki: ", String.Empty));

            form.Controls.Add(new Kontrolki.RadioButtonList("kontrolka", "stawka", new List<string>() 
            {
                "Stawka", 
                "Stawka informacyjna" 
            }, new List<string>() 
            { 
                Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła.ToString(), 
                Enumeratory.Raport.SkładnikiCzynszuStawkaInformacyjna.ToString() 
            }, Enumeratory.Raport.SkładnikiCzynszuStawkaZwykła.ToString(), true, false));

            form.Controls.Add(new Kontrolki.Button("button", "przycisk", "Wybierz", String.Empty));

            if (!String.IsNullOrEmpty(stawka))
            {
                Enumeratory.Raport raport = PobierzWartośćParametru<Enumeratory.Raport>("stawka");

                Response.Redirect(String.Format("KonfiguracjaRaportu.aspx?{0}raport=dummy", raport));
            }

            form.Controls.Add(new Kontrolki.TextBox("klsdas", "id", "ffd'", Kontrolki.TextBox.TextBoxMode.LiczbaNiecałkowita, 34, 1, true));
        }
    }
}