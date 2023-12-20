using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Labib
 * Date created: 31.08.2022
 * Last modified: 06.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(DataContext context) : base(context)
        {

        }

        public async Task<Country> GetCountryByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == key && c.IsDeleted == false, i => i.Provinces);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Country> GetCountryByName(string countryName)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.CountryName.ToLower().Trim() == countryName.ToLower().Trim() && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<Country>> GetCountrybyPage(int start, int take)
        {
            try
            {
                var data =  await context.Countries.Where(c => c.IsDeleted==false).Include(x=>x.Provinces).OrderBy(x=>x.CountryName).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetCountryCount()
        {
            try
            {
                return await context.Countries.Where(c => c.IsDeleted==false).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}