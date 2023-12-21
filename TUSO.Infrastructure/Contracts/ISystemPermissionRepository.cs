using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface ISystemPermissionRepository : IRepository<SystemPermission>
    {
        /// <summary>
        /// Returns a System permission if key matched
        /// </summary>
        /// <param name="key">Primary key of the System table></param>
        /// <returns>Instance of a SystemPermission object.</returns>
        public Task<SystemPermission> GetSystemPermissionByKey(int key);

        /// <summary>
        /// Returns a System permission if RoleID matched
        /// </summary>
        /// <param name="userAccountId">Primary key of the UserAccount table></param>
        /// <returns>Instance of a SystemPermission object.</returns>
        public Task<IEnumerable<SystemPermission>> GetSystemPermissionByUser(int userAccountId);

        /// <summary>
        /// Returns a province if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Province</param>
        /// <returns>Instance of a Province object.</returns>
        public Task<IEnumerable<SystemPermission>> GetSystemPermissionByUserPage(int key, int start, int take);

        /// <summary>
        /// Returns a System permission if SystemID matched
        /// </summary>
        /// <param name="systemId">Primary key of the System table></param>
        /// <returns>Instance of a SystemPermission object.</returns>
        public Task<IEnumerable<SystemPermission>> GetSystemPermissionBySystem(int systemId);

        /// <summary>
        /// Returns a System permission if UserAccountID, SystemID matched
        /// </summary>
        /// <param name="userAccountId">Primary key of the UserAccount table</param>
        /// <param name="systemId">Primary key of the System table</param>
        /// <returns>Instance of a SystemPermission object.</returns>
        public Task<SystemPermission> GetSystemPermission(long userAccountId, int systemId);

        /// <summary>
        /// Returns all System permission.
        /// </summary>
        /// <returns>List of System permission object.</returns>
        public Task<IEnumerable<SystemPermission>> GetSystemPermissions();

        /// <summary>
        /// Count all System permission.
        /// </summary>
        /// <returns>Count number of System permission object.</returns>
        public Task<int> GetSystemPermissionCount(int key);
    }
}