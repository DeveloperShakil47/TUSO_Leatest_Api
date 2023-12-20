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
    /// Project Entity.
    /// </summary>
    public class Project : BaseModel
    {
        // <summary>
        /// Primary key of the table Project.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Title of the project.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        public string Title { get; set; }

        /// <summary>
        /// Purpose of the system
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// SystemPermissions of a System.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<SystemPermission> SystemPermissions { get; set; }

        /// <summary>
        /// Incidents of a System.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Incident> Incidents { get; set; }

        /// <summary>
        /// Funding Agencies of a Projects.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FundingAgency> FundingAgencies { get; set; }

        /// <summary>
        /// Implementing Partners of a Projects.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<ImplementingPartner> ImplementingPartners { get; set; }
    }
}