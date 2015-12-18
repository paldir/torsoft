using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WotuwDataAccess
{
    [Table("swiadczenie", Schema = "public")]
    public class Swiadczenie : Record
    {
        [Key, Column("idSwiadczenia"), DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [Column("idKontaktu")]
        public long? idKontaktu { get; set; }

        [Column("idOsoby")]
        public long? idOsoby { get; set; }

        [Column("rodzajSwiadczenia")]
        public int? rodzajSwiadczenia { get; set; }

        [Column("klasyfikacjaIcd")]
        public string klasyfikacjaIcd { get; set; }

        [Column("podstawaPlatnosci")]
        public string podstawaPlatnosci { get; set; }

        [Column("oplataPacjenta")]
        public string oplataPacjenta { get; set; }

        [Column("doplataKasy")]
        public string doplataKasy { get; set; }

        [Column("dataRealizacji")]
        public DateTime? dataRealizacji { get; set; }

        [Column("dataRealizacjiDo")]
        public DateTime? dataRealizacjiDo { get; set; }

        [Column("zlecajacyOsoba")]
        public string zlecajacyOsoba { get; set; }

        [Column("zlecajacyFirma")]
        public string zlecajacyFirma { get; set; }

        [Column("dataZlecenia")]
        public DateTime? dataZlecenia { get; set; }

        [Column("realizujacyOsoba")]
        public long? realizujacyOsoba { get; set; }

        [Column("realizujacyKomorka")]
        public int? realizujacyKomorka { get; set; }

        [Column("kartoteka")]
        public string kartoteka { get; set; }

        [Column("dataWpisu")]
        public DateTime? dataWpisu { get; set; }

        [Column("kasa")]
        public string kasa { get; set; }

        public static List<Swiadczenie> All
        {
            get
            {
                return DataBase.Swiadczenia.ToList();
            }
        }

        public override void Add()
        {
            DataBase.Swiadczenia.Add(this);
            DataBase.SaveChanges();
        }

        public override void Remove()
        {
            DataBase.Swiadczenia.Remove(this);
            DataBase.SaveChanges();
        }

        public override void Set(string[] fields)
        {
            if (fields.Length != 18)
                Array.Resize(ref fields, 18);
            
            idKontaktu = ConvertIfPossible<long>(fields[1]);
            idOsoby = ConvertIfPossible<long>(fields[2]);
            rodzajSwiadczenia = ConvertIfPossible<int>(fields[3]);
            klasyfikacjaIcd = fields[4];
            podstawaPlatnosci = fields[5];
            oplataPacjenta = fields[6];
            doplataKasy = fields[7];
            dataRealizacji = ConvertIfPossible<DateTime>(fields[8]);
            dataRealizacjiDo = ConvertIfPossible<DateTime>(fields[9]);
            zlecajacyOsoba = fields[10];
            zlecajacyFirma = fields[11];
            dataZlecenia = ConvertIfPossible<DateTime>(fields[12]);
            realizujacyOsoba = ConvertIfPossible<long>(fields[13]);
            realizujacyKomorka = ConvertIfPossible<int>(fields[14]);
            kartoteka = fields[15];
            dataWpisu = ConvertIfPossible<DateTime>(fields[16]);
            kasa = fields[17];
        }

        public static Swiadczenie Find(long idSwiadczenia)
        {
            return DataBase.Swiadczenia.FirstOrDefault(s => s.Id == idSwiadczenia);
        }

        public static string Validate(IList<string> fields)
        {
            string result = String.Empty;

            if (!IsConvertible<DateTime>(fields[8]))
                result += "data realizacji, ";

            if (!IsConvertible<DateTime>(fields[9]))
                result += "data realizacji do, ";

            if (!IsConvertible<DateTime>(fields[12]))
                result += "data zlecenia, ";

            if (!IsConvertible<DateTime>(fields[16]))
                result += "data wpisu";

            if (!String.IsNullOrEmpty(result))
            {
                result = result.Remove(result.LastIndexOf(", "));
                result = "Niewłaściwy format dla pól: " + result;
            }

            return result;
        }
    }
}