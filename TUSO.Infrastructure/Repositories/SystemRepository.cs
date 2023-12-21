using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Bithy
 * Date created: 07.09.2022
 * Last modified: 14.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class SystemRepository : Repository<Project>, ISystemRepository
    {
        public SystemRepository(DataContext context) : base(context)
        {

        }

        public async Task<Project> GetSystemByKey(int key)
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

        public async Task<Project> GetSystemByTitle(string systemTitle)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Title.ToLower().Trim() == systemTitle.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetSystemCount()
        {
            try
            {
                return await context.Projects.Where(c => c.IsDeleted == false).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Project>> GetSystem(int start, int take)
        {
            try
            {
                var data = await context.Projects.Where(c => c.IsDeleted==false).Include(x=>x.FundingAgencies).Where(x=>x.IsDeleted == false).Include(x=>x.ImplementingPartners).Where(x=>x.IsDeleted == false).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Project>> GetAllSystem()
        {
            try
            {
                var data = await context.Projects.Where(c => c.IsDeleted==false).Include(x => x.FundingAgencies).Where(x => x.IsDeleted == false).Include(x => x.ImplementingPartners).Where(x => x.IsDeleted == false).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> TotalOpenTicketUnderSystem(int key)
        {
            try
            {
                return context.Incidents.Where(c => c.SystemId == key && c.IsOpen == true && c.IsDeleted == false).Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}