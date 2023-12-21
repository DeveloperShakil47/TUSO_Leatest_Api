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
    public class FacilityPermissionRepository : Repository<FacilityPermission>, IFacilityPermissionRepository
    {
        public FacilityPermissionRepository(DataContext context) : base(context)
        {

        }

        public async Task<List<FacilityPermission>> GetFacilitiesUserByKey(int key, int start, int take)
        {
            try
            {
                return await context.FacilityPermissions.Where(c => c.FacilityId == key && c.IsDeleted == false).Include(x => x.UserAccount).OrderBy(x => x.DateCreated).Skip((start - 1) * take).Take(take).ToListAsync();
            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task<List<FacilityPermission>> GetFacilityUserByKey(int key)
        {
            try
            {
                return await context.FacilityPermissions.Where(c => c.FacilityId == key && c.IsDeleted == false).Include(x => x.UserId).ToListAsync();
            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task<List<FacilityPermission>> GetFacilitiesUserByKey(int key)
        {
            try
            {
                return await context.FacilityPermissions.Where(c => c.FacilityId == key && c.IsDeleted == false).Include(x => x.UserAccount).ToListAsync();
            }
            catch (Exception)
            {
                throw;

            }
        }
        public async Task<int> GetTotalRows(int key)
        {
            return await context.FacilityPermissions.Where(x=>x.FacilityId == key && x.IsDeleted==false).CountAsync();
        }
        public async Task<FacilityPermission> IsDuplicatePermission(int facilityId, long userId)
        {
            try
            {
                return await context.FacilityPermissions.Where(c => c.FacilityId == facilityId && c.UserId == userId && c.IsDeleted == false).Include(x => x.UserAccount).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<FacilityPermission> GetFacilityPermissionByKey(int key)
        {
            try
            {
                return await context.FacilityPermissions.Where(c => c.Oid == key && c.IsDeleted == false).Include(x => x.UserAccount).Include(x => x.Facility).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task<IEnumerable<FacilityPermission>> GetFacilityPermissions()
        {
            try
            {
                return await context.FacilityPermissions.Where(c => c.IsDeleted == false).Include(x => x.UserAccount).Include(x => x.Facility).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}