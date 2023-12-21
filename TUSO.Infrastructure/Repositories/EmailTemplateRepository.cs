using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(DataContext context) : base(context)
        {

        }

        public async Task<EmailTemplate> GetEmailTemplateByKey(int key)
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

        public async Task<IEnumerable<EmailTemplate>> GetEmailTemplates()
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

        public async Task<EmailTemplate> GetEmailTemplateByBodyType(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.BodyType == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}