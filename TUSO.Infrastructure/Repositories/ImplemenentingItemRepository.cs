﻿using Abp.Domain.Repositories;
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
    public class ImplemenentingItemRepository : Repository<IncidentImplemenentingPartner>,IImplementingItemRepository
    {

        public ImplemenentingItemRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<IncidentImplemenentingPartner>> GetImplemenentingItemByIncident(int key)
        {
            return await QueryAsync(p => p.IncidentId == key && p.IsDeleted == false, o => o.Oid);
        }

        public async Task<IncidentImplemenentingPartner> GetImplemenentingItemByKey(int key)
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

        public async Task<IEnumerable<IncidentImplemenentingPartner>> GetImplemenentingItems()
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