using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TUSO.Domain.Entities
{
    public class TeamLead :BaseModel
    {
        [Key]
        public int Oid { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table UserAccount.
        /// </summary>
        public long UserAccountId { get; set; }

        [ForeignKey("UserAccountId")]
        [JsonIgnore]
        public virtual UserAccount UserAccounts { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Team.
        /// </summary>
        public long TeamId { get; set; }

        [ForeignKey("TeamId")]
        [JsonIgnore]
        public virtual Team Teams { get; set; }
    }
}
