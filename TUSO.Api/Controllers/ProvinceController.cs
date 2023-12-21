using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 20.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///Province Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        public ProvinceController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/province
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateProvince)]
        public async Task<IActionResult> CreateProvince(Province province)
        {
            try
            {
                if (await IsProvinceDuplicate(province) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                province.DateCreated = DateTime.Now;
                province.IsDeleted = false;

                context.ProvinceRepository.Add(province);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadProvinceByKey", new { key = province.Oid }, province);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/provinces
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinces)]
        public async Task<IActionResult> ReadProvinces()
        {
            try
            {
                var province = await context.ProvinceRepository.GetProvinces();
               
                return Ok(province);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByKey)]
        public async Task<IActionResult> ReadProvinceByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByKey(key);

                if (provinceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(provinceInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByCountry)]
        public async Task<IActionResult> ReadProvinceByCountries(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByCountry(key);

                if (provinceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(provinceInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByCountryPage)]
        public async Task<IActionResult> ReadProvinceByCountryPage(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceInDb = await context.ProvinceRepository.GetProvincesByCountry(key, start, take);

                var response = new
                {
                    province = provinceInDb,
                    currentPage = start+1,
                    totalRows = await context.ProvinceRepository.GetProvinceCount( key)
                };
                if (provinceInDb == null)

                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="province">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateProvince)]
        public async Task<IActionResult> UpdateProvince(int key, Province province)
        {
            try
            {
                if (key != province.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsProvinceDuplicate(province) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                province.DateModified = DateTime.Now;
                province.IsDeleted = false;

                context.ProvinceRepository.Update(province);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteProvince)]
        public async Task<IActionResult> DeleteProvince(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByKey(key);

                if (provinceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (provinceInDb.Districts.Where(w => w.IsDeleted == false).ToList().Count > 0)
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);

                provinceInDb.IsDeleted = true;
                provinceInDb.DateModified = DateTime.Now;

                context.ProvinceRepository.Update(provinceInDb);
                await context.SaveChangesAsync();

                return Ok(provinceInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the province is duplicate?
        /// </summary>
        /// <param name="province">Province object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsProvinceDuplicate(Province province)
        {
            try
            {
                var provinceInDb = await context.ProvinceRepository.GetProvinceByNameAndCountry(province.ProvinceName, province.CountryId);

                if (provinceInDb != null)
                    if (provinceInDb.CountryId != province.CountryId)
                        return true;

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}