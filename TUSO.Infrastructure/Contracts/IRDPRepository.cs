using TUSO.Domain.Entities;
using TUSO.Domain.Dto;

/*
* Created by: Stephan
* Date created: 01.01.2024
* Last modified:
* Modified by: 
*/
namespace TUSO.Infrastructure.Contracts
{
    public interface IRDPRepository : IRepository<RdpServerInfo>
    {
        /// <summary>
        /// Get  RdpServerInfo of userName and Password matched.
        /// </summary>
        /// <returns>Instance of a RdpServerInfo object.</returns>
        public Task<RdpServerInfo> GetUserByUserNamePassword(string UserName, string Password);

        /// <summary>
        /// Returns all RdpServerInfo.
        /// </summary>
        /// <returns>List of RdpServerInfo object.</returns>
        public Task<RdpServerInfo> GetSingleRDPServerInfo();

        /// <summary>
        /// Returns all device.
        /// </summary>
        /// <returns>List of Device object.</returns>
        public Task<IEnumerable<Device>> GetRootobject();

        /// <summary>
        /// Get  device of fromDate and toDate matched.
        /// </summary>
        /// <returns>Instance of a Device object.</returns>
        public IEnumerable<RdpDeviceActivityDto> GetDeviceActivity(string fromDate, string toDate);

        /// <summary>
        /// Returns a device if userName matched.
        /// </summary>
        /// <param name="userName">userName of the table Device</param>
        /// <returns>Instance of a Device object.</returns>
        public IEnumerable<RDPDeviceInfo> GetDeviceActivity(string userName);

        /// <summary>
        /// Returns a device if key matched.
        /// </summary>
        /// <param name="key">key of the table Device</param>
        /// <returns>Instance of a Device object.</returns>
        public string GetDeviceByKey(string key);

        /// <summary>
        /// Returns a device if deviceId matched.
        /// </summary>
        /// <param name="deviceId">deviceId key of the table Device</param>
        /// <returns>Instance of a Device object.</returns>
        public string UninstallDeviceByKey(string deviceId);

        /// <summary>
        /// Returns a device user by key matched.
        /// </summary>
        /// <param name="key">Primary key of the table device</param>
        /// <returns>Instance of a device object.</returns>
        public UserAccount GetDeviceUserByKey(string key);
    }
}