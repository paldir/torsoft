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
                    Button allButton = new MyControls.Button("button", "allGeneration", "Generacja całego zestawienia", String.Empty);
                    Button fromToButton = new MyControls.Button("button", "fromToGeneration", "Generacja od-do żądanego lokalu", String.Empty);
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
                    form.Controls.Add(new MyControls.Button("button", "noRepeat", "Ntie", String.Empty));
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

                foreach (DataAccess.ActivePlace place in db.places.ToList())
                    //using (DataAccess.Czynsze_Entities subDb = new DataAccess.Czynsze_Entities())
                        foreach (DataAccess.RentComponentOfPlace rentComponentOfPlace in db.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok).ToList())
                        {
                            DataAccess.RentComponent rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == rentComponentOfPlace.nr_skl);
                            float ilosc;
                            float stawka = rentComponent.stawka;

                            switch (rentComponent.s_zaplat)
                            {
                                case 1:
                                    ilosc = place.pow_uzyt;

                                    break;

                                case 2:
                                    ilosc = rentComponentOfPlace.dan_p;

                                    break;

                                case 3:
                                    ilosc = (float)place.il_osob;

                                    break;

                                case 4:
                                    ilosc = 1;

                                    break;

                                case 5:
                                    ilosc = daysInMonth;

                                    break;

                                case 6:
                                    ilosc = 1;

                                    switch (place.il_osob)
                                    {
                                        case 0:
                                            stawka = rentComponent.stawka_00;

                                            break;

                                        case 1:
                                            stawka = rentComponent.stawka_01;

                                            break;

                                        case 2:
                                            stawka = rentComponent.stawka_02;

                                            break;

                                        case 3:
                                            stawka = rentComponent.stawka_03;

                                            break;

                                        case 4:
                                            stawka = rentComponent.stawka_04;

                                            break;

                                        case 5:
                                            stawka = rentComponent.stawka_05;

                                            break;

                                        case 6:
                                            stawka = rentComponent.stawka_06;

                                            break;

                                        case 7:
                                            stawka = rentComponent.stawka_07;

                                            break;

                                        case 8:
                                            stawka = rentComponent.stawka_08;

                                            break;

                                        default:
                                            stawka = rentComponent.stawka_09;

                                            break;
                                    }

                                    break;
                            }


                        }
            }
        }
    }
}