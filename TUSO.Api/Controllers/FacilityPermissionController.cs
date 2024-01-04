using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan 
 * Date created: 17.09.2022
 * Last modified: 17.09.2022
 * Modified by: Stephan
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///IncidentPriority Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class FacilityPermissionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FacilityPermissionController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public FacilityPermissionController(IUnitOfWork context, ILogger<FacilityPermissionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/facility-permission
        /// </summary>
        /// <param name="facilityPermission">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFacilityPermission)]
        [CustomAuthorization]
        public async Task<ResponseDto> CreateFacilityPermission(FacilityPermission facilityPermission)
        {
            try
            {
                if (await context.FacilityPermissionRepository.IsDuplicatePermission(facilityPermission.FacilityId, facilityPermission.UserId) is not null)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                facilityPermission.DateCreated = DateTime.Now;
                facilityPermission.IsDeleted = false;

                context.FacilityPermissionRepository.Add(facilityPermission);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", facilityPermission);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateFacilityPermission", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility-permission/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table row.</param>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityPermissionsByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilityPermissionsByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);


                var facilityPermission = await context.FacilityPermissionRepository.GetFacilityPermissionByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, facilityPermission == null ? "Data Not Found" : "Successfully Get Data by Key", facilityPermission);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityPermissionsByKey", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL : tuso-api/facility-users/key/{facilityID}
        /// </summary>
        /// <param name="FacilityID">Primary key of the Facility.</param>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityPermission)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilityUsers(int FacilityId)
        {
            try
            {
                var facilityPermission = await context.FacilityPermissionRepository.GetFacilityUserByKey(FacilityId);

                return new ResponseDto(HttpStatusCode.OK, true, facilityPermission == null ? "Data Not Found" : "Successfully Get Data by Key", facilityPermission);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityUsers", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility-users/key/{facilityID}
        /// </summary>
        /// <param name="FacilityID">Primary key of the Facility.</param>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilitiePermissionPage)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilitiesUsers(int FacilityId, int start, int take)
        {
            try
            {
                var facilityPermission = await context.FacilityPermissionRepository.GetFacilitiesUserByKey(FacilityId, start, take);
                var response = new
                {
                    data = facilityPermission,
                    currentPage = start+1,
                    totalRows = await context.FacilityPermissionRepository.GetTotalRows(FacilityId)
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilitiesUsers", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility-permissions
        /// </summary>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityPermissions)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadFacilityPermissions()
        {
            try
            {
                List<FacilitiesPermissionDto> facilitiesPermissionDtos = new List<FacilitiesPermissionDto>();

                var facilityPermissions = await context.FacilityPermissionRepository.GetFacilityPermissions();

                foreach (var facility in facilityPermissions)
                {
                    facilitiesPermissionDtos.Add(new FacilitiesPermissionDto()
                    {
                        OID = facility.Oid,
                        UserID = facility.UserId,
                        FacilityID = facility.FacilityId,
                        CreatedDate = facility.DateCreated?.ToString("dd/MMM/yyyy"),
                        ModifiedDate = facility.DateModified?.ToString("dd/MMM/yyyy"),
                        FacilityName = facility.Facility.FacilityName,
                        UserName = facility.UserAccount.Username

                    });
                }
                return new ResponseDto(HttpStatusCode.OK, true, facilityPermissions == null ? "Data Not Found" : "Successfully Get All Data", facilityPermissions);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFacilityPermissions", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility-permission/{key}
        /// </summary>
        /// <param name="key">primary key of the table</param>
        /// <param name="facilityPermission">Object to be update</param>
        /// <returns>Update row in the table</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFacilityPermissions)]
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateFacilityPermissions(int key, FacilityPermission facilityPermission)
        {
            try
            {
                if (key != facilityPermission.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await context.FacilityPermissionRepository.IsDuplicatePermission(facilityPermission.FacilityId, facilityPermission.UserId) is not null)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                facilityPermission.DateModified = DateTime.Now;
                facilityPermission.IsDeleted = false;

                context.FacilityPermissionRepository.Update(facilityPermission);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", facilityPermission);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateFacilityPermissions", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility-permission/delete/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFacilityPermission)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteFacilityPermission(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError,null);

                var facilityPermission = await context.FacilityPermissionRepository.GetFacilityPermissionByKey(key);

                if (facilityPermission == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                facilityPermission.IsDeleted = true;
                facilityPermission.DateModified = DateTime.Now;

                context.FacilityPermissionRepository.Update(facilityPermission);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", facilityPermission);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFacilityPermission", "FacilityPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}