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
    public class IfNotInteger : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isvalid = Regex.IsMatch(value.ToString(), @"^[0-9]+$", RegexOptions.IgnoreCase);

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotInteger);
            else
                return ValidationResult.Success;
        }
    }
}