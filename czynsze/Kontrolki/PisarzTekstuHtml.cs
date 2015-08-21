using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    class PisarzTekstuHtml : System.Web.UI.HtmlTextWriter
    {
        string _nazwaKontrolki;

        public PisarzTekstuHtml(System.IO.TextWriter writer, string nazwaKontrolki)
            : base(writer)
        {
            _nazwaKontrolki = nazwaKontrolki;
        }

        public override void AddAttribute(System.Web.UI.HtmlTextWriterAttribute key, string value)
        {
            switch (key)
            {
                case System.Web.UI.HtmlTextWriterAttribute.Name:
                    value = _nazwaKontrolki;

                    break;
            }

            base.AddAttribute(key, value);
        }

        public override void WriteAttribute(string name, string value)
        {
            if (String.Equals(name, "name", StringComparison.OrdinalIgnoreCase))
                value = _nazwaKontrolki;

            base.WriteAttribute(name, value);
        }
    }
}