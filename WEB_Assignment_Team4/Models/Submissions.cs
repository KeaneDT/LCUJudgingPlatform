using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.Models
{
    public class Submissions
    {
        [Display(Name = "CompetitionID")]
        public int? CompetitionID { get; set; }

        [Display(Name = "CompetitorID")]
        public int CompetitorID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Date and time of file upload")]
        [DataType(DataType.Date)]
        public DateTime? UploadDateTime { get; set; }

        [StringLength(255)]
        public string Appeal { get; set; }

        [Display(Name = "Total votes")]
        public int VoteCount { get; set; }

        [Display(Name = "Ranking")]
        public int? Ranking { get; set; }

        public List<Submissions> submissionsList { get; set; }
        public List<Submissions> leaderboardSubmissionsList { get; set; }
        public Submissions()
        {
            submissionsList = new List<Submissions>();
            leaderboardSubmissionsList = new List<Submissions>();
        }
    }
}
