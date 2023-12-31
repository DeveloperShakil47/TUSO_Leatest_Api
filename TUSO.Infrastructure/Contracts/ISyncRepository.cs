using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface ISyncRepository : IRepository<Sync>
    {
        
        /// <summary>
        /// Returns all Sync.
        /// </summary>
        /// <returns>List of Syn object.</returns>
        public Task<IEnumerable<Sync>> GetAllSync();
    }
}
