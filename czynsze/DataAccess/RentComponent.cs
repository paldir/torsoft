using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("czynsz", Schema = "public")]
    public class RentComponent : IRecord
    {
        [Key, Column("nr_skl"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public int nr_skl { get; set; }

        [Column("nazwa")]
        public string nazwa { get; set; }

        [Column("s_zaplat")]
        public int s_zaplat { get; set; }

        [Column("typ_skl")]
        public int typ_skl { get; set; }

        [Column("stawka")]
        public float stawka { get; set; }

        [Column("rodz_e")]
        public int rodz_e { get; set; }

        [Column("stawka_inf")]
        public float stawka_inf { get; set; }

        [Column("data_1")]
        public string data_1 { get; set; }

        [Column("data_2")]
        public string data_2 { get; set; }

        [Column("stawka_00")]
        public float stawka_00 { get; set; }

        [Column("stawka_01")]
        public float stawka_01 { get; set; }

        [Column("stawka_02")]
        public float stawka_02 { get; set; }

        [Column("stawka_03")]
        public float stawka_03 { get; set; }

        [Column("stawka_04")]
        public float stawka_04 { get; set; }

        [Column("stawka_05")]
        public float stawka_05 { get; set; }

        [Column("stawka_06")]
        public float stawka_06 { get; set; }

        [Column("stawka_07")]
        public float stawka_07 { get; set; }

        [Column("stawka_08")]
        public float stawka_08 { get; set; }

        [Column("stawka_09")]
        public float stawka_09 { get; set; }

        string Recognize_s_zaplat()
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

        string Recognize_typ_skl()
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

        public string[] ImportantFields()
        {
            return new string[]
            {
                nr_skl.ToString(), 
                nr_skl.ToString(), 
                nazwa, 
                Recognize_s_zaplat(), 
                Recognize_typ_skl(), 
                stawka.ToString("F2") 
            };
        }

        public string[] ImportantFieldsForDropdown()
        {
            return new string[]
            {
                nr_skl.ToString(),
                nr_skl.ToString(),
                nazwa
            };
        }

        public string[] AllFields()
        {
            string data_1, data_2;

            if (this.data_1 == null)
                data_1 = String.Empty;
            else
                data_1 = this.data_1.ToString();

            if (this.data_2 == null)
                data_2 = String.Empty;
            else
                data_2 = this.data_2.ToString();

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

        public string Validate(Enums.Action action, string[] record)
        {
            string result = "";
            int id;

            if (action == Enums.Action.Dodaj)
                if (record[0].Length > 0)
                {
                    try
                    {
                        id = Convert.ToInt16(record[0]);

                        using (Czynsze_Entities db = new Czynsze_Entities())
                            if (db.rentComponents.Count(c => c.nr_skl == id) != 0)
                                result += "Numer składnika jest już używany! <br />";
                    }
                    catch { result += "Numer składnika musi być liczbą całkowitą! <br />"; }
                }
                else
                    result += "Należy podać numer składnika! <br />";

            if (action != Enums.Action.Usuń)
            {
                result += Czynsze_Entities.ValidateFloat("Stawka", ref record[4]);
                result += Czynsze_Entities.ValidateFloat("Stawka do korespondencji", ref record[5]);
                result += Czynsze_Entities.ValidateDate("Początek okresu naliczania", ref record[7]);
                result += Czynsze_Entities.ValidateDate("Koniec okresu naliczania", ref record[8]);
                result += Czynsze_Entities.ValidateFloat("Stawka za zero osób", ref record[9]);
                result += Czynsze_Entities.ValidateFloat("Stawka za jedną osobę", ref record[10]);
                result += Czynsze_Entities.ValidateFloat("Stawka za dwie osoby", ref record[11]);
                result += Czynsze_Entities.ValidateFloat("Stawka za trzy osoby", ref record[12]);
                result += Czynsze_Entities.ValidateFloat("Stawka za cztery osoby", ref record[13]);
                result += Czynsze_Entities.ValidateFloat("Stawka za pięć osób", ref record[14]);
                result += Czynsze_Entities.ValidateFloat("Stawka za sześć osób", ref record[15]);
                result += Czynsze_Entities.ValidateFloat("Stawka za siedem osób", ref record[16]);
                result += Czynsze_Entities.ValidateFloat("Stawka za osiem osób", ref record[17]);
                result += Czynsze_Entities.ValidateFloat("Stawka za dziewięć osób", ref record[18]);
            }
            else
            {
                id = Convert.ToInt16(record[0]);

                using (Czynsze_Entities db = new Czynsze_Entities())
                    if (db.rentComponentsOfPlaces.Count(c => c.nr_skl == id) > 0)
                        result += "Nie można usunąć składnika opłat, który jest przypisany do lokali! <br />";
            }

            return result;
        }

        public void Set(string[] record)
        {
            nr_skl = Convert.ToInt16(record[0]);
            nazwa = record[1];
            rodz_e = Convert.ToInt16(record[2]);
            s_zaplat = Convert.ToInt16(record[3]);
            stawka = Convert.ToSingle(record[4]);
            stawka_inf = Convert.ToSingle(record[5]);
            typ_skl = Convert.ToInt16(record[6]);
            data_1 = record[7];
            data_2 = record[8];
            stawka_00 = Convert.ToSingle(record[9]);
            stawka_01 = Convert.ToSingle(record[10]);
            stawka_02 = Convert.ToSingle(record[11]);
            stawka_03 = Convert.ToSingle(record[12]);
            stawka_04 = Convert.ToSingle(record[13]);
            stawka_05 = Convert.ToSingle(record[14]);
            stawka_06 = Convert.ToSingle(record[15]);
            stawka_07 = Convert.ToSingle(record[16]);
            stawka_08 = Convert.ToSingle(record[17]);
            stawka_09 = Convert.ToSingle(record[18]);
        }
    }
}