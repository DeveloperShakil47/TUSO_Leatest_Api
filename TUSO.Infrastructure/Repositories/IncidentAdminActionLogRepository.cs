using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class IncidentAdminActionLogRepository : Repository<IncidentAdminActionLog>, IIncidentAdminActionLogRepository
    {
        public IncidentAdminActionLogRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<IncidentAdminActionLog>> GetIncidentAdminActionsByIncidentID(long IncidentId)
        {
            try
            {
                return context.IncidentAdminActionLogs.Where(x => x.IncidentId == IncidentId && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentAdminActionLog> GetIncidentAdminActionByIncidentID(long IncidentId)
        {
            try
            {
                return await context.IncidentAdminActionLogs.FirstOrDefaultAsync(x => x.IncidentId == IncidentId && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}