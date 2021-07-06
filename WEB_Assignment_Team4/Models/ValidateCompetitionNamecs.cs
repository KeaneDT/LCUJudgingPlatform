using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Models
{
    public class ValidateCompetitionNamecs : ValidationAttribute
    {
        private CompetitionDAL competitionContext = new CompetitionDAL();

        protected override ValidationResult IsValid
            (object value, ValidationContext validationContext)
        {
            //
            string name = Convert.ToString(value);
            //
            Competition competition = (Competition)validationContext.ObjectInstance;
            int competitionID = competition.CompetitionID;

            if (competitionContext.IsNameExist(name, competitionID))
                //
                return new ValidationResult("Competition Name already exists");
            else
                //
                return ValidationResult.Success;
        }
    }
}
