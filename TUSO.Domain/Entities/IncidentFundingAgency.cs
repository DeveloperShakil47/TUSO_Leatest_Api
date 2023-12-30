using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TUSO.Domain.Entities
{
    public class IncidendtFundingAgency : BaseModel
    {
        [Key]
        public int Oid { get; set; }

        public long IncidentId { get; set; }

        [ForeignKey("IncidentId")]
        [JsonIgnore]
        public virtual Incident Incident { get; set; }

        public int FundingAgencyId { get; set; }
    }
}