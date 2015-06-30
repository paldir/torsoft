using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    public abstract class Obrót : IRekord, IPozycjaDoAnalizy
    {
        public const string Rok = "14";

        public abstract int __record { get; set; }

        public abstract decimal Kwota { get; set; }

        public abstract DateTime Data { get; set; }

        public abstract string opis { get; set; }

        public abstract int nr_kontr { get; set; }

        public abstract int ZewnętrzneId { get; set; }

        public abstract string nr_dowodu { get; set; }

        public abstract int pozycja_d { get; set; }

        public abstract string uwagi { get; set; }

        public decimal Ilość
        {
            get { return 0; }
            set { }
        }

        public decimal Stawka
        {
            get { return 0; }
            set { }
        }

        //public static Dictionary<Enums.SettlementTable, List<Turnover>> SettlementTableToListOfTurnovers { get; private set; }

        enum Konto { Wn, Ma };

        /*static Turnover()
        {
            SettlementTableToListOfTurnovers = new Dictionary<Enums.SettlementTable, List<Turnover>>();

            using (Czynsze_Entities db = new Czynsze_Entities())
            {
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.Czynsze, new Czynsze_Entities().turnoversFor14.ToList().Cast<DostępDoBazy.Turnover>().ToList());
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.SecondSet, new Czynsze_Entities().turnoversFor14From2ndSet.ToList().Cast<DostępDoBazy.Turnover>().ToList());
                SettlementTableToListOfTurnovers.Add(Enums.SettlementTable.ThirdSet, new Czynsze_Entities().turnoversFor14From3rdSet.ToList().Cast<DostępDoBazy.Turnover>().ToList());
            }
        }*/

        public string[] WażnePola()
        {
            string data_obr = null;

            if (this.Data != null)
                data_obr = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.Data);
            
            return new string[] 
            {
                __record.ToString(),
                String.Format("{0:N}", Kwota),
                data_obr,
                DateTime.Today.ToShortDateString(),
                opis,
                nr_dowodu,
                pozycja_d.ToString(),
                uwagi
            };
        }

        public string[] WażnePolaDoNależnościIObrotówNajemcy()
        {
            RodzajPłatności typ;
            Konto konto = Konto.Wn;
            int mnożnik = 1;
            string wn = String.Empty;
            string ma = String.Empty;
            string data_obr = null;

            using (CzynszeKontekst db = new CzynszeKontekst())
                typ = db.RodzajePłatności.FirstOrDefault(t => t.Id == ZewnętrzneId);

            switch (typ.s_rozli)
            {
                case 1:
                    switch (typ.tn_odset)
                    {
                        case 0:
                            konto = Konto.Wn;
                            mnożnik = -1;

                            break;

                        case 1:
                            konto = Konto.Ma;

                            break;
                    }

                    break;

                case 2:
                    konto = Konto.Wn;

                    break;

                case 3:
                    konto = Konto.Ma;
                    mnożnik = -1;

                    break;
            }

            string suma = (this.Kwota * mnożnik).ToString("F2");

            switch (konto)
            {
                case Konto.Wn:
                    wn = suma;

                    break;

                case Konto.Ma:
                    ma = suma;

                    break;
            }

            if (this.Data != null)
                data_obr = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.Data);

            return new string[]
            {
                __record.ToString(),
                wn,
                ma,
                data_obr,
                opis
            };
        }

        public string[] WszystkiePola()
        {
            string data_obr = null;

            if (this.Data != null)
                data_obr = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.Data);
            
            return new string[]
            {
                __record.ToString(),
                Kwota.ToString(),
                data_obr,
                DateTime.Today.ToShortDateString(),
                ZewnętrzneId.ToString(),
                nr_dowodu.Trim(),
                pozycja_d.ToString(),
                uwagi.Trim(),
                nr_kontr.ToString()
            };
        }

        public void Ustaw(string[] rekord)
        {
            __record = Int32.Parse(rekord[0]);
            Kwota = Decimal.Parse(rekord[1]);

            if (!String.IsNullOrEmpty(rekord[2]))
                Data = Convert.ToDateTime(rekord[2]);

            //data_NO = record[3];
            ZewnętrzneId = Int32.Parse(rekord[4]);
            nr_dowodu = rekord[5];
            pozycja_d = Int32.Parse(rekord[6]);
            uwagi = rekord[7];
            nr_kontr = Int32.Parse(rekord[8]);

            using (CzynszeKontekst db = new CzynszeKontekst())
                opis = db.RodzajePłatności.FirstOrDefault(t => t.Id == ZewnętrzneId).Nazwa;
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                wynik += CzynszeKontekst.WalidujFloat("Kwota", ref rekord[1]);
                wynik += CzynszeKontekst.WalidujDatę("Data", ref rekord[2]);
                wynik += CzynszeKontekst.WalidujDatę("Data NO", ref rekord[3]);
                wynik += CzynszeKontekst.WalidujInt("Pozycja", ref rekord[6]);
            }

            return wynik;
        }
    }
}