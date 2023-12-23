using TUSO.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using System.Diagnostics.Metrics;

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

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="UnitOfWork"></param>
        public DeviceControlController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/Sync
        /// </summary>
        /// <param name="Sync">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.UpdateDeviceControl)]
        public async Task<IActionResult> UpdateDeviceControl(int key, DeviceControl deviceControl )
        {
            try
            {
                try
                {
                    if (key != deviceControl.Oid)
                        return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                    deviceControl.DateModified = DateTime.Now;
                    deviceControl.IsDeleted = false;

                    context.DeviceControlRepository.Update(deviceControl);
                    await context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status204NoContent);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/Syncs
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceControl)]
        public async Task<IActionResult> ReadDeviceControl()
        {
            try
            {
                var deviceControlInDb = await context.DeviceControlRepository.GetDeviceControl();

                if (deviceControlInDb.Count() == 0)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(deviceControlInDb.FirstOrDefault());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}