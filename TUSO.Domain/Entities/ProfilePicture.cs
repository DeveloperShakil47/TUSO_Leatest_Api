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
    /// <summary>
    /// ProfilePicture Entity.
    /// </summary>
    public class ProfilePicture : BaseModel
    {
        // <summary>
        /// Primary key of the table ProfilePicture.
        /// </summary>
        [Key]
        public long OID { get; set; }

        /// <summary>
        /// User profile picture
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        public Byte[] ProfilePictures { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity UserAccounts. 
        /// </summary>
        [ForeignKey("OID")]
        public virtual UserAccount UserAccounts { get; set; }
    }
}