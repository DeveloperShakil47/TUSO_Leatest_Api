using System.ComponentModel.DataAnnotations;
using TUSO.Utilities.Constants;

/*
 * Created by: Bithy
 * Date created: 4.10.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    /// <summary>
    /// DTO or View model for account login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Username for login an account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(60)]
        public string UserName { get; set; }

        /// <summary>
        /// Password for login an account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}