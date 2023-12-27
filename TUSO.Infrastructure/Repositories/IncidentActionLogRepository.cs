using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class IncidentActionLogRepository : Repository<IncidentActionLog>, IIncidentActionLogRepository
    {
        public IncidentActionLogRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<IncidentActionLog>> GetIncidentActionsByIncidentID(long IncidentId)
        {
            try
            {
                return context.IncidentActionLogs.Where(x => x.IncidentId == IncidentId && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IncidentActionLog> GetIncidentActionByIncidentID(long IncidentId)
        {
            try
            {
                return await context.IncidentActionLogs.Where(x => x.IncidentId == IncidentId && x.IsDeleted == false).Include(x => x.UserAccountAdmins).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}