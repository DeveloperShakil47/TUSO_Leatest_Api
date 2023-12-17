using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IProvinceRepository : IRepository<Province>
    {
        /// <summary>
        /// Returns a province if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Province</param>
        /// <returns>Instance of a Province object.</returns>
        public Task<Province> GetProvinceByKey(int key);

        /// <summary>
        /// Returns a province if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Province</param>
        /// <returns>Instance of a Province object.</returns>
        public Task<IEnumerable<Province>> GetProvinceByCountry(int key);

        /// <summary>
        /// Returns a province if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Province</param>
        /// <returns>Instance of a Province object.</returns>
        public Task<IEnumerable<Province>> GetProvincesByCountry(int key, int start, int take);

        /// <summary>
        /// Returns a province if the province name matched.
        /// </summary>
        /// <param name="name">Province name of the user</param>
        /// <returns>Instance of a province table object.</returns>
        public Task<Province> GetProvinceByName(string name);

        /// <summary>
        /// Returns a province if the province name matched.
        /// </summary>
        /// <param name="name">Province name of the user</param>
        /// <returns>Instance of a province table object.</returns>
        public Task<Province> GetProvinceByNameAndCountry(string name, int countryId);

        /// <summary>
        /// Returns all province.
        /// </summary>
        /// <returns>List of Province object.</returns>
        public Task<IEnumerable<Province>> GetProvinces();

        /// <summary>
        /// Count province.
        /// </summary>
        /// <returns>Count number of Province object.</returns>
        public Task<int> GetProvinceCount(int key);
    }
}