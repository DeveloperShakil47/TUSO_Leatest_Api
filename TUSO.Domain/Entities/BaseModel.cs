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
    /// Base properties of the model classes.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Creation date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Reference of the user who has created the row.
        /// </summary>
        public long? CreatedBy { get; set; }

        /// <summary>
        /// Last modification date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// Reference of the user who has last modified the row.
        /// </summary>
        public long? ModifiedBy { get; set; }

        /// <summary>
        /// Status of the row. It indicates the row is deleted or not.
        /// </summary>
        [Display(Name = "Row Status")]
        public bool IsDeleted { get; set; }
    }
}