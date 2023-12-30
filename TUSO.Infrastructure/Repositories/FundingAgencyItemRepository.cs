﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class FundingAgencyItemRepository : Repository<IncidendtFundingAgency>, IFundingAgencyItemRepository
    {
        public FundingAgencyItemRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<IncidendtFundingAgency>> GetFundingAgencyItemByIncident(int key)
        {
           return await QueryAsync(p => p.IncidentId == key && p.IsDeleted == false, o => o.Oid);

        }

        public async Task<IncidendtFundingAgency> GetFundingAgencyItemByKey(int key)
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

        public async Task<IEnumerable<IncidendtFundingAgency>> GetFundingAgencyItems()
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