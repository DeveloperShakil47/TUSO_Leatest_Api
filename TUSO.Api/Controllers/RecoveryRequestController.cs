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
    ///RecoveryRequest Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class RecoveryRequestController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<RecoveryRequestController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public RecoveryRequestController(IUnitOfWork context, ILogger<RecoveryRequestController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpPost]
        [Route(RouteConstants.CreateRecoveryRequest)]

        public async Task<ResponseDto> CreateRecoveryRequest(RecoveryRequestDto recoveryRequest)
        {
            try
            {
                var IsExist = await context.UserAccountRepository.GetUserByUsernameCellPhone(recoveryRequest.CellPhone, recoveryRequest.UserName, recoveryRequest.CountryCode);

                if (IsExist != null)
                {
                    if (await IsRequestDuplicate(recoveryRequest) == true)
                        return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                    var userRecovery = new RecoveryRequest()
                    {
                        Cellphone = IsExist.Cellphone,
                        Username = IsExist.Username,
                        DateRequested = DateTime.Now,
                        IsRequestOpen = true,
                        UserAccountId = IsExist.Oid,
                    };

                    userRecovery.DateCreated = DateTime.Now;
                    userRecovery.IsDeleted = false;

                    context.RecoveryRequestRepository.Add(userRecovery);
                    await context.SaveChangesAsync();
                    return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", userRecovery);
                }
                else
                {
                    return new ResponseDto(HttpStatusCode.NotExtended, false, MessageConstants.NoMatchFoundError, null);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateRecoveryRequest", "RecoveryRequestController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRecoveryRequests)]
        public async Task<ResponseDto> ReadRecoveryRequests()
        {
            try
            {
                var recoveryRequest = await context.RecoveryRequestRepository.GetRecoveryRequests();

                return new ResponseDto(HttpStatusCode.OK, true, recoveryRequest == null ? "Data Not Found" : "Successfully Get All Data", recoveryRequest);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRecoveryRequests", "RecoveryRequestController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request-page
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRecoveryRequestsByPage)]
        public async Task<ResponseDto> ReadRecoveryRequestsByPage(int start, int take)
        {
            try
            {
                var recoveryRequests = await context.RecoveryRequestRepository.GetRecoveryRequestByPage(start, take);

                var response = new
                {
                    UserTypes = recoveryRequests,
                    currentPage = start + 1,
                    TotalRows = await context.RecoveryRequestRepository.GetRecoveryRequestCount()
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRecoveryRequestsByPage", "RecoveryRequestController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table RecoveryRequests</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRecoveryRequestByKey)]
        public async Task<ResponseDto> ReadRecoveryRequestByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var recoveryRequest = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, recoveryRequest == null ? "Data Not Found" : "Successfully Get Data by Key", recoveryRequest);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadRecoveryRequestByKey", "RecoveryRequestController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="recoveryRequest">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateRecoveryRequest)]
        public async Task<ResponseDto> UpdateRecoveryRequest(long key, RecoveryRequest recoveryRequest)
        {
            try
            {
                if (key != recoveryRequest.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                recoveryRequest.DateModified = DateTime.Now;
                recoveryRequest.IsDeleted = false;

                context.RecoveryRequestRepository.Update(recoveryRequest);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", recoveryRequest);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateRecoveryRequest", "RecoveryRequestController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Delete row from database</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRecoveryRequest)]
        public async Task<ResponseDto> DeleteRecoveryRequest(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var recoveryRequestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(key);

                if (recoveryRequestInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                recoveryRequestInDb.IsDeleted = true;
                recoveryRequestInDb.DateModified = DateTime.Now;

                context.RecoveryRequestRepository.Update(recoveryRequestInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", recoveryRequestInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteRecoveryRequest", "RecoveryRequestController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Delete row from database</returns>
        [HttpGet]
        [Route(RouteConstants.SearchRecoveryByUserName)]
        public async Task<IActionResult> SearchRecoveryByUserName(string? userName, string? cellphone)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(cellphone))
                {
                    return Ok(new List<RecoveryRequest>());
                }

                var recoveryRequest = await context.RecoveryRequestRepository.SearchByUserName(userName, cellphone);

                if (recoveryRequest == null || !recoveryRequest.Any())
                {
                    return Ok(new List<RecoveryRequest>());
                }

                return Ok(recoveryRequest);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "SearchRecoveryByUserName", "RecoveryRequestController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the recoveryRequest name is duplicate? 
        /// </summary>
        /// <param name="recoveryRequest">RecoveryRequest object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsRequestDuplicate(RecoveryRequestDto recoveryRequest)
        {
            try
            {
                var requestInDb = await context.RecoveryRequestRepository.FirstOrDefaultAsync(r => (r.Username == recoveryRequest.UserName || r.Cellphone ==recoveryRequest.CellPhone) && r.IsRequestOpen == true);

                if (requestInDb != null)
                {
                    if (requestInDb.Username == recoveryRequest.UserName || requestInDb.Cellphone == recoveryRequest.CellPhone)
                        return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsRequestDuplicate", "RecoveryRequestController.cs", ex.Message);

                throw;
            }
        }
    }
}