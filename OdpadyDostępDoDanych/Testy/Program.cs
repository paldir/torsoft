using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OdpadyDostępDoDanych;

namespace Testy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Połączenie połączenie = new Połączenie())
            {
                połączenie.UtwórzKlasęNaPodstawieTabeli("fk_tuz", "Użytkownik");
            }
        }
    }
}