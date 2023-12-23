using Microsoft.EntityFrameworkCore;
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
    public class IncidentPriorityRepository : Repository<IncidentPriority>, IIncidentPriorityRepository
    {
        public IncidentPriorityRepository(DataContext context) : base(context)
        {

        }

        public async Task<IncidentPriority> GetIncidentPriorityByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(i => i.Oid == key && i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentPriority> GetIncidentPriorityByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(i => i.Priority == name && i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IncidentPriority>> GetIncidentPriorities(int start, int take)
        {
            try
            {
                return await context.IncidentPriorities.Where(i => i.IsDeleted == false).Include(o => o.Incidents).OrderBy(x => x.DateCreated).Skip((start - 1) *take).Take(take).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task<IEnumerable<IncidentPriority>> GetIncidentPriorities()
        {
            try
            {
                return await context.IncidentPriorities.Where(i => i.IsDeleted == false).Include(o => o.Incidents).OrderBy(x => x.DateCreated).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> GetIncidentPriorityCount()
        {
            return await context.IncidentPriorities.Where(x => x.IsDeleted == false).CountAsync();
        }
    }
}