using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Sakhawat
 * Date created: 31.08.2022
 * Last modified: 04.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class FundingAgencyRepository : Repository<FundingAgency>, IFundingAgencyRepository
    {
        public FundingAgencyRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<FundingAgency>> GetFindingAgenciesByPage(int start, int take)
        {
            try
            {
                var data = await context.FundingAgencies.Where(c => c.IsDeleted==false).Include(x => x.Projects).OrderBy(x => x.DateCreated).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<FundingAgency>> GetFindingAgencies()
        {
            try
            {
                var data = await context.FundingAgencies.Where(c => c.IsDeleted==false).Include(x => x.Projects).OrderBy(x => x.DateCreated).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetFindingAgenciesCount()
        {
            return await context.FundingAgencies.Where(c => c.IsDeleted == false).CountAsync();
        }

        public async Task<FundingAgency> GetFundingAgencyByKey(int key)
        {
            try
            {
                return await context.FundingAgencies.Where(fundingAgency => fundingAgency.Oid == key && fundingAgency.IsDeleted == false)
                     .Select(f => new FundingAgency
                     {
                         Oid = f.Oid,
                         FundingAgencyName = f.FundingAgencyName,
                         ProjectId = f.ProjectId,
                         CreatedBy = f.CreatedBy,
                         ModifiedBy = f.ModifiedBy,
                         DateCreated = f.DateCreated,
                         DateModified = f.DateModified,
                         IsDeleted = f.IsDeleted,
                         Projects = f.Projects.IsDeleted == false ? f.Projects : null,
                     }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FundingAgency> GetFundingAgencyByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.FundingAgencyName.ToLower().Trim() == name.ToLower().Trim() && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FundingAgency>> GetFundingAgencyBySystem(int key)
        {
            try
            {
                return await QueryAsync(f => f.ProjectId == key && f.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FundingAgency> GetFundingAgencyByNameAndSystem(string name, int key)
        {
            try
            {
                return await FirstOrDefaultAsync(f => (f.FundingAgencyName.ToLower().Trim() == name.ToLower().Trim() && f.ProjectId == key) && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}