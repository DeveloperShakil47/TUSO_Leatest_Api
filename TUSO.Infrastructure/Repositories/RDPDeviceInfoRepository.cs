using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class RDPDeviceInfoRepository : Repository<RDPDeviceInfo>, IRDPDeviceInfoRepository
    {
        public RDPDeviceInfoRepository(DataContext context) : base(context)
        {

        }

        public async Task<RDPDeviceInfo> GetByUsername(string userName)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.UserName.ToLower().Trim() == userName.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RDPDeviceInfo> GetByDeviceId(string deviceId)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.DeviceID == deviceId && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RDPDeviceInfo>> GetRDPDeviceInfoesByDevice(string deviceId)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.DeviceID == deviceId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RDPDeviceInfo> GetRDPDeviceByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.OID == key && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RDPDeviceInfo>> GetRDPDevices()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, o => o.OID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RDPDeviceInfoDto>> GetAllRDPDevices()
        {
            try
            {
                return (IEnumerable<RDPDeviceInfoDto>)await QueryAsync(c => c.IsDeleted == false, o => o.DeviceID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}