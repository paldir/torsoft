using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using System.Reflection.Emit;

namespace czynsze.Formularze
{
    public partial class Start : System.Web.UI.Page
    {
        public static string NazwaFirmy { get; set; }
        public static DateTime Data { get; set; }
        public static ŚcieżkaStrony ŚcieżkaStrony { get; set; }
        public static int LiczbaZbiorów { get; set; }
        public static Enumeratory.Zbiór AktywnyZbiór { get; set; }
        public static string[] NazwyZbiorów { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            company.InnerText = NazwaFirmy;
            user.InnerText = Sesja.Obecna.AktualnieZalogowanyUżytkownik;
            month.InnerText = String.Format("{0} {1}", DostępDoBazy.CzynszeKontekst.NumerMiesiącaNaNazwęZPolskimiZnakami[Data.Month], Data.Year);

            /*AssemblyBuilder budowniczyBiblioteki = AppDomain.CurrentDomain.DefineDynamicAssembly(new System.Reflection.AssemblyName("ass"), AssemblyBuilderAccess.Run);
            ModuleBuilder budowniczyModułu = budowniczyBiblioteki.DefineDynamicModule("tmp");
            TypeBuilder budowniczyTypu = budowniczyModułu.DefineType("tmpclass");
            CustomAttributeBuilder budowniczyAtrybutu = new CustomAttributeBuilder(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { "fdsfs" });

            budowniczyTypu.SetParent(typeof(DostępDoBazy.Należność));
            budowniczyTypu.SetCustomAttribute(budowniczyAtrybutu);

            Type typ = budowniczyTypu.CreateType();*/
        }

        public static string ExceptionMessage(Exception wyjątek)
        {
            if (wyjątek == null)
                return String.Empty;
            else
                return String.Format("{0}<br />{1}", wyjątek.Message, ExceptionMessage(wyjątek.InnerException));
        }
    }
}