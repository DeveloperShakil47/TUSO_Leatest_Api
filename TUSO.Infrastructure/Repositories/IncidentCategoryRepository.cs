using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Sakhawat
 * Date created: 04.09.2022
 * Last modified: 06.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class IncidentCategoryRepository : Repository<IncidentCategory>, IIncidentCategoryRepository
    {
        public IncidentCategoryRepository(DataContext context) : base(context)
        {

        }

        public async Task<IncidentCategory> GetIncidentCategoryBySingleKey(int key)
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


        public async Task<IEnumerable<IncidentCategory>> GetIncidentCategoriesByKey(int key)
        {
            try
            {
                return await QueryAsync(i => i.IsDeleted == false && i.ParentId == key, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IncidentCategory> GetIncidentCategoryByName(string name, int ParentId)
        {
            try
            {
                return await FirstOrDefaultAsync(i => i.IncidentCategorys.ToLower().Trim() == name.ToLower().Trim()&& i.ParentId == ParentId && i.IsDeleted == false);


            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<IncidentCategory>> GetIncidentCategoryPageByLevel(int key, int start, int take)
        {
            try
            {

                var data = await context.IncidentCategories.Where(c => c.IsDeleted == false && c.ParentId == key).Include(x => x.Incidents).OrderBy(x => x.Oid).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IncidentCategory>> GetIncidentCategories()
        {
            try
            {
                return await QueryAsync(i => i.IsDeleted == false && i.ParentId == 0, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<IncidentCategory>> GetIncidentCategoryPageByFirstLevel(int start, int take)
        {
            try
            {
                var data = await context.IncidentCategories.Where(c => c.IsDeleted==false  && c.ParentId == 0).Include(x=>x.Incidents).OrderBy(x => x.Oid).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetIncidentCategoryCount(int key)
        {
            try
            {
                return await context.IncidentCategories.Where(x => x.IsDeleted == false && x.ParentId == key).CountAsync(); ;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}