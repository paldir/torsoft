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
        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("op_1")]
        public string op_1 { get; set; }

        [Column("op_2")]
        public string op_2 { get; set; }

        [Column("op_3")]
        public string op_3 { get; set; }

        [Column("op_4")]
        public string op_4 { get; set; }

        [Column("op_5")]
        public string op_5 { get; set; }

        [Column("op_6")]
        public string op_6 { get; set; }

        [Column("op_7")]
        public string op_7 { get; set; }

        [Column("op_8")]
        public string op_8 { get; set; }

        [Column("op_9")]
        public string op_9 { get; set; }

        [Column("op_10")]
        public string op_10 { get; set; }

        [Column("op_11")]
        public string op_11 { get; set; }

        [Column("op_12")]
        public string op_12 { get; set; }

        [Column("op_13")]
        public string op_13 { get; set; }

        [Column("op_14")]
        public string op_14 { get; set; }

        [Column("op_15")]
        public string op_15 { get; set; }

        [Column("pu_1")]
        public string pu_1 { get; set; }

        [Column("pu_2")]
        public string pu_2 { get; set; }

        [Column("pu_3")]
        public string pu_3 { get; set; }

        [Column("pu_4")]
        public string pu_4 { get; set; }

        [Column("pu_5")]
        public string pu_5 { get; set; }

        [Column("pu_6")]
        public string pu_6 { get; set; }

        [Column("pu_7")]
        public string pu_7 { get; set; }

        [Column("pu_8")]
        public string pu_8 { get; set; }

        [Column("pu_9")]
        public string pu_9 { get; set; }

        [Column("pu_10")]
        public string pu_10 { get; set; }

        [Column("pw_1")]
        public string pw_1 { get; set; }

        [Column("pw_2")]
        public string pw_2 { get; set; }

        [Column("pw_3")]
        public string pw_3 { get; set; }

        [Column("pw_4")]
        public string pw_4 { get; set; }

        [Column("pw_5")]
        public string pw_5 { get; set; }

        [Column("pw_6")]
        public string pw_6 { get; set; }

        [Column("pw_7")]
        public string pw_7 { get; set; }

        [Column("pw_8")]
        public string pw_8 { get; set; }

        [Column("pw_9")]
        public string pw_9 { get; set; }

        [Column("pw_10")]
        public string pw_10 { get; set; }
    }
}