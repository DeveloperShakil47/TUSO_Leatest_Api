/*
 * Created by: Bithy
 * Date created: 4.10.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class RecoveryPasswordDto
    {
        public long UserAccountID { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public int RequestID { get; set; }
    }
}