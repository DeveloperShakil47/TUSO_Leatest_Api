/*
 * Created by: Rakib
 * Date created: 27.09.2022
 * Last modified: 27.09.2022
 * Modified by: Rakib
 */
namespace TUSO.Domain.Dto
{
    public class IncidentSearchDto
    {
        /// <summary>
        /// DateFrom of an Incident.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// DateTo of an Incident.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity System.
        /// </summary>
        public int? SystemId { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Province.
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity District.
        /// </summary>
        public int? DistrictId { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Facility.
        /// </summary>
        public int? FacilityId { get; set; }

        /// <summary>
        /// First level category for ticket creation.
        /// </summary>        
        public int? FirstLevelCategoryId { get; set; }

        /// <summary>
        /// Second Level Category for ticket creation.
        /// </summary>
        public int? SecondLevelCategoryId { get; set; }

        /// <summary>
        /// Third Level Category for ticket creation.
        /// </summary>
        public int? ThirdLevelCategoryId { get; set; }

        /// <summary>
        /// Status of the Ticket
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Primary key of the table User Account.
        /// </summary>
        public int UserAccountId { get; set; }

        /// <summary>
        /// Primary key of the table Role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Primary key of the table Team.
        /// </summary>
        public int? TeamId { get; set; }
    }
}