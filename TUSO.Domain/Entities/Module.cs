using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    /// Module Entity.
    /// </summary>
    public class Module : BaseModel
    {
        /// <summary>
        /// Primary key of the table Module.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the Module
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }

        /// <summary>
        /// Description of the Module.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(500)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// ModulePermissions of a Module.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ModulePermission> ModulePermissions { get; set; }
    }
}