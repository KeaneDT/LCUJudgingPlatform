using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Models
{
    public class ValidateCompetitionJudge : ValidationAttribute
    {
        private JudgeDAL JudgeContext = new JudgeDAL();

        protected override ValidationResult IsValid
            (object value, ValidationContext validationContext)
        {
            // Get the competition judgeID to validate
            int name = Convert.ToInt32(value);
            // Casting the validation context to the "JudgeAssign" model class
            JudgeAssign competition = (JudgeAssign)validationContext.ObjectInstance;
            // Get the CompetitionID from the JudgeAssign instance
            int competitionID = competition.CompetitionID;

            if (JudgeContext.IsCompetitionJudgeExist(name, competitionID))
                // Validation Unsuccessful.
                return new ValidationResult("Judge Assigned Competition record already exists");
            else
                // Validation Successful.
                return ValidationResult.Success;
        }
    }
}
