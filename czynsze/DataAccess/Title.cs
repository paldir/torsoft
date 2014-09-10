using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace czynsze.DataAccess
{
    [Table("tyt_praw", Schema = "public")]
    public class Title
    {
        [Key, Column("kod_praw")]
        public int kod_praw { get; set; }

        [Column("tyt_prawny")]
        public string tyt_prawny { get; set; }

        public string[] ImportantFields()
        {
            return new string[] 
            { 
                kod_praw.ToString(), 
                tyt_prawny 
            };
        }
    }
}