using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IIncidentActionLogRepository : IRepository<IncidentActionLog>
    {
        /// <summary>
        /// Returns a incidentActionLog if key matched.
        /// </summary>
        /// <param name="IncidentID">Primary key of the table incidentActionLog</param>
        /// <returns>Instance of a incidentActionLog object.</returns>
        public Task<IEnumerable<IncidentActionLog>> GetIncidentActionsByIncidentID(long IncidentID);

        /// <summary>
        /// Returns a incidentActionLog if key matched.
        /// </summary>
        /// <param name="IncidentID">Primary key of the table incidentActionLog</param>
        /// <returns>Instance of a incidentActionLog object.</returns>
        public Task<IncidentActionLog> GetIncidentActionByIncidentID(long IncidentID);
    }
}