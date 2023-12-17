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
    /// FundingAgency Entity.
    /// </summary>
    public class FundingAgency:BaseModel
    {
        /// <summary>
        /// Primary Key of the table FundingAgency.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Name of the Funding Agency.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(200)]
        [Display(Name = "Funding Agency Name")]
        public string FundingAgencyName { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Project.
        /// </summary>
        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Projects { get; set; }
    }
}