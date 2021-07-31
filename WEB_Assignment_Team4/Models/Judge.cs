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
        [Required(ErrorMessage = "Judge Name Required!")]
        [Display(Name = "Judge Name")]
        [StringLength(50, ErrorMessage = "Judge Name cannot be more than 50 characters!")]
        public string JudgeName { get; set; }

        //Salutation
        [StringLength(5)]
        public string Salutation { get; set; } //Nullable - from db

        //AreaInterestID
        [Required(ErrorMessage = "Area of Interest Required!")]
        [Display(Name = "Area of Interest")]
        public int? AreaInterestID { get; set; }

        //EmailAddr
        [Required(ErrorMessage = "Email Address Required!")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [ValidateJudgeExists]
        [StringLength(50, ErrorMessage = "Email length cannot be more than 50 characters!")]
        public string EmailAddr { get; set; }

        //Password
        //Default value set
        [Required(ErrorMessage = "Password Required!")]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
