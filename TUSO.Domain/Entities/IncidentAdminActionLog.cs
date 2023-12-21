using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Entities
{
    public class IncidentAdminActionLog : BaseModel
    {
        /// <summary>
        /// Primary key of the table IncidentActionLog.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Incident.
        /// </summary>
        public long IncidentId { get; set; }
        [ForeignKey("IncidentId")]
        ///<summary>
        ///Navigation List
        ///</summary>
        [JsonIgnore]
        public virtual Incident Incident { get; set; }

        /// <summary>
        /// Change history of the entity Incident.
        /// </summary>
        [DataType(DataType.Text)]
        [Display(Name = "Change History")]
        public string ChangeHistory { get; set; } 
    }
}