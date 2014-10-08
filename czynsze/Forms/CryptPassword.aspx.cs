using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Forms
{
    public partial class CryptPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
            {
                DataAccess.User user = db.users.ToList().FirstOrDefault(u => u.uzytkownik.Trim() == Request.Params["uzytkownik"].Trim());
                int[] passwordAscii = Request.Params["haslo"].ToString().Split(',').ToList().Select(a => Convert.ToInt32(a)).ToArray();
                string[] record = user.AllFields();
                record[5] = String.Empty;

                foreach (int ascii in passwordAscii)
                    record[5] += (char)ascii;

                user.Set(record);
                db.SaveChanges();
            }
        }
    }
}