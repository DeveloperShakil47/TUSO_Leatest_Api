using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Selim
 * Date created: 19.12.2022
 * Last modified:
 * Modified by: 
 */

namespace TUSO.Infrastructure.Repositories
{
    public class SyncRepository : Repository<Sync>, ISyncRepository
    {
        public SyncRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Sync>> GetAllSync()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, o => o.OID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}