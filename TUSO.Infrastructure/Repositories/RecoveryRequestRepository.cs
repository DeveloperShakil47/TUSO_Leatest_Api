using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Rakib
 * Date created: 03.10.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Infrastructure.Repositories
{
    public class RecoveryRequestRepository : Repository<RecoveryRequest>, IRecoveryRequestRepository
    {
        public RecoveryRequestRepository(DataContext context) : base(context)
        {

        }

        public async Task<RecoveryRequest> GetRecoveryRequestByKey(long key)
        {
            try
            {
                return await FirstOrDefaultAsync(r => r.Oid == key && r.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
         
        public async Task<IEnumerable<RecoveryRequest>> SearchByUserName(string? userName,string? cellphone)
        {
            try
            {
                var data = await context.RecoveryRequests.Where(r => r.IsDeleted == false && r.Username ==userName || r.Cellphone == cellphone).ToListAsync();

                return data;
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RecoveryRequest>> GetRecoveryRequestByPage(int start, int take)
        {
            try
            {
                var data = await context.RecoveryRequests.Where(c => c.IsDeleted == false && c.IsRequestOpen == true).OrderBy(x => x.Username).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetRecoveryRequestCount()
        {
            return await context.RecoveryRequests.Where(c => c.IsDeleted == false && c.IsRequestOpen == true).CountAsync();
        }

        public async Task<IEnumerable<RecoveryRequest>> GetRecoveryRequests()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.IsRequestOpen == true, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}