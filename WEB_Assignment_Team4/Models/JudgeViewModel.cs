using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{
    public class JudgeViewModel
    {
        public int JudgeID { get; set; }

        //JudgeName
        [Display(Name = "Judge Name")]
        public string JudgeName { get; set; }

        //Salutation
        [Display(Name = "Judge Salutation")]
        public string Salutation { get; set; }

        //AreaInterestID
        [Display(Name = "Area of Interest")]
        public string AreaInterestName { get; set; }

        //EmailAddr
        [Display(Name = "Email Address")]
        public string EmailAddr { get; set; }

        //Password
        //Default value set
        [Display(Name ="Password")]
        public string Password { get; set; }
    }
}
