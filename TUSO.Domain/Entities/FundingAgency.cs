using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// FundingAgency Entity.
    /// </summary>
    public class FundingAgency:BaseModel
    {
        /// <summary>
        /// Primary Key of the table FundingAgency.
        /// </summary>
        [Key]
        public int Oid { get; set; }

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
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        [JsonIgnore]
        public virtual Project Projects { get; set; }
    }
}