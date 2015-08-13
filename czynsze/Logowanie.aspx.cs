using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze
{
    public partial class Logowanie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            //typeof(HttpRuntime).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).First(m => m.Name.Contains("ShutdownAppDomain")).Invoke(null, new object[] { String.Empty });
            //HttpRuntime.UnloadAppDomain();

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                Formularze.Start.NazwaFirmy = companyName.InnerText = db.Konfiguracje.FirstOrDefault().nazwa_1;
                Formularze.Start.LiczbaZbiorów = db.Konfiguracje.FirstOrDefault().p_32;
                Formularze.Start.NazwyZbiorów = new string[] { "CZYNSZE", db.Konfiguracje.FirstOrDefault().nazwa_2zb, db.Konfiguracje.FirstOrDefault().nazwa_3zb };
            }

            DostępDoBazy.CzynszeKontekst.ZmieńRok(14);

            /*System.Data.Entity.DbModelBuilder _budowniczy = new System.Data.Entity.DbModelBuilder();

            _budowniczy.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DostępDoBazy.Budynek>());

            _budowniczy.Entity<DostępDoBazy.Należność1>().ToTable("nal_14__", "public");

            using (DostępDoBazy.CzynszeKontekst db1 = new DostępDoBazy.CzynszeKontekst())
            using (DostępDoBazy.CzynszeKontekst db2 = new DostępDoBazy.CzynszeKontekst(_budowniczy.Build(db1.Database.Connection).Compile()))
            {
                var tmp = db2.Należności1.First().data_nal;
                var t = db2.Budynki.First();
            }*/

            Formularze.Start.Data = DateTime.Today;
            Formularze.Start.ŚcieżkaStrony = new ŚcieżkaStrony();
            Formularze.Start.AktywnyZbiór = Enumeratory.Zbiór.Czynsze;

            Response.Redirect("Formularze/WalidacjaUzytkownika.aspx?uzytkownik=TORSOFT TORSOFT&haslo=JK");
        }
    }
}