/*
 * Created by: Rakib
 * Date created: 27.09.2022
 * Last modified: 27.09.2022
 * Modified by: Rakib
 */
namespace TUSO.Domain.Dto
{
    public class IncidentListReturnDto
    {
        public int TotalIncident { get; set; }

        public int CurrentPage { get; set; }

        public List<IncidentListDto> List { get; set; }
    }
}