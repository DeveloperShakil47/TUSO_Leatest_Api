using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TUSO.Utilities.Constants;
using static TUSO.Utilities.Constants.Enums;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Entities
{
    /// <summary>
    /// Facility Entity.
    /// </summary>
    public class Facility : BaseModel
    {
        /// <summary>
        /// Primary Key of the table Facility.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the Facility.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }

        /// <summary>
        /// The field stores the Facility Master Code of a facility.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(60)]
        [Display(Name = "Facility Master Code")]
        public string FacilityMasterCode { get; set; }

        /// <summary>
        /// The field stores the HMIS Code of a facility.
        /// </summary>
        [StringLength(60)]
        [Display(Name = "HMISCode")]
        public string HMISCode { get; set; }


        /// <summary>
        /// The field stores the Longitude of a facility.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        /// <summary>
        /// The field stores the Latitude  of a facility.
        /// </summary>
        [StringLength(20)]
        [Display(Name = "Latitude ")]
        public string Latitude { get; set; }

        /// <summary>
        /// The field stores the Location of a facility.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Location ")]
        public Location Location { get; set; }

        /// <summary>
        /// The field stores the Facility Type of a facility.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Facility Type  ")]
        public FacilityType FacilityType { get; set; }

        /// <summary>
        /// The field stores the Ownership of a facility.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Ownership  ")]
        public Ownership Ownership { get; set; }

        /// <summary>
        /// The field stores the Ownership type of a facility.
        /// </summary>
        [Display(Name = "Ownership type  ")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity District.
        /// </summary>
        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District Districts { get; set; }

        /// <summary>
        /// Incidents of a Facility.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Incident> Incidents { get; set; }

        /// <summary>
        /// UserAccounts of a Facility.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// UserAccounts of a Role.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<FacilityPermission> FacilityPermissions { get; set; }
    }
}