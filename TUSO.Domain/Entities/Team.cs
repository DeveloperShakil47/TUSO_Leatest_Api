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
    /// Team Entity.
    /// </summary>
    public class Team : BaseModel
    {
        /// <summary>
        /// Primary key of the table Team.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// Title of the Team
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the Team.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(500)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Members of a Team.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Member> Members { get; set; }

        /// <summary>
        /// Incidents of a Team.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Incident> Incidents { get; set; }
    }
}