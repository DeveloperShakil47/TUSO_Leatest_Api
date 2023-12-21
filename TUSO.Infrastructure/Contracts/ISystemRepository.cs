using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface ISystemRepository : IRepository<Project>
    {
        /// <summary>
        /// Returns a System if key matched.
        /// </summary>
        /// <param name="Oid">System key of the table System</param>
        /// <returns>Instance of a System object.</returns>
        public Task<Project> GetSystemByKey(int key);

        /// <summary>
        /// Returns a System if the title matched.
        /// </summary>
        /// <param name="systemTitle">Title of the System.</param>
        /// <returns>Instance of a System object.</returns>
        public Task<Project> GetSystemByTitle(string SystemTitle);

        /// <summary>
        /// Returns all System.
        /// </summary>
        /// <returns>List of System object.</returns>
        public Task<IEnumerable<Project>> GetSystem(int start, int take);

        /// <summary>
        /// Returns all System.
        /// </summary>
        /// <returns>List of System object.</returns>
        public Task<IEnumerable<Project>> GetAllSystem();

        /// <summary>
        /// Count all System.
        /// </summary>
        /// <returns>Count Number of System object.</returns>
        public Task<int> GetSystemCount();

        /// <summary>
        /// Check is there any open ticket under the System.
        /// </summary>
        /// <param name="Oid">Primary key of the table System</param>
        /// <returns>Number of open ticket under the System.</returns>
        public Task<int> TotalOpenTicketUnderSystem(int key);
    }
}