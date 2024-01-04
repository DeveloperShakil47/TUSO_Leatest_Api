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
        [CustomAuthorization]
        public async Task<ResponseDto> CreateSystem(Project system)
        {
            try
            {
                if (await IsSystemDuplicate(system) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                system.DateCreated = DateTime.Now;
                system.IsDeleted = false;

                context.SystemRepository.Add(system);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", system);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateSystem", "SystemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/systems
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystems)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadSystems()
        {
            try
            {
                var systems = await context.SystemRepository.GetSystems();

                return new ResponseDto(HttpStatusCode.OK, true, systems == null ? "Data Not Found" : "Successfully Get All Data", systems);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystems", "SystemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/systems
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemsPagination)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadSystemsPagination(int start, int take)
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
                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemsPagination", "SystemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }



        /// <summary>
        /// URL: tuso-api/system/key/{OID}
        /// </summary>
        /// <param name="OID">Primary key of the table Systems</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadSystemByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var system = await context.SystemRepository.GetSystemByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, system == null ? "Data Not Found" : "Successfully Get Data by Key", system);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemByKey", "SystemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateSystem(int key, Project system)
        {
            try
            {
                if (key != system.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                if (await IsSystemDuplicate(system) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                system.DateModified = DateTime.Now;
                system.IsDeleted = false;

                context.SystemRepository.Update(system);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", system);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateSystem", "SystemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystem)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteSystem(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var systemInDb = await context.SystemRepository.GetSystemByKey(key);

                if (systemInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                var totalOpenTicketUnderSystem = await context.SystemRepository.TotalOpenTicketUnderSystem(systemInDb.Oid);

                if (totalOpenTicketUnderSystem > 0)
                    return new ResponseDto(HttpStatusCode.MethodNotAllowed, false, MessageConstants.DependencyError, null);

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

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", systemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSystem", "SystemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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