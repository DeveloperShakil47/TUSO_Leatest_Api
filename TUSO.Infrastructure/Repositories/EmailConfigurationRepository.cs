using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class EmailConfigurationRepository : Repository<EmailConfiguration>, IEmailConfigurationRepository
    {
        public EmailConfigurationRepository(DataContext context) : base(context)
        {

        }

        public async Task<EmailConfiguration> GetEmailConfigurationByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<EmailConfiguration>> GetEmailConfigurations()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}