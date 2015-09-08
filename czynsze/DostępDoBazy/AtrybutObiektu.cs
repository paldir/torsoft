using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    public abstract class AtrybutObiektu
    {
        [Key]
        public int __record { get; set; }

        public int kod { get; set; }

        [Column("kod_powiaz")]
        public string kod_powiaz_NIE_UŻYWAĆ { get; protected set; }

        [NotMapped]
        public abstract string kod_powiaz { get; set; }

        string _nr_str;

        [NotMapped]
        public string nr_str
        {
            get
            {
                if (String.IsNullOrEmpty(_nr_str))
                    using (DostępDoBazy.CzynszeKontekst db = new CzynszeKontekst())
                        _nr_str = db.Atrybuty.Single(a => a.kod == kod).nr_str;

                return _nr_str;
            }

            set { _nr_str = value; }
        }

        public float wartosc_n { get; private set; }

        public string wartosc_s { get; private set; }

        [NotMapped]
        public object wartosc
        {
            get
            {
                switch (nr_str)
                {
                    case "N":
                        return wartosc_n;

                    case "C":
                        return wartosc_s;

                    default:
                        throw new Exception("nr_str nie ustawiony.");
                }
            }

            set
            {
                switch (nr_str)
                {
                    case "N":
                        wartosc_n = Convert.ToSingle(value);

                        break;

                    case "C":
                        wartosc_s = value.ToString();

                        break;

                    default:
                        throw new Exception("nr_str nie ustawiony.");
                }
            }
        }

        public IEnumerable<string> PolaDoTabeli()
        {
            Atrybut atrybut;

            using (CzynszeKontekst db = new CzynszeKontekst())
                atrybut = db.Atrybuty.FirstOrDefault(a => a.kod == kod);

            return new string[]
            {
                __record.ToString(),
                atrybut.nazwa,
                wartosc.ToString()
            };
        }

        /*public void Ustaw(string[] rekord)
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
        }*/

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