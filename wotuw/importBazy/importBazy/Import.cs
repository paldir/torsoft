using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace importBazy
{
    static class Import
    {
        static readonly Dictionary<Type, string> fromTypeToFileName = new Dictionary<Type, string>()
        {
            {typeof(WotuwDataAccess.Pacjent), "Pacjenci.txt"},
            {typeof(WotuwDataAccess.Swiadczenie), "Swiadczenia.txt"}
        };

        public static void ImportFromFile(Type entityType)
        {
            List<WotuwDataAccess.Record> records = new List<WotuwDataAccess.Record>();
            WotuwDataAccess.WotuwEntities db = WotuwDataAccess.Record.DataBase;
            int i = 0;

            Console.WriteLine("Wczytywanie rekordów z pliku...");

            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fromTypeToFileName[entityType]))
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split('\t');
                        long id;
                        i++;

                        if (Int64.TryParse(line[0], out id))
                        {
                            WotuwDataAccess.Record record = (WotuwDataAccess.Record)Activator.CreateInstance(entityType);
                            record.Id = id;

                            PrepareLine(line);
                            record.Set(line);
                            records.Add(record);
                        }
                    }

                Console.WriteLine("Liczba wczytanych rekordów: {0}.", i);
                Console.WriteLine("Dodawanie rekordów do bazy danych...");
                db.Set(entityType).AddRange(records);
                Console.WriteLine("Zapisywanie zmian. Może to potrwać kilka minut...");
                db.SaveChanges();
            }
            catch (Exception exception)
            {
                DisplayExceptionMessage(exception);
            }
        }

        static void DisplayExceptionMessage(Exception exception)
        {
            Exception innerException = exception.InnerException;

            Console.WriteLine(exception.Message);

            if (innerException != null)
                DisplayExceptionMessage(innerException);
        }

        static string PolishBoolToEnglishBool(string chars)
        {
            switch (chars.ToLower())
            {
                case "prawda":
                case "true":
                    return true.ToString();

                case "fałsz":
                case "false":
                    return false.ToString();

                default:
                    throw new ArgumentException();
            }
        }

        static void PrepareLine(string[] line)
        {
            for (int j = 1; j < line.Length; j++)
            {
                if (String.IsNullOrEmpty(line[j]))
                    line[j] = null;
                else
                {
                    string lowerField = line[j].ToLower();

                    if (lowerField == "fałsz" || lowerField == "prawda")
                        line[j] = PolishBoolToEnglishBool(line[j]);
                }
            }
        }
    }
}