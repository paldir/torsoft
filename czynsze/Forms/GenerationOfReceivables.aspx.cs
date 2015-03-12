using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class GenerationOfReceivables : Page
    {
        readonly Func<DataAccess.Receivable, bool> receivablesFromCurrentMonth = c => c.data_nal >= new DateTime(Hello.Date.Year, Hello.Date.Month, 1) && c.data_nal <= new DateTime(Hello.Date.Year, Hello.Date.Month, DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month));

        int year
        {
            get { return (int)ViewState["year"]; }
            set { ViewState["year"] = value; }
        }
        
        int month
        {
            get { return (int)ViewState["month"]; }
            set { ViewState["month"] = value; }
        }
        
        int day
        {
            get { return (int)ViewState["day"]; }
            set { ViewState["day"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int daysInMonth = DateTime.DaysInMonth(Hello.Date.Year, Hello.Date.Month);
            string generationMode = GetParamValue<string>("Generation");
            string repeatGeneration = GetParamValue<string>("Repeat");

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                if (String.IsNullOrEmpty(generationMode))
                    if (db.completed.ToList().Exists(c => c.rok == Hello.Date.Year && c.miesiac == Hello.Date.Month && c.z_rok && c.z_mies))
                        form.Controls.Add(new LiteralControl("Miesiąc został już zamknięty."));
                    else
                    {
                        day = db.configurations.FirstOrDefault().p_46;

                        if (day >= 1)
                        {
                            if (day == 31)
                                day = daysInMonth;
                        }
                        else
                            day = 15;

                        placeOfDate.Controls.Add(new LiteralControl("Podaj termin płatności: "));
                        placeOfDate.Controls.Add(new LiteralControl("<br />"));
                        placeOfDate.Controls.Add(new MyControls.TextBox("field", "year", Hello.Date.Year.ToString(), MyControls.TextBox.TextBoxMode.Number, 4, 1, true));
                        placeOfDate.Controls.Add(new LiteralControl("-"));
                        placeOfDate.Controls.Add(new MyControls.TextBox("field", "month", Hello.Date.Month.ToString("D2"), MyControls.TextBox.TextBoxMode.Number, 2, 1, true));
                        placeOfDate.Controls.Add(new LiteralControl("-"));
                        placeOfDate.Controls.Add(new MyControls.TextBox("field", "day", day.ToString("D2"), MyControls.TextBox.TextBoxMode.Number, 2, 1, true));
                        placeOfGeneration.Controls.Add(new LiteralControl("<br />"));
                        placeOfGeneration.Controls.Add(new MyControls.Button("button", "allGeneration", "Generacja całego zestawienia", String.Empty));
                        placeOfGeneration.Controls.Add(new LiteralControl("<br />"));
                        placeOfGeneration.Controls.Add(new MyControls.Button("button", "fromToGeneration", "Generacja od-do żądanego lokalu", String.Empty));
                    }
                else
                {
                    year = GetParamValue<int>("year");
                    month = GetParamValue<int>("month");
                    day = GetParamValue<int>("day");
                    
                    if (db.receivablesFrom1stSet.ToList().Count(receivablesFromCurrentMonth) > 0 || db.receivablesFrom2ndSet.Count(receivablesFromCurrentMonth) > 0 || db.receivablesFrom3rdSet.Count(receivablesFromCurrentMonth) > 0)
                    {
                        placeOfGeneration.Controls.Add(new LiteralControl("Generacja była już wykonana. Czy chcesz powtórzyć?<br />"));
                        placeOfGeneration.Controls.Add(new MyControls.Button("button", "yesRepeat", "Tak", String.Empty));
                        placeOfGeneration.Controls.Add(new MyControls.Button("button", "no", "Nie", String.Empty));
                    }
                    else
                        repeatGeneration = "Tak";
                }

                if (!String.IsNullOrEmpty(repeatGeneration))
                {
                    db.receivablesFrom1stSet.RemoveRange(db.receivablesFrom1stSet.Where(receivablesFromCurrentMonth).Cast<DataAccess.ReceivableFrom1stSet>());

                    foreach (DataAccess.ActivePlace place in new List<DataAccess.ActivePlace>() { db.places.FirstOrDefault(p => p.nr_kontr == 125) }/*db.places.ToList()*/)
                        foreach (DataAccess.RentComponentOfPlace rentComponentOfPlace in db.rentComponentsOfPlaces.Where(c => c.kod_lok == place.kod_lok && c.nr_lok == place.nr_lok).ToList())
                        {
                            DataAccess.RentComponent rentComponent = db.rentComponents.FirstOrDefault(c => c.nr_skl == rentComponentOfPlace.nr_skl);
                            float ilosc = 0;
                            float stawka = rentComponent.stawka;
                            DataAccess.ReceivableFrom1stSet receivableFrom1stSet = new DataAccess.ReceivableFrom1stSet();

                            try { new DateTime(year, month, 1); }
                            catch (ArgumentOutOfRangeException) { month = Hello.Date.Month; }

                            try { new DateTime(year, month, day); }
                            catch (ArgumentOutOfRangeException) { day = this.day; }

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

                            receivableFrom1stSet.Set(ilosc * stawka, new DateTime(year, month, day), rentComponent.nazwa.Trim() + " za m-c x", (int)place.nr_kontr, rentComponent.nr_skl, place.kod_lok, place.nr_lok, stawka, ilosc);
                            db.receivablesFrom1stSet.Add(receivableFrom1stSet);
                        }

                    db.SaveChanges();
                }
            }
        }
    }
}