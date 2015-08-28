using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    [System.ComponentModel.DataAnnotations.Schema.Table("pliki_budynek", Schema = "public")]
    public class PlikBudynku : Plik
    {
    }
}