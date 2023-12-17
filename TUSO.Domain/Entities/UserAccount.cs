using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TUSO.Domain.Validators;
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
    /// UserAccount Entity.
    /// </summary>
    public class UserAccount : BaseModel
    {
        /// <summary>
        /// Primary Key of the table UserAccounts.
        /// </summary>
        [Key]
        public long OID { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>       
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(61)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [IfNotAlphabet]
        public string Name { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(30)]
        [DataType(DataType.Text)]
        [Display(Name = "Surname")]
        [IfNotAlphabet]
        public string Surname { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        [StringLength(90)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [IfNotEmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Username of the account.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(60)]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        /// <summary>
        /// Login password.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [DataType(DataType.Password)]
        [Display(Name = "Login Password")]
        public string Password { get; set; }

        /// <summary>
        /// Country Code of the cellphone.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(30)]
        [DataType(DataType.Text)]
        [Display(Name = "Country Code")]
        //[IfNotValidCountryCode]
        public string CountryCode { get; set; }

        /// <summary>
        /// Cellphone number.
        /// </summary>        
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [StringLength(15)]
        [DataType(DataType.Text)]
        [Display(Name = "Cellphone")]
        [IfNotInteger]
        public string Cellphone { get; set; }

        /// <summary>
        /// Useraccount's status active or inactive.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [Display(Name = "Active Status")]
        public bool IsAccountActive { get; set; }

        /// <summary>
        /// Foreign key. Primary key of entity Role.
        /// </summary>
        [Required(ErrorMessage = MessageConstants.RequiredFieldError)]
        [IfNotSelected]
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Roles { get; set; }
        /// <summary>
        /// Members of a UserAccount.
        /// </summary>
        public virtual IEnumerable<Member> Members { get; set; }

        /// <summary>
        /// Incidents of a UserAccount.
        /// </summary>
        public virtual IEnumerable<Incident> Incidents { get; set; }

        //  public virtual IEnumerable<IncidentActionLog> IncidentActionLogs { get; set; }

        public virtual IEnumerable<IncidentAdminActionLog> IncidentAdminActionLogs { get; set; }


        [InverseProperty(nameof(IncidentActionLog.UserAccountAgents))]
        public virtual ICollection<IncidentActionLog> UserAccountAgentsList { get; set; }

        [InverseProperty(nameof(IncidentActionLog.UserAccountsSuperviseds))]
        public virtual ICollection<IncidentActionLog> UserAccountSupervisedsList { get; set; }

        [InverseProperty(nameof(IncidentActionLog.UserAccountExperts))]
        public virtual ICollection<IncidentActionLog> UserAccountExpertsList { get; set; }

        [InverseProperty(nameof(IncidentActionLog.UserAccountAdmins))]
        public virtual ICollection<IncidentActionLog> UserAccountAdminsList { get; set; }

        [InverseProperty(nameof(IncidentActionLog.UserAccountsTeamLeads))]
        public virtual ICollection<IncidentActionLog> UserAccountsTeamLeadsList { get; set; }


        [InverseProperty(nameof(IncidentActionLog.UserAccountsClosed))]
        public virtual ICollection<IncidentActionLog> UserAccountsClosedList { get; set; }
        /// <summary>
        /// SystemPermissions of a UserAccount.
        /// </summary>
        public virtual IEnumerable<SystemPermission> SystemPermissions { get; set; }

        /// <summary>
        /// UserAccounts of a FacilityPermission.
        /// </summary>
        public virtual IEnumerable<FacilityPermission> FacilityPermissions { get; set; }

        /// <summary>
        /// UserAccounts of a RemoteLoginConcent.
        /// </summary>
        public virtual IEnumerable<RemoteLoginConcent> RemoteLoginConcents { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Facility.
        /// </summary>
        public int? FacilityID { get; set; }

        [ForeignKey("FacilityID")]
        public virtual Facility Facilities { get; set; }

        /// <summary>
        /// Foreign key. Primary key of entity UserType.
        /// </summary>
        public int? UsertypeID { get; set; }

        [ForeignKey("UsertypeID")]
        public virtual DeviceType UserTypes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public int[] SystemPermissionList { get; set; }
    }
}