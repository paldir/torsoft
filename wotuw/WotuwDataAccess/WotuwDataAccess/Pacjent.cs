using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WotuwDataAccess
{
    [Table("pacjent", Schema = "public")]
    public class Pacjent : Record
    {
        [Key, Column("idOsoby"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [Column("nazwisko")]
        public string nazwisko { get; set; }

        [Column("imie")]
        public string imie { get; set; }

        [Column("imieOjca")]
        public string imieOjca { get; set; }

        [Column("pesel")]
        public string pesel { get; set; }

        [Column("dataUrodzenia")]
        public DateTime? dataUrodzenia { get; set; }

        [Column("plec")]
        public string plec { get; set; }

        [Column("miejscowosc")]
        public string miejscowosc { get; set; }

        [Column("ulica")]
        public string ulica { get; set; }

        [Column("nrDomu")]
        public string nrDomu { get; set; }

        [Column("kodPocztowy")]
        public string kodPocztowy { get; set; }

        [Column("poczta")]
        public string poczta { get; set; }

        [Column("gmina")]
        public string gmina { get; set; }

        [Column("kasaChorych")]
        public string kasaChorych { get; set; }

        [Column("branza")]
        public string branza { get; set; }

        [Column("nrUbezpieczenia")]
        public string nrUbezpieczenia { get; set; }

        [Column("dataPierwszejWizyty")]
        public DateTime? dataPierwszejWizyty { get; set; }

        [Column("poborowy")]
        public bool? poborowy { get; set; }

        [Column("grupaOpiekuncza")]
        public string grupaOpiekuncza { get; set; }

        [Column("obcy")]
        public bool? obcy { get; set; }

        [Column("oddzial")]
        public string oddzial { get; set; }

        public override void Add()
        {
            DataBase.Pacjenci.Add(this);
            DataBase.SaveChanges();
        }

        public override void Remove()
        {
            DataBase.Pacjenci.Remove(this);
            DataBase.SaveChanges();
        }

        public override void Set(string[] fields)
        {
            if (fields.Length != 21)
                Array.Resize(ref fields, 21);

            nazwisko = fields[1];
            imie = fields[2];
            imieOjca = fields[3];
            pesel = fields[4];
            dataUrodzenia = ConvertIfPossible<DateTime>(fields[5]);
            plec = fields[6];
            miejscowosc = fields[7];
            ulica = fields[8];
            nrDomu = fields[9];
            kodPocztowy = fields[10];
            poczta = fields[11];
            gmina = fields[12];
            kasaChorych = fields[13];
            branza = fields[14];
            nrUbezpieczenia = fields[15];
            dataPierwszejWizyty = ConvertIfPossible<DateTime>(fields[16]);
            poborowy = ConvertIfPossible<bool>(fields[17]);
            grupaOpiekuncza = fields[18];
            obcy = ConvertIfPossible<bool>(fields[19]);
            oddzial = fields[20];
        }

        public static List<Pacjent> All
        {
            get
            {
                return DataBase.Pacjenci.ToList();
            }
        }

        public static Pacjent Find(long idOsoby)
        {
            return DataBase.Pacjenci.FirstOrDefault(p => p.Id == idOsoby);
        }

        public static string Validate(IList<string> fields)
        {
            string result = String.Empty;

            if (!IsConvertible<DateTime>(fields[5]))
                result += "data urodzenia, ";

            if (!IsConvertible<DateTime>(fields[16]))
                result += "data pierwszej wizyty, ";

            if (!IsConvertible<bool>(fields[17]))
                result += "poborowy, ";

            if (!IsConvertible<bool>(fields[19]))
                result += "obcy, ";

            if (!String.IsNullOrEmpty(result))
            {
                result = result.Remove(result.LastIndexOf(", "));
                result = "Niewłaściwy format dla pól: " + result;
            }

            return result;
        }
    }
}