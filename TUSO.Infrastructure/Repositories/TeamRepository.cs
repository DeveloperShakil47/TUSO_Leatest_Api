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
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(DataContext context) : base(context)
        {

        }

        public async Task<Team> GetTeamByKey(long oid)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == oid && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Team> GetTeamByTitle(string title)
        {
            try
            {
                return await FirstOrDefaultAsync(t => t.Title.ToLower().Trim() == title.ToLower().Trim() && t.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Team>> GetTeamsbyPage(int start, int take)
        {
            try
            {
                var data = await context.Teams.Where(c => c.IsDeleted==false).OrderBy(x => x.Title).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetTeamsCount()
        {
            try
            {
                return await context.Teams.Where(c => c.IsDeleted==false).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> TotalOpenTicketUnderTeam(long oid)
        {
            try
            {
                return context.Incidents.Where(c => c.TeamId == oid && c.IsOpen == true && c.IsDeleted == false).Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}