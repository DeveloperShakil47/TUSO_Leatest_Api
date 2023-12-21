using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    /// District Entity.
    /// </summary>
    public class District : BaseModel
    {
        /// <summary>
        /// Primary key of the table District.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the District.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "District Name")]
        public string DistrictName { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Province.
        /// </summary>
        public int ProvinceId { get; set; }

        [ForeignKey("ProvinceId")]
        [JsonIgnore]
        public virtual Province Provinces { get; set; }

        /// <summary>
        /// Facilities of a District.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Facility> Facilities { get; set; }


        /// <summary>
        /// This field in not insert.
        /// </summary>
        [NotMapped]
        public int CountryId { get; set; }

    }
}