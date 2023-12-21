using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Bithy
 * Date created: 07.09.2022
 * Last modified: 14.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class SystemPermissionRepository : Repository<SystemPermission>, ISystemPermissionRepository
    {
        public SystemPermissionRepository(DataContext context) : base(context)
        {

        }

        public async Task<SystemPermission> GetSystemPermissionByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SystemPermission>> GetSystemPermissionByUserPage(int key, int start, int take)
        {
            try
            {
                var data = await context.SystemPermissions.Where(c => c.IsDeleted == false && c.UserAccountId == key).OrderByDescending(x => x.DateCreated).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SystemPermission>> GetSystemPermissionByUser(int userAccountId)
        {
            try
            {
                return await QueryAsync(p => p.UserAccountId == userAccountId && p.IsDeleted == false, o => o.Oid, i => i.Projects);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SystemPermission>> GetSystemPermissionBySystem(int systemId)
        {
            try
            {
                return await QueryAsync(p => p.SystemId == systemId && p.IsDeleted == false, o => o.Oid, i => i.UserAccount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SystemPermission> GetSystemPermission(long userAccountId, int systemId)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.UserAccountId == userAccountId && p.SystemId == systemId && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SystemPermission>> GetSystemPermissions()
        {
            try
            {
                return await QueryAsync(p => p.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetSystemPermissionCount(int key)
        {
            try
            {
                return await context.SystemPermissions.Where(c => c.IsDeleted == false && c.UserAccountId == key).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}