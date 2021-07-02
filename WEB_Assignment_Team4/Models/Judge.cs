using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class Judge
    {
        //JudgeID
        public int JudgeID { get; set; }
        //JudgeName
        [Display(Name = "Judge Name")]
        public string JudgeName { get; set; }
        public string Salutation { get; set; } //Nullable - from db
        public int AreaInterestID { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddr { get; set; }
        public string Password { get; set; }
    }
}
