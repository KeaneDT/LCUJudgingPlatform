using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WEB_Assignment_Team4.Models
{
    public class Interest
    {
        [Display(Name = "ID")]
        public int AreaInterestID { get; set; }
        // Display for Name
        [Required(ErrorMessage = "Please enter the event name")]
        public string Name { get; set; }
    }
}
