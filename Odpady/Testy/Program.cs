using System.Diagnostics;
using System.Linq;
using Odpady.Wydruki;

namespace Testy
{
    internal class Program
    {
        private static void Main()
        {
            /*Użytkownik u = new Użytkownik();
            string hasło = Console.ReadLine();

            u.HASLO = new string(hasło.Select(znak => Convert.ToChar((Convert.ToByte(znak) - 10 - 32 + 95)%95 + 32)).ToArray());*/

            foreach (var proces in Process.GetProcessesByName("AcroRd32"))
                proces.Kill();

            Wydruk.Kpo();
        }
    }
}