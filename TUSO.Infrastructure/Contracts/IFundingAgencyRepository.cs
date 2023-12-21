using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IFundingAgencyRepository: IRepository<FundingAgency>
    {
        /// <summary>
        /// Returns a fundingAgency if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table fundingAgency</param>
        /// <returns>Instance of a FundingAgency object.</returns>        
        public Task<FundingAgency> GetFundingAgencyByKey(int key);

        /// <summary>
        /// Returns a fundingAgency if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgency</param>
        /// <returns>Instance of a FundingAgency object.</returns>
        public Task<IEnumerable<FundingAgency>> GetFundingAgencyBySystem(int key);

        /// <summary>
        /// Returns a Fundind Agency if the FundingAgency name matched.
        /// </summary>
        /// <param name="name">Funding Agency name of the System</param>
        /// <returns>Instance of a FundingAgency table object.</returns>
        public Task<FundingAgency> GetFundingAgencyByName(string name);

        /// <summary>
        /// Returns all FundingAgency.
        /// </summary>
        /// <returns>List of FundingAgency object.</returns>
        public Task<IEnumerable<FundingAgency>> GetFindingAgencies(int start,int take);

        public Task<IEnumerable<FundingAgency>> GetFindingAgencies();

        public Task<int> GetFindingAgenciesCount();

        /// <summary>
        /// Returns a Fundind Agency if the FundingAgency name or project id matched.
        /// </summary>
        /// <param name="name">Funding Agency name of the System</param>
        /// <param name="key">ProjectID of system</param>
        /// <returns>Instance of a FundingAgency table object.</returns>
        public Task<FundingAgency> GetFundingAgencyByNameAndSystem(string name, int key);
    }
}