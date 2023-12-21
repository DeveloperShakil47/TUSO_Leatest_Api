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
    /// Message Entity.
    /// </summary>
    public class Message : BaseModel
    {
        /// <summary>
        /// Primary key of the table Message.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// Message Date date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Message Date")]
        public DateTime MessageDate { get; set; }

        /// <summary>
        /// Name of the Message.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(200)]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Messages { get; set; }

        /// <summary>
        /// Name of the Message.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(91)]
        [DataType(DataType.Text)]
        [Display(Name = "Sender")]
        public string Sender { get; set; }

        /// <summary>
        /// IsOpen is a status of the Message.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Message Date date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Open Date")]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Incident. 
        /// </summary>
        public long IncidentId { get; set; }

        [ForeignKey("IncidentId")]
        [JsonIgnore]
        public virtual Incident Incident { get; set; }
    }
}