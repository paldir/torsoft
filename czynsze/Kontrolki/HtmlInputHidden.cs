﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.Kontrolki
{
    public class HtmlInputHidden : System.Web.UI.HtmlControls.HtmlInputHidden
    {
        public HtmlInputHidden(string id, object wartość)
        {
            ID = id;
            Value = wartość.ToString();
        }
    }
}