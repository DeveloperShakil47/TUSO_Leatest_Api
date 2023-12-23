﻿using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public EmailTemplateController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate
        /// </summary>
        /// <param name="emailTemplate">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateEmailTemplate)]
        public async Task<IActionResult> CreateEmailTemplate(EmailTemplate emailTemplate)
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
                    return CreatedAtAction("ReadEmailTemplateByKey", new { key = emailTemplateInDb.Oid }, emailTemplateInDb);
                }

                emailTemplate.DateCreated = DateTime.Now;
                emailTemplate.IsDeleted = false;

                context.EmailTemplateRepository.Add(emailTemplate);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadEmailTemplateByKey", new { key = emailTemplate.Oid }, emailTemplate);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplates
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailTemplates)]
        public async Task<IActionResult> ReadEmailTemplates()
        {
            try
            {
                var emailTemplates = await context.EmailTemplateRepository.GetEmailTemplates();

                return Ok(emailTemplates);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table EmailTemplate</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailTemplateByKey)]
        public async Task<IActionResult> ReadEmailTemplateByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var emailTemplate = await context.EmailTemplateRepository.GetEmailTemplateByKey(key);

                if (emailTemplate == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(emailTemplate);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateEmailTemplate(int key, EmailTemplate emailTemplate)
        {
            try
            {
                if (key != emailTemplate.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                emailTemplate.DateModified = DateTime.Now;

                context.EmailTemplateRepository.Update(emailTemplate);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailTemplate/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteEmailTemplate)]
        public async Task<IActionResult> DeleteEmailTemplate(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var emailTemplateInDb = await context.EmailTemplateRepository.GetEmailTemplateByKey(key);

                if (emailTemplateInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                emailTemplateInDb.IsDeleted = true;
                emailTemplateInDb.DateModified = DateTime.Now;

                context.EmailTemplateRepository.Update(emailTemplateInDb);
                await context.SaveChangesAsync();

                return Ok(emailTemplateInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}