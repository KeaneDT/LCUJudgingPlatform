using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Models
{
    public class ValidateCompetitionName : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();

        protected override ValidationResult IsValid
            (object value, ValidationContext validationContext)
        {
            // Get the competition name to validate
            string name = Convert.ToString(value);
            // Casting the validation context to the "Competition" model class
            Competition competition = (Competition)validationContext.ObjectInstance;
            // Get the CompetitionID from the Competition instance
            int competitionID = competition.CompetitionID;

            if (competitionContext.IsNameExist(name, competitionID))
                // Validation Unsucessful.
                return new ValidationResult("Competition Name already exists");
            else
                // Validation Successful.
                return ValidationResult.Success;
        }
    }
}
