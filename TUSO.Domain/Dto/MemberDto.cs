using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TUSO.Domain.Entities;

namespace TUSO.Domain.Dto
{
    public class MemberDto
    { 

        /// <summary>
        /// Foreign Key. Primary key of the table UserAccount.
        /// </summary>
        public long UserAccountId { get; set; }

        /// <summary>
        /// Foreign Key. Primary key of the table Team.
        /// </summary>
        public long TeamId { get; set; }

        ///// <summary>
        ///// Indicates is he/she team lead or not.
        ///// </summary>
        //[Display(Name = "Is Team Lead")]
        public bool IsTeamLead { get; set; }
    }

    public class MemberDtoCollection 
    {
        public long Oid { get; set; }
        public string UserAccountName { get; set; }
        public long UserAccountId { get; set; }
        public string TeamName { get; set; }
        public long TeamId { get; set; }
        public bool IsTeamLead { get; set;}
    }
}
