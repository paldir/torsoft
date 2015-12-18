﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class ZmianaDaty : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(PobierzWartośćParametru<string>("zmiana")))
            {
                Start.ŚcieżkaStrony.Wyczyść();

                Kontrolki.Button button = new Kontrolki.Button("button", "zmiana", "Zmień", String.Empty);

                placeOfMonth.Controls.Add(new Kontrolki.TextBox("field", "month", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 2, 1, true, Start.Data.Month.ToString()));
                placeOfYear.Controls.Add(new Kontrolki.TextBox("field", "year", Kontrolki.TextBox.TextBoxMode.LiczbaCałkowita, 4, 1, true, Start.Data.Year.ToString()));
                placeOfButton.Controls.Add(button);
            }
            else
            {
                int month;
                int year;
                int day;

                try { month = PobierzWartośćParametru<int>("month"); }
                catch { month = DateTime.Today.Month; }

                try { year = PobierzWartośćParametru<int>("year"); }
                catch { year = DateTime.Today.Year; }

                if (month == DateTime.Today.Month)
                    day = DateTime.Today.Day;
                else
                    try { day = DateTime.DaysInMonth(year, month); }
                    catch { day = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month); }

                try { Start.Data = new DateTime(year, month, day); }
                catch { Start.Data = DateTime.Today; }

                DostępDoBazy.CzynszeKontekst.Rok = year;

                Response.Redirect("Start.aspx");
            }
        }
    }
}