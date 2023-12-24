using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;
using Utilities.Encryptions;

namespace TUSO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<UserAccountController> logger;

        /// <summary>
        /// System Permission constructor.
        /// </summary>
        /// <param name="context">Inject IUnitOfWork as context</param>
        public UserAccountController(IUnitOfWork context, ILogger<UserAccountController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/user-account
        /// </summary>
        /// <param name="userAccount">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateUserAccount)]
        public async Task<IActionResult> CreateUserAccount(UserAccountCreateDto user)
        {
            try
            {
                UserAccount userAccount = new UserAccount()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password,
                    CountryCode = user.CountryCode,
                    Cellphone = user.Cellphone,
                    IsAccountActive = user.IsAccountActive,
                    RoleId  = user.RoleId,

                };

                if (await IsAccountDuplicate(userAccount) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateUserAccountError);

                var userAccountWithSameCellphone = await context.UserAccountRepository.GetUserAccountByCellphone(userAccount.Cellphone);

                if (userAccountWithSameCellphone != null)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateCellphoneError);

                userAccount.DateCreated = DateTime.Now;
                userAccount.IsDeleted = false;

                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(userAccount.Password);
                userAccount.Password = encryptedPassword;

               context.UserAccountRepository.Add(userAccount);
                await context.SaveChangesAsync();

                if (userAccount.SystemPermissionList != null)
                {
                    foreach (var item in userAccount.SystemPermissionList)
                    {
                        SystemPermission systemPermission = new SystemPermission();

                        systemPermission.SystemId = item;
                        systemPermission.UserAccountId = userAccount.Oid;
                        systemPermission.DateCreated = DateTime.Now;
                        systemPermission.IsDeleted = false;
                        context.SystemPermissionRepository.Add(systemPermission);
                        await context.SaveChangesAsync();
                    }
                }

                return CreatedAtAction("ReadUserAccountByKey", new { key = userAccount.Oid }, userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateUserAccount", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/useraccount/count
        /// </summary>
        /// <returns> Total,Resolved and Unresolved user account count</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccount)]
        public async Task<IActionResult> ReadUserCount()
        {
            try
            {
                UserAccountCountDto useraccountDto = await context.UserAccountRepository.UserAccountCount();

                return Ok(useraccountDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserCount", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-accounts
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountPage)]
        public async Task<IActionResult> ReadUserAccounts(int start, int take)
        {
            try
            {
                var userAccounts = await context.UserAccountRepository.GetUsers(start, take);

                return Ok(userAccounts);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccounts", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-accounts/name
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountsByName)]
        public async Task<IActionResult> ReadUserAccountsByName(string name, int start, int take)
        {
            try
            {
                var userAccounts = await context.UserAccountRepository.GetUsersByName(name, start, take);

                return Ok(userAccounts);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountsByName", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

   

        /// <summary>
        /// URL : tuso-api/user-account/usertype/{devicetypeId}
        /// </summary>
        /// <param name="UsertypeID">UsertypeID of usertype as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserByDeviceType)]
        public async Task<IActionResult> ReadUserByUserType(int devicetypeId)
        {
            try
            {
                if (devicetypeId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccountInDb = await context.UserAccountRepository.GetUserByDevicetypeByKey(devicetypeId);

                if (userAccountInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccountInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserByUserType", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/client/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table useraccount</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByKey)]
        public async Task<IActionResult> ReadUserAccountByKey(long key)
        {
            try
            {
                if (String.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccount = await context.UserAccountRepository.GetClientAccountByKey(key);

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByKey", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/role/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <param name="start">Object to be updated</param>
        /// <param name="take">Object to be updated</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByRole)]
        public async Task<IActionResult> ReadUserAccountByRole(int key, int start, int take)
        {
            try
            {
                if (String.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccount = await context.UserAccountRepository.GetUserAccountByRole(key, start, take);

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByRole", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/expert
        /// </summary>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByExpert)]
        public async Task<IActionResult> ReadUserAccountByExpert()
        {
            try
            {
                var userAccount = await context.UserAccountRepository.GetUserAccountByExpert();

                if (userAccount == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByExpert", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="userAccount">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateUserAccount)]
        public async Task<IActionResult> UpdateUserAccount(long key, UserAccount userAccount)
        {
            try
            {

                if (key != userAccount.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsAccountDuplicate(userAccount) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateUserAccountError);

                var userAccountWithSameCellphone = await context.UserAccountRepository.GetUserAccountByCellphone(userAccount.Cellphone);


                if (userAccountWithSameCellphone != null && userAccountWithSameCellphone.Oid != userAccount.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateCellphoneError);

                userAccount.DateModified = DateTime.Now;

                context.UserAccountRepository.Update(userAccount);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateUserAccount", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/login
        /// </summary>
        /// <param name="login">Object to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.UserLogin)]
        public async Task<ResponseDto> UserLogin(LoginDto login)
        {
            try
            {
                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(login.Password);

                var user = await context.UserAccountRepository.GetUserByUserNamePassword(login.UserName, encryptedPassword);

                if (user != null)
                {
                    var currentLoginUser = await context.UserAccountRepository.GetClientAccountByKey(user.Oid);
                    return new ResponseDto(HttpStatusCode.OK, true, "Login Successfull", currentLoginUser);
                }
                else
                {
                    return new ResponseDto(HttpStatusCode.NotFound, false, "Invalid Username and Password", null);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UserLogin", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/changepassword
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.ChangedPassword)]
        public async Task<IActionResult> ChangedPassword(ResetPasswordDto changePassword)
        {
            try
            {
                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedOldPassword = encryptionHelpers.Encrypt(changePassword.Password);

                var user = await context.UserAccountRepository.GetUserByUserNamePassword(changePassword.UserName, encryptedOldPassword);

                if (user.Password != encryptedOldPassword)
                    return BadRequest(MessageConstants.WrongPasswordError);

                if (user != null)
                {
                    user.Password = changePassword.NewPassword;

                    string encryptedPassword = encryptionHelpers.Encrypt(changePassword.NewPassword);
                    user.Password = encryptedPassword;

                    context.UserAccountRepository.Update(user);
                    await context.SaveChangesAsync();

                    return Ok(user);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ChangedPassword", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/recovery-password
        /// </summary>
        /// <param name="recoveryPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.RecoveryPassword)]
        public async Task<IActionResult> RecoveryPassword(RecoveryPasswordDto recoveryPassword)
        {
            try
            {
                var check = await context.UserAccountRepository.GetUserAccountByKey(recoveryPassword.UserAccountID);

                if (check != null)
                {
                    if (check.Password != recoveryPassword.Password)
                    {
                        EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                        string encryptedOldPassword = encryptionHelpers.Encrypt(recoveryPassword.Password);

                        check.Password = encryptedOldPassword;

                        context.UserAccountRepository.Update(check);

                        var recovery = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(recoveryPassword.RequestID);

                        recovery.IsRequestOpen = false;

                        context.RecoveryRequestRepository.Update(recovery);
                        await context.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RecoveryPassword", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteUserAccount)]
        public async Task<IActionResult> DeleteUserAccount(long key)
        {
            try
            {
                if (String.IsNullOrEmpty(key.ToString()))
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByKey(key);

                if (userAccountInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (userAccountInDb.RoleId == 1)
                {
                    var totalOpenTicketUnderClient = await context.UserAccountRepository.TotalOpenTicketUnderClient(userAccountInDb.Oid);

                    if (totalOpenTicketUnderClient > 0)
                        return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);
                }
                else if (userAccountInDb.RoleId == 4)
                {
                    var isTeamLead = await context.UserAccountRepository.IsTeamLeader(userAccountInDb.Oid);

                    if (isTeamLead == false)
                    {
                      //  var incidentAssignedToExpert = await context.IncidentRepository.GetDeletedExpertIncidents(userAccountInDb.Oid);

                        //if (incidentAssignedToExpert != null)
                        //{
                        //    foreach (var incident in incidentAssignedToExpert)
                        //    {

                        //        incident.AssignedToState = incident.AssignedTo;
                        //        incident.AssignedTo = null;
                        //        incident.IsReassigned = true;
                        //        incident.DateModified = DateTime.Now;
                        //     //   context.IncidentRepository.Update(incident);
                        //    }
                        //}
                        var userMembers = await context.MemberRepository.GetMembersByUser(userAccountInDb.Oid);

                        if (userMembers != null)
                        {
                            foreach (var member in userMembers)
                            {
                                member.DateModified = DateTime.Now;
                                context.MemberRepository.Delete(member);
                            }
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);
                    }
                }

                userAccountInDb.IsDeleted = true;
                userAccountInDb.DateModified = DateTime.Now;

                context.UserAccountRepository.Update(userAccountInDb);
                await context.SaveChangesAsync();

                return Ok(userAccountInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUserAccount", "UserAccountController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the user account is duplicate? 
        /// </summary>
        /// <param name="userAccount">UserAccount object.</param>
        /// <returns>Boolean</returns>
        [HttpGet]
        [Route(RouteConstants.IsUniqueUserName)]
        public async Task<bool> IsAccountUnique(string key)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByName(key);

                if (userAccountInDb != null)
                    return true;

                return false;
            }
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsAccountUnique", "UserAccountController.cs", ex.Message);

                throw;
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/unique-cellphone/{key}
        /// </summary>
        /// <param name="key">UserAccount object.</param>
        /// <returns>Boolean</returns>
        [HttpGet]
        [Route(RouteConstants.IsUniqueCellphone)]
        public async Task<bool> IsCellphoneUnique(string key)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByCellphone(key);

                if (userAccountInDb != null)
                    return true;

                return false;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCellphoneUnique", "UserAccountController.cs", ex.Message);

                throw;
            }
        }


        /// <summary>
        /// URL: tuso-api/user-account/search/{name}
        /// </summary>
        /// <param name="name">UserAccount object.</param>
        /// <returns>Boolean</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUsersByName)]
        public async Task<List<UserAccount>> ReadUsersByName(string name)
        {
            try
            {
                List<UserAccount> userAccounts = new List<UserAccount>();

                if (name.Length < 3)
                    return userAccounts;

                userAccounts = await context.UserAccountRepository.GetUserAccountByFullName(name);

                if (userAccounts != null)
                    return userAccounts;

                return userAccounts;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUsersByName", "UserAccountController.cs", ex.Message);

                throw;
            }
        }

        /// <summary>
        /// Checks whether the userAccount name is duplicate? 
        /// </summary>
        /// <param name="userAccount">UserAccount object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsAccountDuplicate(UserAccount userAccount)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByName(userAccount.Username);

                if (userAccountInDb != null)
                {
                    if (userAccountInDb.Oid != userAccount.Oid)
                        return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsAccountDuplicate", "UserAccountController.cs", ex.Message);

                throw;
            }
        }

        private async Task<UserAccount?> GetUserInfo(UserAccount userAccount)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByName(userAccount.Username);

                if (userAccountInDb != null)
                    return userAccountInDb;

                return null;
            }
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetUserInfo", "UserAccountController.cs", ex.Message);

                throw;
            }
        }
    }
}
