using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        /// <summary>
        /// Returns a facility if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Facility</param>
        /// <returns>Instance of a Facility object.</returns>        
        public Task<Facility> GetFacilityByKey(int key);

        /// <summary>
        /// Returns a facility if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Facility</param>
        /// <returns>Instance of a Facility object.</returns>
        public Task<IEnumerable<Facility>> GetFacilityByDistrict(int key);

        /// <summary>
        /// Returns a facility if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Facility</param>
        /// <returns>Instance of a Facility object.</returns>
        public Task<IEnumerable<Facility>> GetFacilitiesByDistrict(int key, int start, int take, string? search);

        /// <summary>
        /// Returns a user facility if the facility name matched.
        /// </summary>
        /// <param name="name">Facility name of the user</param>
        /// <returns>Instance of a facility table object.</returns>
        public Task<Facility> GetFacilityByName(string name);

        /// <summary>
        /// Returns a user facility if the facility name matched.
        /// </summary>
        /// <param name="name">Facility name of the user</param>
        /// <returns>Instance of a facility table object.</returns>
        public Task<Facility> GetFacilityByName(string facilityMasterCode, int districtId, int provinceId, int countryId);

        /// <summary>
        /// Returns a user facility if the facility name matched.
        /// </summary>
        /// <param name="facilityName">Facility name of the user</param>
        /// <returns>Instance of a facility table object.</returns>
        public Task<IEnumerable<Facility>> GetFacilityByFacilityName(string facilityName);

        /// <summary>
        /// Returns all facility.
        /// </summary>
        /// <returns>List of Facility object.</returns>
        public Task<IEnumerable<Facility>> GetFacilities();

        /// <summary>
        /// Count  facility.
        /// </summary>
        /// <returns>Count number of Facility object.</returns>
        public Task<int> GetFacilitieCount(int key);
    }
}