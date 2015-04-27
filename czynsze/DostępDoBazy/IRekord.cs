using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    public interface IRekord
    {
        void Ustaw(string[] rekord);
        string Waliduj(Enums.Akcja akcja, string[] rekord);
        string[] WszystkiePola();
    }
}