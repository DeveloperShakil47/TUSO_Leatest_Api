using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class FundingAgencyItemRepository : Repository<FundingAgencyItem>, IFundingAgencyItemRepository
    {
        public FundingAgencyItemRepository(DataContext context) : base(context)
        {

        }

        public async Task<FundingAgencyItem> GetFundingAgencyItemByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.Oid == key && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FundingAgencyItem>> GetFundingAgencyItems()
        {
            try
            {
                return await QueryAsync(b => b.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}