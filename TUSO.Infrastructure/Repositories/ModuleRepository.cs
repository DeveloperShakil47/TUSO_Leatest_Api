using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Stephan
 * Date created: 27.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(DataContext context) : base(context)
        {

        }

        public async Task<Module> GetModuleByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.Oid == key && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Module> GetModuleByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(m => m.ModuleName.ToLower().Trim() == name.ToLower().Trim() && m.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Module>> GetModules()
        {
            try
            {
                return await QueryAsync(m => m.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Module>> GetModulebyPage(int start, int take)
        {
            try
            {
                var data = await context.Modules.Where(c => c.IsDeleted == false).OrderBy(x => x.ModuleName).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetModuleCount()
        {
            try
            {
                return await context.Modules.Where(c => c.IsDeleted == false).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}