using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IModulePermissionRepository : IRepository<ModulePermission>
    {
        /// <summary>
        /// Returns a module permission if key matched
        /// </summary>
        /// <param name="OID"Primary key of the table ></param>
        /// <returns>Instance of a ModulePermisstion object.</returns>
        public Task<ModulePermission> GetModulePermissionByKey(int OID);

        /// <summary>
        /// Returns a module permission if RoleID matched
        /// </summary>
        /// <param name="RoleID"Primary key of the role table ></param>
        /// <returns>Instance of a ModulePermission object.</returns>
        public Task<IEnumerable<ModulePermission>> GetModulePermissionByRole(int RoleID);

        /// <summary>
        /// Returns a module permission if RoleID matched
        /// </summary>
        /// <param name="RoleID"Primary key of the role table ></param>
        /// <returns>Instance of a ModulePermission object.</returns>
        public Task<IEnumerable<ModulePermission>> GetModulePermissionsByRole(int RoleID, int start, int take);

        /// <summary>
        /// Count  module permission if RoleID matched
        /// </summary>
        /// <param name="RoleID"Primary key of the role table ></param>
        /// <returns>Count number of  ModulePermission object.</returns>
        public Task<int> GetModulePermissionCountByRole(int RoleID);

        /// <summary>
        /// Returns a module permission if ModuleID matched
        /// </summary>
        /// <param name="ModuleID"Primary key of the module table></param>
        /// <returns>Instance of a ModulePermission Object</returns>
        public Task<IEnumerable<ModulePermission>> GetModulePermissionByModule(int ModuleID);

        /// <summary>
        /// Returns a module permission if RoleID, ModuleID matched.
        /// </summary>
        /// <param name="RoleID">Primary key of the role table</param>
        /// <param name="ModuleID">Primary key of the module table</param>
        /// <returns>Instance of a ModulePermission object</returns>
        public Task<ModulePermission> GetModulePermission(int RoleID, int ModuleID);

        /// <summary>
        /// Returns all Module permission.
        /// </summary>
        /// <returns>List of Module permission object.</returns>
        public Task<IEnumerable<ModulePermission>> GetModulePermissions();
    }
}