using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naprawaPolskichZnaków
{
    class Program
    {
        static void Main(string[] args)
        {
            WotuwDataAccess.WotuwEntities db = WotuwDataAccess.Record.DataBase;
            int i = 0;

            using (System.IO.StreamReader sr = new System.IO.StreamReader("PacjenciD.txt"))
                while (!sr.EndOfStream)
                {
                    List<string> lineTmp = sr.ReadLine().Split('\t').ToList();
                    long id;
                    i++;

                    lineTmp.RemoveAt(0);
                    Console.WriteLine("Analiza rekordu nr {0}.", i);

                    string[] line = lineTmp.ToArray();

                    if (Int64.TryParse(line[0], out id))
                    {
                        WotuwDataAccess.Pacjent tmpPacjent = new WotuwDataAccess.Pacjent();
                        tmpPacjent.Id = id;

                        PrepareLine(line);
                        tmpPacjent.Set(line);

                        WotuwDataAccess.Pacjent pacjent = WotuwDataAccess.Pacjent.Find(id);

                        if (pacjent != null)
                        {
                            pacjent.nazwisko = tmpPacjent.nazwisko;
                            pacjent.imie = tmpPacjent.imie;
                            pacjent.imieOjca = tmpPacjent.imieOjca;
                            pacjent.miejscowosc = tmpPacjent.miejscowosc;
                            pacjent.ulica = tmpPacjent.ulica;
                            pacjent.poczta = tmpPacjent.poczta;
                        }
                    }
                }

            Console.WriteLine("Zapisywanie zmian...");
            db.SaveChanges();
            Console.WriteLine("Naciśnij klawisz, aby kontynuować...");
            Console.ReadKey();
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
