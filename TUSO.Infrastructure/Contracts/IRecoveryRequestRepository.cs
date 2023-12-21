using TUSO.Domain.Entities;

/*
 * Created by: Rakib
 * Date created: 03.10.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IRecoveryRequestRepository : IRepository<RecoveryRequest>
    {
        /// <summary>
        /// Returns a recoveryRequest if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table RecoveryRequests</param>
        /// <returns>Instance of a RecoveryRequest object.</returns>
        public Task<RecoveryRequest> GetRecoveryRequestByKey(long key);

        /// <summary>
        /// Returns all recoveryRequest.
        /// </summary>
        /// <returns>List of RecoveryRequest object.</returns>
        public Task<IEnumerable<RecoveryRequest>> GetRecoveryRequests();

        /// <summary>
        /// Returns all RecoveryRequest.
        /// </summary>
        /// <returns>List of RecoveryRequest object.</returns>
        public Task<IEnumerable<RecoveryRequest>> GetRecoveryRequestByPage(int start, int take);

        /// <summary>
        /// Returns all RecoveryRequest.
        /// </summary>
        /// <returns>List of RecoveryRequest object.</returns>
        public Task<IEnumerable<RecoveryRequest>> SearchByUserName(string? userName, string? cellphone);

        /// <summary>
        /// Count all RecoveryRequest.
        /// </summary>
        /// <returns>Count of RecoveryRequest object.</returns>
        public Task<int> GetRecoveryRequestCount();
    }
}