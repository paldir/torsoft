using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class RentComponentsOfPlace : Page
    {
        List<DataAccess.RentComponentOfPlace> rentComponentsOfPlace
        {
            get { return (List<DataAccess.RentComponentOfPlace>)Session["rentComponentsOfPlace"]; }
            set { Session["rentComponentsOfPlace"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //int kod_lok = GetParamValue<int>("kod_lok");
            //int nr_lok = GetParamValue<int>("nr_lok");
            List<string[]> rows = new List<string[]>();

            for (int i = 0; i < rentComponentsOfPlace.Count; i++)
            {
                string index = (i + 1).ToString();

                rows.Add(new string[] { index, index }.Concat(rentComponentsOfPlace.ElementAt(i).ImportantFields()).ToArray());
            }

            placeOfTable.Controls.Add(new MyControls.Table("mainTable tabTable", rows, new string[] { "Lp.", "Nr", "Nazwa", "Stawka", "Ilość", "Wartość" }, false, String.Empty, new List<int>() { 1, 2, 4, 5, 6 }, new List<int>() { 6 }));
        }
    }
}