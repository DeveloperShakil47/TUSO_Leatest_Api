using TUSO.Domain.Dto;
using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IRDPDeviceInfoRepository : IRepository<RDPDeviceInfo>
    {

        /// <summary>
        /// Returns a rdpdeviceInfo if the name matched.
        /// </summary>
        /// <param name="userName">Device Info of the user.</param>
        /// <returns>Instance of a Device Info object.</returns>
        public Task<RDPDeviceInfo> GetByUsername(string userName);

        /// <summary>
        /// Returns a rdpdeviceInfo if the deviceid matched.
        /// </summary>
        /// <param name="deviceId">Device Info of the user.</param>
        /// <returns>Instance of a Device Info object.</returns>
        public Task<RDPDeviceInfo> GetByDeviceId(string deviceId);

        /// <summary>
        /// Returns a rdpdeviceInfo List if the deviceid matched.
        /// </summary>
        /// <param name="deviceId">Device Info of the user.</param>
        /// <returns>Instance of a Device Info object.</returns>
        public Task<IEnumerable<RDPDeviceInfo>> GetRDPDeviceInfoesByDevice(string deviceId);

        /// <summary>
        /// Returns all rdpdeviceInfo.
        /// </summary>
        /// <returns>List of RDPDevice object.</returns>
        public Task<IEnumerable<RDPDeviceInfo>> GetRDPDevices();

        /// <summary>
        /// Returns all rdpdeviceInfo.
        /// </summary>
        /// <returns>List of RDPDevice object.</returns>
        public Task<IEnumerable<RDPDeviceInfoDto>> GetAllRDPDevices();

        /// <summary>
        /// Returns a rdpdevice of the key matched.
        /// </summary>
        /// <param name="key">Device  of the rdp.</param>
        /// <returns>Instance of a Device object.</returns>
        public Task<RDPDeviceInfo> GetRDPDeviceByKey(int key);
    }
}