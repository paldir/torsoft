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
                    /*int indeksOstatniegoDolara = value.LastIndexOf('$');

                    if (indeksOstatniegoDolara != -1)
                        value = value.Substring(indeksOstatniegoDolara + 1);*/
                    value = _nazwaKontrolki;

                    break;
            }

            base.AddAttribute(key, value);
        }
    }
}