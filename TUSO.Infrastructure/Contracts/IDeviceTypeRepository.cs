using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IDeviceTypeRepository : IRepository<DeviceType>
    {
        /// <summary>
        /// Returns a DeviceType if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a DeviceType object.</returns>
        public Task<DeviceType> GetDeviceTypeByKey(int key);

        /// <summary>
        /// Returns a DeviceType if the name matched.
        /// </summary>
        /// <param name="DeviceTypeName">DeviceType name of the user.</param>
        /// <returns>Instance of a DeviceType object.</returns>
        public Task<DeviceType> GetDeviceTypeByName(string deviceTypeName);

        /// <summary>
        /// Returns all DeviceType.
        /// </summary>
        /// <returns>List of DeviceType object.</returns>
        public Task<IEnumerable<DeviceType>> GetDeviceTypes();

        /// <summary>
        /// Returns all DeviceType.
        /// </summary>
        /// <returns>List of DeviceType object.</returns>
        public Task<IEnumerable<DeviceType>> GetDeviceTypeByPage(int start, int take);

        public Task<int> GetDeviceTypeCount();
    }
}