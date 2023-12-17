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
    public class IfNotEmailAddress : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if(value==null||value=="") return ValidationResult.Success; 
            bool isvalid = Regex.IsMatch(value.ToString(), @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", RegexOptions.IgnoreCase);

            if (!isvalid)
                return new ValidationResult(MessageConstants.IfNotEmailAddress);
            else
                return ValidationResult.Success;
        }
    }
}