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
    public class IfNotAlphabet : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isvalid = Regex.IsMatch(value.ToString(), @"^[a-zA-Z ]+$", RegexOptions.IgnoreCase);

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotAlphabet);
            else
                return ValidationResult.Success;
        }
    }
}