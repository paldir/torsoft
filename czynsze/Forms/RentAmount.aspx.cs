using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class RentAmount : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Enums.RentAmount rentAmount = GetParamValue<Enums.RentAmount>("mode");

            placeOfContent.Controls.Add(new MyControls.Button("button", "allRentAmount", "Wszystkie lokale", String.Empty));
            //placeOfContent.Controls.Add(new MyControls.Button("field", ))
        }
    }
}