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
    ///Facility Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IUnitOfWork context;

        private readonly ILogger<FacilityController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public FacilityController(IUnitOfWork context, ILogger<FacilityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/facility
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFacility)]
        public async Task<IActionResult> CreateFacility(Facility facility)
        {
            try
            {
                if (await IsFacilityDuplicate(facility) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                facility.DateCreated = DateTime.Now;
                facility.IsDeleted = false;

                context.FacilityRepository.Add(facility);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFacilityByKey", new { key = facility.Oid }, facility);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateFacility", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/facilities
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilities)]
        public async Task<IActionResult> ReadFacilities()
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetFacilities();

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilities", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByKey)]
        public async Task<IActionResult> ReadFacilityByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilityByKey(key);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByKey", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByDistrict)]
        public async Task<IActionResult> ReadFacilityByDistrict(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilityByDistrict(key);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByDistrict", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilitieByDistrictPage)]
        public async Task<IActionResult> ReadFacilitiesByDistrict(int key, int start, int take, string? search = "")
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilitiesByDistrict(key, start, take, search);
                var response = new
                {
                    facility = facilityInDb,
                    currentPage = start+1,
                    totalRows = await context.FacilityRepository.GetFacilitieCount(key)
                };

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilitiesByDistrict", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/name/{name}
        /// </summary>
        /// <param name="facilityName">facility name of a client.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByName)]
        public async Task<IActionResult> ReadFacilityByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityIndb = await context.FacilityRepository.GetFacilityByFacilityName(name);

                if (facilityIndb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var facilityInOrder = facilityIndb.OrderByDescending(c => c.DateCreated);

                return Ok(facilityInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByName", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="facility">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFacility)]
        public async Task<IActionResult> UpdateFacility(int key, Facility facility)
        {
            try
            {
                if (key != facility.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsFacilityDuplicate(facility) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                facility.DateModified = DateTime.Now;
                facility.IsDeleted = false;

                context.FacilityRepository.Update(facility);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateFacility", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFacility)]
        public async Task<IActionResult> DeleteFacility(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var facilityInDb = await context.FacilityRepository.GetFacilityByKey(key);

                if (facilityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                facilityInDb.IsDeleted = true;
                facilityInDb.DateModified = DateTime.Now;

                context.FacilityRepository.Update(facilityInDb);
                await context.SaveChangesAsync();

                return Ok(facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFacility", "FacilityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the facility is duplicate?
        /// </summary>
        /// <param name="facility"></param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFacilityDuplicate(Facility facility)
        {
            try
            {
                var userfacilityInDb = await context.FacilityRepository.GetFacilityByName(facility.FacilityMasterCode, facility.DistrictId, facility.ProvinceId, facility.CountryId);

                if (userfacilityInDb != null)

                    if (userfacilityInDb.Oid != facility.Oid)
                        return true;

                return false;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFacilityDuplicate", "FacilityController.cs", ex.Message);

                throw;
            }
        }
    }
}