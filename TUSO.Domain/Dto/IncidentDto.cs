using TUSO.Domain.Entities;

/*
 * Created by: Rakib
 * Date created: 27.09.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class IncidentDto
    {
        public IncidentListDto Incidents { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
    
}