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
    /// Holds all the login recovery requests.
    /// </summary>
    public class RecoveryRequest : BaseModel
    {
        /// <summary>
        /// Primary key of the table RecoveryRequests.
        /// </summary>
        [Key]
        public long OID { get; set; }

        /// <summary>
        /// Cellphone number of the user.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(15)]
        [Display(Name = "Cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        /// Username of the user-account.
        /// </summary>
        [StringLength(30)]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        /// <summary>
        /// Date of recovery request.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Date Requested")]
        [Column(TypeName = "smalldatetime")]
        public DateTime DateRequested { get; set; }

        /// <summary>
        /// Describes the recovery request is sorted or not.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Is RequestOpen")]
        public bool IsRequestOpen { get; set; }

        /// <summary>
        ///Foreign key, referance of UserAccounts table.
        /// </summary>        
        public long UserAccountID { get; set; }

        [ForeignKey("UserAccountID")]
        public virtual UserAccount UserAccounts { get; set; }
    }
}