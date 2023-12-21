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
    /// SystemPermission Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class SystemPermissionController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        /// System Permission constructor.
        /// </summary>
        /// <param name="context">Inject IUnitOfWork as context</param>
        public SystemPermissionController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/system-permission
        /// </summary>
        [HttpPost]
        [Route(RouteConstants.CreateSystemPermission)]
        public async Task<IActionResult> CreateSystemPermission(SystemPermission permission)
        {
            try
            {
                if (await IsPermissionDuplicate(permission) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                permission.DateCreated = DateTime.Now;
                permission.IsDeleted = false;

                context.SystemPermissionRepository.Add(permission);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadSystemPermissionByKey", new { key = permission.Oid }, permission);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/system-permissions
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissions)]
        public async Task<IActionResult> ReadSystemPermissions()
        {
            try
            {
                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissions();
                return Ok(permissionInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/system-permission/key/{OID}
        /// </summary>
        /// <param name="OID">Primary key of entity as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByKey)]
        public async Task<IActionResult> ReadSystemPermissionByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByKey(key);

                if (permissionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(permissionInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>     
        /// URL : tuso-api/system-permission/role?RoleID={RoleID}
        /// </summary>
        /// <param name="RoleID">RoleID of role as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByUser)]
        public async Task<IActionResult> ReadSystemPermissionByUser(int userAccountId)
        {
            try
            {
                if (userAccountId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByUser(userAccountId);

                if (permissionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(permissionInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        // <summary>
        /// URL : tuso-api/system-permission-page/user/{UserAccountID}
        /// </summary>
        /// <param name="key">Primary key of the table SystemPermission</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByUserPage)]
        public async Task<IActionResult> ReadSystemPermissionByUserPage(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var systemPermissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByUserPage(key, start, take);

                var response = new
                {
                    systemPermission = systemPermissionInDb,
                    currentPage = start + 1,
                    totalRows = await context.SystemPermissionRepository.GetSystemPermissionCount(key)
                };
                if (systemPermissionInDb == null)

                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL : tuso-api/system-permission/system?SystemID={SystemID}
        /// </summary>
        /// <param name="SystemID">SystemID of project as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadSystemPermissionByProject)]
        public async Task<IActionResult> ReadSystemPermissionByProject(int systemId)
        {
            try
            {
                if (systemId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionBySystem(systemId);

                if (permissionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(permissionInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> ReadSystemPermission(long userAccountId, int systemId)
        {
            try
            {
                if (userAccountId <= 0 && systemId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermission(userAccountId, systemId);

                if (permissionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(permissionInDb);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateSystemPermission(int key, SystemPermission permission)
        {
            try
            {
                if (key != permission.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsPermissionDuplicate(permission) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                permission.DateModified = DateTime.Now;

                context.SystemPermissionRepository.Update(permission);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/system-permission/{key}
        /// </summary>
        /// <param name="key">Primary key of the entity as parameter</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteSystemPermission)]
        public async Task<IActionResult> DeleteSystemPermission(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var permissionInDb = await context.SystemPermissionRepository.GetSystemPermissionByKey(key);

                if (permissionInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                permissionInDb.IsDeleted = true;
                permissionInDb.DateModified = DateTime.Now;

                context.SystemPermissionRepository.Update(permissionInDb);
                await context.SaveChangesAsync();

                return Ok(permissionInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
            catch
            {
                throw;
            }
        }
    }
}