using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace czynsze.DostępDoBazy
{
    [System.ComponentModel.DataAnnotations.Schema.Table("pliki_lokal", Schema = "public")]
    public class PlikLokalu : Plik
    {
    }
}