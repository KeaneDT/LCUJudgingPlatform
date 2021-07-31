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
            //
            int name = Convert.ToInt32(value);
            //
            JudgeAssign competition = (JudgeAssign)validationContext.ObjectInstance;
            int competitionID = competition.CompetitionID;

            if (JudgeContext.IsCompetitionJudgeExist(name, competitionID))
                //
                return new ValidationResult("Judge Assigned Competition record already exists");
            else
                //
                return ValidationResult.Success;
        }
    }
}
