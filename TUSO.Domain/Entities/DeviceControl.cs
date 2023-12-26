using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */

namespace TUSO.Domain.Entities
{
    public class DeviceControl : BaseModel
    {
        /// <summary>
        /// Primary key of the table Sync.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Show the CPU uses percentage of the device.
        /// </summary>
        [Display(Name = "CPU Uses")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CPUUses { get; set; }

        /// <summary>
        /// Show the memory uses percentage of the device.
        /// </summary>
        [Display(Name = "Memory Uses")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MemoryUses { get; set; }
    }
}