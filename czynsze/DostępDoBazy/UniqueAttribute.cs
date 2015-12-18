using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Reflection;

namespace czynsze.DostępDoBazy
{
    public class UniqueAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        int _numerGrupy;
        public int NumerGrupy
        {
            get { return _numerGrupy; }

            private set
            {
                if (value == 0)
                    throw new Exception("Numer grupy 0 jest zarezerwowany.");
                else
                    _numerGrupy = value;
            }
        }

        public UniqueAttribute() { }

        public UniqueAttribute(int numerGrupy)
        {
            NumerGrupy = numerGrupy;
        }

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            using (CzynszeKontekst db = new CzynszeKontekst())
            {
                Type typTegoAtrybutu = typeof(UniqueAttribute);
                IEnumerable<PropertyInfo> właściwościUnikalne = validationContext.ObjectType.GetProperties().Where(w => Attribute.IsDefined(w, typTegoAtrybutu));
                Dictionary<int, List<PropertyInfo>> numerGrupyNaWłaściwości = new Dictionary<int, List<PropertyInfo>>();
                System.Data.Entity.DbSet tabela = db.Set(validationContext.ObjectType);

                foreach (PropertyInfo właściwość in właściwościUnikalne)
                {
                    int numerGrupy = właściwość.GetCustomAttribute<UniqueAttribute>().NumerGrupy;

                    if (!numerGrupyNaWłaściwości.ContainsKey(numerGrupy))
                        numerGrupyNaWłaściwości[numerGrupy] = new List<PropertyInfo>();

                    numerGrupyNaWłaściwości[numerGrupy].Add(właściwość);
                }

                if (numerGrupyNaWłaściwości.ContainsKey(0))
                { }

                foreach(KeyValuePair<int, List<PropertyInfo>> pozycja in numerGrupyNaWłaściwości)
                {

                }

                return new System.ComponentModel.DataAnnotations.ValidationResult("");
            }
        }
    }
}