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
    ///Configuration Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class EmailControlController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<EmailControlController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public EmailControlController(IUnitOfWork context, ILogger<EmailControlController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/district
        /// </summary>
        /// <param name="configuration">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateEmailControl)]
        public async Task<ResponseDto> CreateEmailControl(EmailControl emailControl)
        {
            try
            {
                if (emailControl is null)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                emailControl.DateCreated = DateTime.Now;
                emailControl.IsDeleted = false;

                context.EmailControlRepository.Add(emailControl);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", emailControl);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateEmailControl", "EmailControlController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/configuration/key/{key}
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailControlByKey)]
        public async Task<ResponseDto> ReadEmailControlByKey(int key)
        {
            try
            {
                if(key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);


                var emailControl = await context.EmailControlRepository.GetEmailControlByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, emailControl == null ? "Data Not Found" : "Successfully Get Data by Key", emailControl);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEmailControlByKey", "EmailControlController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/configuration/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="configuration">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateEmailControl)]
        public async Task<ResponseDto> UpdateEmailControl(int key, EmailControl emailControl)
        {
            try
            {
                if (key != emailControl.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);


                emailControl.DateModified = DateTime.Now;
                emailControl.IsDeleted = false;


                context.EmailControlRepository.Update(emailControl);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", emailControl);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateEmailControl", "EmailControlController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/configuration/delete/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteEmailControl)]
        public async Task<ResponseDto> DeleteEmailControl(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var emailControl = await context.EmailControlRepository.GetEmailControlByKey(key);

                if (emailControl == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                emailControl.IsDeleted = true;
                emailControl.DateModified = DateTime.Now;

                context.EmailControlRepository.Update(emailControl);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", emailControl);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteEmailControl", "EmailControlController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}