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
    /// IncidentPriority Entity.
    /// </summary>
    public class IncidentPriority : BaseModel
    {
        /// <summary>
        /// Primary key of the table IncidentPriority.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Name of the IncidentPriority.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Incident Priority")]
        public string Priority { get; set; }

        /// <summary>
        /// Incidents of a IncidentPriority.
        /// </summary>
        public virtual IEnumerable<Incident> Incidents { get; set; }
    }
}