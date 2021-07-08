using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB_Assignment_Team4.DAL;

namespace WEB_Assignment_Team4.Models
{
    public class ValidateInterestName : ValidationAttribute
    {
        private InterestDAL interestContext = new InterestDAL();
        protected override ValidationResult IsValid
            (object value, ValidationContext validationContext)
        {
            //
            string name = Convert.ToString(value);
            //
            Interest interestID = (Interest)validationContext.ObjectInstance;
            int AreaInterestID = interestID.AreaInterestID;

            if (interestContext.IsInterestExist(name, AreaInterestID))
                //
                return new ValidationResult("Interest Name already exists");
            else
                //
                return ValidationResult.Success;
        }
    }
}
