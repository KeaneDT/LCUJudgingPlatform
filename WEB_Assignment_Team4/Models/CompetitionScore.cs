using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class CompetitionScore
    {
       
        public int CriteriaID { get; set; }
        public int CompetitorID { get; set; }
        public int CompetitionID { get; set; }
        [Range(0,10, ErrorMessage ="Score Range from 0 to 10!")]
        public int Score { get; set; }

    }
}
