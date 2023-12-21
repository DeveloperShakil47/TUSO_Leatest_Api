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
        [JsonIgnore]
        public virtual UserAccount UserAccount { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Project.
        /// </summary>
        public int SystemId { get; set; }

        [ForeignKey("SystemId")]
        [JsonIgnore]
        public virtual Project Projects { get; set; }
    }
}