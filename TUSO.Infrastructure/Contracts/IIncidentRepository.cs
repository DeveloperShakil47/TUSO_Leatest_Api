using TUSO.Domain.Dto;
using TUSO.Domain.Entities;

/*
 * Created by: Sakhawat
 * Date created: 05.09.2022
 * Last modified: 14.09.2022
 * Modified by: Sakhawat
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IIncidentRepository : IRepository<Incident>
    {
        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListDto> GetIncidentByKey(long key);

        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<Incident> GetIncidentDataByKey(long key);

        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">UserID of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsByKey(long key, long UserAccountID, int start, int take, int status);

        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">UserID of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsByClient(long key, int start, int take, int status);

        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">AssaignTo of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsByExpart(long key, int start, int take, int status);

        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">AssaignTo of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsByExpartLeader(int key, int assignedTo, int start, int take, int status);

        /// <summary>
        /// Returns a Incident if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsBySearch(IncidentSearchDto search, int start, int take);

        /// <summary>
        /// Returns all Incident.
        /// </summary>
        /// <returns>List of Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidents(int start, int take, int status);

        /// <summary>
        /// Returns all Incident by status.
        /// </summary>
        /// <returns>List of Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsByStatus(bool key, int start, int take);

        /// <summary>
        /// Returns all Incident by date range, status, facility and ticket no.
        /// </summary>
        /// <returns>List of Incident object.</returns>
        public Task<IncidentLifeCycleListDto> GetIncidentBySearch(int start, int take, int? Status, DateTime? FromDate, DateTime? ToDate, int? TicketNo, int? Facilty, int? Province, int? District, int? SystemID);

        /// <summary>
        /// Returns all Incident by status, facility and ticket no.
        /// </summary>
        /// <returns>List of Incident object.</returns>
        public Task<IncidentLifeCycleListDto> GetWeeklyIncidentBySearch(int start, int take, int? Status, int? TicketNo, int? Facilty, int? Province, int? District, int? SystemID);

        /// <summary>
        /// Returns all Incident of expert member.
        /// </summary>
        /// <returns>List of Incident object.</returns>
        public Task<IEnumerable<Incident>> GetDeletedExpertIncidents(long OID);

        /// <summary>
        /// Returns a Incident if username matched.
        /// </summary>
        /// <param name="UserName">Username of associated user of an incident</param>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentListReturnDto> GetIncidentsByUserName(string UserName, int start, int take, int status);

        public Task<IncidentListReturnDto> GetIncidentsByAssignUserName(string UserName, int start, int take, int status);

        /// <summary>
        /// Returns Total,Resolved and Unresolved incidents count.
        /// </summary>
        /// <returns>Instance of a Incident object.</returns>
        public Task<IncidentCountDto> IncidentCount(string? UserName);

        /// <summary>
        /// Returns Client incidents count.
        /// </summary>
        /// <returns>Instance of a Incident object.</returns>
        public Task<ClientIncidentCountDto> IncidentClientCount(string? UserName);
    }
}