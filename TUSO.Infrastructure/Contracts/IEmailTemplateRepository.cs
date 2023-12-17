using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IEmailTemplateRepository : IRepository<EmailTemplate>
    {
        /// <summary>
        /// Returns a EmailTemplate if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table EmailTemplate</param>
        /// <returns>Instance of a EmailTemplate object.</returns>
        public Task<EmailTemplate> GetEmailTemplateByKey(int key);

        /// <summary>
        /// Returns all EmailTemplate.
        /// </summary>
        /// <returns>List of EmailTemplate object.</returns>
        public Task<IEnumerable<EmailTemplate>> GetEmailTemplates();

        /// <summary>
        /// Returns a EmailTemplate if body type matched.
        /// </summary>
        /// <param name="key">Body type of the table EmailTemplate</param>
        /// <returns>Instance of a EmailTemplate object.</returns>
        public Task<EmailTemplate> GetEmailTemplateByBodyType(int key);
    }
}