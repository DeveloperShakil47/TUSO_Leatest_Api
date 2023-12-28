using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IFundingAgencyItemRepository : IRepository<FundingAgencyItem>
    {
        /// <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgencyItem.</param>
        /// <returns>Returns a FundingAgencyItem if the key is matched.</returns>
        public Task<FundingAgencyItem> GetFundingAgencyItemByKey(int key);

        /// <summary>
        /// The method is used to get the list of FundingAgencyItem.
        /// </summary>
        /// <returns>Returns a list of all FundingAgencyItem.</returns>
        public Task<IEnumerable<FundingAgencyItem>> GetFundingAgencyItems();

        /// <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgencyItem.</param>
        /// <returns>Returns a FundingAgencyItem if the key is matched.</returns>
        public Task<IEnumerable<FundingAgencyItem>> GetFundingAgencyItemByIncident(int key);
    }
}