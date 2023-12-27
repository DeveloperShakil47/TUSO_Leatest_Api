using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TUSO.Domain.Entities;
using TUSO.Domain.Validators;
using TUSO.Utilities.Constants;

namespace TUSO.Domain.Dto
{
    public class IncidentCreateDto
    {
     
        /// <summary>
        /// DateofIncident of an Incident.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "smalldatetime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Date of Incident")]
        public DateTime? DateOfIncident { get; set; }

        /// <summary>
        /// DateReported of an Incident.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Column(TypeName = "smalldatetime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Date Reported")]
        public DateTime DateReported { get; set; }

        /// <summary>
        /// Description of an Incident.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// TicketTitle of an Incident.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(90)]
        [DataType(DataType.Text)]
        [Display(Name = "Ticket Title")]
        public string TicketTitle { get; set; }

        /// <summary>
        /// Status of the row. It indicates the Ticket is solve or not from expert.
        /// </summary>
        [Display(Name = "Resolved Request")]
        public bool ResolvedRequest { get; set; }

        /// <summary>
        ///  DateResolved of an Incident.
        /// </summary>       
        [Column(TypeName = "smalldatetime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Date Resolved")]
        public DateTime? DateResolved { get; set; }

        /// <summary>
        /// Status of the row. It indicates the Ticket is solve or not.
        /// </summary>
        [Display(Name = "IsResolved")]
        public bool IsResolved { get; set; }

        /// <summary>
        /// Status of the row. It indicates the Ticket is Open or Close.
        /// </summary>
        [Display(Name = "IsOpen")]
        public bool IsOpen { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity System.
        /// </summary>
        public int SystemId { get; set; }

        [ForeignKey("SystemId")]
        public virtual Project Projects { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Facility.
        /// </summary>
        public int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        [JsonIgnore]
        public virtual Facility Facilities { get; set; }

        /// <summary>
        /// User who capture the ticket.
        /// </summary>
        [Display(Name = "Reported By")]
        public long ReportedBy { get; set; }

        [ForeignKey("ReportedBy")]
        [JsonIgnore]
        public virtual UserAccount UserAccounts { get; set; }

        /// <summary>
        /// Foregin key. Primary key of the Team Table.
        /// </summary>
        [Display(Name = "Team")]

        public long? TeamId { get; set; }

        [ForeignKey("TeamId")]
        [JsonIgnore]
        public virtual Team? Teams { get; set; }

        /// <summary>
        /// User who assign for solving ticket.
        /// </summary>      
        [Display(Name = "AssignedTo")]
        public long? AssignedTo { get; set; }

        /// <summary>
        /// User who reassign for Resolving ticket.
        /// </summary>      
        [Display(Name = "Reassigned")]
        public long? ReassignedTo { get; set; }

        /// <summary>
        /// User who assign for solving ticket for the first time.
        /// </summary>      
        [Display(Name = "Assign state")]
        public long? AssignedToState { get; set; }

        /// <summary>
        /// Reassaigned date of an Incident.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Reassign Date")]
        public DateTime? ReassignDate { get; set; }

        /// <summary>
        /// Name of the Caller.
        /// </summary>
        [StringLength(60)]
        [DataType(DataType.Text)]
        [Display(Name = "Name of Caller")]
        public string? CallerName { get; set; }

        /// <summary>
        /// Country Code of the Caller cellphone.
        /// </summary>

        [DataType(DataType.Text)]
        [StringLength(4)]
        [MaxLength(4), MinLength(2)]
        [Display(Name = "Caller Country Code")]
        public string? CallerCountryCode { get; set; }

        /// <summary>
        ///caller Cellphone number.
        /// </summary>        
        [StringLength(11)]
        [DataType(DataType.Text)]
        [Display(Name = "Caller Phone No")]
        public string? CallerCellphone { get; set; }

        /// <summary>
        /// Email of the Caller.
        /// </summary>
        [StringLength(60)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Caller Email")]
        [IfNotEmailAddress]
        public string? CallerEmail { get; set; }

        /// <summary>
        /// Job description of the Caller.
        /// </summary>
        [StringLength(60)]
        [DataType(DataType.Text)]
        [Display(Name = "Caller Job Title")]
        public string? CallerJobTitle { get; set; }

        /// <summary>
        /// Status of the row. It indicates the Ticket is reassigned or not.
        /// </summary>
        [Display(Name = "Is Re-assign")]
        public bool IsReassigned { get; set; }

        /// <summary>
        /// First Level Category for ticket creation.
        /// </summary>
        [Display(Name = "FirstLevelCategory")]
        public int? FirstLevelCategoryId { get; set; }

        /// <summary>
        /// Second Level Category for ticket creation.
        /// </summary>
        [Display(Name = "SecondLevelCategory")]
        public int? SecondLevelCategoryId { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the table Incident Category.
        /// </summary>
        [Display(Name = "ThirdLevelCategory")]
        public int? ThirdLevelCategoryId { get; set; }

   


        [NotMapped]
        public long? AgentId { get; set; }

        [NotMapped]
        public DateTime? AgentDateModified { get; set; }

        [NotMapped]
        public long? SupervisedId { get; set; }

        [NotMapped]
        public DateTime? SupervisedDateModified { get; set; }

        [NotMapped]
        public long? ExpertId { get; set; }

        [NotMapped]
        public DateTime? ExpertDateModified { get; set; }

        [NotMapped]
        public long? AdminId { get; set; }

        [NotMapped]
        public DateTime? AdminDateModified { get; set; }

        [NotMapped]
        public long? TeamLeadId { get; set; }

        [NotMapped]
        public DateTime? TeamLeadDateModified { get; set; }

        [NotMapped]
        public long? CloseUserAccountId { get; set; }

        [NotMapped]
        public DateTime? DateClosed { get; set; }
    }
}
