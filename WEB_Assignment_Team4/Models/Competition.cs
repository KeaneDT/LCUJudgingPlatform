using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class Competition
    {
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Interest ID")]
        public int? AreaInterestID { get; set; }

        [Display(Name = "Competition Name")]
        [ValidateCompetitionNamecs]
        public string Name { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Result Release Date")]
        [DataType(DataType.Date)]
        public DateTime? ResultReleaseDate { get; set; }
    }
}
