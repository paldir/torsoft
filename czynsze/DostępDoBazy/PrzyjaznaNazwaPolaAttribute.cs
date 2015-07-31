using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrzyjaznaNazwaPolaAttribute : Attribute
    {
        public string Nazwa { get; private set; }

        public PrzyjaznaNazwaPolaAttribute(string nazwa)
        {
            Nazwa = nazwa;
        }
    }
}