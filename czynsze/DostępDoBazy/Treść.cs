using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DostępDoBazy
{
    [Table("tresc", Schema = "public")]
    public class Treść
    {
        [Key]
        public int __record { get; set; }

        public string op_1 { get; set; }

        public string op_2 { get; set; }

        public string op_3 { get; set; }

        public string op_4 { get; set; }

        public string op_5 { get; set; }

        public string op_6 { get; set; }

        public string op_7 { get; set; }

        public string op_8 { get; set; }

        public string op_9 { get; set; }

        public string op_10 { get; set; }

        public string op_11 { get; set; }

        public string op_12 { get; set; }

        public string op_13 { get; set; }

        public string op_14 { get; set; }

        public string op_15 { get; set; }

        public string pu_1 { get; set; }

        public string pu_2 { get; set; }

        public string pu_3 { get; set; }

        public string pu_4 { get; set; }

        public string pu_5 { get; set; }

        public string pu_6 { get; set; }

        public string pu_7 { get; set; }

        public string pu_8 { get; set; }

        public string pu_9 { get; set; }

        public string pu_10 { get; set; }

        public string pw_1 { get; set; }

        public string pw_2 { get; set; }

        public string pw_3 { get; set; }

        public string pw_4 { get; set; }

        public string pw_5 { get; set; }

        public string pw_6 { get; set; }

        public string pw_7 { get; set; }

        public string pw_8 { get; set; }

        public string pw_9 { get; set; }

        public string pw_10 { get; set; }
    }
}