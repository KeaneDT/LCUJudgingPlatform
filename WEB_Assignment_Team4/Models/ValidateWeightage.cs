using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Models
{
    public class ValidateWeightage : ValidationAttribute
    {
        private CriteriaDAL criteriaContext = new CriteriaDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int weightage = Convert.ToInt32(value);
            Criteria criteria = (Criteria)validationContext.ObjectInstance;
            int competitionID = criteria.CompetitionID;

            if (criteriaContext.GetCriteriaTotal(competitionID) + weightage > 100)
            {
                return new ValidationResult
                    ("Weightage cannot be more than 100%!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
