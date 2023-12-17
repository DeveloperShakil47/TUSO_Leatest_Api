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
    /// ModulePermission Entity.
    /// </summary>
    public class ModulePermission : BaseModel
    {
        /// <summary>
        /// Primary Key of the table ModulePermission.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Role.
        /// </summary>
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Roles { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Module.
        /// </summary>
        public int ModuleID { get; set; }

        [ForeignKey("ModuleID")]
        public virtual Module Modules { get; set; }
    }
}