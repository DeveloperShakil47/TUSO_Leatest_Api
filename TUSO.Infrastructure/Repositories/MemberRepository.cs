using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Dto;
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
                return await context.Members.Where(m => m.IsDeleted == false && m.TeamId == key).Include(x => x.UserAccounts).Include(x => x.Teams).Skip((start - 1) *take).Take(take).ToListAsync();
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
        public async Task<IEnumerable<MemberDtoCollection>> GetMembers()
        {
            try
            {
                var memberDtoCollection = await (from member in context.Members
                                                 join userAccount in context.UserAccounts on member.UserAccountId equals userAccount.Oid
                                                 join team in context.Teams on member.TeamId equals team.Oid
                                                 join teamLead in context.TeamLeads on member.UserAccountId equals teamLead.UserAccountId into teamLeadsGroup
                                                 from teamLead in teamLeadsGroup.DefaultIfEmpty()
                                                 select new MemberDtoCollection
                                                 {
                                                     Oid = member.Oid,
                                                     UserAccountName = userAccount.Name,
                                                     UserAccountId = member.UserAccountId,
                                                     TeamName = team.Title,
                                                     TeamId = member.TeamId,
                                                     IsTeamLead = teamLead != null
                                                 }).ToListAsync();

                return memberDtoCollection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetMemberCount(int key)
        {
            return await context.Members.Where(m => m.IsDeleted == false && m.TeamId == key).CountAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersByUser(long key)
        {
            try
            {
                return await QueryAsync(m => m.UserAccountId == key && m.IsDeleted == false, t => t.Teams);
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