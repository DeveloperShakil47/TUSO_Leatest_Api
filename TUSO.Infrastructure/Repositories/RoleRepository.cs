using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Labib
 * Date created: 31.08.2022
 * Last modified: 06.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {

        }

        public async Task<Role> GetRoleByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(r => r.Oid == key && r.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Role> GetRoleByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(r => r.RoleName.ToLower().Trim() == name.ToLower().Trim() && r.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            try
            {
                return await QueryAsync(r => r.IsDeleted == false,o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetRolePage(int start, int take)
        {
            try
            {  
                var data = await context.Roles.Where(x => x.IsDeleted == false).Include(x => x.UserAccounts).Include(x => x.ModulePermissions).OrderBy(x => x.DateCreated).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetRoleCount()
        {
            return await context.Roles.Where(x => x.IsDeleted == false).CountAsync();
        }
    }
}