using TUSO.Domain.Entities;

/*
* Created by: Stephan
* Date created: 17.12.2023
* Last modified:
* Modified by: 
*/
namespace TUSO.Infrastructure.Contracts
{
    public interface IIncidentCategoryRepository : IRepository<IncidentCategory>
    {
        /// <summary>
        /// Returns a IncidentCategory if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table IncidentCategory</param>
        /// <returns>Instance of a IncidentCategory object.</returns>
        public Task<IncidentCategory> GetIncidentCategoryBySingleKey(int key);

        /// <summary>
        /// Returns all IncidentCategory.
        /// </summary>
        /// <returns>List of IncidentCategory object.</returns>
        public Task<IEnumerable<IncidentCategory>> GetIncidentCategories();

        /// <summary>
        /// Returns a incidentCategory if the incidentCategory name matched.
        /// </summary>
        /// <param name="name">Category of incident</param>
        /// <returns>Instance of a IncidentCategory table object.</returns>
        public Task<IncidentCategory> GetIncidentCategoryByName(string name,int parentId);

        /// <summary>
        /// Returns all IncidentCategory.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>List of IncidentCategory object.</returns>
        public Task<IEnumerable<IncidentCategory>> GetIncidentCategoriesByKey(int key);

        /// <summary>
        /// Returns all IncidentCategory.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>List of IncidentCategory object.</returns>
        public Task<IEnumerable<IncidentCategory>> GetIncidentCategoryPageByLevel(int key, int start, int take);

        /// <summary>
        /// Returns all IncidentCategory.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>List of IncidentCategory object.</returns>
        //public Task<IEnumerable<IncidentCategory>> GetIncidentCategoryPageByThirdLevel(int key, int start, int take);

        /// <summary>
        /// Returns all IncidentCategory.
        /// </summary>
        /// <returns>List of IncidentCategory object.</returns>
        public Task<IEnumerable<IncidentCategory>> GetIncidentCategoryPageByFirstLevel(int start, int take);

        public Task<int> GetIncidentCategoryCount(int key);
    }
}