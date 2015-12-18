using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naprawaPeseli
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<WotuwDataAccess.Pacjent> patients = WotuwDataAccess.Pacjent.All.Where(p => p.dataUrodzenia != null && p.dataUrodzenia >= new DateTime(2000, 1, 1) && !String.IsNullOrEmpty(p.pesel) && p.pesel.Trim().Length < 11);
            int repaired = 0;

            Console.WriteLine("{0} peseli wymaga naprawy.", patients.Count());
            System.Threading.Thread.Sleep(1000);

            foreach (WotuwDataAccess.Pacjent patient in patients)
            {
                string year = patient.dataUrodzenia.Value.Year.ToString().Substring(2, 2);
                int leadingZeros = 0;
                string pesel = patient.pesel.Trim();

                foreach (char character in year)
                    if (character == '0')
                        leadingZeros++;
                    else
                        break;

                if (leadingZeros == 11 - pesel.Length)
                {
                    string tmpPesel = new String('0', leadingZeros) + pesel;

                    if (tmpPesel.StartsWith(year))
                    {
                        WotuwDataAccess.Pacjent patientToModify = WotuwDataAccess.Pacjent.Find(patient.Id);
                        patientToModify.pesel = tmpPesel;
                        repaired++;

                        patientToModify.Update();
                    }
                }
            }

            Console.WriteLine("Naprawiono {0} peseli.", repaired);
            Console.WriteLine("\nNaciśnij klawisz, aby kontynuować...");
            Console.ReadKey();
        }
    }
}