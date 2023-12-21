using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IFacilityPermissionRepository : IRepository<FacilityPermission>
    {
        /// <summary>
        /// Returns a facility users if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table FacilityPermission</param>
        /// <returns>Instance of a FacilityPermission object.</returns>        
        public Task<List<FacilityPermission>> GetFacilitiesUserByKey(int key, int start, int take);

        public Task<List<FacilityPermission>> GetFacilityUserByKey(int key);

        /// <summary>
        /// Returns a facility users if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table FacilityPermission</param>
        /// <returns>Instance of a FacilityPermission object.</returns>        
        public Task<FacilityPermission> IsDuplicatePermission(int facilityId, long userId);

        /// <summary>
        /// Returns a FacilityPermission with user info
        /// </summary>
        /// <returns>Instance of a FacilityPermission object.</returns>
        public Task<IEnumerable<FacilityPermission>> GetFacilityPermissions();

        /// <summary>
        /// Returns a FacilityPermission with user info
        /// </summary>
        /// <returns>Instance of a FacilityPermission object.</returns>
        public Task<FacilityPermission> GetFacilityPermissionByKey(int key);

        public Task<int> GetTotalRows(int key);
    }
}