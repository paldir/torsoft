using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    interface IInformacjeOPozycji
    {
        int Id { get; }
        string Nazwa { get; }
        int RodzajEwidencji { get; }
    }
}