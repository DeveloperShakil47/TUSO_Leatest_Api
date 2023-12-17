using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IEmailConfigurationRepository : IRepository<EmailConfiguration>
    {
        /// <summary>
        /// Returns a email configuration if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table EmailConfiguration</param>
        /// <returns>Instance of a EmailConfiguration object.</returns>
        public Task<EmailConfiguration> GetEmailConfigurationByKey(int key);

        /// <summary>
        /// Returns all EmailConfiguration.
        /// </summary>
        /// <returns>List of EmailConfiguration object.</returns>
        public Task<IEnumerable<EmailConfiguration>> GetEmailConfigurations();
    }
}
