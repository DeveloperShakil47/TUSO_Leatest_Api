using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IImplementingPartnerRepository : IRepository<ImplementingPartner>
    {
        /// <summary>
        /// Returns a implementingPartner if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table implementingPartner</param>
        /// <returns>Instance of a ImplementingPartner object.</returns>        
        public Task<ImplementingPartner> GetImplementingPartnerByKey(int key);

        /// <summary>
        /// Returns a implementingPartner if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table System</param>
        /// <returns>Instance of a ImplementingPartner object.</returns>
        public Task<IEnumerable<ImplementingPartner>> GetImplementingPartnerBySystem(int key);

        /// <summary>
        /// Returns a Implementing Partner if the ImplementingPartner name matched.
        /// </summary>
        /// <param name="name">Implementing Partner name of the System</param>
        /// <returns>Instance of a ImplementingPartner table object.</returns>
        public Task<ImplementingPartner> GetImplementingPartnerByNameAndSystem(string name, int key);

        /// <summary>
        /// Returns all ImplementingPartners.
        /// </summary>
        /// <returns>List of ImplementingPartners object.</returns>
        public Task<IEnumerable<ImplementingPartner>> GetImplementingPatrnerByPage(int start, int take);

        public Task<IEnumerable<ImplementingPartner>> GetImplementingPatrners();

        public Task<int> GetImplementingPatrnersCount();
    }
}