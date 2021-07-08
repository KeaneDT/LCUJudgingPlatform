using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class Submissions
    {
        [Display(Name = "CompetitionID")]
        public int CompetitionID { get; set; }

        [Display(Name = "CompetitorID")]
        public int CompetitorID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Date and time of file upload")]
        [DataType(DataType.Date)]
        public DateTime UploadDateTime { get; set; }

        [Display(Name = "Total votes")]
        public int VoteCount { get; set; }

        [Display(Name = "Ranking")]
        public int Ranking { get; set; }
    }
}
