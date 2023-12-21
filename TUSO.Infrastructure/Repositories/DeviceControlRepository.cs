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
    public class DeviceControlRepository : Repository<DeviceControl>, IDeviceControlRepository
    {
        public DeviceControlRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<DeviceControl>> GetDeviceControl()
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
    }
}