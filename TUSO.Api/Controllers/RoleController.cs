using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<RoleController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="UnitOfWork"></param>
        public RoleController(IUnitOfWork context, ILogger<RoleController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/user-role
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateUserRole)]
        public async Task<ResponseDto> CreateUserRole(Role role)
        {
            try
            {
                if (await IsRoleDuplicate(role) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                role.DateCreated = DateTime.Now;
                role.IsDeleted = false;

                context.RoleRepository.Add(role);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", role);
            }
            catch (Exception ex )
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateUserRole", "RoleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/countries
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserRoles)]
        public async Task<ResponseDto> ReadUserRoles()
        {
            try
            { 
                var role = await context.RoleRepository.GetRoles();

                return new ResponseDto(HttpStatusCode.OK, true, role == null ? "Data Not Found" : "Successfully Get All Data", role);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserRoles", "RoleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-roles
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserRolesPage)]
        public async Task<ResponseDto> ReadUserRolesPage(int start, int take)
        {
            try
            {
                var userAccounts = await context.RoleRepository.GetRolePage(start, take);

                var response = new
                {
                    data = userAccounts,
                    currentPage = start + 1,
                    totalRows = await context.RoleRepository.GetRoleCount()
                };
                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserRolesPage", "RoleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-role/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserRoleByKey)]
        public async Task<ResponseDto> ReadUserRoleByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var userAccount = await context.RoleRepository.GetRoleByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, userAccount == null ? "Data Not Found" : "Successfully Get Data by Key", userAccount);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserRoleByKey", "RoleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-role/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="role">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateUserRole)]
        public async Task<ResponseDto> UpdateUserRole(int key, Role role)
        {
            try
            {
                if (key != role.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                if (await IsRoleDuplicate(role) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                role.DateModified = DateTime.Now;
                role.IsDeleted = false;

                context.RoleRepository.Update(role);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", role);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateUserRole", "RoleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-role/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteUserRole)]
        public async Task<ResponseDto> DeleteUserRole(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var userAccountInDb = context.RoleRepository.Get(key);

                if (userAccountInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                userAccountInDb.IsDeleted = true;
                userAccountInDb.DateModified = DateTime.Now;

                context.RoleRepository.Update(userAccountInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", userAccountInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUserRole", "RoleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the user role is duplicate? 
        /// </summary>
        /// <param name="role">UserRole object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsRoleDuplicate(Role role)
        {
            try
            {
                var userAccountInDb = await context.RoleRepository.GetRoleByName(role.RoleName);

                if (userAccountInDb != null)
                    if (userAccountInDb.Oid != role.Oid)
                        return true;

                return false;
            }
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsRoleDuplicate", "RoleController.cs", ex.Message);

                throw;
            }
        }
    }
}