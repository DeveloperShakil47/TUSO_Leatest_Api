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
    /// IncidentCategory Entity.
    /// </summary>
    public class IncidentCategory : BaseModel
    {
        // <summary>
        /// Primary key of the table IncidentCategory.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Name of the IncidentCategory.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Incident Category")]
        public string IncidentCategorys { get; set; }

        /// <summary>
        /// Purpose of the IncidentCategory.
        /// </summary>        
        [StringLength(500)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// ParentID of Incidents.
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// Incident of a IncidentCategory.
        /// </summary>
        public virtual IEnumerable<Incident> Incidents { get; set; }
    }
}