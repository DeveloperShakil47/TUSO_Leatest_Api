﻿using System.ComponentModel.DataAnnotations;
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
    /// Province Entity.
    /// </summary>
    public class Province : BaseModel
    {
        /// <summary>
        /// Primary key of the table Province.
        /// </summary>
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Name of the Province.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Province Name")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity Country. 
        /// </summary>
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        [JsonIgnore]
        public virtual Country Countries { get; set; }

        /// <summary>
        /// Districts of a Province.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<District> Districts { get; set; }
    }
}