using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class Criteria
    {
        //CriteriaID
        public int CriteriaID { get; set; }
        //CompetitionID
        public int CompetitionID { get; set; }
        //CriteriaName
        [Required(ErrorMessage = "Criteria Name is Required!")]
        [StringLength(50, ErrorMessage = "Judge Name cannot be more than 50 characters!")]
        public string CriteriaName { get; set; }
        //Weightage
        [Required(ErrorMessage = "Weightage is Required!")]
        [ValidateWeightage]
        [Range(1,100, ErrorMessage = "Criteria Weightage cannot be the integer specified!")]
        public int Weightage { get; set; }
    }
}
