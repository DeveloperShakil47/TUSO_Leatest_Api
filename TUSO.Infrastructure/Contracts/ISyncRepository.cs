using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface ISyncRepository : IRepository<DeviceType>
    {

        /// <summary>
        /// Returns all DeviceType.
        /// </summary>
        /// <returns>List of Syn object.</returns>
        public Task<IEnumerable<DeviceType>> GetAllSync();
    }
}
