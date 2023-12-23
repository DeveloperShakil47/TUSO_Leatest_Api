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
    public class LeadMemberRepository : Repository<TeamLead>, ILeadMemberRepository
    {
        public LeadMemberRepository(DataContext context) : base(context)
        {

        }

        public async Task<TeamLead> GetMemberByKey(long oid)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.Oid == oid && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TeamLead> GetLeaderByTeam(long key)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.TeamId == key && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TeamLead> GetMemberByUser(long key)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.UserAccountId == key && m.IsDeleted == false, t => t.Teams);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TeamLead>> GetMemberByTeam(long key)
        {
            try
            {
                return await QueryAsync(m => m.IsDeleted == false && m.TeamId == key, o => o.Oid, i => i.UserAccounts);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TeamLead>> GetMembersByTeam(long key, int start, int take)
        {
            try
            {
                return await context.TeamLeads.Where(m => m.IsDeleted == false && m.TeamId == key).Include(x=>x.UserAccounts).Include(x=>x.Teams).Skip((start - 1) *take).Take(take).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TeamLead> GetMemberPermission(long userAccountId, long teamId)
        {
            try
            {
                return await FirstOrDefaultAsync(m => (m.UserAccountId == userAccountId || m.TeamId == teamId) && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TeamLead> GetLeadMemberByTeamId(long teamId)
        {
            try
            {
                return await FirstOrDefaultAsync(m =>  m.TeamId == teamId && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<TeamLead>> GetMembers()
        {
            try
            {
                var data = await context.TeamLeads.Where(c => c.IsDeleted==false).Include(x => x.UserAccounts).OrderBy(x => x.UserAccountId).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetMemberCount(int key)
        {
            return await context.TeamLeads.Where( m=> m.IsDeleted == false && m.TeamId == key).CountAsync();
        }

        public async Task<IEnumerable<TeamLead>> GetMembersByUser(long key)
        {
            try
            {
                return await QueryAsync(m => m.UserAccountId == key && m.IsDeleted == false , t => t.Teams);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TeamLead> GetMemberByTeamLead(long userAccountId, long teamId)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.UserAccountId == userAccountId  && m.TeamId != teamId && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}