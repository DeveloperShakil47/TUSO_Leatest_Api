using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IDistrictRepository : IRepository<District>
    {
        /// <summary>
        /// Returns a district if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Districts</param>
        /// <returns>Instance of a District object.</returns>
        public Task<District> GetDistrictByKey(int key);

        /// <summary>
        /// Returns a District if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table District</param>
        /// <returns>Instance of a District object.</returns>
        public Task<IEnumerable<District>> GetDistrictByProvince(int key);

        /// <summary>
        /// Returns a district if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Districts</param>
        /// <returns>Instance of a District object.</returns>
        public Task<IEnumerable<District>> GetDistrictsByProvince(int key, int start, int take);

        /// <summary>
        /// Returns a district if the name matched.
        /// </summary>
        /// <param name="name">District name of the user</param>
        /// <returns>Instance of a District object.</returns>
        public Task<District> GetDistrictByName(string name);

        public Task<District> GetDistrictByNameByDistric(string name, int proviceId, int countryId);

        /// <summary>
        /// Returns all district.
        /// </summary>
        /// <returns>List of District object.</returns>
        public Task<IEnumerable<District>> GetDistricts();

        /// <summary>
        /// Count district.
        /// </summary>
        /// <returns>Count number  of District object.</returns>
        public Task<int> GetDistrictCount(int key);
    }
}