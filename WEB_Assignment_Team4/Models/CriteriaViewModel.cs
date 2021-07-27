using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class CriteriaViewModel
    {
        public int CompetitionID { get; set; }
        public int CompetitorID { get; set; }
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public int Weightage { get; set; }
        public int Score { get; set; }
    }
}
