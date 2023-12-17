using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int OID { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Role.
        /// </summary>
        public int FacilityID { get; set; }

        [ForeignKey("FacilityID")]
        public virtual Facility Facility { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Module.
        /// </summary>
        public long UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual UserAccount UserAccount  { get; set; }
    }
}