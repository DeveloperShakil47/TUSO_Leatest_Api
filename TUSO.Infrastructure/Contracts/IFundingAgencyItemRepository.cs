using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IFundingAgencyItemRepository : IRepository<IncidendtFundingAgency>
    {
        /// <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgencyItem.</param>
        /// <returns>Returns a FundingAgencyItem if the key is matched.</returns>
        public Task<IncidendtFundingAgency> GetFundingAgencyItemByKey(int key);

        /// <summary>
        /// The method is used to get the list of FundingAgencyItem.
        /// </summary>
        /// <returns>Returns a list of all FundingAgencyItem.</returns>
        public Task<IEnumerable<IncidendtFundingAgency>> GetFundingAgencyItems();

        /// <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgencyItem.</param>
        /// <returns>Returns a FundingAgencyItem if the key is matched.</returns>
        public Task<IEnumerable<IncidendtFundingAgency>> GetFundingAgencyItemByIncident(int key);
    }
}