using System.ComponentModel.DataAnnotations;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Entities
{
    /// <summary>
    /// Configuration Entity.
    /// </summary>
    public class EmailControl : BaseModel
    {
        /// <summary>
        /// Primary Key of the table Configuration.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Is Emai lSend For Incident Create .
        /// </summary>
        [Display(Name = "Is Email Send For Incident Create")]
        public bool IsEmailSendForIncidentCreate { get; set; }

        /// <summary>
        /// Is Email Send For Incident Close.
        /// </summary>
        [Display(Name = "Is Email Send For Incident Close")]
        public bool IsEmailSendForIncidentClose { get; set; }
    }
}