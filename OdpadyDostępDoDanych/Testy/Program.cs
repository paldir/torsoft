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
                Kontrahent k = new Kontrahent();
                k.KOD_POCZTOWY = "87-500";
                k.MIASTO = "Rypin";
                k.ULICA = "Lubicka";
                k.NR_DOMU = 23;

                int kod = połączenie.Aktualizuj(k.ID, k);
            }
        }
    }
}