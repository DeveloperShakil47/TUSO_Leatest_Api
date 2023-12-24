using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class ImplementingPartnerRepository : Repository<ImplementingPartner>, IImplementingPartnerRepository
    {
        public ImplementingPartnerRepository(DataContext context) : base(context)
        {

        }

        public async Task<ImplementingPartner> GetImplementingPartnerByKey(int key)
        {
            try
            {
                return await context.ImplementingPartners.Where(implementingPartners => implementingPartners.Oid == key && implementingPartners.IsDeleted == false)
                   .Select(i => new ImplementingPartner
                   {
                       Oid = i.Oid,
                       ImplementingPartnerName = i.ImplementingPartnerName,
                       ProjectId = i.ProjectId,
                       CreatedBy = i.CreatedBy,
                       DateCreated = i.DateCreated,
                       ModifiedBy = i.ModifiedBy,
                       DateModified = i.DateModified,
                       IsDeleted = i.IsDeleted,
                       Projects = i.Projects.IsDeleted == false ? i.Projects : null,
                   }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ImplementingPartner> GetImplementingPartnerByNameAndSystem(string name, int key)
        {
            try
            {
                return await FirstOrDefaultAsync(f => (f.ImplementingPartnerName.ToLower().Trim() == name.ToLower().Trim() && f.ProjectId == key) && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ImplementingPartner>> GetImplementingPartnerBySystem(int key)
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

        public async Task<IEnumerable<ImplementingPartner>> GetImplementingPatrnerByPage(int start, int take)
        {
            try
            {
                var data = await context.ImplementingPartners.Where(c => c.IsDeleted==false).Include(x=>x.Projects).OrderBy(x => x.DateCreated).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ImplementingPartner>> GetImplementingPatrners()
        {
            try
            {
                var data = await context.ImplementingPartners.Where(c => c.IsDeleted==false).Include(x => x.Projects).OrderBy(x => x.DateCreated).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetImplementingPatrnersCount()
        {
            return await context.ImplementingPartners.Where(c => c.IsDeleted == false).CountAsync();
        }
    }
}