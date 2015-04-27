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

            MyControls.Button button = new MyControls.Button("button", "Change", "Zmień", "#");
            button.Click += button_Click;

            placeOfMonth.Controls.Add(new MyControls.TextBox("field", "month", Hello.Date.Month.ToString(), MyControls.TextBox.TextBoxMode.Number, 2, 1, true));
            placeOfYear.Controls.Add(new MyControls.TextBox("field", "year", Hello.Date.Year.ToString(), MyControls.TextBox.TextBoxMode.Number, 4, 1, true));
            placeOfButton.Controls.Add(button);
        }

        void button_Click(object sender, EventArgs e)
        {
            Control monthTextBox = placeOfMonth.FindControl("month");
            Control yearTextBox = placeOfYear.FindControl("year");
            int month;
            int year;
            int day;

            try { month = Int32.Parse(((TextBox)monthTextBox).Text); }
            catch { month = DateTime.Today.Month; }

            try { year = Int32.Parse(((TextBox)yearTextBox).Text); }
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