using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DataAccess
{
    public abstract class AttributeOfObject
    {
        public abstract int __record { get; set; }

        public abstract int kod { get; set; }

        public abstract string kod_powiaz { get; set; }

        public abstract decimal wartosc_n { get; set; }

        public abstract string wartosc_s { get; set; }

        public string[] ImportantFields()
        {
            Attribute attribute;
            string wartosc = String.Empty;

            using (Czynsze_Entities db = new Czynsze_Entities())
                attribute = db.attributes.FirstOrDefault(a => a.kod == kod);

            switch (attribute.nr_str)
            {
                case "N":
                    wartosc = wartosc_n.ToString("F2");

                    break;

                case "C":
                    wartosc = wartosc_s;

                    break;
            }

            return new string[]
            {
                __record.ToString(),
                attribute.nazwa,
                wartosc
            };
        }

        public void Set(string[] record)
        {
            __record = Convert.ToInt32(record[0]);
            kod = Convert.ToInt32(record[1]);

            using (DataAccess.Czynsze_Entities db = new Czynsze_Entities())
                switch (db.attributes.FirstOrDefault(a => a.kod == kod).nr_str)
                {
                    case "N":
                        wartosc_n = Convert.ToDecimal(record[2]);

                        break;

                    case "C":
                        wartosc_s = record[2];

                        break;
                }

            kod_powiaz = record[3];
        }

        public static bool Validate(Enums.Action action, string[] record, List<DataAccess.AttributeOfObject> attributesOfObject)
        {
            if (action != Enums.Action.Edytuj)
                if (attributesOfObject.Any(a => a.kod == Convert.ToInt32(record[1]) && Convert.ToInt32(a.kod_powiaz) == Convert.ToInt32(record[3])))
                    return false;

            using (DataAccess.Czynsze_Entities db = new Czynsze_Entities())
                if (db.attributes.ToList().FirstOrDefault(a => a.kod == Convert.ToInt32(record[1])).nr_str == "N")
                    try { Convert.ToSingle(record[2]); }
                    catch { record[2] = "0"; }

            return true;
        }
    }
}