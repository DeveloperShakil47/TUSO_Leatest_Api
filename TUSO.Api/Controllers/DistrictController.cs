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
    ///District Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<DistrictController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public DistrictController(IUnitOfWork context, ILogger<DistrictController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/district
        /// </summary>
        /// <param name="district">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDistrict)]
        public async Task<IActionResult> CreateDistrict(District district)
        {
            try
            {
                if (await IsDistrictDuplicate(district) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                district.DateCreated = DateTime.Now;
                district.IsDeleted = false;

                context.DistrictRepository.Add(district);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDistrictByKey", new { key = district.Oid }, district);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateDistrict", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/districts
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDistrict)]
        public async Task<IActionResult> ReadDistricts()
        {
            try
            {
                var district = await context.DistrictRepository.GetDistricts();

                return Ok(district);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDistricts", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDistrictByKey)]
        public async Task<IActionResult> ReadDistrictByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var district = await context.DistrictRepository.GetDistrictByKey(key);

                if (district == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(district);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDistrictByKey", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDistrictByProvince)]
        public async Task<IActionResult> ReadDistrictByProvince(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var provinceInDb = await context.DistrictRepository.GetDistrictByProvince(key);

                if (provinceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(provinceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDistrictByProvince", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDistrictByProvincePage)]
        public async Task<IActionResult> ReadDistrictsByProvince(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var district = await context.DistrictRepository.GetDistrictsByProvince(key, start, take);
                var response = new
                {
                    district = district,
                    currentPage = start + 1,
                    totalRows = await context.DistrictRepository.GetDistrictCount(key)
                };

                if (district == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDistrictsByProvince", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="district">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDistrict)]
        public async Task<IActionResult> UpdateDistrict(int key, District district)
        {
            try
            {
                if (key != district.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsDistrictDuplicate(district) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                district.DateModified = DateTime.Now;
                district.IsDeleted = false;

                context.DistrictRepository.Update(district);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateDistrict", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDistrict)]
        public async Task<IActionResult> DeleteDistrict(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var districtInDb = await context.DistrictRepository.GetDistrictByKey(key);

                if (districtInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (districtInDb.Facilities.Where(w => w.IsDeleted == false).ToList().Count > 0)
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);

                districtInDb.IsDeleted = true;
                districtInDb.DateModified = DateTime.Now;

                context.DistrictRepository.Update(districtInDb);
                await context.SaveChangesAsync();

                return Ok(districtInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDistrict", "DistrictController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the district name is duplicate?
        /// </summary>
        /// <param name="district">District object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDistrictDuplicate(District district)
        {
            try
            {
                var districtInDb = await context.DistrictRepository.GetDistrictByNameByDistric(district.DistrictName, district.ProvinceId, district.CountryId);

                if (districtInDb != null)

                    if (districtInDb.Oid != district.Oid)
                        return true;

                return false;
            }
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDistrictDuplicate", "DistrictController.cs", ex.Message);

                throw;
            }
        }
    }
}