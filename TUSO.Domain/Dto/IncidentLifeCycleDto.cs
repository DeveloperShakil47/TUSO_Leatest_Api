/*
 * Created by: Selim
 * Date created: 06.12.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    /// <summary>
    /// DTO or view model for showing Incident Life cycle.
    /// </summary>
    public class IncidentLifeCycleDto
    {
        public string TicketNo { get; set; }

        public string TicketTitle { get; set; }

        public string FacilityName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? OpenByAdmin { get; set; }

        public DateTime? OpenByAgent { get; set; }

        public DateTime? OpenBySupervisor { get; set; }

        public DateTime? OpenByExpertLead { get; set; }

        public DateTime? OpenByExpert { get; set; }

        public DateTime? TicketClosed { get; set; }

        public string TotalTime { get; set; }

        public string Status { get; set; }

        public string ExpertLeadName { get; set; }

        public string ExpertName { get; set; }

        public string AgentName { get; set; }

        public string SupervisorName { get; set; }

        public string TicketOpenedBy { get; set; }  

        public string ProvinceName { get; set; }

        public string DistrictName { get; set; }

        public string Description { get; set; }

        public string FirstLevelCategory { get; set; }

        public string SecondLevelCategory { get; set; }

        public string ThirdLevelCategory { get; set; }

        public string Priority { get; set; }

        public string TotalPendingTime { get; set; }

        public string SystemName { get; set; }

        public string? CallerName { get; set; }

        public string? CallerCellphone { get; set; }

        public string? CallerEmail { get; set; }

        public string? CallerJobTitle { get; set; }

        public DateTime? CallingDate { get; set; }

        public string ReassignedTo { get; set; }

        public DateTime? ReassignDate { get; set; }

        public string AssignedToState { get; set; }

        public DateTime? DateOfIncident { get; set; }

        public string? ClientName { get; set; }

        public string? AdminName { get; set; }

        public string FundingAgencyName { get; set; }

        public string ImplementingPartnerName { get; set; }

        public string? UserCellphone { get; set; }

        public string? UserEmail { get; set; }
    }

    public class IncidentLifeCycleListDto
    {
        public int TotalIncident { get; set; }

        public int CurrentPage { get; set; }

        public string AvgHandlingDuration { get; set; }

        public string MinHandlingDuration { get; set; }

        public string MaxHandlingDuration { get; set; }

        public List<IncidentLifeCycleDto> List { get; set; }
    }
}