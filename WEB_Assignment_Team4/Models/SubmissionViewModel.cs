using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WEB_Assignment_Team4.Models
{
    public class SubmissionViewModel
    {
        public int CompetitorID { get; set; }
        public string Salutation { get; set; }
        [Display(Name = "Competitor Name")]
        public string CompetitorName { get; set; }
        [Display(Name = "Email")]
        public string EmailAddr { get; set; }
        public int CompetitionID { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        [Display(Name = "Upload Time")]
        public DateTime? UploadDateTime { get; set; }
        public string Appeal { get; set; }
        [Display(Name = "Vote Count")]
        public int VoteCount { get; set; }
        public double Score { get; set; }
        public int? Ranking { get; set; }
    }
}
