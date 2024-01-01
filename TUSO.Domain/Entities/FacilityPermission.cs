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
    /// <summary>
    /// FacilityPermission Entity.
    /// </summary>
    public class FacilityPermission : BaseModel
    {
        // <summary>
        /// Primary Key of the table FacilityPermission.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Role.
        /// </summary>
        public int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        [JsonIgnore]
        public virtual Facility Facility { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Module.
        /// </summary>
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual UserAccount UserAccount  { get; set; }

        public bool IsITExpart {  get; set; }
    }
}