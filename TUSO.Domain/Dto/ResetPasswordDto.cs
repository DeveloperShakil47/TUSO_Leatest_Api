using System.ComponentModel.DataAnnotations;

/*
 * Created by: Bithy
 * Date created: 04.09.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}