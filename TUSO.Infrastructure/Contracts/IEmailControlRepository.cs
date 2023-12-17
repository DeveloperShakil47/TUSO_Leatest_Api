using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IEmailControlRepository : IRepository<EmailControl>
    {
        /// <summary>
        /// Returns a EmailControl if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table EmailControl</param>
        /// <returns>Instance of a EmailControl object.</returns>
        public Task<EmailControl> GetEmailControlByKey(int key);
    }
}