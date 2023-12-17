using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TUSO.Utilities.Constants;

/*
 * Created by: Rakib Hasan
 * Date created: 03.09.2022
 * Last modified: 03.09.2022
 * Modified by: Rakib Hasan
 */
namespace TUSO.Domain.Validators
{
    public class IfNotValidCountryCode : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isvalid = Regex.IsMatch(value.ToString(), @"^\+?([0-9]{3}|[0-9]{2}|[0-9]{1})$");

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotCountryCode);
            else
                return ValidationResult.Success;
        }
    }
}