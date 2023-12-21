using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
* Created by: Stephan
* Date created: 17.12.2023
* Last modified:
* Modified by: 
*/
namespace TUSO.Infrastructure.Repositories
{
    public class EmailControlRepository : Repository<EmailControl>, IEmailControlRepository
    {
        public EmailControlRepository(DataContext context) : base(context)
        {

        }

        public async Task<EmailControl> GetEmailControlByKey(int key)
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
    }
}