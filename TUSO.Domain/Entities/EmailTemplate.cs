using System.ComponentModel.DataAnnotations;
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
    /// EmailTemplate Entity.
    /// </summary>
    public class EmailTemplate : BaseModel
    {
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Subject of the Email.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(300)]
        [Display(Name = "Email Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Body of the Email.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(2000)]
        [Display(Name = "Email Body")]
        public string MailBody { get; set; }

        /// <summary>
        /// BodyType of the Email.
        /// </summary>
        [Display(Name = "Body Type")]
        public int? BodyType { get; set; }
    }
}