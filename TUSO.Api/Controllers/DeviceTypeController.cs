using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///Role Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class DeviceTypeController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly  ILogger<DeviceTypeController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="UnitOfWork"></param>
        public DeviceTypeController(IUnitOfWork context , ILogger<DeviceTypeController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/user-type
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateDeviceType)]
        [CustomAuthorization]
        public async Task<ResponseDto> CreateDeviceType(DeviceType deviceType)
        {
            try
            {
                if (await IsDeviceTypeDuplicate(deviceType) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                deviceType.DateCreated = DateTime.Now;
                deviceType.IsDeleted = false;

                context.DeviceTypeRepository.Add(deviceType);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", deviceType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateDeviceType", "DeviceTypeController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-types
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceTypes)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadDeviceTypes()
        {
            try
            {
                var deviceTypes = await context.DeviceTypeRepository.GetDeviceTypes();

                return new ResponseDto(HttpStatusCode.OK, true, deviceTypes == null ? "Data Not Found" : "Successfully Get All Data", deviceTypes);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceTypes", "DeviceTypeController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-types
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceTypeByPage)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadDeviceTypeByPage(int start, int take)
        {
            try
            {
                var deviceTypes = await context.DeviceTypeRepository.GetDeviceTypeByPage(start, take);
                var response = new
                {
                    deviceTypes = deviceTypes,
                    currentPage = start + 1,
                    TotalRows = await context.DeviceTypeRepository.GetDeviceTypeCount()
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceTypeByPage", "DeviceTypeController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-t ype/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceTypeByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadDeviceTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var deviceType = await context.DeviceTypeRepository.GetDeviceTypeByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, deviceType == null ? "Data Not Found" : "Successfully Get Data by Key", deviceType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceTypeByKey", "DeviceTypeController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="userType">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateDeviceType)]
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateDeviceType(int key, DeviceType deviceType)
        {
            try
            {
                if (key != deviceType.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsDeviceTypeDuplicate(deviceType) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                deviceType.DateModified = DateTime.Now;
                deviceType.IsDeleted = false;

                context.DeviceTypeRepository.Update(deviceType);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", deviceType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateDeviceType", "DeviceTypeController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/user-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDeviceType)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteDeviceType(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var deviceTypeInDb = context.DeviceTypeRepository.Get(key);

                if (deviceTypeInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                deviceTypeInDb.IsDeleted = true;
                deviceTypeInDb.DateModified = DateTime.Now;

                context.DeviceTypeRepository.Update(deviceTypeInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", deviceTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDeviceType", "DeviceTypeController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the user role is duplicate? 
        /// </summary>
        /// <param name="userType">UserRole object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsDeviceTypeDuplicate(DeviceType deviceType)
        {
            try
            {
                var deviceTypeInDb = await context.DeviceTypeRepository.GetDeviceTypeByName(deviceType.DeviceTypeName);

                if (deviceTypeInDb != null)
                {
                    if (deviceTypeInDb.Oid != deviceType.Oid)
                        return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsDeviceTypeDuplicate", "DeviceTypeController.cs", ex.Message);

                throw;
            }
        }

    }
}