using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TUSO.Domain.Entities;
using TUSO.Domain.Validators;
using TUSO.Utilities.Constants;

namespace TUSO.Domain.Dto
{
    public class UserAccountDto
    {
        public string Name { get; set; }

        public string SureName { get; set; }

        public string Email { get; set; }

        public string Cellphone { get; set; }
    }
    public class UserAccountCreateDto
    {
        /// <summary>
        /// Name of the user.
        /// </summary>       
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(61)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [IfNotAlphabet]
        public string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(30)]
        [DataType(DataType.Text)]
        [Display(Name = "Surname")]
        [IfNotAlphabet]
        public string Surname { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        [StringLength(90)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [IfNotEmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Username of the account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(60)]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        /// <summary>
        /// Login password.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Password)]
        [Display(Name = "Login Password")]
        public string Password { get; set; }

        /// <summary>
        /// Country Code of the cellphone.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(30)]
        [DataType(DataType.Text)]
        [Display(Name = "Country Code")]
        //[IfNotValidCountryCode]
        public string CountryCode { get; set; }

        /// <summary>
        /// Cellphone number.
        /// </summary>        
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(15)]
        [DataType(DataType.Text)]
        [Display(Name = "Cellphone")]
        [IfNotInteger]
        public string Cellphone { get; set; }

        /// <summary>
        /// Useraccount's status active or inactive.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Active Status")]
        public bool IsAccountActive { get; set; }


        /// <summary>
        /// Foreign key. Primary key of entity Role.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public int RoleId { get; set; }
    }
}