using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IScreenshotRepository : IRepository<Screenshot>
    {
        /// <summary>
        /// Returns a screenshot if key matched.
        /// </summary>
        /// <param name="OID">Primary key of the table Screenshot</param>
        /// <returns>Instance of a Screenshot object.</returns>
        public Task<Screenshot> GetScreenshotByKey(long OID);

        /// <summary>
        /// Returns all screenshot.
        /// </summary>
        /// <returns>List of screenshot object.</returns>
        public Task<IEnumerable<Screenshot>> GetScreenshots();
    }
}