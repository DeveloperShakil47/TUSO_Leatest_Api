using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.Repositories;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DataContext context;
        public UnitOfWork(DataContext context)
        {
            this.context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        #region CountryRepository
        private ICountryRepository countryRepository;
        public ICountryRepository CountryRepository
        {
            get
            {
                if (countryRepository == null)
                    countryRepository = new CountryRepository(context);

                return countryRepository;
            }
        }
        #endregion
    }
}