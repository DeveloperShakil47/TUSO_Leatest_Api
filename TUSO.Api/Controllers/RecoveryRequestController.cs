using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Rakib
 * Date created: 03.10.2022
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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public RecoveryRequestController(IUnitOfWork context)
        {
            this.context = context;
        }

        //[HttpPost]
        //[Route(RouteConstants.CreateRecoveryRequest)]
        //public async Task<IActionResult> CreateRecoveryRequest(RecoveryRequestDto recoveryRequest)
        //{
        //    try
        //    {
        //        var IsExist = await context.UserAccountRepository.GetUserByUsernameCellPhone(recoveryRequest.CellPhone, recoveryRequest.UserName, recoveryRequest.CountryCode);
                
        //        if (IsExist != null)
        //        {
        //            if (await IsRequestDuplicate(recoveryRequest) == true)
        //                return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

        //            var userRecovery = new RecoveryRequest()
        //            {
        //                Cellphone = IsExist.Cellphone,
        //                Username = IsExist.Username,
        //                DateRequested = DateTime.Now,
        //                IsRequestOpen = true,
        //                UserAccountId = IsExist.Oid,
        //            };

        //            userRecovery.DateCreated = DateTime.Now;
        //            userRecovery.IsDeleted = false;

        //            context.RecoveryRequestRepository.Add(userRecovery);
        //            await context.SaveChangesAsync();
        //            return Ok(userRecovery);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status510NotExtended, MessageConstants.NoMatchFoundError);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}

        /// <summary>
        /// URL: tuso-api/recovery-request
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRecoveryRequests)]
        public async Task<IActionResult> ReadRecoveryRequests()
        {
            try
            {
                var recoveryRequest = await context.RecoveryRequestRepository.GetRecoveryRequests();

                return Ok(recoveryRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request-page
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRecoveryRequestsByPage)]
        public async Task<IActionResult> ReadRecoveryRequestsByPage(int start, int take)
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

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table RecoveryRequests</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRecoveryRequestByKey)]
        public async Task<IActionResult> ReadRecoveryRequestByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var recoveryRequest = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(key);

                if (recoveryRequest == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(recoveryRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateRecoveryRequest(long key, RecoveryRequest recoveryRequest)
        {
            try
            {
                if (key != recoveryRequest.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                recoveryRequest.DateModified = DateTime.Now;

                context.RecoveryRequestRepository.Update(recoveryRequest);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/recovery-request/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Delete row from database</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRecoveryRequest)]
        public async Task<IActionResult> DeleteRecoveryRequest(long key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var recoveryRequestInDb = await context.RecoveryRequestRepository.GetRecoveryRequestByKey(key);

                if (recoveryRequestInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                recoveryRequestInDb.IsDeleted = true;
                recoveryRequestInDb.DateModified = DateTime.Now;

                context.RecoveryRequestRepository.Update(recoveryRequestInDb);
                await context.SaveChangesAsync();

                return Ok(recoveryRequestInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
            catch (Exception)
            {
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
            catch
            {
                throw;
            }
        }
    }
}