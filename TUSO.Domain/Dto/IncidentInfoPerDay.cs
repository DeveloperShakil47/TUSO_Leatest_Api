/*
 * Created by: Selim
 * Date created: 06.12.2022
 * Last modified: 
 * Modified by: 
 */

namespace TUSO.Domain.Dto
{
    public class IncidentInfoPerDay
    {
        public DateTime? IncidentDate { get; set; }

        public int TotalOpenIncident { get; set; }

        public int TotalClosedIncident { get; set; }
    }
}
