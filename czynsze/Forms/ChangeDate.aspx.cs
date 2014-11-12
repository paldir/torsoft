using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class ChangeDate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Hello.SiteMapPath.Clear();

            ControlsP.ButtonP button = new ControlsP.ButtonP("button", "Change", "Zmień", "#");
            button.Click += button_Click;

            placeOfMonth.Controls.Add(new ControlsP.TextBoxP("field", "month", Hello.Date.Month.ToString(), ControlsP.TextBoxP.TextBoxModeP.Number, 2, 1, true));
            placeOfYear.Controls.Add(new ControlsP.TextBoxP("field", "year", Hello.Date.Year.ToString(), ControlsP.TextBoxP.TextBoxModeP.Number, 4, 1, true));
            placeOfButton.Controls.Add(button);
        }

        void button_Click(object sender, EventArgs e)
        {
            Control monthTextBox = placeOfMonth.FindControl("month");
            Control yearTextBox = placeOfYear.FindControl("year");
            int month;
            int year;
            int day;

            try { month = Convert.ToInt16(((TextBox)monthTextBox).Text); }
            catch { month = DateTime.Today.Month; }

            try { year = Convert.ToInt16(((TextBox)yearTextBox).Text); }
            catch { year = DateTime.Today.Year; }

            if (month == DateTime.Today.Month)
                day = DateTime.Today.Day;
            else
                try { day = DateTime.DaysInMonth(year, month); }
                catch { day = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month); }

            try { Hello.Date = new DateTime(year, month, day); }
            catch { Hello.Date = DateTime.Today; }

            Response.Redirect("Hello.aspx");
        }
    }
}