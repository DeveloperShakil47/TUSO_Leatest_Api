/*
 * Created by: Rakib
 * Date created: 4.10.2022
 * Last modified: 
 * Modified by: 
 */
using TUSO.Domain.Entities;

namespace TUSO.Domain.Dto
{
    /// <summary>
    /// Incident List DTO.
    /// </summary>
    public class IncidentListDto
    {
        /// <summary>
        /// Primary key of the table Incident.
        /// </summary>       
        public long Oid { get; set; }

        /// <summary>
        /// DateReported of an Incident.
        /// </summary>
        public DateTime DateReported { get; set; }

        /// <summary>
        /// The entry data and time of the ticket.
        /// </summary>
        public DateTime? DateOfIncident { get; set; }

        /// <summary>
        /// Description of an Incident.
        /// </summary>
        public string Description { get; set; }

        // <summary>
        /// Description of an Incident.
        /// </summary>
        public string TicketTitle { get; set; }

        /// <summary>
        ///  DateResolved of an Incident.
        /// </summary> 
        public DateTime? DateResolved { get; set; }

        /// <summary>
        /// Status of the row. It indicates the Ticket is solve or not.
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Status of the row. It indicates the Ticket is Open or Close.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity System.
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// The project/system name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Facility.
        /// </summary>
        public int FacilityId { get; set; }

        /// <summary>
        /// Name of the Facility.
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Name of the Facility.
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Name of the Facility.
        /// </summary>
        public string ProvincName { get; set; }

        /// <summary>
        /// User who capture the ticket.
        /// </summary>
        public long ReportedBy { get; set; }

        /// <summary>
        /// Fullanme of the User.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Cellphone of the Useraccount.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Foregin key. Primary key of the Team Table.
        /// </summary>       
        public long? TeamId { get; set; }

        /// <summary>
        /// Name of the Team.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// User who assign for solving ticket.
        /// </summary> 
        public long? AssignedTo { get; set; }

        public string AssignedName { get; set; }

        /// <summary>
        /// First Level Category for ticket creation.
        /// </summary>
        public int? FirstLevelCategoryId { get; set; }

        /// <summary>
        /// Second Level Category for ticket creation.
        /// </summary>
        public int? SecondLevelCategoryId { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Incident Category.
        /// </summary>
        public int? ThirdLevelCategoryId { get; set; }

        /// <summary>
        /// Foregin key. Primary key of the Incident Priority Table.
        /// </summary>
        public int? PriorityId { get; set; }

        /// <summary>
        /// Check the image has or not.
        /// </summary> 
        public bool HasImg { get; set; }

        /// <summary>
        /// Is the ticket reassigned to the expert or not.
        /// </summary>
        public bool IsReassigned { get; set; }

        /// <summary>
        /// The name of the caller who called for creating ticket.
        /// </summary>
        public string? CallerName { get; set; }

        /// <summary>
        /// The caller cell phone number.
        /// </summary>
        public string? CallerCellphone { get; set; }

        /// <summary>
        /// The caller email.
        /// </summary>
        public string? CallerEmail { get; set; }

        /// <summary>
        /// The Caller Country Code.
        /// </summary>
        public string? CallerCountryCode { get; set; }

        /// <summary>
        /// The caller job title.
        /// </summary>
        public string? CallerJobTitle { get; set; }

        /// <summary>
        /// The first level category of the ticket.
        /// </summary>
        public string FirstLevelCategory { get; set; }

        /// <summary>
        /// The second level category of the ticket.
        /// </summary>
        public string SecondLevelCategory { get; set; }

        /// <summary>
        /// The third level category of the ticket.
        /// </summary>
        public string ThirdLevelCategory { get; set; }

        /// <summary>
        /// To whom reassign the ticket.
        /// </summary>
        public long? ReassignedTo { get; set; }

        /// <summary>
        /// The reassign time.
        /// </summary
        public DateTime? ReassignDate { get; set; }

        /// <summary>
        /// The Created time.
        /// </summary
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// The Updated time.
        /// </summary
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// The user created incident record.
        /// </summary
        public long? CreatedBy { get; set; }

        /// <summary>
        /// The user modified incident record.
        /// </summary
        public long? ModifiedBy { get; set; }

        /// <summary>
        /// The system funding agency name.
        /// </summary 
        public string FundingAgencyName { get; set; }

        /// <summary>
        /// The system implementing partner name.
        /// </summary 
        public string ImplementingPartnerName { get; set; }

        public virtual IEnumerable<FundingAgencyItem> FundingAgencyItems { get; set; }

        public virtual IEnumerable<ImplemenentingItem> ImplemenentingItems { get; set; }
    }
}