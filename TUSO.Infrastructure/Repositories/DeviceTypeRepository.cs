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
    internal class DeviceTypeRepository : Repository<DeviceType>, IDeviceTypeRepository
    {
        public DeviceTypeRepository(DataContext context) : base(context)
        {

        }

        public async Task<DeviceType> GetDeviceTypeByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeviceType> GetDeviceTypeByName(string deviceTypeName)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.DeviceTypeName.ToLower().Trim() == deviceTypeName.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DeviceType>> GetDeviceTypes()
        {

            try
            {
                var data = await context.DeviceTypes.Where(c => c.IsDeleted==false).OrderBy(x => x.DeviceTypeName).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<DeviceType>> GetDeviceTypeByPage(int start, int take)
        {

            try
            {
                var data = await context.DeviceTypes.Where(c => c.IsDeleted == false).OrderBy(x => x.DeviceTypeName).Skip((start - 1) * take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetDeviceTypeCount()
        {
            return await context.DeviceTypes.Where(c => c.IsDeleted==false).CountAsync();
        }
    }
}