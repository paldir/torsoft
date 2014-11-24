using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DataAccess
{
    public abstract class Turnover
    {
        public abstract int __record { get; set; }

        public abstract float suma { get; set; }

        public abstract string data_obr { get; set; }

        public abstract string opis { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int kod_wplat { get; set; }

        public abstract string nr_dowodu { get; set; }

        public abstract int pozycja_d { get; set; }

        public abstract string uwagi { get; set; }

        enum Account { Wn, Ma };

        public string[] ImportantFields()
        {
            return new string[] 
            {
                __record.ToString(),
                String.Format("{0:N2}", suma),
                data_obr,
                "?",
                opis,
                nr_dowodu,
                pozycja_d.ToString(),
                uwagi
            };
        }

        public string[] ImportantFieldsForReceivablesAndTurnoversOfTenant()
        {
            TypeOfPayment type;
            Account account = Account.Wn;
            int factor = 1;
            string wn = String.Empty;
            string ma = String.Empty;

            using (Czynsze_Entities db = new Czynsze_Entities())
                type = db.typesOfPayment.FirstOrDefault(t => t.kod_wplat == kod_wplat);

            switch (type.s_rozli)
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

        public string[] AllFields()
        {
            return new string[]
            {
                __record.ToString(),
                suma.ToString(),
                data_obr.Trim(),
                "?",
                kod_wplat.ToString(),
                nr_dowodu.Trim(),
                pozycja_d.ToString(),
                uwagi.Trim()
            };
        }
    }
}