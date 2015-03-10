using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class GenerationOfReceivables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                if (db.completed.ToList().Exists(c => c.rok == Hello.Date.Year && c.miesiac == Hello.Date.Month && c.z_rok && c.z_mies))
                    form.Controls.Add(new LiteralControl("Miesiąc został już zamknięty."));
                else
                {
                    Button allButton = new MyControls.Button("button", "allGeneration", "Generuj dla wszystkich lokali", String.Empty);
                    Button fromToButton = new MyControls.Button("button", "fromToGeneration", "Generuj od do", String.Empty);
                    allButton.Click += allButton_Click;

                    form.Controls.Add(allButton);
                    form.Controls.Add(new LiteralControl("<br />"));
                    form.Controls.Add(fromToButton);
                }
        }

        void allButton_Click(object sender, EventArgs e)
        {
            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                DateTime start = new DateTime(Hello.Date.Year, Hello.Date.Month, 1);
                DateTime end = new DateTime(Hello.Date.Year, Hello.Date.Month, DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month));
                System.Linq.Expressions.Expression<Func<DataAccess.Receivable, bool>> predicate = c => c.data_nal >= start && c.data_nal <= end;

                if (db.receivablesFrom1stSet.Count(predicate) > 0 || db.receivablesFrom2ndSet.Count(predicate) > 0 || db.receivablesFrom3rdSet.Count(predicate) > 0)
                {
                    MyControls.Button generate = new MyControls.Button("button", "yesRepeat", "Tak", String.Empty);
                    generate.Click += generate_Click;

                    form.Controls.Clear();
                    form.Controls.Add(new LiteralControl("Generacja była już wykonana. Czy chcesz powtórzyć?<br />"));
                    form.Controls.Add(generate);
                    form.Controls.Add(new MyControls.Button("button", "noRepeat", "Nie", String.Empty));
                }
                else
                    generate_Click(null, null);
            }
        }

        void generate_Click(object sender, EventArgs e)
        {
            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                int p_46 = db.configurations.FirstOrDefault().p_46;
                float daysInMonth = DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month);
                float rentFactor;

                if (p_46 >= 1)
                    if (p_46 == 31)
                        rentFactor = 1;
                    else
                        rentFactor = p_46 / daysInMonth;
                else
                    rentFactor = 15 / daysInMonth;

                foreach (DataAccess.ActivePlace place in db.places)
                    using (DataAccess.Czynsze_Entities subDb = new DataAccess.Czynsze_Entities())
                        foreach (DataAccess.RentComponentOfPlace rentComponentOfPlace in subDb.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok))
                        {
                            DataAccess.RentComponent rentComponent;

                            using (DataAccess.Czynsze_Entities subSubDb = new DataAccess.Czynsze_Entities())
                                rentComponent = subSubDb.rentComponents.FirstOrDefault(c => c.nr_skl == rentComponentOfPlace.nr_skl);

                            switch (rentComponent.s_zaplat)
                            {
                                case 1:

                                    break;

                                case 2:

                                    break;

                                case 3:

                                    break;

                                case 4:

                                    break;

                                case 5:

                                    break;

                                case 6:

                                    break;
                            }
                        }
            }
        }
    }
}