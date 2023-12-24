using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateDeviceType(DeviceType deviceType)
        {
            try
            {
                if (await IsDeviceTypeDuplicate(deviceType) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                deviceType.DateCreated = DateTime.Now;
                deviceType.IsDeleted = false;

                context.DeviceTypeRepository.Add(deviceType);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadDeviceTypeByKey", new { key = deviceType.Oid }, deviceType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateDeviceType", "DeviceTypeController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-types
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceTypes)]
        public async Task<IActionResult> ReadDeviceTypes()
        {
            try
            {
                var deviceTypes = await context.DeviceTypeRepository.GetDeviceTypes();

                return Ok(deviceTypes);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceTypes", "DeviceTypeController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-types
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceTypeByPage)]
        public async Task<IActionResult> ReadDeviceTypeByPage(int start, int take)
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceTypeByPage", "DeviceTypeController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-t ype/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceTypeByKey)]
        public async Task<IActionResult> ReadDeviceTypeByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deviceType = await context.DeviceTypeRepository.GetDeviceTypeByKey(key);

                if (deviceType == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(deviceType);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceTypeByKey", "DeviceTypeController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateDeviceType(int key, DeviceType deviceType)
        {
            try
            {
                if (key != deviceType.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsDeviceTypeDuplicate(deviceType) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                deviceType.DateModified = DateTime.Now;
                deviceType.IsDeleted = false;

                context.DeviceTypeRepository.Update(deviceType);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateDeviceType", "DeviceTypeController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: tuso-api/user-type/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteDeviceType)]
        public async Task<IActionResult> DeleteDeviceType(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deviceTypeInDb = context.DeviceTypeRepository.Get(key);

                if (deviceTypeInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                deviceTypeInDb.IsDeleted = true;
                deviceTypeInDb.DateModified = DateTime.Now;

                context.DeviceTypeRepository.Update(deviceTypeInDb);
                await context.SaveChangesAsync();

                return Ok(deviceTypeInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteDeviceType", "DeviceTypeController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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