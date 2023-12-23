using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IIncidentPriorityRepository : IRepository<IncidentPriority>
    {
        /// <summary>
        /// Return a incidentpriority if key match.
        /// </summary>
        /// <param name="key">Priority of incident</param>
        /// <returns>Instance of a IncidentPriority table object.</returns>
        public Task<IncidentPriority> GetIncidentPriorityByKey(int key);

        /// <summary>
        /// Returns a IncidentPriority if the IncidentPriority name matched.
        /// </summary>
        /// <param name="name">Priority of incident</param>
        /// <returns>Instance of a IncidentPriority table object.</returns>
        public Task<IncidentPriority> GetIncidentPriorityByName(string name);

        /// <summary>
        /// Returns all IncidentPriority.
        /// </summary>
        /// <returns>List of IncidentPriority object.</returns>
        public Task<IEnumerable<IncidentPriority>> GetIncidentPriorities(int start, int take);

        public Task<IEnumerable<IncidentPriority>> GetIncidentPriorities();

        public Task<int> GetIncidentPriorityCount();
    }
}