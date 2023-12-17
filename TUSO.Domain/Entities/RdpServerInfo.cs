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
    /// RdpServerInfo Entity.
    /// </summary>
    public class RdpServerInfo:BaseModel
    {
        /// <summary>
        /// Primary key of the table RdpServerInfo.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// ServerURL of the RdpServerInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Server URL")]
        public string ServerURL { get; set; }

        /// <summary>
        /// OrganizationID of the RdpServerInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Organization ID")]
        public string OrganizationID { get; set; }
    }
}