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
        [Required]
        [StringLength(50)]
        public string JudgeName { get; set; }
        //Salutation
        [StringLength(5)]
        public string? Salutation { get; set; } //Nullable - from db
        //AreaInterestID
        public int AreaInterestID { get; set; }
        //EmailAddr
        [Display(Name = "Email Address")]
        [EmailAddress]
        [StringLength(50)]
        public string EmailAddr { get; set; }
        //Password
        [StringLength(255)]
        public string Password { get; set; }
    }
}
