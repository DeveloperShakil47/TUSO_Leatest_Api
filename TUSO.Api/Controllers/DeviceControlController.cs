using TUSO.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using System.Diagnostics.Metrics;
using System.Net;
using TUSO.Domain.Dto;

/*
* Created by: Stephan
* Date created: 17.12.2023
* Last modified:
* Modified by: 
*/
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///Sync Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class DeviceControlController : ControllerBase
    {
        private readonly IUnitOfWork context;

        private readonly ILogger<DeviceControlController> logger;

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="UnitOfWork"></param>
        public DeviceControlController(IUnitOfWork context, ILogger<DeviceControlController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/Sync
        /// </summary>
        /// <param name="Sync">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.UpdateDeviceControl)]
        public async Task<ResponseDto> UpdateDeviceControl(int key, DeviceControl deviceControl)
        {
            try
            {
                if (key != deviceControl.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                deviceControl.DateModified = DateTime.Now;
                deviceControl.IsDeleted = false;

                context.DeviceControlRepository.Update(deviceControl);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", deviceControl);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateDeviceControl", " DeviceControlController.cs", ex.Message);
                
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/Syncs
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceControl)]
        public async Task<ResponseDto> ReadDeviceControl()
        {
            try
            {
                var deviceControlInDb = await context.DeviceControlRepository.GetDeviceControl();

                return new(HttpStatusCode.OK, true, deviceControlInDb == null ? "Data Not Found" : "Successfully Get All Data", deviceControlInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadDeviceControl", " DeviceControlController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}