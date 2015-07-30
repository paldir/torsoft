using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    interface IRekord
    {
        int id { get; }

        void Ustaw(string[] rekord);
        string Waliduj(Enumeratory.Akcja akcja, string[] rekord);
        string[] WszystkiePola();
        string[] PolaDoTabeli();
    }
}