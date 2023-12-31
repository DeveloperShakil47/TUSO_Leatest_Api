﻿using TUSO.Domain.Entities;

/*
* Created by: Stephan
* Date created: 17.12.2023
* Last modified:
* Modified by: 
*/
namespace TUSO.Infrastructure.Contracts
{
    public interface IDeviceControlRepository : IRepository<DeviceControl>
    {

        /// <summary>
        /// Returns all DeviceType.
        /// </summary>
        /// <returns>List of Syn object.</returns>
        public Task<IEnumerable<DeviceControl>> GetDeviceControl();
    }
}
