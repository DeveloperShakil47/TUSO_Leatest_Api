using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateEmailControl(EmailControl emailControl)
        {
            try
            {
                if (emailControl is null)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                emailControl.DateCreated = DateTime.Now;
                emailControl.IsDeleted = false;

                context.EmailControlRepository.Add(emailControl);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadEmailControlByKey", new { key = emailControl.Oid }, emailControl);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateEmailControl", "EmailControlController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/configuration/key/{key}
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailControlByKey)]
        public async Task<IActionResult> ReadEmailControlByKey(int key)
        {
            try
            {
                var configuration = await context.EmailControlRepository.GetEmailControlByKey(key);

                return Ok(configuration);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadEmailControlByKey", "EmailControlController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateEmailControl(int key, EmailControl emailControl)
        {
            try
            {
                if (key != emailControl.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

               
                emailControl.DateModified = DateTime.Now;
                emailControl.IsDeleted = false;


                context.EmailControlRepository.Update(emailControl);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateEmailControl", "EmailControlController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/configuration/delete/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteEmailControl)]
        public async Task<IActionResult> DeleteEmailControl(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var configuration = await context.EmailControlRepository.GetEmailControlByKey(key);

                if (configuration == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                configuration.IsDeleted = true;
                configuration.DateModified = DateTime.Now;

                context.EmailControlRepository.Update(configuration);
                await context.SaveChangesAsync();

                return Ok(configuration);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteEmailControl", "EmailControlController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}