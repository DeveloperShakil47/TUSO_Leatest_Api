using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;
        /// <summary>
        /// System Permission constructor.
        /// </summary>
        /// <param name="context">Inject IUnitOfWork as context</param>
        public UserAccountController(IConfiguration configuration, IUnitOfWork context, ILogger<UserAccountController> logger)
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
        public async Task<ResponseDto> CreateUserAccount(UserAccountCreateDto user)
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
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.DuplicateUserAccountError, null);

                var userAccountWithSameCellphone = await context.UserAccountRepository.GetUserAccountByCellphone(userAccount.Cellphone);

                if (userAccountWithSameCellphone != null)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.DuplicateCellphoneError, null);

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
                var currentUser = context.UserAccountRepository.GetUserAccountByKey(userAccount.Oid);
                return new ResponseDto(HttpStatusCode.OK, true, "Data save successfully", currentUser);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateUserAccount", "UserAccountController.cs", ex.Message);
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/useraccount/count
        /// </summary>
        /// <returns> Total,Resolved and Unresolved user account count</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccount)]
        public async Task<ResponseDto> ReadUserCount()
        {
            try
            {
                UserAccountCountDto useraccountDto = await context.UserAccountRepository.UserAccountCount();
                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, useraccountDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserCount", "UserAccountController.cs", ex.Message);
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-accounts
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountPage)]
        public async Task<ResponseDto> ReadUserAccounts(int start, int take)
        {
            try
            {
                var userAccounts = await context.UserAccountRepository.GetUsers(start, take);

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, userAccounts);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccounts", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-accounts/name
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountsByName)]
        public async Task<ResponseDto> ReadUserAccountsByName(string name, int start, int take)
        {
            try
            {
                var userAccounts = await context.UserAccountRepository.GetUsersByName(name, start, take);

                return new ResponseDto(HttpStatusCode.InternalServerError, true, "", userAccounts);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountsByName", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

   

        /// <summary>
        /// URL : tuso-api/user-account/usertype/{devicetypeId}
        /// </summary>
        /// <param name="UsertypeID">UsertypeID of usertype as parameter</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserByDeviceType)]
        public async Task<ResponseDto> ReadUserByUserType(int devicetypeId)
        {
            try
            {
                if (devicetypeId <= 0)
                   return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var userAccountInDb = await context.UserAccountRepository.GetUserByDevicetypeByKey(devicetypeId);

                if (userAccountInDb == null)
                   return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, userAccountInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserByUserType", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/client/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table useraccount</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByKey)]
        public async Task<ResponseDto> ReadUserAccountByKey(long key)
        {
            try
            {
                if (String.IsNullOrEmpty(key.ToString()))
                  return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var userAccount = await context.UserAccountRepository.GetClientAccountByKey(key);

                if (userAccount == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);


                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByKey", "UserAccountController.cs", ex.Message);
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
        public async Task<ResponseDto> ReadUserAccountByRole(int key, int start, int take)
        {
            try
            {
                if (String.IsNullOrEmpty(key.ToString()))
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var userAccount = await context.UserAccountRepository.GetUserAccountByRole(key, start, take);

                if (userAccount == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByRole", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/expert
        /// </summary>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUserAccountByExpert)]
        public async Task<ResponseDto> ReadUserAccountByExpert()
        {
            try
            {
                var userAccount = await context.UserAccountRepository.GetUserAccountByExpert();

                if (userAccount == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, userAccount);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUserAccountByExpert", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
        public async Task<ResponseDto> UpdateUserAccount(long key, UserAccount userAccount)
        {
            try
            {

                if (key != userAccount.Oid)
                     return new ResponseDto(HttpStatusCode.Unauthorized, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsAccountDuplicate(userAccount) == true)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.DuplicateUserAccountError, null);

                var userAccountWithSameCellphone = await context.UserAccountRepository.GetUserAccountByCellphone(userAccount.Cellphone);


                if (userAccountWithSameCellphone != null && userAccountWithSameCellphone.Oid != userAccount.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.DuplicateCellphoneError, null);

                userAccount.DateModified = DateTime.Now;

                context.UserAccountRepository.Update(userAccount);
                await context.SaveChangesAsync();


                return new ResponseDto(HttpStatusCode.OK, true, "Update Successfully", null);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateUserAccount", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
                  
                        var tokenString = GenerateJwtToken(currentLoginUser.Email);
                      
                    
                    return new ResponseDto(HttpStatusCode.OK, true, "Login Successfull",new {token= tokenString, user= currentLoginUser });
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
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userName) }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// URL: tuso-api/user-account/changepassword
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.ChangedPassword)]
        public async Task<ResponseDto> ChangedPassword(ResetPasswordDto changePassword)
        {
            try
            {
                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedOldPassword = encryptionHelpers.Encrypt(changePassword.Password);

                var user = await context.UserAccountRepository.GetUserByUserNamePassword(changePassword.UserName, encryptedOldPassword);

                if (user.Password != encryptedOldPassword)              
                   return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.WrongPasswordError, null);

                if (user != null)
                {
                    user.Password = changePassword.NewPassword;

                    string encryptedPassword = encryptionHelpers.Encrypt(changePassword.NewPassword);
                    user.Password = encryptedPassword;

                    context.UserAccountRepository.Update(user);
                    await context.SaveChangesAsync();

                    return new ResponseDto(HttpStatusCode.OK, true, "Password Successfully Changed", user);
                }
                else
                {
                 
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ChangedPassword", "UserAccountController.cs", ex.Message);
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/recovery-password
        /// </summary>
        /// <param name="recoveryPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.RecoveryPassword)]
        public async Task<ResponseDto> RecoveryPassword(RecoveryPasswordDto recoveryPassword)
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

                        return new ResponseDto(HttpStatusCode.OK, true, "Password Recovery Successfull", null);
                    }
                    else
                    {
                      
                        return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);
                    }
                }
                else
                {
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "RecoveryPassword", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteUserAccount)]
        public async Task<ResponseDto> DeleteUserAccount(long key)
        {
            try
            {
                if (String.IsNullOrEmpty(key.ToString()))
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByKey(key);

                if (userAccountInDb == null)
                   return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (userAccountInDb.RoleId == 1)
                {
                    var totalOpenTicketUnderClient = await context.UserAccountRepository.TotalOpenTicketUnderClient(userAccountInDb.Oid);

                    if (totalOpenTicketUnderClient > 0)
                    return new ResponseDto(HttpStatusCode.MethodNotAllowed, false, MessageConstants.DependencyError, null);
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
                        return new ResponseDto(HttpStatusCode.MethodNotAllowed, false, MessageConstants.DependencyError, null);

                    }
                }

                userAccountInDb.IsDeleted = true;
                userAccountInDb.DateModified = DateTime.Now;

                context.UserAccountRepository.Update(userAccountInDb);
                await context.SaveChangesAsync();


                return new ResponseDto(HttpStatusCode.OK, true, "Delete Successfully", userAccountInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteUserAccount", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the user account is duplicate? 
        /// </summary>
        /// <param name="userAccount">UserAccount object.</param>
        /// <returns>Boolean</returns>
        [HttpGet]
        [Route(RouteConstants.IsUniqueUserName)]
        public async Task<ResponseDto> IsAccountUnique(string key)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByName(key);

                if (userAccountInDb != null)
                    return new ResponseDto(HttpStatusCode.OK, true, "This is unique username", null);
                return new ResponseDto(HttpStatusCode.NotAcceptable, false, "This username already used", null);
            }
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsAccountUnique", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, ex.Message, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/user-account/unique-cellphone/{key}
        /// </summary>
        /// <param name="key">UserAccount object.</param>
        /// <returns>Boolean</returns>
        [HttpGet]
        [Route(RouteConstants.IsUniqueCellphone)]
        public async Task<ResponseDto> IsCellphoneUnique(string key)
        {
            try
            {
                var userAccountInDb = await context.UserAccountRepository.GetUserAccountByCellphone(key);

                if (userAccountInDb != null)
                    return new ResponseDto(HttpStatusCode.OK, true, "This is unique cellphone", null);

                return new ResponseDto(HttpStatusCode.NotAcceptable, false, "This cellphone already used", null);
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCellphoneUnique", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, ex.Message, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/user-account/search/{name}
        /// </summary>
        /// <param name="name">UserAccount object.</param>
        /// <returns>Boolean</returns>
        [HttpGet]
        [Route(RouteConstants.ReadUsersByName)]
        public async Task<ResponseDto> ReadUsersByName(string name)
        {
            try
            {
                List<UserAccount> userAccounts = new List<UserAccount>();

                if (name.Length < 3)
                       return new ResponseDto(HttpStatusCode.NotAcceptable, false, "Must be put more than 3 characters", null);

                userAccounts = await context.UserAccountRepository.GetUserAccountByFullName(name);

                return new ResponseDto(HttpStatusCode.OK, true, userAccounts != null?string.Empty: "Data Not Found", userAccounts);
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadUsersByName", "UserAccountController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, ex.Message, null);
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
