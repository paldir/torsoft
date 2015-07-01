using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    interface IPozycjaDoAnalizy
    {
        DateTime Data { get; }
        decimal Kwota { get; }
        decimal Ilość { get; }
        decimal Stawka { get; }
        int IdInformacji { get; }
        int[] KodLokalu { get; }
    }
}