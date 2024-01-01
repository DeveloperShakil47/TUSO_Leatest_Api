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
    /// RDPDeviceInfo Entity.
    /// </summary>
    public class RDPDeviceInfo : BaseModel
    {
        /// <summary>
        /// Primary key of the table RDPDeviceInfo.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// UserName of the RDPDeviceInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        /// <summary>
        /// DeviceID of the RDPDeviceInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Device Id")]
        public string DeviceId { get; set; }

        /// <summary>
        /// PrivateIP of the RDPDeviceInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Private IP")]
        public string PrivateIp { get; set; }

        /// <summary>
        /// MACAddress of the RDPDeviceInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "MAC Address")]
        public string MACAddress { get; set; }

        /// <summary>
        /// MotherBoardSerial of the RDPDeviceInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Mother Boad Serial")]
        public string MotherBoardSerial { get; set; }

        /// <summary>
        /// PublicIP of the RDPDeviceInfo.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(100)]
        [DataType(DataType.Text)]
        [Display(Name = "Public IP")]
        public string PublicIp { get; set; }

        /// <summary>
        /// This field in not insert
        /// </summary>
        [NotMapped]
        public string FacilityName { get; set; }

        /// <summary>
        /// This field in not insert
        /// </summary>
        [NotMapped]
        public string DistrictName { get; set; }

        /// <summary>
        /// This field in not insert
        /// </summary>
        [NotMapped]
        public string ProvinceName { get; set; }

        /// <summary>
        /// This field in not insert
        /// </summary>
        [NotMapped]
        public string UserTypeName { get; set; }

    }
}