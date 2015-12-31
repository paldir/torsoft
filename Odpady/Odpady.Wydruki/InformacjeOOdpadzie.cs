﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odpady.Wydruki
{
    public class InformacjeOOdpadzie
    {
        public string Nazwa { get; set; }
        public string Ilość { get; set; }

        public InformacjeOOdpadzie(string nazwa, string ilość)
        {
            Nazwa = nazwa;
            Ilość = ilość;
        }
    }
}