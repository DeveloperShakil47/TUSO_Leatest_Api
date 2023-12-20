using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Entities
{
    /// <summary>
    /// IncidentActionLog Entity.
    /// </summary>
    public class IncidentActionLog : BaseModel
    {
        /// <summary>
        /// Primary key of the table IncidentActionLog.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity Incident.
        /// </summary>
        public long IncidentId { get; set; }
        [ForeignKey("IncidentId")]

        public virtual Incident Incident { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity UserAccount.
        /// </summary>
        [ForeignKey("UserAccountAgents")]
        public long? AgentId { get; set; }

        public virtual UserAccount UserAccountAgents { get; set; }

        /// <summary>
        ///  AgentDateModified of an UserAccount.
        /// </summary>
        public DateTime? AgentDateModified { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity UserAccount.
        /// </summary>
        [ForeignKey("UserAccountsSuperviseds")]
        public long? SupervisedId { get; set; }

        public virtual UserAccount UserAccountsSuperviseds { get; set; }

        /// <summary>
        ///  SupervisedDateModified of an UserAccount.
        /// </summary>
        public DateTime? SupervisedDateModified { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity UserAccount.
        /// </summary>
        [ForeignKey("UserAccountsTeamLeads")]
        public long? TeamLeadId { get; set; }

        public virtual UserAccount UserAccountsTeamLeads { get; set; }

        /// <summary>
        ///  TeamLeadDateModified of an UserAccount.
        /// </summary>
        public DateTime? TeamLeadDateModified { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity UserAccount.
        /// </summary>
        [ForeignKey("UserAccountExperts")]
        public long? ExpertId { get; set; }

        public virtual UserAccount UserAccountExperts { get; set; }

        /// <summary>
        ///  ExpertDateModified of an UserAccount.
        /// </summary>
        public DateTime? ExpertDateModified { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity UserAccount.
        /// </summary>
        [ForeignKey("UserAccountAdmins")]
        public long? AdminId { get; set; }
        public virtual UserAccount UserAccountAdmins { get; set; }

        /// <summary>
        ///  AdminDateModified of an UserAccount.
        /// </summary>
        public DateTime? AdminDateModified { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the entity UserAccount.
        /// </summary>
        [ForeignKey("UserAccountsClosed")]
        public long? CloseUserAccountId { get; set; }
        public virtual UserAccount UserAccountsClosed { get; set; }

        /// <summary>
        ///  DateClosed of an UserAccount.
        /// </summary>
        public DateTime? DateClosed { get; set; }
    }
}