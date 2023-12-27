using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IIncidentAdminActionLogRepository : IRepository<IncidentAdminActionLog>
    {
        /// <summary>
        /// Returns a incidentAdminActionLog if key matched.
        /// </summary>
        /// <param name="IncidentID">Primary key of the table incidentAdminActionLog</param>
        /// <returns>Instance of a incidentAdminActionLog object.</returns>
        public Task<IEnumerable<IncidentAdminActionLog>> GetIncidentAdminActionsByIncidentID(long IncidentID);

        /// <summary>
        /// Returns a incidentAdminActionLog if key matched.
        /// </summary>
        /// <param name="IncidentID">Primary key of the table incidentAdminActionLog</param>
        /// <returns>Instance of a incidentAdminActionLog object.</returns>
        public Task<IncidentAdminActionLog> GetIncidentAdminActionByIncidentID(long IncidentID);
    }
}