using System.ComponentModel.DataAnnotations;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Entities
{
    /// <summary>
    /// UserType Entity.
    /// </summary>
    public class DeviceType : BaseModel
    {
        /// <summary>
        /// Primary key of the table UserTypes.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the UserTypes.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "DeviceTypeName")]
        public string DeviceTypeName { get; set; }

        /// <summary>
        /// Navigation Property
        /// </summary>
        public virtual IEnumerable<UserAccount> UserAccounts { get; set; }

    }
}