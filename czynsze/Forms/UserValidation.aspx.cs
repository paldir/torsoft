using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

namespace czynsze.Forms
{
    public partial class UserValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = Request.Params["uzytkownik"];
            int[] passwordAscii = Request.Params["haslo"].ToString().Split(',').ToList().Select(a => Convert.ToInt32(a)).ToArray();
            string correctPassword;
            bool validated = false;
            byte[] asciiOfCorrectPassword;
            bool validationFailed;

            using (DataAccess.Czynsze_Entities db = new DataAccess.Czynsze_Entities())
                correctPassword = db.users.Where(u => u.uzytkownik == user).FirstOrDefault().haslo.Trim();

            foreach (EncodingInfo encoding in Encoding.GetEncodings())
            {
                asciiOfCorrectPassword = Encoding.GetEncoding(encoding.CodePage).GetBytes(correctPassword);
                validationFailed = false;

                if (passwordAscii.Length == asciiOfCorrectPassword.Length)
                {
                    for (int i = 0; i < passwordAscii.Length; i++)
                        if (passwordAscii[i] != Convert.ToInt16(asciiOfCorrectPassword[i]))
                        {
                            validationFailed = true;

                            break;
                        }

                    if (!validationFailed)
                    {
                        validated = true;

                        break;
                    }
                }
            }

            if (validated)
            {
                Session["user"] = user;
                Response.Redirect("List.aspx?table=" + EnumP.Table.Buildings.ToString());
            }
            else
                Response.Redirect("../Login.aspx");
        }
    }
}