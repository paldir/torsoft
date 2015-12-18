using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czynsze.DostępDoBazy
{
    public interface IInformacjeOPozycji
    {
        int IdInformacji { get; }
        string Nazwa { get; }
        int RodzajEwidencji { get; }
        int Grupa { get; }
    }
}