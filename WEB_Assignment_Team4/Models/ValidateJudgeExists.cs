using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Models
{
    public class ValidateJudgeExists : ValidationAttribute
    {
        private JudgeDAL judgeContext = new JudgeDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validation context to the "Staff" model class
            Judge judge = (Judge)validationContext.ObjectInstance;
            // Get the Staff Id from the staff instance
            int judgeId = judge.JudgeID;

            if (judgeContext.IsJudgeExist(email, judgeId))
            {
                // validation failed
                return new ValidationResult("Email address already exists!");
            }
            else
            {
                // validation passed
                return ValidationResult.Success;
            }
        }
    }
}
