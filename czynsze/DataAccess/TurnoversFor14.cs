using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("obr_14__", Schema = "public")]
    public class TurnoversFor14
    {
        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("suma")]
        public float suma { get; set; }

        [Column("data_obr")]
        public string data_obr { get; set; }

        [Column("opis")]
        public string opis { get; set; }

        [Column("nr_kontr")]
        public int nr_kontr { get; set; }

        [Column("kod_wplat")]
        public int kod_wplat { get; set; }

        enum Account { Wn, Ma };

        public string[] ImportantFields()
        {
            TypeOfPayment type;
            Account account = Account.Wn;
            int factor = 1;
            string wn = String.Empty;
            string ma = String.Empty;

            using (Czynsze_Entities db = new Czynsze_Entities())
                type = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == kod_wplat);

            switch(type.s_rozli)
            {
                case 1:
                    switch (type.tn_odset)
                    {
                        case 0:
                            account = Account.Wn;
                            factor = -1;
                            break;
                        case 1:
                            account = Account.Ma;
                            break;
                    }

                    break;
                case 2:
                    account = Account.Wn;
                    break;
                case 3:
                    account = Account.Ma;
                    factor = -1;
                    break;
            }

            string suma = (this.suma * factor).ToString("F2");

            switch (account)
            {
                case Account.Wn:
                    wn = suma;
                    break;
                case Account.Ma:
                    ma = suma;
                    break;
            }

            return new string[]
            {
                __record.ToString(),
                wn,
                ma,
                data_obr,
                opis
            };
        }
    }
}