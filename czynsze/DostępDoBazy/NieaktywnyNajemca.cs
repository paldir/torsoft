using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("najemc_a", Schema = "public")]
    public class NieaktywnyNajemca : Najemca
    {

    }
}