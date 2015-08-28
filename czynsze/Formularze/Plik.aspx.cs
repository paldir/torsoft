using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class Plik : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = PobierzWartośćParametru<int>("id");
            DostępDoBazy.Plik plik = WartościSesji.Pliki.Single(p => p.__record == id);
            byte[] tablicaBajtów = Convert.FromBase64String(plik.plik);

            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Type", "application/pdf");
            Response.AddHeader("Content-Length", tablicaBajtów.Length.ToString());
            Response.AddHeader("Content-Disposition", "inline; filename=plik.pdf");
            Response.BinaryWrite(tablicaBajtów);
            Response.Flush();
            Response.End();
        }
    }
}