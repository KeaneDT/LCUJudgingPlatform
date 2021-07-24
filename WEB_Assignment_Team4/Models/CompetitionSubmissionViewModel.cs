using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{
    public class CompetitionSubmissionViewModel
    {
        public List<Competition> competitionList { get; set; }
        public List<Submissions> submissionsList { get; set; }

        public CompetitionSubmissionViewModel()
        {
            competitionList = new List<Competition>();
            submissionsList = new List<Submissions>();
        }
    }
}

