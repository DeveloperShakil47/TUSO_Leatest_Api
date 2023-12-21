using TUSO.Domain.Entities;

/*
 * Created by: Labib
 * Date created: 31.08.2022
 * Last modified: 04.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Returns a role if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Roles</param>
        /// <returns>Instance of a Role object.</returns>
        public Task<Role> GetRoleByKey(int key);

        /// <summary>
        /// Returns a role if the role name matched.
        /// </summary>
        /// <param name="name">Role name</param>
        /// <returns>Instance of a Role table object.</returns>
        public Task<Role> GetRoleByName(string name);

        /// <summary>
        /// Returns all role.
        /// </summary>
        /// <returns>List of role object.</returns>
        public Task<IEnumerable<Role>> GetRoles();

        /// <summary>
        /// Returns all role.
        /// </summary>
        /// <returns>List of Role object.</returns>
        public Task<IEnumerable<Role>> GetRolePage(int start, int take);

        public Task<int> GetRoleCount();
    }
}