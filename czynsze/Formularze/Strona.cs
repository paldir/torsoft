using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Formularze
{
    public class Strona : System.Web.UI.Page
    {
        public T PobierzWartośćParametru<T>(string klucz)
        {
            string wartość = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith(klucz))];

            if (wartość == null)
                return default(T);

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), wartość);
            else
                return (T)Convert.ChangeType(wartość, typeof(T));
        }

        public void DodajNowąLinię(System.Web.UI.Control pojemnik)
        {
            pojemnik.Controls.Add(new System.Web.UI.LiteralControl("<br />"));
        }
    }
}