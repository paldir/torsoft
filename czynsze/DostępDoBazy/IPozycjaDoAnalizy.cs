using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    interface IPozycjaDoAnalizy
    {
        DateTime Data { get; set; }
        decimal Kwota { get; set; }
        decimal Ilość { get; set; }
        decimal Stawka { get; set; }
        int ZewnętrzneId { get; set; }
    }
}