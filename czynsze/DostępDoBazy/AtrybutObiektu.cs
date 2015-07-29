using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    public abstract class AtrybutObiektu
    {
        public abstract int __record { get; set; }

        public abstract int kod { get; set; }

        public abstract string kod_powiaz { get; set; }

        public abstract float wartosc_n { get; set; }

        public abstract string wartosc_s { get; set; }

        public string[] PolaDoTabeli()
        {
            Atrybut atrybut;
            string wartosc = String.Empty;

            using (CzynszeKontekst db = new CzynszeKontekst())
                atrybut = db.Atrybuty.FirstOrDefault(a => a.kod == kod);

            switch (atrybut.nr_str)
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
                atrybut.nazwa,
                wartosc
            };
        }

        public void Ustaw(string[] rekord)
        {
            __record = Int32.Parse(rekord[0]);
            kod = Int32.Parse(rekord[1]);

            using (DostępDoBazy.CzynszeKontekst db = new CzynszeKontekst())
                switch (db.Atrybuty.FirstOrDefault(a => a.kod == kod).nr_str)
                {
                    case "N":
                        wartosc_n = Single.Parse(rekord[2]);

                        break;

                    case "C":
                        wartosc_s = rekord[2];

                        break;
                }

            kod_powiaz = rekord[3];
        }

        public static bool Waliduj(Enumeratory.Akcja akcja, string[] rekord, List<DostępDoBazy.AtrybutObiektu> atrybutyObiektu)
        {
            if (akcja != Enumeratory.Akcja.Edytuj)
                if (atrybutyObiektu.Any(a => a.kod == Int32.Parse(rekord[1]) && Int32.Parse(a.kod_powiaz) == Int32.Parse(rekord[3])))
                    return false;

            using (DostępDoBazy.CzynszeKontekst db = new CzynszeKontekst())
                if (db.Atrybuty.ToList().FirstOrDefault(a => a.kod == Int32.Parse(rekord[1])).nr_str == "N")
                    try { Single.Parse(rekord[2]); }
                    catch { rekord[2] = "0"; }

            return true;
        }
    }
}