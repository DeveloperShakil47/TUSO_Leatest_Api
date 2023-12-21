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
    /// ModulePermission Entity.
    /// </summary>
    public class ModulePermission : BaseModel
    {
        /// <summary>
        /// Primary Key of the table ModulePermission.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Role.
        /// </summary>
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        [JsonIgnore]
        public virtual Role Roles { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Module.
        /// </summary>
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        [JsonIgnore]
        public virtual Module Modules { get; set; }
    }
}