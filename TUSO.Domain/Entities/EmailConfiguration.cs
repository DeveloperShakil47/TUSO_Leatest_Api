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
    /// EmailConfiguration Entity.
    /// </summary>
    public class EmailConfiguration:BaseModel
    {
        /// <summary>
        /// Primary Key of the table EmailConfiguration.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the Domain.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Domain Name")]
        public string DomainName { get; set; }

        /// <summary>
        /// Name of the Email.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Password of the email.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Name of the SMTP Server.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "SMTP Server")]
        public string SMTPServer { get; set; }

        /// <summary>
        /// SSL/TLS Port.
        /// </summary>
        [StringLength(90)]
        [Display(Name = "SSL/TLS Port")]
        public string? Port { get; set; }

        /// <summary>
        /// Name of the audit mail.
        /// </summary>
        [StringLength(200)]
        [Display(Name = "Audit Email Address")]
        public string? Auditmails { get; set; }
    }
}