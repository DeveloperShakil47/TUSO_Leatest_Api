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
    /// Screenshot entity
    /// </summary>
    public class Screenshot : BaseModel
    {
        /// <summary>
        /// Primary key of the table Screenshot.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// User Ticket Attachment
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public Byte[] Screenshots { get; set; }

        public long IncidentId { get; set; }
        /// <summary>
        /// Foreign key. Primary key of the entity Incident. 
        /// </summary>
        [ForeignKey("IncidentId")]
        [JsonIgnore]
        public virtual Incident Incidents { get; set; }
    }
}