using System.ComponentModel.DataAnnotations;
using TUSO.Utilities.Constants;

/*
 * Created by: Rakib Hasan
 * Date created: 03.09.2022
 * Last modified: 03.09.2022
 * Modified by: Rakib Hasan
 */
namespace TUSO.Domain.Validators
{
    public class IfNotSelected : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (int.Parse(value.ToString()) <= 0)
                return new ValidationResult(MessageConstants.IfNotSelected);
            else
                return ValidationResult.Success;
        }
    }
}