using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("cechy_b", Schema="public")]
    public class AttributeOfBuilding
    {
        [Key, Column("__record")]
        public int __record { get; set; }
        
        [Column("kod")]
        public int kod { get; set; }

        [Column("kod_powiaz")]
        public string kod_powiaz { get; set; }

        [Column("wartosc_n")]
        public float wartosc_n { get; set; }

        [Column("wartosc_s")]
        public string wartosc_s { get; set; }

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
    }
}