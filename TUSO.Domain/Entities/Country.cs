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
    /// Country Entity.
    /// </summary>
    public class Country : BaseModel
    {
        /// <summary>
        /// Primary key of the table Country.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Name of the Country.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        /// <summary>
        /// ISO Alpha-2 of a counrty.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(5)]
        [Display(Name = "ISO Alpha-2")]
        public string ISOCodeAlpha2 { get; set; }

        /// <summary>
        /// Country code of a country.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(4)]
        [MaxLength(4), MinLength(2)]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Provinces of a Country.
        /// </summary>
        public virtual IEnumerable<Province> Provinces { get; set; }   
    }
}