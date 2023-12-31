using System.ComponentModel.DataAnnotations;
using TUSO.Utilities.Constants;

/*
 * Created by: Selim
 * Date created: 01.11.2022
 * Last modified:
 * Modified by:
 */

namespace TUSO.Domain.Entities
{
    public class Sync : BaseModel
    {
        /// <summary>
        /// Primary key of the table Sync.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Synced field.
        /// </summary>
        [StringLength(60)]
        [DataType(DataType.Text)]
        [Display(Name = "Sync")]
        public string? Synced { get; set; }

        /// <summary>
        /// Show the CPU uses percentage of the device.
        /// </summary>
        [Display(Name = "CPU Uses")]
        public decimal? CPUUses { get; set; }

        /// <summary>
        /// Show the memory uses percentage of the device.
        /// </summary>
        [Display(Name = "Memory Uses")]
        public decimal? MemoryUses { get; set; }
    }
}