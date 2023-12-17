using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface ICountryRepository : IRepository<Country>
    {
        /// <summary>
        /// Returns a country if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a Country object.</returns>
        public Task<Country> GetCountryByKey(int key);

        /// <summary>
        /// Returns a country if the name matched.
        /// </summary>
        /// <param name="countryName">Country name of the user.</param>
        /// <returns>Instance of a Country object.</returns>
        public Task<Country> GetCountryByName(string countryName);

        /// <summary>
        /// Returns all country.
        /// </summary>
        /// <returns>List of Country object.</returns>
        public Task<IEnumerable<Country>> GetCountries();

        /// <summary>
        /// Returns all country.
        /// </summary>
        /// <returns>List of Country object.</returns>
        public Task<IEnumerable<Country>> GetCountrybyPage(int start, int take);

        /// <summary>
        /// Count all country.
        /// </summary>
        /// <returns>Count number of Country object.</returns>
        public Task<int> GetCountryCount();
    }
}