using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// ImplementingPartner Entity.
    /// </summary>
    public class ImplementingPartner : BaseModel
    {
        /// <summary>
        /// Primary Key of the table ImplementingPartner.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Name of the Implementing Partner.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(200)]
        [Display(Name = "Implementing Partner Name")]
        public string ImplementingPartnerName { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Project.
        /// </summary>
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]

        public virtual Project Projects { get; set; }
    }
}