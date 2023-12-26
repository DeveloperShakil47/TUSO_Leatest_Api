using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    /// SystemPermission Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class SystemPermissionController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<SystemPermissionController> logger;

        /// <summary>
        /// System Permission constructor.
        /// </summary>
        /// <param name="context">Inject IUnitOfWork as context</param>
        public SystemPermissionController(IUnitOfWork context, ILogger<SystemPermissionController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/system-permission
        /// </summary>
        [HttpPost]
        [Route(RouteConstants.CreateSystemPermission)]
        public async Task<ResponseDto> CreateSystemPermission(SystemPermission permission)
        {
            try
            {
                if (await IsPermissionDuplicate(permission) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                permission.DateCreated = DateTime.Now;
                permission.IsDeleted = false;

                context.SystemPermissionRepository.Add(permission);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", permission);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateSystemPermission", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/system-permissions
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissions)]
        public async Task<ResponseDto> ReadSystemPermissions()
        {
            try
            {
                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissions();
                return new ResponseDto(HttpStatusCode.OK, true, permissionInDb == null ? "Data Not Found" : "Successfully Get All Data", permissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemPermissions", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/system-permission/key/{OID}
        /// </summary>
        /// <param name="OID">Primary key of entity as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByKey)]
        public async Task<ResponseDto> ReadSystemPermissionByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, permissionInDb == null ? "Data Not Found" : "Successfully Get Data by Key", permissionInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemPermissionByKey", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>     
        /// URL : tuso-api/system-permission/role?RoleID={RoleID}
        /// </summary>
        /// <param name="RoleID">RoleID of role as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByUser)]
        public async Task<ResponseDto> ReadSystemPermissionByUser(int userAccountId)
        {
            try
            {
                if (userAccountId <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByUser(userAccountId);

                return new ResponseDto(HttpStatusCode.OK, true, permissionInDb == null ? "Data Not Found" : "Successfully Get Data by Key", permissionInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemPermissionByUser", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        // <summary>
        /// URL : tuso-api/system-permission-page/user/{UserAccountID}
        /// </summary>
        /// <param name="key">Primary key of the table SystemPermission</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByUserPage)]
        public async Task<ResponseDto> ReadSystemPermissionByUserPage(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var systemPermissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByUserPage(key, start, take);

                var response = new
                {
                    systemPermission = systemPermissionInDb,
                    currentPage = start + 1,
                    totalRows = await context.SystemPermissionRepository.GetSystemPermissionCount(key)
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemPermissionByUserPage", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL : tuso-api/system-permission/system?SystemID={SystemID}
        /// </summary>
        /// <param name="SystemID">SystemID of project as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByProject)]
        public async Task<ResponseDto> ReadSystemPermissionByProject(int systemId)
        {
            try
            {
                if (systemId <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionBySystem(systemId);

                return new ResponseDto(HttpStatusCode.OK, true, permissionInDb == null ? "Data Not Found" : "Successfully Get All Data", permissionInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemPermissionByProject", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/system-permission/key?RoleID={RoleID}&SystemID={SystemID}
        /// </summary>
        /// <param name="RoleID">RoleID of role as parameter</param>
        /// <param name="SystemID">SystemID of system as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermission)]
        public async Task<ResponseDto> ReadSystemPermission(long userAccountId, int systemId)
        {
            try
            {
                if (userAccountId <= 0 && systemId <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermission(userAccountId, systemId);

                return new ResponseDto(HttpStatusCode.OK, true, permissionInDb == null ? "Data Not Found" : "Successfully Get All Data", permissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadSystemPermission", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/system-permission/{key}
        /// </summary>
        /// <param name="key">Primary key of the entity as parameter</param>
        /// <param name="permission">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateSystemPermission)]
        public async Task<ResponseDto> UpdateSystemPermission(int key, SystemPermission permission)
        {
            try
            {
                if (key != permission.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                if (await IsPermissionDuplicate(permission) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                permission.DateModified = DateTime.Now;

                context.SystemPermissionRepository.Update(permission);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", permission);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateSystemPermission", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/system-permission/{key}
        /// </summary>
        /// <param name="key">Primary key of the entity as parameter</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystemPermission)]
        public async Task<ResponseDto> DeleteSystemPermission(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByKey(key);

                if (permissionInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                permissionInDb.IsDeleted = true;
                permissionInDb.DateModified = DateTime.Now;

                context.SystemPermissionRepository.Update(permissionInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", permissionInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteSystemPermission", "SystemPermissionController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the permission is duplicate? 
        /// </summary>
        /// <param name="system">Country object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsPermissionDuplicate(SystemPermission permission)
        {
            try
            {
                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermission(permission.UserAccountId, permission.SystemId);

                if (permissionInDb != null)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsPermissionDuplicate", "SystemPermissionController.cs", ex.Message);

                throw;
            }
        }
    }
}