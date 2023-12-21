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
    public class FacilityRepository : Repository<Facility>, IFacilityRepository
    {
        public FacilityRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to get a facility by key.
        /// </summary>
        /// <param name="key">districtId of a facility.</param>
        /// <returns>Returns a facility if the key is matched.</returns>
        public async Task<Facility> GetFacilityByKey(int key)
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

        /// <summary>
        /// The method is used to get a facility by districtId.
        /// </summary>
        /// <param name="districtId">districtId of a facility.</param>
        /// <returns>Returns a facility if the districtId is matched.</returns>
        public async Task<IEnumerable<Facility>> GetFacilityByDistrict(int key)
        {
            try
            {
                return await QueryAsync(f => f.DistrictId == key && f.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a facility by districtId.
        /// </summary>
        /// <param name="districtId">districtId of a facility.</param>
        /// <returns>Returns a facility if the districtId is matched.</returns>
        public async Task<IEnumerable<Facility>> GetFacilitiesByDistrict(int key, int start, int take, string? search)
        {
            try
            {
                List<Facility> data = new();
                if (search?.Length>0)
                {
                     data = await context.Facilities.Where(c => c.FacilityName.Contains(search) && c.IsDeleted==false && c.DistrictId == key).Include(x => x.Districts).OrderBy(x => x.FacilityName).Skip((start - 1) *take).Take(take).ToListAsync();
                }
                else
                {
                     data = await context.Facilities.Where(c => c.IsDeleted==false && c.DistrictId == key).Include(x => x.Districts).OrderBy(x => x.FacilityName).Skip((start - 1) *take).Take(take).ToListAsync();
                }
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a facility by name.
        /// </summary>
        /// <param name="name">name of a facility.</param>
        /// <returns>Returns a facility if the name is matched.</returns>
        public async Task<Facility> GetFacilityByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(f => f.FacilityName.ToLower().Trim() == name.ToLower().Trim() && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a facility by name,districtId,provinceId,countryId.
        /// </summary>
        /// <param name="name">name of a facility.</param>
        /// <param name="districtId">districtId of a facility.</param>
        /// <param name="provinceId">provinceId of a facility.</param>
        /// <param name="countryId">countryId of a facility.</param>
        /// <returns>Returns a facility if the name,districtId,provinceId,countryId is matched.</returns>
        public async Task<Facility> GetFacilityByName(string facilityMasterCode, int districtId,int provinceId, int countryId)
        {
            try
            {
                return await FirstOrDefaultAsync(f =>f.FacilityMasterCode.ToLower().Trim() == facilityMasterCode.ToLower().Trim() && f.DistrictId == districtId && f.Districts.ProvinceId == provinceId && f.Districts.Provinces.CountryId == countryId && f.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get all client.
        /// </summary>
        /// <returns>Returns all list of client</returns>
        public async Task<IEnumerable<Facility>> GetFacilities()
        {
            try
            {
                var data = await context.Facilities.Where(c => c.IsDeleted==false).ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetFacilitieCount(int key)
        {
            try
            {
                return await context.Facilities.Where(c => c.IsDeleted==false && c.DistrictId == key).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get a client by facilityName.
        /// </summary>
        /// <param name="facilityName">facilityName of a client.</param>
        /// <returns>Returns a client if the facilityName is matched.</returns>
        public async Task<IEnumerable<Facility>> GetFacilityByFacilityName(string facilityName)
        {
            try
            {
                return await QueryAsync(f => f.IsDeleted == false && f.FacilityName.Trim().Contains(facilityName.Trim()), f => f.Districts);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}