using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{
    public class CompetitionCriteriaViewModel
    {
        public List<Competition> competitionList { get; set; }
        public List<Criteria> criteriaList { get; set; }

        public CompetitionCriteriaViewModel()
        {
            competitionList = new List<Competition>();
            criteriaList = new List<Criteria>();
        }
    }
}
