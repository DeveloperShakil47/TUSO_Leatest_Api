/*
 * Created by:Labib
 * Date created: 15.02.2023
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class IncidentCountDto
    {
        public int TotalIncidents { get; set; }

        public int ResolvedIncidents { get; set; }

        public int UnresolvedIncidents { get; set; }

        public int TotalUsers { get; set; }

        public int TotalAssigned { get; set; }

        public int TotalUnassigned { get; set; }

        public int TotalUrgent { get; set; }

        public int TotalPendingRecoveryRequest { get; set; }

        public List<TopProvincesByIncident> TopProvincesByIncidents { get; set; }

        public List<TopSystemByIncident> TopSystemByIncidents { get; set; }

        public List<TopTeamByUnresolvedIncident> TopTeamByUnresolvedIncidents { get; set; }  

        public List<IncidentInfoPerDay> IncidentInfoPerDays { get; set; }

        public List<LastMonthTotalTicket> LastMonthTotalTickets { get; set; }
    }

    public class TopTeamByUnresolvedIncident
    {
        public string TeamName { get; set; }

        public int IncidentCount { get; set; }
    }
    public class TopProvincesByIncident
    {
        public string ProviceName { get; set; }

        public int IncidentCount { get; set; }
    }

    public class TopSystemByIncident
    {
        public string SystemName { get; set; }

        public int IncidentCount { get; set; }
    }
}