using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("czynsz", Schema = "public")]
    public class SkładnikCzynszu : Rekord, IInformacjeOPozycji
    {
        [Display(Name = "nr składnika")]
        public int nr_skl { get; set; }

        [Display(Name = "nazwa składnika")]
        public string nazwa { get; set; }

        [Display(Name = "sposób zapłaty")]
        public int s_zaplat { get; set; }

        [Display(Name = "typ składnika")]
        public int typ_skl { get; set; }

        [Display(Name = "stawka")]
        public decimal stawka { get; set; }

        [Display(Name = "rodzaj ewidencji")]
        public int rodz_e { get; set; }

        [Display(Name = "stawka informacyjna")]
        public decimal stawka_inf { get; set; }

        [Display(Name = "początek okresu naliczania")]
        public Nullable<DateTime> data_1 { get; set; }

        [Display(Name = "koniec okresu naliczania")]
        public Nullable<DateTime> data_2 { get; set; }

        [Display(Name = "grupa składników")]
        public int kod { get; set; }

        [Display(Name = "stawka za 0 osób")]
        public decimal stawka_00 { get; set; }

        [Display(Name = "stawka za 1 osobę")]
        public decimal stawka_01 { get; set; }

        [Display(Name = "stawka za 2 osoby")]
        public decimal stawka_02 { get; set; }

        [Display(Name = "stawka za 3 osoby")]
        public decimal stawka_03 { get; set; }

        [Display(Name = "stawka za 4 osoby")]
        public decimal stawka_04 { get; set; }

        [Display(Name = "stawka za 5 osób")]
        public decimal stawka_05 { get; set; }

        [Display(Name = "stawka za 6 osób")]
        public decimal stawka_06 { get; set; }

        [Display(Name = "stawka za 7 osób")]
        public decimal stawka_07 { get; set; }

        [Display(Name = "stawka za 8 osób")]
        public decimal stawka_08 { get; set; }

        [Display(Name = "stawka za 9 osób")]
        public decimal stawka_09 { get; set; }

        [Display(Name = "nr składnika")]
        public override int id
        {
            get { return nr_skl; }
            set { nr_skl = value; }
        }

        public int IdInformacji
        {
            get { return nr_skl; }
        }

        public string Nazwa
        {
            get { return nazwa; }
        }

        public int RodzajEwidencji
        {
            get { return rodz_e; }
        }

        public int Grupa
        {
            get { return kod; }
        }

        string Rozpoznaj_s_zaplat()
        {
            switch (s_zaplat)
            {
                case 1:
                    return "za m<sup>2</sup> pow. użytkowej";
                case 2:
                    return "za określoną ilość";
                case 3:
                    return "za osobę";
                case 4:
                    return "za lokal";
                case 5:
                    return "za ilość dni w miesiącu";
                case 6:
                    return "za osobę - przedziały";
                default:
                    return String.Empty;
            }
        }

        string Rozpoznaj_typ_skl()
        {
            switch (typ_skl)
            {
                case 0:
                    return "stały";
                case 1:
                    return "zmienny";
                default:
                    return String.Empty;
            }
        }

        public override string[] PolaDoTabeli()
        {
            return new string[]
            {
                nr_skl.ToString(), 
                nr_skl.ToString(), 
                nazwa, 
                Rozpoznaj_s_zaplat(), 
                Rozpoznaj_typ_skl(), 
                stawka.ToString("F2") 
            };
        }

        public string[] WażnePolaDoRozwijanejListy()
        {
            return new string[]
            {
                nr_skl.ToString(),
                nr_skl.ToString(),
                nazwa
            };
        }

        public override string[] WszystkiePola()
        {
            string data_1, data_2;

            if (this.data_1 == null)
                data_1 = null;
            else
                data_1 = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.data_1);

            if (this.data_2 == null)
                data_2 = null;
            else
                data_2 = String.Format(DostępDoBazy.CzynszeKontekst.FormatDaty, this.data_2);

            return new string[]
            {
                nr_skl.ToString(),
                nazwa.Trim(),
                rodz_e.ToString(),
                s_zaplat.ToString(),
                stawka.ToString("F2"),
                stawka_inf.ToString("F2"),
                typ_skl.ToString(), 
                data_1,
                data_2,
                kod.ToString(),
                stawka_00.ToString("F2"),
                stawka_01.ToString("F2"),
                stawka_02.ToString("F2"),
                stawka_03.ToString("F2"),
                stawka_04.ToString("F2"),
                stawka_05.ToString("F2"),
                stawka_06.ToString("F2"),
                stawka_07.ToString("F2"),
                stawka_08.ToString("F2"),
                stawka_09.ToString("F2")
            };
        }

        public override string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = "";
            int id;

            if (akcja == Enumeratory.Akcja.Dodaj)
                if (rekord[0].Length > 0)
                {
                    try
                    {
                        id = Int32.Parse(rekord[0]);

                        using (CzynszeKontekst db = new CzynszeKontekst())
                            if (db.SkładnikiCzynszu.Any(c => c.nr_skl == id))
                                wynik += "Numer składnika jest już używany! <br />";
                    }
                    catch { wynik += "Numer składnika musi być liczbą całkowitą! <br />"; }
                }
                else
                    wynik += "Należy podać numer składnika! <br />";

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                wynik += CzynszeKontekst.WalidujFloat("Stawka", ref rekord[4]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka do korespondencji", ref rekord[5]);
                wynik += CzynszeKontekst.WalidujDatę("Początek okresu naliczania", ref rekord[7]);
                wynik += CzynszeKontekst.WalidujDatę("Koniec okresu naliczania", ref rekord[8]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za zero osób", ref rekord[10]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za jedną osobę", ref rekord[11]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za dwie osoby", ref rekord[12]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za trzy osoby", ref rekord[13]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za cztery osoby", ref rekord[14]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za pięć osób", ref rekord[15]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za sześć osób", ref rekord[16]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za siedem osób", ref rekord[17]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za osiem osób", ref rekord[18]);
                wynik += CzynszeKontekst.WalidujFloat("Stawka za dziewięć osób", ref rekord[19]);
            }
            else
            {
                id = Int32.Parse(rekord[0]);

                using (CzynszeKontekst db = new CzynszeKontekst())
                    if (db.SkładnikiCzynszuLokalu.Any(c => c.nr_skl == id))
                        wynik += "Nie można usunąć składnika opłat, który jest przypisany do lokali! <br />";
            }

            return wynik;
        }

        public override void Ustaw(string[] rekord)
        {
            nr_skl = Int32.Parse(rekord[0]);
            nazwa = rekord[1];
            rodz_e = Int32.Parse(rekord[2]);
            s_zaplat = Int32.Parse(rekord[3]);
            stawka = Decimal.Parse(rekord[4]);
            stawka_inf = Decimal.Parse(rekord[5]);
            typ_skl = Int32.Parse(rekord[6]);

            if (!String.IsNullOrEmpty(rekord[7]))
                data_1 = Convert.ToDateTime(rekord[7]);

            if (!String.IsNullOrEmpty(rekord[8]))
                data_2 = Convert.ToDateTime(rekord[8]);

            kod = Int32.Parse(rekord[9]);
            stawka_00 = Decimal.Parse(rekord[10]);
            stawka_01 = Decimal.Parse(rekord[11]);
            stawka_02 = Decimal.Parse(rekord[12]);
            stawka_03 = Decimal.Parse(rekord[13]);
            stawka_04 = Decimal.Parse(rekord[14]);
            stawka_05 = Decimal.Parse(rekord[15]);
            stawka_06 = Decimal.Parse(rekord[16]);
            stawka_07 = Decimal.Parse(rekord[17]);
            stawka_08 = Decimal.Parse(rekord[18]);
            stawka_09 = Decimal.Parse(rekord[19]);
        }
    }
}