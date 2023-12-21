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
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(DataContext context) : base(context)
        {

        }

        public async Task<Member> GetMemberByKey(long oid)
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

        public async Task<Member> GetLeaderByTeam(long key)
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

        public async Task<Member> GetMemberByUser(long key)
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

        public async Task<IEnumerable<Member>> GetMemberByTeam(long key)
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

        public async Task<IEnumerable<Member>> GetMembersByTeam(long key, int start, int take)
        {
            try
            {
                return await context.Members.Where(m => m.IsDeleted == false && m.TeamId == key).Include(x=>x.UserAccounts).Include(x=>x.Teams).Skip((start - 1) *take).Take(take).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Member> GetMemberPermission(long userAccountId, long teamId)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.UserAccountId == userAccountId && m.TeamId == teamId && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Member>> GetMembers()
        {
            try
            {
                var data = await context.Members.Where(c => c.IsDeleted==false).Include(x => x.UserAccounts).OrderBy(x => x.UserAccountId).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetMemberCount(int key)
        {
            return await context.Members.Where( m=> m.IsDeleted == false && m.TeamId == key).CountAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersByUser(long key)
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

        public async Task<Member> GetMemberByTeamLead(long userAccountId, long teamId)
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