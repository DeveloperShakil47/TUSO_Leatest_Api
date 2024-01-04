using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
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
        [CustomAuthorization]
        public async Task<ResponseDto> CreateFacility(Facility facility)
        {
            try
            {
                if (await IsFacilityDuplicate(facility) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                facility.DateCreated = DateTime.Now;
                facility.IsDeleted = false;

                context.FacilityRepository.Add(facility);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", facility);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateFacility", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/facilities
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilities)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilities()
        {
            try
            {
                var facilityInDb = await context.FacilityRepository.GetFacilities();

                return new ResponseDto(HttpStatusCode.OK, true, facilityInDb == null ? "Data Not Found" : "Successfully Get All Data", facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilities", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilityByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var facilityInDb = await context.FacilityRepository.GetFacilityByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, facilityInDb == null ? "Data Not Found" : "Successfully Get Data by Key", facilityInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByKey", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByDistrict)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilityByDistrict(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var facilityInDb = await context.FacilityRepository.GetFacilityByDistrict(key);

                return new ResponseDto(HttpStatusCode.OK, true, facilityInDb == null ? "Data Not Found" : "Successfully Get Data by Key", facilityInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByDistrict", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility/district/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Facilities</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilitieByDistrictPage)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilitiesByDistrict(int key, int start, int take, string? search = "")
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var facilityInDb = await context.FacilityRepository.GetFacilitiesByDistrict(key, start, take, search);

                var response = new
                {
                    facility = facilityInDb,
                    currentPage = start+1,
                    totalRows = await context.FacilityRepository.GetFacilitieCount(key)
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilitiesByDistrict", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: sc-api/facility/name/{name}
        /// </summary>
        /// <param name="facilityName">facility name of a client.</param>
        /// <returns>Http Status Code: Ok.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityByName)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilityByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                var facilityIndb = await context.FacilityRepository.GetFacilityByFacilityName(name);

                if (facilityIndb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError,null);

                var facilityInOrder = facilityIndb.OrderByDescending(c => c.DateCreated);

                return new ResponseDto(HttpStatusCode.OK, true, "Successfully Get Data by Key", facilityInOrder);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityByName", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateFacility(int key, Facility facility)
        {
            try
            {
                if (key != facility.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsFacilityDuplicate(facility) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                facility.DateModified = DateTime.Now;
                facility.IsDeleted = false;

                context.FacilityRepository.Update(facility);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", facility);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateFacility", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFacility)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteFacility(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var facilityInDb = await context.FacilityRepository.GetFacilityByKey(key);

                if (facilityInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                facilityInDb.IsDeleted = true;
                facilityInDb.DateModified = DateTime.Now;

                context.FacilityRepository.Update(facilityInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", facilityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFacility", "FacilityController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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