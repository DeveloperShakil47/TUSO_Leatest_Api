using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Entities
{
    public class RemoteLoginConcent:BaseModel
    {
        /// <summary>
        /// Primary key of the table RemoteLoginConcent.
        /// </summary>
        [Key]
        public long Oid { get; set; }

        /// <summary>
        /// Connect date of the row.
        /// </summary>
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Date Modified")]
        public DateTime? ConnectDate { get; set; }

        /// <summary>
        /// Foreign key. Primary key of the entity CountUserAccountry. 
        /// </summary>
        public long UserAccountId { get; set; }

        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccounts { get; set; }

    }
}
