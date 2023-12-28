using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TUSO.Domain.Entities
{
    public class ImplemenentingItem : BaseModel
    {
        public int Oid { get; set; }
        public long IncidentId { get; set; }

        [ForeignKey("IncidentId")]
        [JsonIgnore]
        public virtual Incident Incident { get; set; }

        public int FundingAgencyId { get; set; }
        [ForeignKey("FundingAgencyId")]
        [JsonIgnore]
        public virtual FundingAgency FundingAgency { get; set; }
    }
}