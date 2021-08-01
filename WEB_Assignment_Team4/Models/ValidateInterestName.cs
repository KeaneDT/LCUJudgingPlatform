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
            // Get the Interest name to validate
            string name = Convert.ToString(value);
            // Casting the validation context to the "Interest" model class
            Interest interestID = (Interest)validationContext.ObjectInstance;
            // Get the AreaInterestID from the Interest instance
            int AreaInterestID = interestID.AreaInterestID;

            if (interestContext.IsInterestExist(name, AreaInterestID))
                // Validation Unsuccessful. 
                return new ValidationResult("Interest Name already exists");
            else
                // Validation Successful.
                return ValidationResult.Success;
        }
    }
}
