using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace importBazy
{
    class Program
    {
        static void Main(string[] args)
        {
            Import.ImportFromFile(typeof(WotuwDataAccess.Swiadczenie));

            Console.WriteLine("{0}Naciśnij klawisz, aby kontynuować...", Environment.NewLine);
            Console.ReadKey();
        }
    }
}