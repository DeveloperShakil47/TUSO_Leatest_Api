using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    /*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
    public interface ISyncRepository : IRepository<DeviceControl>
    {

        /// <summary>
        /// Returns all DeviceType.
        /// </summary>
        /// <returns>List of Syn object.</returns>
        public Task<IEnumerable<DeviceControl>> GetAllSync();
    }
}
