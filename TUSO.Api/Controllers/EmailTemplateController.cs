using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

namespace TUSO.Api.Controllers
{
    /// <summary>
    ///EmailTemplate Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<EmailTemplateController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public EmailTemplateController(IUnitOfWork context, ILogger<EmailTemplateController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate
        /// </summary>
        /// <param name="emailTemplate">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateEmailTemplate)]
        [CustomAuthorization]
        public async Task<ResponseDto> CreateEmailTemplate(EmailTemplate emailTemplate)
        {
            try
            {
                var emailTemplateInDb = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(Convert.ToInt32(emailTemplate.BodyType));

                if (emailTemplateInDb != null)
                {
                    emailTemplateInDb.Subject = emailTemplate.Subject;
                    emailTemplateInDb.MailBody = emailTemplate.MailBody;
                    emailTemplateInDb.DateModified = DateTime.Now;

                    context.EmailTemplateRepository.Update(emailTemplateInDb);
                    await context.SaveChangesAsync();

                    return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", emailTemplateInDb);
                }

                emailTemplate.DateCreated = DateTime.Now;
                emailTemplate.IsDeleted = false;

                context.EmailTemplateRepository.Add(emailTemplate);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", emailTemplate);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateEmailTemplate", "EmailTemplateController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplates
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailTemplates)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadEmailTemplates()
        {
            try
            {
                var emailTemplates = await context.EmailTemplateRepository.GetEmailTemplates();

                return new ResponseDto(HttpStatusCode.OK, true, emailTemplates == null ? "Data Not Found" : "Successfully Get All Data", emailTemplates);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEmailTemplates", "EmailTemplateController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table EmailTemplate</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailTemplateByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadEmailTemplateByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var emailTemplate = await context.EmailTemplateRepository.GetEmailTemplateByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, emailTemplate == null ? "Data Not Found" : "Successfully Get Data by Key", emailTemplate);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEmailTemplateByKey", "EmailTemplateController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="emailTemplate">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateEmailTemplate)]
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateEmailTemplate(int key, EmailTemplate emailTemplate)
        {
            try
            {
                if (key != emailTemplate.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                emailTemplate.DateModified = DateTime.Now;

                context.EmailTemplateRepository.Update(emailTemplate);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", emailTemplate);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateEmailTemplate", "EmailTemplateController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteEmailTemplate)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteEmailTemplate(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var emailTemplateInDb = await context.EmailTemplateRepository.GetEmailTemplateByKey(key);

                if (emailTemplateInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                emailTemplateInDb.IsDeleted = true;
                emailTemplateInDb.DateModified = DateTime.Now;

                context.EmailTemplateRepository.Update(emailTemplateInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", emailTemplateInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteEmailTemplate", "EmailTemplateController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}