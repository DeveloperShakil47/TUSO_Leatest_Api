using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 27.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IModuleRepository : IRepository<Module>
    {
        /// <summary>
        /// Returns a module if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Module</param>
        /// <returns>Instance of a Module object.</returns>
        public Task<Module> GetModuleByKey(int key);

        /// <summary>
        /// Returns a module if the module name matched.
        /// </summary>
        /// <param name="name">Name of Module</param>
        /// <returns>Instance of a module table object.</returns>
        public Task<Module> GetModuleByName(string name);

        /// <summary>
        /// Returns all module.
        /// </summary>
        /// <returns>List of Module object.</returns>
        public Task<IEnumerable<Module>> GetModules();

        /// <summary>
        /// Returns all team.
        /// </summary>
        /// <returns>List of team object.</returns>
        public Task<IEnumerable<Module>> GetModulebyPage(int start, int take);

        public Task<int> GetModuleCount();
    }
}