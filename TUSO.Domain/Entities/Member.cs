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
    public class Member : BaseModel
    {
        /// <summary>
        /// Primary key of the table Member.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table UserAccount.
        /// </summary>
        public long UserAccountId { get; set; }

        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccounts { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Team.
        /// </summary>
        public long TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Team Teams { get; set; }

        /// <summary>
        /// Indicates is he/she team lead or not.
        /// </summary>
        [Display(Name = "Is Team Lead")]
        public bool IsTeamLead { get; set; }
    }
}