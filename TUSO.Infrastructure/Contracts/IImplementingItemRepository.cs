﻿using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IImplementingItemRepository : IRepository<IncidentImplemenentingPartner>
    {
        // <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table ImplemenentingItem.</param>
        /// <returns>Returns a ImplemenentingItem if the key is matched.</returns>
        public Task<IncidentImplemenentingPartner> GetImplemenentingItemByKey(int key);

        /// <summary>
        /// The method is used to get the list of ImplemenentingItem.
        /// </summary>
        /// <returns>Returns a list of all ImplemenentingItem.</returns>
        public Task<IEnumerable<IncidentImplemenentingPartner>> GetImplemenentingItems();

        /// <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgencyItem.</param>
        /// <returns>Returns a FundingAgencyItem if the key is matched.</returns>
        public Task<IEnumerable<IncidentImplemenentingPartner>> GetImplemenentingItemByIncident(int key);
    }
}