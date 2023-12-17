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
    /// ProjectPermission Entity.
    /// </summary>
    public class SystemPermission : BaseModel
    {
        // <summary>
        /// Primary key of the table ProjectPermission.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Role.
        /// </summary>
        public long UserAccountID { get; set; }

        [ForeignKey("UserAccountID")]
        public virtual UserAccount UserAccount { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Project.
        /// </summary>
        public int SystemID { get; set; }

        [ForeignKey("SystemID")]
        public virtual Project Projects { get; set; }
    }
}