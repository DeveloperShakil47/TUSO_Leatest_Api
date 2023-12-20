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
    /// Role Entity.
    /// </summary>
    public class Role : BaseModel
    {
        /// <summary>
        /// Primary key of the table Role.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the Role.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        /// <summary>
        /// Description of the role.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(500)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// UserAccounts of a Role.
        /// </summary>
        public virtual IEnumerable<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// ModulePermissions of a Role.
        /// </summary>
        public virtual IEnumerable<ModulePermission> ModulePermissions { get; set; }
    }
}