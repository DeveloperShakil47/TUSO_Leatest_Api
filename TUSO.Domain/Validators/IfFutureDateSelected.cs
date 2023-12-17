using System.ComponentModel.DataAnnotations;
using TUSO.Utilities.Constants;

/*
 * Created by: Sakhawat Hossain
 * Date created: 17.09.2022
 * Last modified: 17.09.2022
 * Modified by: Sakhawat Hossain
 */
namespace TUSO.Domain.Validators
{
   public class DateValidatorAttribute : ValidationAttribute
   {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         try
         {
            DateTime date = (DateTime)value;
            if (date > DateTime.Now)
               return new ValidationResult(MessageConstants.IfFutureDateSelected);
            else
               return ValidationResult.Success;

         }
         catch (Exception ex)
         {
            return new ValidationResult(ex.Message);
         }
      }
   }
}