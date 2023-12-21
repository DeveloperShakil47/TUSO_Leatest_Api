using Microsoft.EntityFrameworkCore;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Stephan
 * Date created: 20.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Repositories
{
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(DataContext context) : base(context)
        {

        }

        public async Task<Province> GetProvinceByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.Oid == key && p.IsDeleted == false, i => i.Districts);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Province>> GetProvinceByCountry(int key)
        {
            try
            {
                return await QueryAsync(p => p.CountryId == key && p.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Province>> GetProvincesByCountry(int key, int start, int take)
        {
            try
            {
              
                var data = await context.Provinces.Where(c => c.IsDeleted==false && c.CountryId == key).Include(x => x.Districts).OrderBy(x => x.ProvinceName).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Province> GetProvinceByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.ProvinceName.ToLower().Trim() == name.ToLower().Trim() && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Province> GetProvinceByNameAndCountry(string name, int countryId)
        {
            try
            {
                return await FirstOrDefaultAsync(p => p.ProvinceName.ToLower().Trim() == name.ToLower().Trim() && p.CountryId == countryId && p.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Province>> GetProvinces()
        {
            try
            {
                var data = await context.Provinces.Where(c => c.IsDeleted==false).Include(x => x.Districts).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetProvinceCount(int key)
        {
            try
            {
                return await context.Provinces.Where(c => c.IsDeleted == false && c.CountryId == key).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}