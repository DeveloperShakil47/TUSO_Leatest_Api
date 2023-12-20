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
        public int Oid { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Role.
        /// </summary>
        public long UserAccountId { get; set; }

        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccount { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Project.
        /// </summary>
        public int SystemId { get; set; }

        [ForeignKey("SystemId")]
        public virtual Project Projects { get; set; }
    }
}