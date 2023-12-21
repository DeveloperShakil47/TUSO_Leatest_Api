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
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        public DistrictRepository(DataContext context) : base(context)
        {

        }
        public async Task<District> GetDistrictByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.Oid == key && d.IsDeleted == false, i => i.Facilities);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<District>> GetDistrictByProvince(int key)
        {
            try
            {
                return await QueryAsync(p => p.ProvinceId == key && p.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<District>> GetDistrictsByProvince(int key, int start,int take)
        {
            try
            {

                var data = await context.Districts.Where(c => c.IsDeleted==false && c.ProvinceId == key).Include(x => x.Provinces).Include(x => x.Facilities).OrderBy(x => x.DistrictName).Skip((start - 1) *take).Take(take).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<District> GetDistrictByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.DistrictName.ToLower().Trim() == name.ToLower().Trim() && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<District> GetDistrictByNameByDistric(string name, int proviceId, int countryId)
        {
            try
            {
                return await FirstOrDefaultAsync(d => d.DistrictName.ToLower().Trim() == name.ToLower().Trim() && d.ProvinceId == proviceId && d.Provinces.CountryId == countryId && d.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<District>> GetDistricts()
        {
            try
            {
                var data = await context.Districts.Where(c => c.IsDeleted==false).Include(x => x.Provinces).Include(x=>x.Facilities).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetDistrictCount(int key)
        {
            return await context.Districts.Where(c => c.IsDeleted==false && c.ProvinceId == key).CountAsync();
        }
    }
}