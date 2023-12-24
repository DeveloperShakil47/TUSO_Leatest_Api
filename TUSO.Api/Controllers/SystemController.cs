using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
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
    /// Project Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IUnitOfWork context;

        private readonly ILogger<SystemController> logger;

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="context"></param>
        public SystemController(IUnitOfWork context , ILogger<SystemController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/system
        /// </summary>
        /// <param name="system">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateSystem)]
        public async Task<IActionResult> CreateSystem(Project system)
        {
            try
            {
                if (await IsSystemDuplicate(system) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                system.DateCreated = DateTime.Now;
                system.IsDeleted = false;

                context.SystemRepository.Add(system);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSystemByKey", new { key = system.Oid }, system);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateSystem", "SystemController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/systems
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystems)]
        public async Task<IActionResult> ReadSystems()
        {
            try
            {
                var systems = await context.SystemRepository.GetSystems();

                return Ok(systems);
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystems", "SystemController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/systems
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemsPagination)]
        public async Task<IActionResult> ReadSystemsPagination(int start, int take)
        {
            try
            {
                var system = await context.SystemRepository.GetSystemByPage(start, take);

                var response = new
                {
                    systems = system,
                    currentPage = start+1,
                    totalRows = await context.SystemRepository.GetSystemCount()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemsPagination", "SystemController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }



        /// <summary>
        /// URL: tuso-api/system/key/{OID}
        /// </summary>
        /// <param name="OID">Primary key of the table Systems</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemByKey)]
        public async Task<IActionResult> ReadSystemByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var system = await context.SystemRepository.GetSystemByKey(key);

                if (system == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(system);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemByKey", "SystemController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="system">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSystem)]
        public async Task<IActionResult> UpdateSystem(int key, Project system)
        {
            try
            {
                if (key != system.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsSystemDuplicate(system) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                system.DateModified = DateTime.Now;
                system.IsDeleted = false;

                context.SystemRepository.Update(system);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateSystem", "SystemController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystem)]
        public async Task<IActionResult> DeleteSystem(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemInDb = await context.SystemRepository.GetSystemByKey(key);

                if (systemInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var totalOpenTicketUnderSystem = await context.SystemRepository.TotalOpenTicketUnderSystem(systemInDb.Oid);

                if (totalOpenTicketUnderSystem > 0)
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);

                var systemPermissions = await context.SystemPermissionRepository.GetSystemPermissionBySystem(systemInDb.Oid);
                if (systemPermissions != null)
                {
                    foreach(var systemPermission in systemPermissions)
                    {
                        systemPermission.DateModified = DateTime.Now;
                        context.SystemPermissionRepository.Delete(systemPermission);
                    }
                }

                systemInDb.IsDeleted = true;
                systemInDb.DateModified = DateTime.Now;

                context.SystemRepository.Update(systemInDb);
                await context.SaveChangesAsync();

                return Ok(systemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSystem", "SystemController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the system title is duplicate? 
        /// </summary>
        /// <param name="system">System object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsSystemDuplicate(Project system)
        {
            try
            {
                var systemInDb = await context.SystemRepository.GetSystemByTitle(system.Title);

                if (systemInDb != null)
                {
                    if (systemInDb.Oid != system.Oid)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsSystemDuplicate", "SystemController.cs", ex.Message);

                throw;
            }
        }
    }
}