using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Forms
{
    public class Page : System.Web.UI.Page
    {
        public T GetParamValue<T>(string key)
        {
            string value = Request.Params[Request.Params.AllKeys.FirstOrDefault(k => k.EndsWith(key))];

            if (value == null)
            {
                if (Nullable.GetUnderlyingType(typeof(T)) == null)
                    value = "-1";
                else
                    value = String.Empty;
            }

            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), value);
            else
                return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}