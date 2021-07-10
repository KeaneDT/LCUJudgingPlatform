using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class Competitor
    {
        //CompetitorID
        public int CompetitorID { get; set; }

        //JudgeName
        [Required(ErrorMessage = "Competitor Name Required!")]
        [Display(Name = "Competitor Name")]
        [StringLength(50, ErrorMessage = "Competitor Name cannot be more than 50 characters!")]
        public string CompetitorName { get; set; }

        //Salutation
        [StringLength(5)]
        public string Salutation { get; set; } //Nullable - from db


        //EmailAddr
        [Required(ErrorMessage = "Email Address Required!")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [ValidateCompetitorExists]
        [StringLength(50, ErrorMessage = "Email length cannot be more than 50 characters!")]
        public string EmailAddr { get; set; }

        //Password
        //Default value set
        [Required(ErrorMessage = "Password Required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "Password must be atleast 6 characters long!", MinimumLength = 6)]
        public string Password { get; set; }
    }
}

